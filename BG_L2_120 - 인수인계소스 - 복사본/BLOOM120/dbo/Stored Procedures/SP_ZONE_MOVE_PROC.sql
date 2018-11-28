CREATE PROCEDURE dbo.SP_ZONE_MOVE_PROC
 @P_HEAT_NO    VARCHAR(6)
,@P_HEAT_SEQ   INT
,@P_TO_ZONE_CD VARCHAR(3)
,@P_USER_ID    VARCHAR(20)
,@P_PROC_STAT  VARCHAR(3)     OUTPUT
,@P_PROC_MSG   VARCHAR(1000)  OUTPUT
AS
--------------------------------------------------------
---- 입력변수 : HEAT_NO,순번, TO존코드, 사용자ID,
---- 출력변수 : 처리상태(OK,ERR), 처리결과MSG
--------------------------------------------------------
SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_ZONE_MOVE_PROC';
DECLARE @Err_Msg        VARCHAR(300); 
DECLARE @Err_CD         VARCHAR(20); 
DECLARE @Err_TP         VARCHAR(10); 
DECLARE @ERR_LINE       NUMERIC(10,0); 
DECLARE @ERR_NUMBER     NUMERIC(10,0);
DECLARE @REMARK         VARCHAR(20);

DECLARE @DELETE_CNT     NUMERIC(10) = 0;
DECLARE @INSERT_CNT     NUMERIC(10) = 0;
DECLARE @UPDATE_CNT     NUMERIC(10) = 0;
DECLARE @SELECT_CNT     NUMERIC(10) = 0;
DECLARE @PROC_CNT     NUMERIC(10) = 0;

DECLARE @SQL            NVARCHAR(4000)
DECLARE @CUR_DATE       VARCHAR(8);
DECLARE @BEF_DATE       VARCHAR(8);

DECLARE @S_HEAT_NO        VARCHAR(6);
DECLARE @S_HEAT_END_YN    VARCHAR(1);
DECLARE @S_BEF_HEAT_NO        VARCHAR(6);
DECLARE @S_BEF_HEAT_END_YN    VARCHAR(1);
DECLARE @N_HEAT_NO        VARCHAR(6);
DECLARE @N_WORK_PLN_DATE  VARCHAR(8);
DECLARE @N_WORK_SEQ       integer;
DECLARE @N_TAKE_OVER_PCS  integer;
DECLARE @N_HEAT_SEQ       integer;
DECLARE @HEAT_SEQ         integer;
DECLARE @S_WORK_STAT      VARCHAR(5);
DECLARE @S_HEAT_SEQ_CNT   integer;
DECLARE @STEEL            VARCHAR(10);
DECLARE @ITEM             VARCHAR(2);
DECLARE @ITEM_SIZE        VARCHAR(4);
DECLARE @STEEL_TYPE       VARCHAR(3);
DECLARE @MFG_DATE         VARCHAR(8);
DECLARE @ZONE_CD          VARCHAR(3);
DECLARE @PROG_STAT        VARCHAR(5);
DECLARE @W_PROG_STAT      VARCHAR(5);
DECLARE @W_REWORK_YN      VARCHAR(5);
DECLARE @W_REWORK_SNO     INT;

BEGIN
    SET  @P_PROC_STAT = 'OK';
    SET  @P_PROC_MSG  = '정상적으로 처리되었습니다. ';

        
    SET @CUR_DATE = CONVERT(VARCHAR(8),GETDATE()-6/24,112);
    
    --BEGIN TRAN
    BEGIN
        SELECT TOP 1
                @ZONE_CD    = ZONE_CD
               ,@PROG_STAT  = PROG_STAT
        FROM   TB_RL_TM_TRACKING
        WHERE  HEAT_NO   = @P_HEAT_NO
        AND    HEAT_SEQ  = @P_HEAT_SEQ;
        IF  @@ROWCOUNT = 0
            BEGIN
                SET  @ERR_TP   = 'ERR';
                SET  @ERR_MSG  = '트래킹정보가 없습니다. ' + @P_HEAT_NO + CONVERT(VARCHAR(5),@P_HEAT_SEQ);
                EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @ERR_MSG;
                GOTO ERROR_RTN;
            END;
        ----ZONE코드에 정의한 재작업 CHECK
        SELECT TOP 1
               @W_REWORK_YN  = ISNULL(COLUMNC,'N')
        FROM   TB_CM_COM_CD
        WHERE CATEGORY = 'ZONE_CD'
        AND   USE_YN   = 'Y'       
        AND   COLUMNA  = @ZONE_CD
        AND   COLUMNB  = @P_TO_ZONE_CD;
        IF  @@ROWCOUNT = 0
            BEGIN
                SET  @ERR_TP   = 'ERR';
                SET  @ERR_MSG  = '트래킹정보가 없습니다. ' + @P_HEAT_NO + CONVERT(VARCHAR(5),@P_HEAT_SEQ);
                EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @ERR_MSG;
                GOTO ERROR_RTN;
            END;
        SET  @W_PROG_STAT = 'RUN';
        IF  @P_TO_ZONE_CD = 'Z00'
            SET  @W_PROG_STAT = 'WAT';
        SET  @W_REWORK_SNO = 0;
        IF  @W_REWORK_YN  = 'Y'
            SET  @W_REWORK_SNO = 1;
        UPDATE TB_RL_TM_TRACKING
        SET     ZONE_CD           = @P_TO_ZONE_CD
               ,ZONE_CD_UPD_DDTT  = GETDATE()
               ,BEF_ZONE_CD       = ZONE_CD
               ,BEF_ZONE_UPD_DDTT = ZONE_CD_UPD_DDTT
               ,PROG_STAT         = @W_PROG_STAT
               ,BEF_PROG_STAT     = PROG_STAT
               ,REWORK_YN         = @W_REWORK_YN
               ,REWORK_SNO        = REWORK_SNO + @W_REWORK_SNO
        WHERE  HEAT_NO    = @P_HEAT_NO
        AND    HEAT_SEQ   = @P_HEAT_SEQ;
        IF  @@ERROR <> 0
            BEGIN
                SET  @ERR_TP   = 'ERR';
                SET  @ERR_MSG  = '트래킹 UPDATE 오류. ' + @P_HEAT_NO + CONVERT(VARCHAR(5),@P_HEAT_SEQ);
                EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @ERR_MSG;
                GOTO ERROR_RTN;
            END;
    END;
            
    SET  @ERR_TP  = 'INFO';
    SET  @ERR_CD  = '정상'; 
    SET  @ERR_MSG = 'INSERT_CNT : ' + CONVERT(VARCHAR(5),@INSERT_CNT) + 'UPDATE_CNT : ' + CONVERT(VARCHAR(5),@UPDATE_CNT);
    EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @ERR_MSG;
    RETURN 0;
    
ERROR_RTN:
    BEGIN
        SET  @P_PROC_STAT = 'ERR';
        SET  @P_PROC_MSG  = 'ZONE 이동 처리 오류 : 전산담당자에게 문의하십시요 ';
        RETURN 1;
    END
    
END