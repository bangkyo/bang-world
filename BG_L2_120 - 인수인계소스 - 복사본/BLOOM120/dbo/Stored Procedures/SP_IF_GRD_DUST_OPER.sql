CREATE PROCEDURE [dbo].[SP_IF_GRD_DUST_OPER]
 @P_IF_SEQ    NUMERIC(12)
,@P_TC_CD     VARCHAR(4)  = 'K030'
,@P_PROC_STAT VARCHAR(3)     OUTPUT
,@P_PROC_MSG  VARCHAR(1000)  OUTPUT
AS
--------------------------------------------------------
---- 입력변수 : IF_SEQ, TC_CD
---- 출력변수 : 처리상태(OK,ERR), 처리결과MSG
--------------------------------------------------------
SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_IF_GRD_DUST_OPER';
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
DECLARE @DATA_28        VARCHAR(30);
DECLARE @DATA_29        VARCHAR(30);
DECLARE @DATA_30        VARCHAR(30);
DECLARE @DATA_31        VARCHAR(30);
DECLARE @DATA_32        VARCHAR(30);
DECLARE @DATA_33        VARCHAR(30);
DECLARE @DATA_34        VARCHAR(30);
DECLARE @DATA_35        VARCHAR(30);
DECLARE @DATA_36        VARCHAR(30);
DECLARE @DATA_37        VARCHAR(30);
DECLARE @DATA_38        VARCHAR(30);
DECLARE @DATA_39        VARCHAR(30);
DECLARE @DATA_40        VARCHAR(30);
DECLARE @DATA_41        VARCHAR(30);
DECLARE @DATA_42        VARCHAR(30);
DECLARE @DATA_43        VARCHAR(30);
DECLARE @DATA_44        VARCHAR(30);
DECLARE @DATA_45        VARCHAR(30);
DECLARE @DATA_46        VARCHAR(30);
DECLARE @DATA_47        VARCHAR(30);
DECLARE @DATA_48        VARCHAR(30);


DECLARE @GRD_DUST_STAT      VARCHAR(10);
DECLARE @GRD_DUST_ON_DDTT   DATETIME;
DECLARE @GRD_DUST_OFF_DDTT  DATETIME;

BEGIN
    SET  @P_PROC_STAT = 'OK';
    SET  @P_PROC_MSG  = '정상적으로 처리되었습니다. ';

    BEGIN TRY
        
        SET @CUR_DATE = CONVERT(VARCHAR(8),GETDATE()-6/24,112);
        BEGIN TRAN
            IF  @P_TC_CD  <> 'K030'
                BEGIN
                    SET  @P_PROC_STAT  = 'ERR';
                    SET  @P_PROC_MSG   = 'TC CD ERROR : ' + @P_TC_CD;
                    RAISERROR('TC CD ERROR ', 16, 1);
                END;
            
            BEGIN
                ----I/F받은 QR정보 select 
                SELECT TOP 1
                        @REG_DDTT      = REG_DDTT
                       ,@DATA_01       = CASE WHEN ISNUMERIC(DATA_01) = 0 THEN '0' ELSE DATA_01	END
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
                       ,@DATA_21       = CASE WHEN ISNUMERIC(DATA_21) = 0 THEN '0' ELSE DATA_21	END
                       ,@DATA_22       = CASE WHEN ISNUMERIC(DATA_22) = 0 THEN '0' ELSE DATA_22	END
                       ,@DATA_23       = CASE WHEN ISNUMERIC(DATA_23) = 0 THEN '0' ELSE DATA_23	END
					   ,@DATA_24       = CASE WHEN ISNUMERIC(DATA_24) = 0 THEN '0' ELSE DATA_24 END
                       ,@DATA_25       = CASE WHEN ISNUMERIC(DATA_25) = 0 THEN '0' ELSE DATA_25 END
                       ,@DATA_26       = CASE WHEN ISNUMERIC(DATA_26) = 0 THEN '0' ELSE DATA_26 END
                       ,@DATA_27       = CASE WHEN ISNUMERIC(DATA_27) = 0 THEN '0' ELSE DATA_27 END
                       ,@DATA_28       = CASE WHEN ISNUMERIC(DATA_28) = 0 THEN '0' ELSE DATA_28 END
                       ,@DATA_29       = CASE WHEN ISNUMERIC(DATA_29) = 0 THEN '0' ELSE DATA_29 END
                       ,@DATA_30       = CASE WHEN ISNUMERIC(DATA_30) = 0 THEN '0' ELSE DATA_30 END
                       ,@DATA_31       = CASE WHEN ISNUMERIC(DATA_31) = 0 THEN '0' ELSE DATA_31 END
                       ,@DATA_32       = CASE WHEN ISNUMERIC(DATA_32) = 0 THEN '0' ELSE DATA_32 END
                       ,@DATA_33       = CASE WHEN ISNUMERIC(DATA_33) = 0 THEN '0' ELSE DATA_33 END
                       ,@DATA_34       = CASE WHEN ISNUMERIC(DATA_34) = 0 THEN '0' ELSE DATA_34 END
                       ,@DATA_35       = CASE WHEN ISNUMERIC(DATA_35) = 0 THEN '0' ELSE DATA_35 END
                       ,@DATA_36       = CASE WHEN ISNUMERIC(DATA_36) = 0 THEN '0' ELSE DATA_36 END
                       ,@DATA_37       = CASE WHEN ISNUMERIC(DATA_37) = 0 THEN '0' ELSE DATA_37 END
                       ,@DATA_38       = CASE WHEN ISNUMERIC(DATA_38) = 0 THEN '0' ELSE DATA_38 END
                       ,@DATA_39       = CASE WHEN ISNUMERIC(DATA_39) = 0 THEN '0' ELSE DATA_39 END
                       ,@DATA_40       = CASE WHEN ISNUMERIC(DATA_40) = 0 THEN '0' ELSE DATA_40 END
                       ,@DATA_41       = CASE WHEN ISNUMERIC(DATA_41) = 0 THEN '0' ELSE DATA_41 END
                       ,@DATA_42       = CASE WHEN ISNUMERIC(DATA_42) = 0 THEN '0' ELSE DATA_42 END
                       ,@DATA_43       = CASE WHEN ISNUMERIC(DATA_43) = 0 THEN '0' ELSE DATA_43 END
                       ,@DATA_44       = CASE WHEN ISNUMERIC(DATA_44) = 0 THEN '0' ELSE DATA_44 END
                       ,@DATA_45       = CASE WHEN ISNUMERIC(DATA_45) = 0 THEN '0' ELSE DATA_45 END
                       ,@DATA_46       = CASE WHEN ISNUMERIC(DATA_46) = 0 THEN '0' ELSE DATA_46 END
                       ,@DATA_47       = CASE WHEN ISNUMERIC(DATA_47) = 0 THEN '0' ELSE DATA_47 END
                       ,@DATA_48       = CASE WHEN ISNUMERIC(DATA_48) = 0 THEN '0' ELSE DATA_48 END

                FROM   TB_L1_RECEIVE
                WHERE  IF_SEQ       = @P_IF_SEQ;
                IF  @@ROWCOUNT = 0
                    BEGIN
                        SET  @P_PROC_STAT  = 'ERR';
                        SET  @P_PROC_MSG   = CONVERT(VARCHAR(10),@P_IF_SEQ) + '에 작업할 정보가 없습니다. ';
                        RAISERROR(@P_PROC_MSG, 16, 1);
                    END;
                ----집진정보관리의 ON상태 CHECK
                SELECT  @GRD_DUST_STAT      = ISNULL(GRD_DUST_STAT,'1')
                       ,@GRD_DUST_ON_DDTT   = GRD_DUST_ON_DDTT 
                       ,@GRD_DUST_OFF_DDTT  = GRD_DUST_OFF_DDTT
                FROM   TB_DUST_STAT_MGMT;
                IF  @@ROWCOUNT = 0
                    BEGIN
                        INSERT INTO TB_DUST_STAT_MGMT 
                               ( GRD_DUST_STAT, GRD_DUST_OFF_DDTT )
                        VALUES ( 'OFF', @REG_DDTT );
                    END;
                ELSE
                    --IF  @GRD_DUST_STAT = 'ON'
                        BEGIN
							INSERT INTO [dbo].[TB_GRD_DUST_INFO]
									   ([WORK_DDTT]
									   ,[DUST_On_TR]
									   ,[Bag_BEF_STATICP]
									   ,[INH_TEMP]
									   ,[Rotary_Valve_NO1_STAT]
									   ,[Rotary_Valve_NO1_ABNORM_STAT]
									   ,[Rotary_Valve_NO2_STAT]
									   ,[Rotary_Valve_NO2_ABNORM_STAT]
									   ,[Rotary_Valve_NO3_STAT]
									   ,[Rotary_Valve_NO3_ABNORM_STAT]
									   ,[Rotary_Valve_NO4_STAT]
									   ,[Rotary_Valve_NO4_ABNORM_STAT]
									   ,[Vibrator_NO1_STAT]
									   ,[Vibrator_NO1_ABNORM_STAT]
									   ,[Vibrator_NO2_STAT]
									   ,[Vibrator_NO2_ABNORM_STAT]
									   ,[Vibrator_NO3_STAT]
									   ,[Vibrator_NO3_ABNORM_STAT]
									   ,[Pulse_Controller_STAT1]
									   ,[Pulse_Controller_STAT2]
									   ,[Pulse_Controller_STAT3]
									   ,[Pulse_Controller_STAT4]
									   ,[Pulse_Controller_STAT5]
									   ,[Pulse_Controller_STAT6]
									   ,[Pulse_Controller_STAT7]
									   ,[Pulse_Controller_STAT8]
									   ,[Pulse_Controller_STAT9]
									   ,[Pulse_Controller_STAT10]
									   ,[Pulse_Controller_STAT11]
									   ,[Pulse_Controller_STAT12]
									   ,[Pulse_Controller_STAT13]
									   ,[Pulse_Controller_STAT14]
									   ,[Pulse_Controller_STAT15]
									   ,[Pulse_Controller_STAT16]
									   ,[Pulse_Controller_STAT17]
									   ,[Pulse_Controller_STAT18]
									   ,[Pulse_Controller_STAT19]
									   ,[Popet_Damper_NO1_STAT]
									   ,[Popet_Damper_NO2_STAT]
									   ,[Popet_Damper_NO3_STAT]
									   ,[DEF_PRES_NO1]
									   ,[DEF_PRES_NO2]
									   ,[DEF_PRES_NO3]
									   ,[OPRN_RT]
									   ,[SETV]
									   ,[LOAD_BRG_TEMP]
									   ,[H_LOAD_BRG_TEMP]
									   ,[IVT_SPEED_SETV]
									   ,[LOAD_VIBR]
									   ,[H_LOAD_VIBR]
									   ,[REGISTER]
									   ,[REG_DDTT])
								 VALUES
									   (@REG_DDTT                               
                                       ,@GRD_DUST_ON_DDTT
                                       ,CONVERT(NUMERIC(7,2),@DATA_01)   
                                       ,CONVERT(NUMERIC(6,1),@DATA_02)  
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
									   ,CONVERT(INT,@DATA_27)
									   ,CONVERT(INT,@DATA_28)
									   ,CONVERT(INT,@DATA_29)
									   ,CONVERT(INT,@DATA_30)
									   ,CONVERT(INT,@DATA_31)
									   ,CONVERT(INT,@DATA_32)
									   ,CONVERT(INT,@DATA_33)
									   ,CONVERT(INT,@DATA_34)
									   ,CONVERT(INT,@DATA_35)
									   ,CONVERT(NUMERIC(6,1),@DATA_36)
                                       ,CONVERT(NUMERIC(6,1),@DATA_37)
                                       ,CONVERT(NUMERIC(6,1),@DATA_38)
                                       ,CONVERT(NUMERIC(6,1),@DATA_39)
                                       ,CONVERT(NUMERIC(6,1),@DATA_40)
                                       ,CONVERT(NUMERIC(6,1),@DATA_41)
                                       ,CONVERT(NUMERIC(6,1),@DATA_42)
                                       ,CONVERT(NUMERIC(6,1),@DATA_43)
                                       ,CONVERT(NUMERIC(6,1),@DATA_44)
                                       ,CONVERT(NUMERIC(6,1),@DATA_45)
                                       ,CONVERT(NUMERIC(6,1),@DATA_46)
                                       ,CONVERT(NUMERIC(6,1),@DATA_47)
                                       ,CONVERT(NUMERIC(6,1),@DATA_48)
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
            SET  @P_PROC_STAT  = 'ERR';
            SET  @P_PROC_MSG   = @ERR_MSG;
        END
    END CATCH
    
END





























