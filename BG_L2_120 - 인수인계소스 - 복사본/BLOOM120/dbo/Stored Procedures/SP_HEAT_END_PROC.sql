CREATE PROCEDURE [dbo].[SP_HEAT_END_PROC]
 @P_HEAT_NO   VARCHAR(6)
,@P_USER_ID   VARCHAR(20)
,@P_PROC_STAT VARCHAR(3)     OUTPUT
,@P_PROC_MSG  VARCHAR(1000)  OUTPUT
AS
/*--------------------------------------------------------
---- HEAT 종료 처리 
---- 입력변수 : HEAT_NO, 사용자ID,
---- 출력변수 : 처리상태(OK,ERR), 처리결과MSG
---- 처리 방법
---- 1. 스케줄 확인 : 이미 종료처리 및 존재여부 CHECK
---- 2. HEAT 진행관리 ( 작업중인 HEAT관리 )
---- 3. 트래킹에 진행중인것 남아있는 경우 오류처리
---- 4. 그라인딩실적이 없는 경우 오류처리
---- 5. HEAT진행관리 종료처리 갱신
---- 6. 스케줄에 진행상태 및 작업본수 갱신
---- 7. 스케줄에서 다음에 작업할 HEAT SELECT
---- 8. 트래킹에 없는 경우 블룸정보편성 SP 호출
--------------------------------------------------------*/
SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_HEAT_END_PROC';
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

BEGIN
    SET  @P_PROC_STAT = 'OK';
    SET  @P_PROC_MSG  = '정상적으로 처리되었습니다. ';

        SET @CUR_DATE = CONVERT(VARCHAR(8),GETDATE(),112);
        
        BEGIN TRAN
            ----스케줄을 확인한다. 이미 종료처리 및 존재여부 CHECK
            BEGIN
                SELECT TOP 1
                       @S_HEAT_NO   = HEAT_NO
                      ,@S_WORK_STAT = WORK_STAT
                FROM  TB_SCHEDULE      
                WHERE HEAT_NO      = @P_HEAT_NO;
                IF  @@ROWCOUNT = 0
                    BEGIN
                        SET  @P_PROC_STAT = 'ERR';
                        SET  @P_PROC_MSG  = @P_HEAT_NO + ' 스케줄에 없는 HEAT입니다.';
                        GOTO ERROR_RTN;
                    END;
                IF  @S_WORK_STAT = 'END'
                    BEGIN
                        SET  @P_PROC_STAT = 'ERR';
                        SET  @P_PROC_MSG  = @P_HEAT_NO + ' 이미 종료처리된 HEAT입니다.';
                        GOTO ERROR_RTN;
                    END
            END;
            ----HEAT진행관리 확인 및 종료처리
            BEGIN
                SELECT TOP 1
                       @S_HEAT_NO         = HEAT_NO
                      ,@S_HEAT_END_YN     = HEAT_END_YN
                      ,@S_BEF_HEAT_NO     = BEF_HEAT_NO
                      ,@S_BEF_HEAT_END_YN = BEF_HEAT_END_YN
                FROM   TB_HEAT_PROG_MGMT;
                IF  @@ROWCOUNT = 0
                    BEGIN
                        ----없는 경우는 처음처리하는 경우 이므로 INSERT
                        INSERT INTO TB_HEAT_PROG_MGMT (
                               HEAT_NO        
                              ,HEAT_END_YN    
                              ,BEF_HEAT_NO    
                              ,BEF_HEAT_END_YN
                              ,REGISTER
                              ,REG_DDTT
                              ) VALUES (
                               @P_HEAT_NO        
                              ,'N'
                              ,''
                              ,'N'
                              ,@P_USER_ID
                              ,GETDATE() );
                        
                        COMMIT TRAN;
                        RETURN (0);
                    END;       
                ELSE
                    BEGIN
                        IF  @S_HEAT_NO = @P_HEAT_NO AND @S_HEAT_END_YN = 'Y'
                            BEGIN
                                SET  @P_PROC_STAT = 'ERR';
                                SET  @P_PROC_MSG  = @P_HEAT_NO + ' 이미 종료처리된 HEAT입니다.';
                                GOTO ERROR_RTN;
                            END;
                    END    
            END;
----20180808 회의에 의해 check사항 제외     
--            ----트래킹에 진행중인것 남아 있는 경우 CHECK
--            BEGIN
--                SELECT @SELECT_CNT  = COUNT(*)
--                FROM   TB_RL_TM_TRACKING
--                WHERE  HEAT_NO      = @P_HEAT_NO
--                AND    PROG_STAT    = 'RUN'
--                AND    ZONE_CD    not in ('Z90','Z91','Z99');
--                IF  @SELECT_CNT > 0
--                    BEGIN
--                        SET  @P_PROC_STAT = 'ERR';
--                        SET  @P_PROC_MSG  = @P_HEAT_NO + ' 작업진행중인 정보가 있습니다. (모니터링 확인)';
--                        GOTO ERROR_RTN;
--                    END
--            END;
--            ----GRD작업실적 존재여부 CHECK (없는 경우는 안됨) 
--            BEGIN
--                SELECT @SELECT_CNT  = COUNT(*)
--                FROM   TB_GRD_WR
--                WHERE  HEAT_NO      = @P_HEAT_NO;
--                IF  @SELECT_CNT = 0
--                    BEGIN
--                        SET  @P_PROC_STAT = 'ERR';
--                        SET  @P_PROC_MSG  = @P_HEAT_NO + ' 그라인딩 실적이 없습니다. [확인 바랍니다]';
--                        GOTO ERROR_RTN;
--                    END
--            END;
            ----HEAT진행관리 갱신
            BEGIN
                UPDATE TB_HEAT_PROG_MGMT
                SET    HEAT_NO         = @P_HEAT_NO
                      ,HEAT_END_YN     = 'Y'
                      ,BEF_HEAT_NO     = HEAT_NO
                      ,BEF_HEAT_END_YN = HEAT_END_YN
                      ,MODIFIER        = @P_USER_ID
                      ,MOD_DDTT        = GETDATE();
            END;
            ----스케줄에 진행상태 갱신
            BEGIN
                UPDATE TB_SCHEDULE
                SET    WORK_STAT  = 'END'
                      ,WORK_PCS   = ( SELECT COUNT(*) FROM TB_GRD_WR WHERE HEAT_NO   = @P_HEAT_NO AND REWORK_SNO = 0 )
                      ,MODIFIER   = @P_USER_ID
                      ,MOD_DDTT   = GETDATE() 
                WHERE HEAT_NO   = @P_HEAT_NO; 
            END;    
            ----트래킹에서 진행중인것 종료로
--            BEGIN
--                UPDATE TB_RL_TM_TRACKING
--                SET    PROG_STAT     = 'END'
--                      ,BEF_PROG_STAT = PROG_STAT
--                      ,MODIFIER      = 'HEAT_END'
--                      ,MOD_DDTT      = GETDATE()
--                WHERE  HEAT_NO      = @P_HEAT_NO
--                AND    PROG_STAT   <> 'END';
--                IF  @@ERROR <> 0
--                    BEGIN
--                        SET  @P_PROC_STAT = 'ERR';
--                        SET  @P_PROC_MSG  = @P_HEAT_NO + ' HEAT 종료 처리 오류.';
--                        GOTO ERROR_RTN;
--                    END
--            END;            
        COMMIT TRAN;
        
        SET  @ERR_TP  = 'INFO';
        SET  @ERR_CD  = '정상'; 
        SET  @ERR_MSG = '수동 HEAT 정상 종료  ' ;
        EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @ERR_MSG;
        RETURN 0;
        
ERROR_RTN:

        ROLLBACK TRAN -- 실패!
 
        -- 해당 시스템 함수는 반드시 CATCH문에서 사용해야한다. (CATCH블록 외부에서 호출되는 경우 NULL값 반환)
        BEGIN
            SET  @ERR_LINE    = ERROR_LINE();
            SET  @ERR_NUMBER  = ERROR_NUMBER();
            SET  @ERR_MSG     = SUBSTRING(ERROR_MESSAGE(),1,200);
            SELECT CONVERT(VARCHAR(5),@ERR_LINE) + '-' +  @ERR_MSG;
            SET  @ERR_TP  = 'ERR';
            SET  @ERR_CD  = CONVERT(VARCHAR(10),@ERR_NUMBER); 
            EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @P_PROC_MSG;
            --SET  @P_PROC_STAT = @ERR_TP;
            --SET  @P_PROC_MSG  = @ERR_MSG;
        END
END;
