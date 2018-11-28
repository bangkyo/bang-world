CREATE PROCEDURE [dbo].[SP_TRACKING_CRE]
 @P_HEAT_NO   VARCHAR(6)
,@P_USER_ID   VARCHAR(20)
,@P_PROC_STAT VARCHAR(3)     OUTPUT
,@P_PROC_MSG  VARCHAR(1000)  OUTPUT
AS
--------------------------------------------------------
---- 입력변수 : HEAT_NO, 인수본수, 사용자ID,
---- 출력변수 : 처리상태(OK,ERR), 처리결과MSG
--------------------------------------------------------
SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_TRACKING_CRE';
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
DECLARE @TRK_IN_ZONE_CD          VARCHAR(3);

BEGIN
    SET  @P_PROC_STAT = 'OK';
    SET  @P_PROC_MSG  = '정상적으로 처리되었습니다. ';

    BEGIN TRY
        
        SET @CUR_DATE = CONVERT(VARCHAR(8),GETDATE()-6/24,112);
        
        ----BEGIN TRAN
            BEGIN
                SELECT TOP 1
                        @N_WORK_PLN_DATE = WORK_PLN_DATE
                       ,@N_WORK_SEQ      = WORK_SEQ
                       ,@N_TAKE_OVER_PCS = TAKE_OVER_PCS
                       ,@STEEL           = STEEL     
                       ,@ITEM            = ITEM      
                       ,@ITEM_SIZE       = ITEM_SIZE 
                       ,@STEEL_TYPE      = STEEL_TYPE
                       ,@MFG_DATE        = MFG_DATE  
                FROM   TB_SCHEDULE
                WHERE  HEAT_NO   = @P_HEAT_NO;
                IF  @@ROWCOUNT = 0
                    BEGIN
                        SET  @ERR_TP   = 'ERR';
                        SET  @ERR_MSG  = '스케줄정보가 없습니다. ' + @P_HEAT_NO;
                        RAISERROR('스케줄정보 없습니다.', 16, 1);
                    END;
                ----인수본수 만큼 트래킹정보 INSERT
                
                --2018-09-12 박도우 대리 투입 존을 Z60으로 요청
                --공통코드를 이용한 투입코드 설정
                SET @TRK_IN_ZONE_CD = 'Z00'
                
                SELECT TOP 1
                        @TRK_IN_ZONE_CD =  CD_NM
                  FROM TB_CM_COM_CD
                WHERE CATEGORY = 'TRK_IN_ZON'
                  AND CD_NM IN ('Z00', 'Z60')
                  AND USE_YN = 'Y';

                SET @PROC_CNT = 0;
                
                WHILE  @N_TAKE_OVER_PCS > @PROC_CNT
                BEGIN
                    SET @PROC_CNT = @PROC_CNT + 1;
                    IF  EXISTS ( SELECT HEAT_SEQ FROM TB_RL_TM_TRACKING WHERE HEAT_NO = @P_HEAT_NO AND HEAT_SEQ = @PROC_CNT )
                        BEGIN
                            SET @UPDATE_CNT = @UPDATE_CNT + 1;
                            /*
                            IF  NOT EXISTS ( SELECT HEAT_SEQ FROM TB_GRD_WR WHERE HEAT_NO = @P_HEAT_NO AND HEAT_SEQ = @PROC_CNT AND REWORK_SNO = 0)
                            BEGIN
                                --불룸정보가 있는 경우 ZONE코드 @TRK_IN_ZONE_CD으로 갱신 및 대기 상태로 변경
                                UPDATE TB_RL_TM_TRACKING
                                SET     ZONE_CD           = @TRK_IN_ZONE_CD
                                       ,ZONE_CD_UPD_DDTT  = GETDATE()
                                       ,BEF_ZONE_CD       = ZONE_CD
                                       ,BEF_ZONE_UPD_DDTT = ZONE_CD_UPD_DDTT
                                       ,PROG_STAT         = 'WAT'
                                       ,BEF_PROG_STAT     = PROG_STAT
                                WHERE  HEAT_NO    = @P_HEAT_NO
                                AND    HEAT_SEQ   = @PROC_CNT;
                                SET @UPDATE_CNT = @UPDATE_CNT + 1;
                            END;
                            */
                        END;
                    ELSE
                        BEGIN
                            INSERT INTO TB_RL_TM_TRACKING (
                                     HEAT_NO
                                    ,HEAT_SEQ
                                    ,STEEL
                                    ,ITEM
                                    ,ITEM_SIZE
                                    ,STEEL_TYPE
                                    ,MFG_DATE
                                    ,WORK_DATE
                                    ,ZONE_CD
                                    ,ZONE_CD_UPD_DDTT
                                    ,PROG_STAT
                                    ,REWORK_YN
                                    ,REWORK_SNO
                                    ,WORK_SEQ
                                    ,REGISTER
                                    ,REG_DDTT
                                    ) VALUES (
                                     @P_HEAT_NO
                                    ,@PROC_CNT
                                    ,@STEEL
                                    ,@ITEM
                                    ,@ITEM_SIZE
                                    ,@STEEL_TYPE
                                    ,@MFG_DATE
                                    ,@CUR_DATE
                                    ,@TRK_IN_ZONE_CD
                                    ,GETDATE()
                                    ,'WAT'
                                    ,'N'
                                    ,0
                                    ,@PROC_CNT
                                    ,@P_USER_ID
                                    ,GETDATE()
                                );
                            SET @INSERT_CNT = @INSERT_CNT + 1;    
                        END;
                END;
            END;
            
        ----COMMIT TRAN
        
        SET  @ERR_TP  = 'INFO';
        SET  @ERR_CD  = '정상'; 
        SET  @ERR_MSG = 'INSERT_CNT : ' + CONVERT(VARCHAR(5),@INSERT_CNT) + 'UPDATE_CNT : ' + CONVERT(VARCHAR(5),@UPDATE_CNT);
        ----EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @ERR_MSG;
        
    END TRY
    BEGIN CATCH --//EXCEPTION 처리부분 : 쿼리문이 오류가 났을 경우 실행할 쿼리문

        ----ROLLBACK TRAN -- 실패!
 
        -- 해당 시스템 함수는 반드시 CATCH문에서 사용해야한다. 
        BEGIN
            SET  @ERR_LINE    = ERROR_LINE();
            SET  @ERR_NUMBER  = ERROR_NUMBER();
            SET  @ERR_MSG     = SUBSTRING(ERROR_MESSAGE(),1,200);
            SELECT CONVERT(VARCHAR(5),@ERR_LINE) + '-' +  @ERR_MSG;
            SET  @ERR_TP  = 'ERR';
            SET  @ERR_CD  = CONVERT(VARCHAR(10),@ERR_NUMBER); 
            EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @ERR_MSG;
            SET  @P_PROC_STAT = 'ERR';
            SET  @P_PROC_MSG  = 'HEAT 종료 처리 오류 : 전산담당자에게 문의하십시요 ';
        END
    END CATCH
    
END