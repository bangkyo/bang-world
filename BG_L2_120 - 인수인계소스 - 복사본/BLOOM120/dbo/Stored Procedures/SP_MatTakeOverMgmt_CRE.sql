CREATE PROCEDURE [dbo].[SP_MatTakeOverMgmt_CRE]
 @P_HEAT_NO   VARCHAR(6)
,@P_STEEL     VARCHAR(10)
,@P_ITEM_CODE VARCHAR(6)
,@P_MFG_PCS   INT
,@P_MFG_DATE  VARCHAR(8)
,@P_USER_ID   VARCHAR(20)
,@P_PROC_STAT VARCHAR(3)     OUTPUT
,@P_PROC_MSG  VARCHAR(1000)  OUTPUT

AS
-----------------------------
----강제 등록 SP
-----------------------------

SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_MatTakeOverMgmt_CRE';
DECLARE @Err_Msg        VARCHAR(300); 
DECLARE @Err_CD         VARCHAR(20); 
DECLARE @Err_TP         VARCHAR(10); 

DECLARE @ERR_LINE       NUMERIC(10,0); 
DECLARE @ERR_NUMBER     NUMERIC(10,0);
DECLARE @REMARK         VARCHAR(20);

DECLARE @DELETE_CNT     NUMERIC(10)
DECLARE @INSERT_CNT     NUMERIC(10)
DECLARE @UPDATE_CNT     NUMERIC(10);

DECLARE @SQL            NVARCHAR(4000)
DECLARE @CUR_DATE       VARCHAR(8);

DECLARE @V_FR_DATE  VARCHAR(8)
DECLARE @V_TO_DATE  VARCHAR(8);
DECLARE @HEAT_SEQ_LIST VARCHAR(4000);
DECLARE @HEAT_SEQ_TMP  VARCHAR(10);
DECLARE @HEAT_SEQ_INT  INT;
DECLARE @TAKE_OVER_PCS INT;
DECLARE @ITEM          VARCHAR(2);
DECLARE @ITEM_SIZE     VARCHAR(4);
DECLARE @STEEL_TYPE    VARCHAR(3);
DECLARE @S_PROC_STAT   VARCHAR(3);
DECLARE @S_PROC_MSG    VARCHAR(1000);
DECLARE @MFG_PCS INT;
DECLARE @S_MFG_PCS INT;
DECLARE @S_HEAT_NO     VARCHAR(6);
DECLARE @S_GRADE       VARCHAR(10);
DECLARE @S_ITEM_CODE   VARCHAR(6);
DECLARE @S_STEEL_TYPE  VARCHAR(3);
DECLARE @S_DAILY_DATE  VARCHAR(8);

BEGIN
    
    SET @CUR_DATE = CONVERT(VARCHAR(8), GETDATE()-6/24, 112);
    SET  @ERR_TP  = 'INFO';
    SET  @ERR_MSG = @P_HEAT_NO + '_' + @P_STEEL + '_' + @P_ITEM_CODE + '_' + CONVERT(VARCHAR(10), @P_MFG_PCS) + '_' + @P_MFG_DATE + '_' + @P_USER_ID;
    EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @ERR_MSG;

  BEGIN TRY
    BEGIN TRAN
    ------------------------------------------------------------------
    --HEAT 인수정보 INSERT ( 있는 경우는 오류 )
    ------------------------------------------------------------------
    IF EXISTS ( SELECT HEAT_NO FROM TB_MAT_TAKE_OVER_HEAT WHERE HEAT_NO = @P_HEAT_NO )
        BEGIN
            ----강제 등록 이므로 이미 있는 경우는 오류 처리
            SET  @P_PROC_STAT = 'ERR';
            SET  @P_PROC_MSG  = '이미 등록된 HEAT번호 입니다 (' + @P_HEAT_NO + ')';
            RAISERROR(@P_PROC_MSG, 16, 1);
        END;     
    ELSE
        BEGIN
            ----변수 설정
            SELECT @S_HEAT_NO    = @P_HEAT_NO 
                  ,@S_GRADE      = @P_STEEL
                  ,@S_ITEM_CODE  = @P_ITEM_CODE
                  ,@S_DAILY_DATE = @CUR_DATE
                  ,@S_MFG_PCS    = @P_MFG_PCS; 
            
            INSERT INTO TB_MAT_TAKE_OVER_HEAT
                   ( HEAT_NO
                    ,STEEL
                    ,ITEM
                    ,ITEM_SIZE
                    ,STEEL_TYPE
                    ,MFG_DATE
                    ,MFG_PCS
                    ,TAKE_OVER_PCS
                    ,TAKE_OVER_DATE
                    ,SURFACE_STAT
                    ,REGISTER
                    ,REG_DDTT  )
            SELECT   @S_HEAT_NO 
                    ,@S_GRADE
                    ,SUBSTRING(@S_ITEM_CODE,1,2) ITEM
                    ,SUBSTRING(@S_ITEM_CODE,3,4) ITEM_SIZE
                    ,(SELECT COLUMNA FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = @S_GRADE )
                    ,@S_DAILY_DATE
                    ,@S_MFG_PCS
                    ,0
                    ,@CUR_DATE
                    ,''
                    ,@P_USER_ID
                    ,GETDATE();
        END;
    -------------------------------------------------------------------
    --BLOOM별 인수정보 INSERT 
    -------------------------------------------------------------------
    IF EXISTS ( SELECT HEAT_NO FROM TB_MAT_TAKE_OVER_BLOOM WHERE HEAT_NO = @P_HEAT_NO )
        BEGIN
            DELETE FROM TB_MAT_TAKE_OVER_BLOOM
            WHERE  HEAT_NO  = @P_HEAT_NO; 
        END;     
    -----------------------------------------------
    ----BLOOM본수정보 생성 만큼 만든다.
    ---- TEMP TABLE에 본수정보 INSERT
    CREATE TABLE #tmp_HEAT_SEQ
         ( HEAT_NO  VARCHAR(6),
           HEAT_SEQ INT );
    SET    @MFG_PCS      = @S_MFG_PCS
    SET    @HEAT_SEQ_INT = 0;
    WHILE  @MFG_PCS > @HEAT_SEQ_INT 
    BEGIN
        SET  @HEAT_SEQ_INT = @HEAT_SEQ_INT + 1;
        INSERT INTO #tmp_HEAT_SEQ ( HEAT_NO, HEAT_SEQ )
        SELECT @P_HEAT_NO, @HEAT_SEQ_INT; 
    END;   
    -----------------------------------------------
    --BLOOM INSERT
    -----------------------------------------------
    BEGIN
        INSERT INTO TB_MAT_TAKE_OVER_BLOOM
                ( HEAT_NO
                ,HEAT_SEQ
                ,STEEL
                ,STRAND_NO
                ,ITEM
                ,ITEM_SIZE
                ,STEEL_TYPE
                ,MFG_DATE
                ,TAKE_OVER_DATE
                ,THEORY_LENGTH
                ,REAL_LENGTH
                ,Marking_Code
                ,REGISTER
                ,REG_DDTT  )
        SELECT   A.HEAT_NO   AS HEAT_NO
                ,A.HEAT_SEQ  AS HEAT_SEQ
                ,@S_GRADE       AS STEEL
                ,''   --STRAND_NO
                ,SUBSTRING(@S_ITEM_CODE,1,2)  AS ITEM
                ,SUBSTRING(@S_ITEM_CODE,3,4)  AS ITEM_SIZE
                ,(SELECT COLUMNA FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = @S_GRADE )
                ,@S_DAILY_DATE as mfg_date
                ,@CUR_DATE
                ,0  --C.LEGNTH
                ,0  --C.LEGNTH
                ,'' --C.MARKING_CODE
                ,@P_USER_ID
                ,GETDATE()
        FROM   #tmp_HEAT_SEQ   A 
        WHERE A.HEAT_NO  = @P_HEAT_NO;
        ----PRINT @@ROWCOUNT;
        IF  @@ERROR <> 0
            BEGIN
                SET  @P_PROC_STAT = 'ERR';
                SET  @P_PROC_MSG  = 'BLOOM정보 INSERT 오류.';
                RAISERROR('BLOOM정보 INSERT 오류.', 16, 1);
            END;
    END;
    ----본수 집계 및 규격 SELECT & SET
    BEGIN
        SELECT @TAKE_OVER_PCS = COUNT(*)
              ,@ITEM          = MAX(ITEM)
              ,@ITEM_SIZE     = MAX(ITEM_SIZE)
              ,@STEEL_TYPE    = MAX(STEEL_TYPE)
        FROM   TB_MAT_TAKE_OVER_BLOOM
        WHERE  HEAT_NO  = @P_HEAT_NO;
    END;
    ----HEAT에 인수본수 갱신
    BEGIN
        UPDATE TB_MAT_TAKE_OVER_HEAT 
        SET    TAKE_OVER_PCS = @TAKE_OVER_PCS
              ,MODIFIER      = @P_USER_ID
              ,MOD_DDTT      = GETDATE()
        WHERE HEAT_NO  = @P_HEAT_NO;
    END;

    -------------------------------------------------------------------
    --스케줄 자동 편성
    -------------------------------------------------------------------
    IF EXISTS ( SELECT HEAT_NO FROM TB_SCHEDULE WHERE HEAT_NO = @P_HEAT_NO )
       BEGIN
            UPDATE  TB_SCHEDULE
            SET    TAKE_OVER_PCS = @TAKE_OVER_PCS
                  ,MODIFIER      = @P_USER_ID
                  ,MOD_DDTT      = GETDATE()
            WHERE HEAT_NO  = @P_HEAT_NO;
       END;
    ELSE
       BEGIN
            INSERT INTO TB_SCHEDULE (
                   HEAT_NO
                  ,WORK_PLN_DATE
                  ,WORK_SEQ
                  ,STEEL
                  ,ITEM
                  ,ITEM_SIZE
                  ,STEEL_TYPE
                  ,MFG_DATE
                  ,TAKE_OVER_DATE
                  ,TAKE_OVER_PCS
                  ,WORK_STAT
                  ,REGISTER
                  ,REG_DDTT  )
            SELECT A.HEAT_NO
                  ,TAKE_OVER_DATE
                  ,(SELECT isnull(MAX(WORK_SEQ),0) + 1 FROM TB_SCHEDULE WHERE WORK_PLN_DATE = A.TAKE_OVER_DATE)
                  ,STEEL
                  ,ITEM
                  ,ITEM_SIZE
                  ,STEEL_TYPE
                  ,MFG_DATE
                  ,TAKE_OVER_DATE
                  ,TAKE_OVER_PCS
                  ,'WAT'
                  ,@P_USER_ID
                  ,GETDATE()
            FROM   TB_MAT_TAKE_OVER_HEAT A
            WHERE  HEAT_NO  = @P_HEAT_NO;
       END;   
    ----트래킹정보 편성
    EXEC dbo.SP_TRACKING_CRE @P_HEAT_NO, @P_USER_ID, @S_PROC_STAT OUTPUT, @S_PROC_MSG OUTPUT;
    IF  @S_PROC_STAT = 'ERR'
        BEGIN
            SET  @P_PROC_STAT = 'ERR';
            SET  @P_PROC_MSG  = @S_PROC_MSG;
            RAISERROR(@S_PROC_MSG, 16, 1);
        END;
    COMMIT TRAN;
    SET @P_PROC_STAT = 'OK';
    SET  @P_PROC_MSG  = '정상처리 완료 !!!';
    
  END TRY
    BEGIN CATCH --//EXCEPTION 처리부분 : 쿼리문이 오류가 났을 경우 실행할 쿼리문

        ROLLBACK TRAN; -- 실패!
        BEGIN

            SET  @ERR_LINE    = ERROR_LINE();
            SET  @ERR_NUMBER  = ERROR_NUMBER();
            SET  @ERR_MSG     = SUBSTRING(ERROR_MESSAGE(),1,200);
            SELECT CONVERT(VARCHAR(5),@ERR_LINE) + '-' +  @ERR_MSG;
            SET  @ERR_TP  = 'ERR';
            SET  @ERR_CD  = CONVERT(VARCHAR(10),@ERR_NUMBER); 
            EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @ERR_MSG;
            
        END
    END CATCH

END;    
    
    