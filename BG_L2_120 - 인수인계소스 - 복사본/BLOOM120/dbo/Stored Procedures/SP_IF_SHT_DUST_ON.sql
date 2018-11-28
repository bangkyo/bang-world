CREATE PROCEDURE dbo.SP_IF_SHT_DUST_ON
 @P_IF_SEQ    NUMERIC(12)
,@P_TC_CD     VARCHAR(4)  = 'H010'
,@P_PROC_STAT VARCHAR(3)     OUTPUT
,@P_PROC_MSG  VARCHAR(1000)  OUTPUT
AS
--------------------------------------------------------
---- 입력변수 : IF_SEQ, TC_CD
---- 출력변수 : 처리상태(OK,ERR), 처리결과MSG
--------------------------------------------------------
SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_IF_SHT_DUST_ON';
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
DECLARE @PROC_CNT       NUMERIC(10) = 0;

DECLARE @SQL            NVARCHAR(4000)
DECLARE @CUR_DATE       VARCHAR(8);
DECLARE @BEF_DATE       VARCHAR(8);

DECLARE @ZONE_CD        VARCHAR(3) = 'Z10';
DECLARE @BEF_ZONE_CD    VARCHAR(3) = '-';
DECLARE @S_HEAT_NO      VARCHAR(6) = '';
DECLARE @S_HEAT_SEQ     INT        = 0;

DECLARE @REG_DDTT       VARCHAR(17);
DECLARE @DATA_01        VARCHAR(30);

DECLARE @SHT_DUST_STAT      VARCHAR(10);
DECLARE @SHT_DUST_ON_DDTT   DATETIME;
DECLARE @SHT_DUST_OFF_DDTT  DATETIME;

BEGIN
    SET  @P_PROC_STAT = 'OK';
    SET  @P_PROC_MSG  = '정상적으로 처리되었습니다. ';

    BEGIN TRY
        
        BEGIN TRAN
            SET @CUR_DATE = CONVERT(VARCHAR(8),GETDATE()-6/24,112);
            IF  @P_TC_CD  <> 'H010'
                BEGIN
                    SET  @P_PROC_STAT  = 'ERR';
                    SET  @P_PROC_MSG   = 'TC CD ERROR : ' + @P_TC_CD;
                    RAISERROR('TC CD ERROR ', 16, 1);
                END;
            
            BEGIN
                ----I/F받은 QR정보 select 
                SELECT TOP 1
                        @REG_DDTT      = REG_DDTT
                       ,@DATA_01       = RTRIM(DATA_01)
                FROM   TB_L1_RECEIVE
                WHERE  IF_SEQ       = @P_IF_SEQ;
                IF  @@ROWCOUNT = 0
                    BEGIN
                        SET  @P_PROC_STAT  = 'ERR';
                        SET  @P_PROC_MSG   = CONVERT(VARCHAR(10),@P_IF_SEQ) + '에 작업할 정보가 없습니다. ';
                        RAISERROR(@P_PROC_MSG, 16, 1);
                    END;
                ----집진정보관리의 ON상태 CHECK
                SELECT  @SHT_DUST_STAT      = ISNULL(SHT_DUST_STAT,'1')
                       ,@SHT_DUST_ON_DDTT   = SHT_DUST_ON_DDTT 
                       ,@SHT_DUST_OFF_DDTT  = SHT_DUST_OFF_DDTT
                FROM   TB_DUST_STAT_MGMT
                IF  @@ROWCOUNT = 0
                    BEGIN
                        INSERT INTO TB_DUST_STAT_MGMT 
                               ( SHT_DUST_STAT, SHT_DUST_ON_DDTT )
                        VALUES ( 'ON', @REG_DDTT );
                    END;
                ELSE
                    IF  @SHT_DUST_STAT <> 'ON'
                        BEGIN
                            UPDATE TB_DUST_STAT_MGMT
                            SET    SHT_DUST_STAT     = 'ON'
                                  ,SHT_DUST_ON_DDTT  = @REG_DDTT
                                  ,SHT_DUST_OFF_DDTT = NULL
                            /*      
                            INSERT INTO  TB_SHT_DUST_INFO (
                                    WORK_DDTT
                                   ,DUST_ON_TR
                                  ) VALUES (
                                    GETDATE()
                                   ,@REG_DDTT
                                  );  
                            IF  @@ERROR = 0
                                SET @INSERT_CNT = @INSERT_CNT + 1;
                            */    
                        END;
            END;
            
        COMMIT TRAN
        
        SET  @P_PROC_STAT  = 'OK';
        SET  @P_PROC_MSG   = '정상처리완료 ' + CONVERT(VARCHAR(5),@INSERT_CNT);
        
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





























