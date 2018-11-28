CREATE PROCEDURE [dbo].[SP_IF_Z20_IN]
 @P_IF_SEQ    NUMERIC(12)
,@P_TC_CD     VARCHAR(4)  = 'A090'
,@P_PROC_STAT VARCHAR(3)     OUTPUT
,@P_PROC_MSG  VARCHAR(1000)  OUTPUT
AS
--------------------------------------------------------
---- 입력변수 : IF_SEQ, TC_CD
---- 출력변수 : 처리상태(OK,ERR), 처리결과MSG
--------------------------------------------------------
SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_IF_Z20_IN';
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

DECLARE @ZONE_CD        VARCHAR(3) = 'Z20';
DECLARE @BEF_ZONE_CD    VARCHAR(3) = 'Z90';
DECLARE @S_HEAT_NO      VARCHAR(6) = '';
DECLARE @S_HEAT_SEQ     INT        = 0;
DECLARE @PROG_STAT        VARCHAR(5);
DECLARE @W_PROG_STAT      VARCHAR(5);
DECLARE @W_REWORK_YN      VARCHAR(5);
DECLARE @W_REWORK_SNO     INT;

DECLARE @REG_DDTT       VARCHAR(17);
DECLARE @DATA_01        VARCHAR(30);
DECLARE @DATA_02        VARCHAR(30);

BEGIN
    SET  @P_PROC_STAT = 'OK';
    SET  @P_PROC_MSG  = '정상적으로 처리되었습니다. ';

    BEGIN TRY
        
        BEGIN TRAN
            SET @CUR_DATE = CONVERT(VARCHAR(8),GETDATE()-6/24,112);
            IF  @P_TC_CD  <> 'A090'
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
                        SET  @P_PROC_MSG   = '@P_IF_SEQ:'+CONVERT(VARCHAR(10),@P_IF_SEQ) + ', TB_L1_RECEIVE 에 작업할 정보가 없습니다. ';
                        RAISERROR(@P_PROC_MSG, 16, 1);
                    END;
                ----이동(작업)할 BLOOM SELECT ( 없는 경우는 오류 처리 ) 
                SELECT TOP 1
                        @S_HEAT_NO       = HEAT_NO
                       ,@S_HEAT_SEQ      = HEAT_SEQ
                FROM   TB_RL_TM_TRACKING
                WHERE  ZONE_CD       = @BEF_ZONE_CD
                AND    PROG_STAT     = 'RUN'
                ORDER BY HEAT_NO, WORK_SEQ;
                IF  @@ROWCOUNT = 0
                    BEGIN
                        SET  @P_PROC_STAT  = 'ERR';
                        SET  @P_PROC_MSG   = 'ZONE:'+ @BEF_ZONE_CD + '에 작업할 정보가 없습니다. ';
                        RAISERROR(@P_PROC_MSG, 16, 1);
                    END;
				--진행상태 체크
                SELECT TOP 1
					   @PROG_STAT  = PROG_STAT
				FROM   TB_RL_TM_TRACKING
				WHERE  HEAT_NO   = @S_HEAT_NO
				AND    HEAT_SEQ  = @S_HEAT_SEQ;
				IF  @@ROWCOUNT = 0
					BEGIN
						SET  @P_PROC_STAT = 'ERR';
						SET  @P_PROC_MSG  = '트래킹정보가 없습니다. ' + @S_HEAT_NO + '_'+CONVERT(VARCHAR(5),@S_HEAT_SEQ);
						RAISERROR(@P_PROC_MSG, 16, 1);
					END;
				----ZONE코드에 정의한 재작업 CHECK
				SELECT TOP 1
					   @W_REWORK_YN  = ISNULL(COLUMNC,'N')
				FROM   TB_CM_COM_CD
				WHERE CATEGORY = 'ZONE_CD'
				AND   USE_YN   = 'Y'       
				AND   COLUMNA  = @BEF_ZONE_CD
				AND   COLUMNB  = @ZONE_CD;
				IF  @@ROWCOUNT = 0
					BEGIN
						SET  @P_PROC_STAT  = 'ERR';
						SET  @P_PROC_MSG   = 'FROM ZONE:'+@BEF_ZONE_CD+', TO ZONE:'+@ZONE_CD+',ZONE 재작업 정보가 없습니다. ' + @S_HEAT_NO +'_'+ CONVERT(VARCHAR(5),@S_HEAT_SEQ);
						RAISERROR(@P_PROC_MSG, 16, 1);
					END;

				SET  @W_PROG_STAT = 'RUN';
				SET  @W_REWORK_SNO = 0;
				IF  @W_REWORK_YN  = 'Y'
					SET  @W_REWORK_SNO = 1;
				UPDATE TB_RL_TM_TRACKING
				SET     ZONE_CD           = @ZONE_CD
					   ,ZONE_CD_UPD_DDTT  = GETDATE()
					   ,BEF_ZONE_CD       = ZONE_CD
					   ,BEF_ZONE_UPD_DDTT = ZONE_CD_UPD_DDTT
					   ,PROG_STAT         = @W_PROG_STAT
					   ,BEF_PROG_STAT     = PROG_STAT
					   ,REWORK_YN         = @W_REWORK_YN
					   ,REWORK_SNO        = REWORK_SNO + @W_REWORK_SNO
				WHERE  HEAT_NO    = @S_HEAT_NO
				AND    HEAT_SEQ   = @S_HEAT_SEQ;
				IF  @@ERROR <> 0
					BEGIN
						SET  @P_PROC_STAT  = 'ERR';
						SET  @P_PROC_MSG   = '트래킹 UPDATE 오류. ' + @S_HEAT_NO +'_'+ CONVERT(VARCHAR(5),@S_HEAT_SEQ);
						RAISERROR(@P_PROC_MSG, 16, 1);
					END;
			END;
            
			SET  @ERR_TP  = 'INFO';
			SET  @ERR_CD  = '정상'; 
			SET  @ERR_MSG = 'INSERT_CNT : ' + CONVERT(VARCHAR(5),@INSERT_CNT) + 'UPDATE_CNT : ' + CONVERT(VARCHAR(5),@UPDATE_CNT);
			EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @ERR_MSG;
			--RETURN 0;
		        
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
		        EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @ERR_MSG;
		    END
		END CATCH
    
END;