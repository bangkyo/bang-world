CREATE PROCEDURE dbo.SP_STOP_TIME_CALC
 @P_START_DDTT    DATETIME
,@P_END_DDTT      DATETIME
,@P_STOP_RSN      VARCHAR(150)
AS
--------------------------------------------------------
---- 입력변수 : 시작일시, 종료일시, 정지사유(1:그라인딩시작SP 콜, 다른것은 사유)
---- 출력변수 : 처리상태(OK,ERR), 처리결과MSG
--------------------------------------------------------
SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_STOP_TIME_CALC';
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

DECLARE @ZONE_CD        VARCHAR(3) = 'Z40';
DECLARE @BEF_ZONE_CD    VARCHAR(3) = 'Z30';
DECLARE @S_HEAT_NO      VARCHAR(6) = '';
DECLARE @S_HEAT_SEQ     INT        = 0;

DECLARE @REG_DDTT       VARCHAR(17);
DECLARE @DATA_01        VARCHAR(30);
DECLARE @DATA_02        VARCHAR(30);
DECLARE @START_DDTT  DATETIME;
DECLARE @END_DDTT    DATETIME;

DECLARE @DATA_OCC_GP VARCHAR(10); 
DECLARE @STOP_TM     INT;
DECLARE @STOP_RSN    VARCHAR(200);

BEGIN
    SET  @Err_TP = 'OK';
    SET  @ERR_MSG  = '정상적으로 처리되었습니다. ';

    BEGIN TRY
        
        SET @CUR_DATE = CONVERT(VARCHAR(8),GETDATE()-6/24,112);
        
        BEGIN TRAN
            BEGIN
                ----연마지석, 칩제거시 등록한 정지시간이 있는 경우 등록 안함
                SELECT TOP 1
                        @START_DDTT    = START_DDTT
                       ,@END_DDTT      = END_DDTT
                       ,@DATA_OCC_GP   = DATA_OCC_GP
                FROM   TB_STOP_HR
                WHERE (START_DDTT      >= @P_START_DDTT  AND
                       START_DDTT      <= @P_END_DDTT)
                OR    (END_DDTT        >= @P_START_DDTT  AND
                       END_DDTT        <= @P_END_DDTT)
                OR    (START_DDTT      <= @P_START_DDTT  AND
                       END_DDTT        >= @P_END_DDTT);
                IF  @@ROWCOUNT > 0
                    BEGIN
                        SET  @Err_TP    = 'INFO';
                        SET  @Err_Msg   = 'PASS 중복되어 저장안함';
                    END;
                ELSE
                    BEGIN
                        --------------------------------------------------------------
                        ----정지 실적 생성    
                        --------------------------------------------------------------
                        IF  @P_STOP_RSN = '1'
                            SET   @STOP_RSN = '';
                        ELSE    
                            SET  @STOP_RSN  = @P_STOP_RSN;
                        INSERT INTO [dbo].[TB_STOP_HR]
                                   ([START_DDTT]
                                   ,[END_DDTT]
                                   ,[STOP_RSN]
                                   ,[DATA_OCC_GP]
                                   ,[REGISTER]
                                   ,[REG_DDTT])
                             VALUES
                                   (@P_START_DDTT
                                   ,@p_END_DDTT
                                   ,@STOP_RSN
                                   ,'1'
                                   ,'SYSTEM'
                                   ,GETDATE() );
                        IF  @@ERROR <> 0
                            BEGIN
                                SET  @Err_TP  = 'ERR';
                                SET  @Err_Msg = ' INSERT 작업중 오류 ' + SUBSTRING(ERROR_MESSAGE(),1,200);
                                EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @Err_TP, @Err_Msg;
                                RAISERROR('INSERT 작업중 오류가 발생했습니다.', 16, 1);
                            END;
                    END;    
            END;
            
        COMMIT TRAN
        
        SET  @ERR_TP  = 'OK';
        SET  @ERR_MSG   = '정상처리완료 ' + CONVERT(VARCHAR(5),@UPDATE_CNT);
        
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
    
END
