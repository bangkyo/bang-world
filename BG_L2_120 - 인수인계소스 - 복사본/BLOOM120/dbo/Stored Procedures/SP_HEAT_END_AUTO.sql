CREATE PROCEDURE [dbo].[SP_HEAT_END_AUTO]

AS
/*--------------------------------------------------------
---- HEAT 종료 처리 (자동)
---- 입력변수 : HEAT_NO, 사용자ID,
---- 처리 방법
---- 1. 스케줄에서 진행상태가 종료, 진행중인것 커서선언
---- 2. 한건씩 읽어서 인수본수와 작업본수가 같은 경우 
----    트래킹 종료로 갱신 
--------------------------------------------------------*/
SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_HEAT_END_AUTO';
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
DECLARE @S_PROC_STAT VARCHAR(3)   ;
DECLARE @S_PROC_MSG  VARCHAR(1000);
DECLARE @HEAT_NO        VARCHAR(6);
DECLARE @WORK_PLN_DATE  VARCHAR(8);
DECLARE @WORK_SEQ       INT;
DECLARE @TAKE_OVER_PCS  INT;
DECLARE @WORK_PCS       INT;
DECLARE @WORK_STAT      VARCHAR(5);
DECLARE @UPDATE_YN   VARCHAR(5);

BEGIN

    SET @CUR_DATE = CONVERT(VARCHAR(8),GETDATE(),112);
    
    BEGIN TRAN
        
    DECLARE CUR_C1  CURSOR FOR 
        SELECT  HEAT_NO
               ,WORK_PLN_DATE
               ,WORK_SEQ
               ,TAKE_OVER_PCS
               ,WORK_PCS
               ,WORK_STAT
        FROM   TB_SCHEDULE 
        WHERE  WORK_STAT IN ('END','RUN')
        ORDER  BY WORK_PLN_DATE
                 ,WORK_SEQ;
    --SET @UPDATE_YN = 'N';
    OPEN CUR_C1;
    FETCH NEXT FROM  CUR_C1 INTO @HEAT_NO
                                ,@WORK_PLN_DATE
                                ,@WORK_SEQ
                                ,@TAKE_OVER_PCS
                                ,@WORK_PCS
                                ,@WORK_STAT;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        ----진행상태 갱신
        ----인수본수=작업본수 이고 상태가 END아니면 END갱신
		SET @UPDATE_YN = 'N';   -- UPDATE 초기화 설정 추가 2018-08-20 MJ
        IF  @TAKE_OVER_PCS = @WORK_PCS 
            BEGIN
                IF  @WORK_STAT <> 'END'
                    BEGIN
                        UPDATE  TB_SCHEDULE
                        SET    WORK_STAT     = 'END' 
                              ,MODIFIER      = 'HEAT_AUTO'
                              ,MOD_DDTT      = GETDATE()
                        WHERE  HEAT_NO       = @HEAT_NO;
                    END;
                SET @UPDATE_YN = 'Y';
            END;
        ELSE
            BEGIN
                IF  @WORK_STAT = 'END'
                    BEGIN
                        SELECT @SELECT_CNT  = COUNT(*)
                        FROM   TB_RL_TM_TRACKING
                        WHERE  HEAT_NO      = @HEAT_NO
                        AND    PROG_STAT    = 'RUN'
                        AND    ZONE_CD    not in ('Z90','Z91','Z99','Z50');
                        IF  @SELECT_CNT > 0
                            BEGIN
                                SET @ERR_MSG  = @HEAT_NO + ' 작업진행중인 정보가 있습니다. (모니터링 확인)';
                                ----진행중인것 있으면 트래킹 종료로 처리
                                SET @UPDATE_YN = 'N';
                            END
                        ELSE
                            SET @UPDATE_YN = 'Y';
                        
                    END;
            END;
            IF  @UPDATE_YN = 'Y'
                ----트래킹에서 진행중인것 종료로
                BEGIN
                    UPDATE TB_RL_TM_TRACKING
                    SET    PROG_STAT     = 'END'
                          ,BEF_PROG_STAT = PROG_STAT
                          ,MODIFIER      = 'HEAT_AUTO'
                          ,MOD_DDTT      = GETDATE()
                    WHERE  HEAT_NO      = @HEAT_NO
                    AND    PROG_STAT   <> 'END';
                    IF  @@ERROR <> 0
                        BEGIN
                            SET  @ERR_TP = 'ERR';
                            SET  @ERR_MSG  = @HEAT_NO + ' HEAT 종료 처리 오류.';
                            GOTO ERROR_RTN;
                        END
					
                END;   
		--EXEC DBO.SP_ERR_LOG 'T_H_F', 'HFIN', 'TEST', @HEAT_NO;         
        FETCH NEXT FROM  CUR_C1 INTO @HEAT_NO
                                    ,@WORK_PLN_DATE
                                    ,@WORK_SEQ
                                    ,@TAKE_OVER_PCS
                                    ,@WORK_PCS
                                    ,@WORK_STAT;
    END;

    CLOSE CUR_C1;
    DEALLOCATE CUR_C1;

    COMMIT TRAN;
    
    SET  @ERR_TP  = 'INFO';
    SET  @ERR_CD  = '정상'; 
    SET  @ERR_MSG = 'HEAT 종료 처리  ' ;
    EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @ERR_MSG;
    RETURN 0;
        
ERROR_RTN:

        ROLLBACK TRAN -- 실패!
 
        BEGIN
            SET  @ERR_LINE    = ERROR_LINE();
            SET  @ERR_NUMBER  = ERROR_NUMBER();
            SET  @ERR_MSG     = SUBSTRING(ERROR_MESSAGE(),1,200);
            SELECT CONVERT(VARCHAR(5),@ERR_LINE) + '-' +  @ERR_MSG;
            SET  @ERR_TP  = 'ERR';
            SET  @ERR_CD  = CONVERT(VARCHAR(10),@ERR_NUMBER); 
            EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @ERR_MSG;
        END
END;