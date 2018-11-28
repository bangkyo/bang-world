CREATE PROCEDURE [dbo].[SP_IF_Z01_IN]
 @P_IF_SEQ    NUMERIC(12)
,@P_TC_CD     VARCHAR(4)  = 'A010'
,@P_PROC_STAT VARCHAR(3)     OUTPUT
,@P_PROC_MSG  VARCHAR(1000)  OUTPUT
AS
--------------------------------------------------------
---- 입력변수 : IF_SEQ, TC_CD
---- 출력변수 : 처리상태(OK,ERR), 처리결과MSG
--------------------------------------------------------
SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_IF_Z01_IN';
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

DECLARE @ZONE_CD        VARCHAR(3) = 'Z01';
DECLARE @BEF_ZONE_CD    VARCHAR(3) = 'Z00';
DECLARE @S_HEAT_NO      VARCHAR(6) = '';
DECLARE @S_HEAT_SEQ     INT        = 0;

DECLARE @REG_DDTT       VARCHAR(17);
DECLARE @DATA_01        VARCHAR(30);
DECLARE @DATA_02        VARCHAR(30);

DECLARE @S_HEAT_END_YN    VARCHAR(1);
DECLARE @S_BEF_HEAT_NO        VARCHAR(6);
DECLARE @S_BEF_HEAT_END_YN    VARCHAR(1);
DECLARE @C_HEAT_NO        VARCHAR(6);
DECLARE @C_HEAT_END_YN    VARCHAR(1);
DECLARE @N_HEAT_NO        VARCHAR(6);
DECLARE @N_WORK_PLN_DATE  VARCHAR(8);
DECLARE @N_WORK_SEQ       integer;
DECLARE @N_TAKE_OVER_PCS  integer;
DECLARE @N_HEAT_SEQ       integer;
DECLARE @S_HEAT_SEQ_CNT   integer;
DECLARE @S_PROC_STAT VARCHAR(3)   ;
DECLARE @S_PROC_MSG  VARCHAR(1000);
DECLARE @P_USER_ID   VARCHAR(20)  = 'SYSTEM' ;

DECLARE @E_HEAT_NO        VARCHAR(6);
DECLARE @E_WORK_SEQ       integer;

BEGIN
    SET  @P_PROC_STAT = 'OK';
    SET  @P_PROC_MSG  = '정상적으로 처리되었습니다. ';

    BEGIN TRY
        
        BEGIN TRAN
            SET @CUR_DATE = CONVERT(VARCHAR(8),GETDATE()-6/24,112);
            IF  @P_TC_CD  <> 'A010'
                BEGIN
                    SET  @P_PROC_STAT  = 'ERR';
                    SET  @P_PROC_MSG   = 'TC CD ERROR : ' + @P_TC_CD;
                    RAISERROR('TC CD ERROR ', 16, 1);
                END;
            
            BEGIN
                ----I/F받은 QR정보 select 
                SELECT TOP 1
                        @REG_DDTT      = REG_DDTT
                       ,@DATA_01       = DATA_01
                FROM   TB_L1_RECEIVE
                WHERE  IF_SEQ       = @P_IF_SEQ;
                IF  @@ROWCOUNT = 0
                    BEGIN
                        SET  @P_PROC_STAT  = 'ERR';
                        SET  @P_PROC_MSG   = CONVERT(VARCHAR(10),@P_IF_SEQ) + '에 작업할 정보가 없습니다. ';
                        RAISERROR(@P_PROC_MSG, 16, 1);
                    END;
                ----이동(작업)할 BLOOM SELECT ( 없는 경우는 오류 처리 ) 
                SELECT TOP 1
                        @S_HEAT_NO       = A.HEAT_NO
                       ,@S_HEAT_SEQ      = A.HEAT_SEQ
                FROM   TB_RL_TM_TRACKING A
                      ,TB_SCHEDULE       B
                WHERE  A.ZONE_CD       = @BEF_ZONE_CD
                AND    A.PROG_STAT     = 'WAT'
                AND    A.HEAT_NO       = B.HEAT_NO
                AND    B.WORK_STAT    <> 'END'
                ORDER BY B.WORK_PLN_DATE, B.WORK_SEQ, A.HEAT_NO, A.WORK_SEQ;
                IF  @@ROWCOUNT = 0
                    BEGIN
                        SET  @P_PROC_STAT  = 'ERR';
                        SET  @P_PROC_MSG   = 'Z00에 작업할 정보가 없습니다. ';
                        RAISERROR(@P_PROC_MSG, 16, 1);
                    END;
                ----HEAT진행관리 CHECK
                SELECT  @C_HEAT_NO     = HEAT_NO
                       ,@C_HEAT_END_YN = HEAT_END_YN
                FROM   TB_HEAT_PROG_MGMT
                IF  @@ROWCOUNT = 0
                    BEGIN
                        INSERT INTO TB_HEAT_PROG_MGMT (
                               HEAT_NO        
                              ,HEAT_END_YN    
                              ,BEF_HEAT_NO    
                              ,BEF_HEAT_END_YN
                              ,REGISTER
                              ,REG_DDTT
                              ) VALUES (
                               @S_HEAT_NO
                              ,'N'
                              ,''
                              ,'N'
                              ,' '
                              ,GETDATE() );
                        SET @C_HEAT_NO     = @S_HEAT_NO;
                    END;
                ----20180808 회의 결과 HEAT CHECK 삭제    
--                IF  @C_HEAT_NO <> @S_HEAT_NO
--                    BEGIN
--                        IF  @C_HEAT_END_YN = 'Y'
--                            BEGIN
--                                ----정상 : 이전HEAT종료 처리 상태
--                                UPDATE TB_HEAT_PROG_MGMT
--                                SET    HEAT_NO         = @S_HEAT_NO
--                                      ,HEAT_END_YN     = 'N'
--                                      ,BEF_HEAT_NO     = HEAT_NO
--                                      ,BEF_HEAT_END_YN = HEAT_END_YN
--                                      ,MODIFIER        = 'Z01_IN'
--                                      ,MOD_DDTT        = GETDATE();
--                            END;
--                        ELSE
--                            BEGIN
--                                SET  @P_PROC_STAT  = 'ERR';
--                                SET  @P_PROC_MSG   = 'HEAT 종료 처리후 작업하세요. ';
--                                RAISERROR('HEAT 종료 처리후 작업하세요.', 16, 1);
--                            END;
--                    END;
                ----이동할 위치(Z01)에 진행중인 BLOOM이 있으면 오류처리
                SELECT   TOP 1
				         @E_HEAT_NO  = HEAT_NO
				        ,@E_WORK_SEQ = HEAT_SEQ
				        ,@SELECT_CNT = COUNT(*) OVER()
                FROM   TB_RL_TM_TRACKING
                WHERE  ZONE_CD       = @ZONE_CD
                AND    PROG_STAT     = 'RUN';
                IF  @SELECT_CNT > 0
                    BEGIN
                        SET  @P_PROC_STAT  = 'ERR';
                        SET  @P_PROC_MSG   = @ZONE_CD + '에 작업중인 정보가 있습니다. _'+@E_HEAT_NO+'_'+@E_WORK_SEQ;
                        RAISERROR(@P_PROC_MSG, 16, 1);
                    END;
                --SELECT @SELECT_CNT = COUNT(*)
                --FROM   TB_RL_TM_TRACKING
                --WHERE  ZONE_CD       = @ZONE_CD
                --AND    PROG_STAT     = 'RUN';
                --IF  @SELECT_CNT > 0
                --    BEGIN
                --        SET  @P_PROC_STAT  = 'ERR';
                --        SET  @P_PROC_MSG   = @ZONE_CD + '에 작업중인 정보가 있습니다. ';
                --        RAISERROR(@P_PROC_MSG, 16, 1);
                --    END;

                ----위치 이동 UPDATE
                UPDATE  TB_RL_TM_TRACKING
                SET     ZONE_CD           = @ZONE_CD
                       ,ZONE_CD_UPD_DDTT  = GETDATE()
                       ,BEF_ZONE_CD       = ZONE_CD
                       ,BEF_ZONE_UPD_DDTT = ZONE_CD_UPD_DDTT
                       ,PROG_STAT         = 'RUN'
                       ,BEF_PROG_STAT     = PROG_STAT
                       ,MODIFIER          = 'Z01_IN'
                       ,MOD_DDTT          = GETDATE()
                WHERE  HEAT_NO    = @S_HEAT_NO
                AND    HEAT_SEQ   = @S_HEAT_SEQ;
                IF  @@ERROR <> 0
                    BEGIN
                        SET  @P_PROC_STAT  = 'ERR';
                        SET  @P_PROC_MSG   = @ZONE_CD + '에 UPDATE 오류 ';
                        RAISERROR(@P_PROC_MSG, 16, 1);
                    END;
                SET @UPDATE_CNT = @UPDATE_CNT + 1;
                IF  EXISTS ( SELECT HEAT_NO FROM TB_SCHEDULE WHERE HEAT_NO = @S_HEAT_NO AND WORK_STAT = 'WAT' )
                    BEGIN
                        UPDATE TB_SCHEDULE
                        SET    WORK_STAT = 'RUN'
                              ,MODIFIER  = 'Z01_IN'
                              ,MOD_DDTT  = GETDATE()
                        WHERE HEAT_NO = @S_HEAT_NO;
                    END;
            END;
            
        COMMIT TRAN
        
        SET  @P_PROC_STAT  = 'OK';
        SET  @P_PROC_MSG   = '정상처리완료 ' + CONVERT(VARCHAR(5),@UPDATE_CNT);
        
    END TRY
    BEGIN CATCH --//EXCEPTION 처리부분 : 쿼리문이 오류가 났을 경우 실행할 쿼리문

        ROLLBACK TRAN -- 실패!
 
        -- 해당 시스템 함수는 반드시 CATCH문에서 사용해야한다. 
        BEGIN
            SET  @ERR_LINE    = ERROR_LINE();
            SET  @ERR_NUMBER  = ERROR_NUMBER();
            SET  @ERR_MSG     = SUBSTRING(ERROR_MESSAGE(),1,200);
            SELECT CONVERT(VARCHAR(5),@ERR_LINE) + '-' +  @ERR_MSG;
            SET  @ERR_TP  = 'ERR';
            SET  @ERR_CD  = CONVERT(VARCHAR(10),@ERR_NUMBER); 
            SET  @P_PROC_STAT  = 'ERR';
            SET  @P_PROC_MSG   = CONVERT(VARCHAR(10),@P_IF_SEQ) + '_' + @P_PROC_MSG;
            EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @P_PROC_STAT, @P_PROC_MSG;
        END
    END CATCH
    
END
