CREATE PROCEDURE [dbo].[SP_IF_SHT_OPER]
 @P_IF_SEQ    NUMERIC(12)
,@P_TC_CD     VARCHAR(4)  = 'B010'
,@P_PROC_STAT VARCHAR(3)     OUTPUT
,@P_PROC_MSG  VARCHAR(1000)  OUTPUT
AS
--------------------------------------------------------
---- 입력변수 : IF_SEQ, TC_CD
---- 출력변수 : 처리상태(OK,ERR), 처리결과MSG
--------------------------------------------------------
SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_IF_SHT_OPER';
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

BEGIN
    SET  @P_PROC_STAT = 'OK';
    SET  @P_PROC_MSG  = '정상적으로 처리되었습니다. ';

    BEGIN TRY
        
        BEGIN TRAN
        SET @CUR_DATE = CONVERT(VARCHAR(8),GETDATE()-6/24,112);
        IF  @P_TC_CD  <> 'B010'
            BEGIN
                SET  @P_PROC_STAT  = 'ERR';
                SET  @P_PROC_MSG   = 'TC CD ERROR : ' + @P_TC_CD;
                RAISERROR('TC CD ERROR ', 16, 1);
            END;
            
            BEGIN
                ----I/F받은 QR정보 select 
                SELECT TOP 1
                        @REG_DDTT      = REG_DDTT
                       ,@DATA_01       = CASE WHEN ISNUMERIC(DATA_01) = 0 THEN '0' ELSE DATA_01 END
                       ,@DATA_02       = CASE WHEN ISNUMERIC(DATA_02) = 0 THEN '0' ELSE DATA_02	END
                       ,@DATA_03       = CASE WHEN ISNUMERIC(DATA_03) = 0 THEN '0' ELSE DATA_03	END
                       ,@DATA_04       = CASE WHEN ISNUMERIC(DATA_04) = 0 THEN '0' ELSE DATA_04	END
                       ,@DATA_05       = CASE WHEN ISNUMERIC(DATA_05) = 0 THEN '0' ELSE DATA_05	END
                       ,@DATA_06       = CASE WHEN ISNUMERIC(DATA_06) = 0 THEN '0' ELSE DATA_06	END
                       ,@DATA_07       = CASE WHEN ISNUMERIC(DATA_07) = 0 THEN '0' ELSE DATA_07	END
                       ,@DATA_08       = CASE WHEN ISNUMERIC(DATA_08) = 0 THEN '0' ELSE DATA_08	END
                       ,@DATA_09       = CASE WHEN ISNUMERIC(DATA_09) = 0 THEN '0' ELSE DATA_09	END
                       ,@DATA_10       = CASE WHEN ISNUMERIC(DATA_10) = 0 THEN '0' ELSE DATA_10	END
                       ,@DATA_11       = CASE WHEN ISNUMERIC(DATA_11) = 0 THEN '0' ELSE DATA_11	END
                       ,@DATA_12       = CASE WHEN ISNUMERIC(DATA_12) = 0 THEN '0' ELSE DATA_12	END
                       ,@DATA_13       = CASE WHEN ISNUMERIC(DATA_13) = 0 THEN '0' ELSE DATA_13	END
                       ,@DATA_14       = CASE WHEN ISNUMERIC(DATA_14) = 0 THEN '0' ELSE DATA_14	END
                       ,@DATA_15       = CASE WHEN ISNUMERIC(DATA_15) = 0 THEN '0' ELSE DATA_15	END
                       ,@DATA_16       = CASE WHEN ISNUMERIC(DATA_16) = 0 THEN '0' ELSE DATA_16	END
                       ,@DATA_17       = CASE WHEN ISNUMERIC(DATA_17) = 0 THEN '0' ELSE DATA_17	END
                       ,@DATA_18       = CASE WHEN ISNUMERIC(DATA_18) = 0 THEN '0' ELSE DATA_18	END
                       ,@DATA_19       = CASE WHEN ISNUMERIC(DATA_19) = 0 THEN '0' ELSE DATA_19	END
                       ,@DATA_20       = CASE WHEN ISNUMERIC(DATA_20) = 0 THEN '0' ELSE DATA_20	END
                FROM   TB_L1_RECEIVE
                WHERE  IF_SEQ       = @P_IF_SEQ;
                IF  @@ROWCOUNT = 0
                    BEGIN
                        SET  @P_PROC_STAT  = 'ERR';
                        SET  @P_PROC_MSG   = CONVERT(VARCHAR(10),@P_IF_SEQ) + '에 작업할 정보가 없습니다. ';
                        RAISERROR(@P_PROC_MSG, 16, 1);
                    END;
                ---- 조업정보 INSERT
                INSERT INTO [dbo].[TB_SHT_OPER_INFO]
                           ([WORK_DDTT]
                           ,[ROLLER_CONVR_SPEED]
                           ,[IMPELLER_NO1_SPEED]
                           ,[IMPELLER_NO2_SPEED]
                           ,[IMPELLER_NO3_SPEED]
                           ,[IMPELLER_NO4_SPEED]
                           ,[ROLLER_CONVR_CURRENT]
                           ,[IMPELLER_NO1_CURRENT]
                           ,[IMPELLER_NO2_CURRENT]
                           ,[IMPELLER_NO3_CURRENT]
                           ,[IMPELLER_NO4_CURRENT]
                           ,[ROTY_SCR_CURRENT]
                           ,[BCKT_LVTR_CURRENT]
                           ,[SCREW_CONVR1_CURRENT]
                           ,[SCREW_CONVR2_CURRENT]
                           ,[FAN_CURRENT]
                           ,[ROLLER_CONVR_SPEED_SETV]
                           ,[IMPELLER_NO1_SPEED_SETV]
                           ,[IMPELLER_NO2_SPEED_SETV]
                           ,[IMPELLER_NO3_SPEED_SETV]
                           ,[IMPELLER_NO4_SPEED_SETV]
                           ,[REGISTER]
                           ,[REG_DDTT]
                        )  VALUES  (
                            CONVERT(datetime,@REG_DDTT)
                           ,CONVERT(numeric(5),@DATA_01)
                           ,CONVERT(NUMERIC(5),@DATA_02)
                           ,CONVERT(NUMERIC(5),@DATA_03)
                           ,CONVERT(NUMERIC(5),@DATA_04)
                           ,CONVERT(NUMERIC(5),@DATA_05)
                           ,CONVERT(NUMERIC(5),@DATA_06)
                           ,CONVERT(NUMERIC(5),@DATA_07)
                           ,CONVERT(NUMERIC(5),@DATA_08)
                           ,CONVERT(NUMERIC(5),@DATA_09)
                           ,CONVERT(NUMERIC(5),@DATA_10)
                           ,CONVERT(NUMERIC(5),@DATA_11)
                           ,CONVERT(NUMERIC(5),@DATA_12)
                           ,CONVERT(NUMERIC(5),@DATA_13)
                           ,CONVERT(NUMERIC(5),@DATA_14)
                           ,CONVERT(NUMERIC(5),@DATA_15)
                           ,CONVERT(NUMERIC(5),@DATA_16)
                           ,CONVERT(NUMERIC(5),@DATA_17)
                           ,CONVERT(NUMERIC(5),@DATA_18)
                           ,CONVERT(NUMERIC(5),@DATA_19)
                           ,CONVERT(NUMERIC(5),@DATA_20)
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

        ROLLBACK TRAN; -- 실패!
 
        -- 해당 시스템 함수는 반드시 CATCH문에서 사용해야한다. 
        BEGIN
            SET  @ERR_LINE    = ERROR_LINE();
            SET  @ERR_NUMBER  = ERROR_NUMBER();
            SET  @ERR_MSG     = SUBSTRING(ERROR_MESSAGE(),1,200);
            SELECT CONVERT(VARCHAR(5),@ERR_LINE) + '-' +  @ERR_MSG;
            SET  @ERR_TP  = 'ERR';
            SET  @ERR_CD  = CONVERT(VARCHAR(10),@ERR_NUMBER); 
            EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @ERR_MSG;
            SET  @P_PROC_STAT  = 'ERR';
            SET  @P_PROC_MSG   = CONVERT(VARCHAR(10),@P_IF_SEQ) + '_' + @ERR_MSG;
        END
    END CATCH
    
END





























