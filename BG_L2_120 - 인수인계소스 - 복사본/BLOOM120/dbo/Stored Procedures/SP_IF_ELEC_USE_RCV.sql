CREATE PROCEDURE dbo.SP_IF_ELEC_USE_RCV
 @P_IF_SEQ    NUMERIC(12)
,@P_TC_CD     VARCHAR(4)  = 'M010'
,@P_PROC_STAT VARCHAR(3)     OUTPUT
,@P_PROC_MSG  VARCHAR(1000)  OUTPUT
AS
--------------------------------------------------------
---- 입력변수 : IF_SEQ, TC_CD
---- 출력변수 : 처리상태(OK,ERR), 처리결과MSG
--------------------------------------------------------
SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_IF_ELEC_USE_RCV';
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

DECLARE @ZONE_CD        VARCHAR(3) = 'Z10';
DECLARE @BEF_ZONE_CD    VARCHAR(3) = '-';
DECLARE @S_HEAT_NO      VARCHAR(6) = '';
DECLARE @S_HEAT_SEQ     INT        = 0;

DECLARE @REG_DDTT       VARCHAR(17);
DECLARE @DATA_01        VARCHAR(30);
DECLARE @DATA_02        VARCHAR(30);
DECLARE @DATA_03        VARCHAR(30);
DECLARE @DATA_04        VARCHAR(30);
DECLARE @DATA_05        VARCHAR(30);

DECLARE @SHT_DUST_TOT_USE_QTY    NUMERIC(12,0);
DECLARE @GRD_DUST_TOT_USE_QTY    NUMERIC(12,0);
DECLARE @GRD_TOT_USE_QTY         NUMERIC(12,0);
DECLARE @FACTORY_ALL_TOT_USE_QTY NUMERIC(12,0);
DECLARE @SHT_DUST_DAY_USE_QTY    NUMERIC(12,0);
DECLARE @GRD_DUST_DAY_USE_QTY    NUMERIC(12,0);
DECLARE @GRD_DAY_USE_QTY         NUMERIC(12,0);
DECLARE @FACTORY_ALL_DAY_USE_QTY NUMERIC(12,0);
DECLARE @B_SHT_DUST_TOT_USE_QTY    NUMERIC(12,0);
DECLARE @B_GRD_DUST_TOT_USE_QTY    NUMERIC(12,0);
DECLARE @B_GRD_TOT_USE_QTY         NUMERIC(12,0);
DECLARE @B_FACTORY_ALL_TOT_USE_QTY NUMERIC(12,0);

BEGIN
    SET  @P_PROC_STAT = 'OK';
    SET  @P_PROC_MSG  = '정상적으로 처리되었습니다. ';

    BEGIN TRY
        
        SET @CUR_DATE = CONVERT(VARCHAR(8),GETDATE()-8/24,112);
        SET @BEF_DATE = CONVERT(VARCHAR(8),GETDATE()-32/24,112);
        
        IF  @P_TC_CD  <> 'M010'
            BEGIN
                SET  @P_PROC_STAT  = 'ERR';
                SET  @P_PROC_MSG   = 'TC CD ERROR : ' + @P_TC_CD;
                RAISERROR('TC CD ERROR ', 16, 1);
            END;
            
        BEGIN TRAN
            BEGIN
                ----I/F받은 QR정보 select 
                SELECT TOP 1
                        @REG_DDTT      = REG_DDTT
                       ,@DATA_01       = RTRIM(DATA_01)
                       ,@DATA_02       = RTRIM(DATA_02)
                       ,@DATA_03       = RTRIM(DATA_03)
                       ,@DATA_04       = RTRIM(DATA_04)
                FROM   TB_L1_RECEIVE
                WHERE  IF_SEQ       = @P_IF_SEQ;
                IF  @@ROWCOUNT = 0
                    BEGIN
                        SET  @P_PROC_STAT  = 'ERR';
                        SET  @P_PROC_MSG   = CONVERT(VARCHAR(10),@P_IF_SEQ) + '에 작업할 정보가 없습니다. ';
                        RAISERROR(@P_PROC_MSG, 16, 1);
                    END;
                ----전일의 전력사용량을 읽어온다 
                ----(누적량으로 인터페이스 받으므로 당일 사용량 계산을 위해
                SELECT  @B_SHT_DUST_TOT_USE_QTY     = SHT_DUST_TOT_USE_QTY   
                       ,@B_GRD_DUST_TOT_USE_QTY     = GRD_DUST_TOT_USE_QTY   
                       ,@B_GRD_TOT_USE_QTY          = GRD_TOT_USE_QTY        
                       ,@B_FACTORY_ALL_TOT_USE_QTY  = FACTORY_ALL_TOT_USE_QTY
                FROM   TB_ELEC_USE_WR
                WHERE  WORK_DATE   = @BEF_DATE;
                IF  @@ROWCOUNT = 0
                    BEGIN
                        SET    @B_SHT_DUST_TOT_USE_QTY     = 0;   
                        SET    @B_GRD_DUST_TOT_USE_QTY     = 0;
                        SET    @B_GRD_TOT_USE_QTY          = 0;
                        SET    @B_FACTORY_ALL_TOT_USE_QTY  = 0;
                    END;
                SET    @SHT_DUST_TOT_USE_QTY     = CONVERT(NUMERIC(12),@DATA_01);   
                SET    @GRD_DUST_TOT_USE_QTY     = CONVERT(NUMERIC(12),@DATA_02);
                SET    @GRD_TOT_USE_QTY          = CONVERT(NUMERIC(12),@DATA_03);
                SET    @FACTORY_ALL_TOT_USE_QTY  = CONVERT(NUMERIC(12),@DATA_04);

                SET    @SHT_DUST_DAY_USE_QTY     = @SHT_DUST_TOT_USE_QTY     - @B_SHT_DUST_TOT_USE_QTY   ;   
                SET    @GRD_DUST_DAY_USE_QTY     = @GRD_DUST_TOT_USE_QTY     - @B_GRD_DUST_TOT_USE_QTY   ;
                SET    @GRD_DAY_USE_QTY          = @GRD_TOT_USE_QTY          - @B_GRD_TOT_USE_QTY        ;
                SET    @FACTORY_ALL_DAY_USE_QTY  = @FACTORY_ALL_TOT_USE_QTY  - @B_FACTORY_ALL_TOT_USE_QTY;
                IF  EXISTS ( SELECT WORK_DATE FROM TB_ELEC_USE_WR WHERE WORK_DATE = @CUR_DATE )
                    BEGIN
                        UPDATE TB_ELEC_USE_WR
                        SET         [SHT_DUST_DAY_USE_QTY]    = @SHT_DUST_DAY_USE_QTY   
                                   ,[GRD_DUST_DAY_USE_QTY]    = @GRD_DUST_DAY_USE_QTY   
                                   ,[GRD_DAY_USE_QTY]         = @GRD_DAY_USE_QTY        
                                   ,[FACTORY_ALL_DAY_USE_QTY] = @FACTORY_ALL_DAY_USE_QTY
                                   ,[SHT_DUST_TOT_USE_QTY]    = @SHT_DUST_TOT_USE_QTY   
                                   ,[GRD_DUST_TOT_USE_QTY]    = @GRD_DUST_TOT_USE_QTY   
                                   ,[GRD_TOT_USE_QTY]         = @GRD_TOT_USE_QTY        
                                   ,[FACTORY_ALL_TOT_USE_QTY] = @FACTORY_ALL_TOT_USE_QTY
                        WHERE  WORK_DATE = @CUR_DATE;
                    END;
                ELSE
                    BEGIN;
                        ----  INSERT
                        INSERT INTO [dbo].[TB_ELEC_USE_WR]
                                   ([WORK_DATE]
                                   ,[SHT_DUST_DAY_USE_QTY]
                                   ,[GRD_DUST_DAY_USE_QTY]
                                   ,[GRD_DAY_USE_QTY]
                                   ,[FACTORY_ALL_DAY_USE_QTY]
                                   ,[SHT_DUST_TOT_USE_QTY]
                                   ,[GRD_DUST_TOT_USE_QTY]
                                   ,[GRD_TOT_USE_QTY]
                                   ,[FACTORY_ALL_TOT_USE_QTY]
                                   ,[REGISTER]
                                   ,[REG_DDTT]
                              ) VALUES  (
                                    @CUR_DATE
                                   ,@SHT_DUST_DAY_USE_QTY   
                                   ,@GRD_DUST_DAY_USE_QTY   
                                   ,@GRD_DAY_USE_QTY        
                                   ,@FACTORY_ALL_DAY_USE_QTY
                                   ,@SHT_DUST_TOT_USE_QTY   
                                   ,@GRD_DUST_TOT_USE_QTY   
                                   ,@GRD_TOT_USE_QTY        
                                   ,@FACTORY_ALL_TOT_USE_QTY
                                   ,'SYSTEM'
                                   ,GETDATE()
                                 );
                        IF  @@ERROR = 0
                            SET @INSERT_CNT = @INSERT_CNT + 1;
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





























