CREATE PROCEDURE dbo.SP_IF_MPI_OPER
 @P_IF_SEQ    NUMERIC(12)
,@P_TC_CD     VARCHAR(4)  = 'C010'
,@P_PROC_STAT VARCHAR(3)     OUTPUT
,@P_PROC_MSG  VARCHAR(1000)  OUTPUT
AS
--------------------------------------------------------
---- 입력변수 : IF_SEQ, TC_CD
---- 출력변수 : 처리상태(OK,ERR), 처리결과MSG
--------------------------------------------------------
SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_IF_MPI_OPER';
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

DECLARE @ZONE_CD        VARCHAR(3) = 'Z30';
DECLARE @BEF_ZONE_CD    VARCHAR(3) = '-';
DECLARE @S_HEAT_NO      VARCHAR(6) = '';
DECLARE @S_HEAT_SEQ     INT        = 0;

DECLARE @REG_DDTT       VARCHAR(17);
DECLARE @DATA_01        VARCHAR(30);
DECLARE @DATA_02        VARCHAR(30);
DECLARE @DATA_03        VARCHAR(30);
DECLARE @DATA_04        VARCHAR(30);
DECLARE @DATA_05        VARCHAR(30);
DECLARE @DATA_06        VARCHAR(30);
DECLARE @DATA_07        VARCHAR(30);
DECLARE @DATA_08        VARCHAR(30);
DECLARE @DATA_09        VARCHAR(30);
DECLARE @DATA_10        VARCHAR(30);
DECLARE @DATA_11        VARCHAR(30);
DECLARE @DATA_12        VARCHAR(30);
DECLARE @DATA_13        VARCHAR(30);
DECLARE @DATA_14        VARCHAR(30);
DECLARE @DATA_15        VARCHAR(30);
DECLARE @DATA_16        VARCHAR(30);
DECLARE @DATA_17        VARCHAR(30);
DECLARE @DATA_18        VARCHAR(30);
DECLARE @DATA_19        VARCHAR(30);
DECLARE @DATA_20        VARCHAR(30);
DECLARE @DATA_21        VARCHAR(30);
DECLARE @DATA_22        VARCHAR(30);
DECLARE @DATA_23        VARCHAR(30);
DECLARE @DATA_24        VARCHAR(30);
DECLARE @DATA_25        VARCHAR(30);
DECLARE @DATA_26        VARCHAR(30);
DECLARE @DATA_27        VARCHAR(30);

BEGIN
    SET  @P_PROC_STAT = 'OK';
    SET  @P_PROC_MSG  = '정상적으로 처리되었습니다. ';

    BEGIN TRY
        
        BEGIN TRAN
            SET @CUR_DATE = CONVERT(VARCHAR(8),GETDATE()-6/24,112);
            IF  @P_TC_CD  <> 'C010'
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
                       ,@DATA_02       = RTRIM(DATA_02)
                       ,@DATA_03       = RTRIM(DATA_03)
                       ,@DATA_04       = RTRIM(DATA_04)
                       ,@DATA_05       = RTRIM(DATA_05)
                       ,@DATA_06       = RTRIM(DATA_06)
                       ,@DATA_07       = RTRIM(DATA_07)
                       ,@DATA_08       = RTRIM(DATA_08)
                       ,@DATA_09       = RTRIM(DATA_09)
                       ,@DATA_10       = RTRIM(DATA_10)
                       ,@DATA_11       = RTRIM(DATA_11)
                       ,@DATA_12       = RTRIM(DATA_12)
                       ,@DATA_13       = RTRIM(DATA_13)
                       ,@DATA_14       = RTRIM(DATA_14)
                       ,@DATA_15       = RTRIM(DATA_15)
                       ,@DATA_16       = RTRIM(DATA_16)
                       ,@DATA_17       = RTRIM(DATA_17)
                       ,@DATA_18       = RTRIM(DATA_18)
                       ,@DATA_19       = RTRIM(DATA_19)
                       ,@DATA_20       = RTRIM(DATA_20)
                       ,@DATA_21       = RTRIM(DATA_21)
                       ,@DATA_22       = RTRIM(DATA_22)
                       ,@DATA_23       = RTRIM(DATA_23)
                       ,@DATA_24       = RTRIM(DATA_24)
                       ,@DATA_25       = RTRIM(DATA_25)
                       ,@DATA_26       = RTRIM(DATA_26)
                FROM   TB_L1_RECEIVE
                WHERE  IF_SEQ       = @P_IF_SEQ;
                IF  @@ROWCOUNT = 0
                    BEGIN
                        SET  @P_PROC_STAT  = 'ERR';
                        SET  @P_PROC_MSG   = CONVERT(VARCHAR(10),@P_IF_SEQ) + '에 작업할 정보가 없습니다. ';
                        RAISERROR(@P_PROC_MSG, 16, 1);
                    END;
                ---- 조업정보 INSERT
                INSERT INTO [dbo].[TB_MPI_OPER_INFO]
                           ([WORK_DDTT]
                           ,[CLWTR_PMP1_STAT]
                           ,[CLWTR_PMP2_STAT]
                           ,[FIL_MTR_STAT]
                           ,[CLWTR_PMP_HDR_STAT]
                           ,[MAGNET_PMP1_STAT]
                           ,[MAGNET_PMP2_STAT]
                           ,[STIR_PMP_HDR_STAT]
                           ,[Entry_Motor_FWD]
                           ,[Entry_Motor_REV]
                           ,[Inspection_Motor_FWD]
                           ,[Inspection_Motor_REV]
                           ,[OPRS_Motor1_STAT]
                           ,[OPRS_Motor2_STAT]
                           ,[OPRS_Fan_STAT]
                           ,[Dark_Room_Hood_Fan]
                           ,[Yoke_TRANS_Motor1_FWD]
                           ,[Yoke_TRANS_Motor1_BWD]
                           ,[Yoke_TRANS_Motor2_BWD]
                           ,[Yoke_TRANS_Motor2_FWD]
                           ,[OPRS_MTR_HDR_STAT]
                           ,[OPRS_SUP_HDR_STAT]
                           ,[Yoke_MTR_NO1_CURRENT]
                           ,[Yoke_MTR_NO2_CURRENT]
                           ,[Yoke_MTR_NO3_CURRENT]
                           ,[Yoke_MTR_NO4_CURRENT]
                           ,[COIL_NO1_2_CURRENT]
                           ,[REGISTER]
                           ,[REG_DDTT]
                        )  VALUES  (
                            CONVERT(datetime,@REG_DDTT)
                           ,CONVERT(INT,@DATA_01)
                           ,CONVERT(INT,@DATA_02)
                           ,CONVERT(INT,@DATA_03)
                           ,CONVERT(INT,@DATA_04)
                           ,CONVERT(INT,@DATA_05)
                           ,CONVERT(INT,@DATA_06)
                           ,CONVERT(INT,@DATA_07)
                           ,CONVERT(INT,@DATA_08)
                           ,CONVERT(INT,@DATA_09)
                           ,CONVERT(INT,@DATA_10)
                           ,CONVERT(INT,@DATA_11)
                           ,CONVERT(INT,@DATA_12)
                           ,CONVERT(INT,@DATA_13)
                           ,CONVERT(INT,@DATA_14)
                           ,CONVERT(INT,@DATA_15)
                           ,CONVERT(INT,@DATA_16)
                           ,CONVERT(INT,@DATA_17)
                           ,CONVERT(INT,@DATA_18)
                           ,CONVERT(INT,@DATA_19)
                           ,CONVERT(INT,@DATA_20)
                           ,CONVERT(INT,@DATA_21)
                           ,CONVERT(INT,@DATA_22)
                           ,CONVERT(INT,@DATA_23)
                           ,CONVERT(INT,@DATA_24)
                           ,CONVERT(INT,@DATA_25)
                           ,CONVERT(INT,@DATA_26)
                           ,'SYSTEM'
                           ,GETDATE()
                         );
                IF  @@ERROR = 0
                    SET @INSERT_CNT = @INSERT_CNT + 1;
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





























