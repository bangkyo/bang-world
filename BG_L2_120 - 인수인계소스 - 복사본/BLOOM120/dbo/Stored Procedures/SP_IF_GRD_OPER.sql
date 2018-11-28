CREATE PROCEDURE [dbo].[SP_IF_GRD_OPER]
 @P_IF_SEQ    NUMERIC(12)
,@P_TC_CD     VARCHAR(4)  = 'F010'
,@P_PROC_STAT VARCHAR(3)     OUTPUT
,@P_PROC_MSG  VARCHAR(1000)  OUTPUT
AS
--------------------------------------------------------
---- 입력변수 : IF_SEQ, TC_CD
---- 출력변수 : 처리상태(OK,ERR), 처리결과MSG
--------------------------------------------------------
SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_IF_GRD_OPER';
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

DECLARE @ZONE_CD        VARCHAR(3) = 'Z71';
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

BEGIN
    SET  @P_PROC_STAT = 'OK';
    SET  @P_PROC_MSG  = '정상적으로 처리되었습니다. ';

    BEGIN TRY
        
        BEGIN TRAN

        SET @CUR_DATE = CONVERT(VARCHAR(8),GETDATE()-6/24,112);
        IF  @P_TC_CD  <> 'F010'
            BEGIN
                SET  @P_PROC_STAT  = 'ERR';
                SET  @P_PROC_MSG   = 'TC CD ERROR : ' + @P_TC_CD;
                RAISERROR('TC CD ERROR ', 16, 1);
            END;
            
            BEGIN
                ----I/F받은 QR정보 select --드라이브 모드(전체 1(표면+코너),표면2,코너3,부분4)에 따라 실적값 변경 등록.(공통코드참조)
                SELECT TOP 1
                        @REG_DDTT      = REG_DDTT
                       ,@DATA_01       = RTRIM(DATA_01)
                       ,@DATA_02       = RTRIM(DATA_02)
                       ,@DATA_03       = RTRIM(DATA_03)
                       ,@DATA_04       = RTRIM(DATA_04)
                       ,@DATA_05       = RTRIM(DATA_05)
                       ,@DATA_06       = RTRIM(DATA_06)
                       ,@DATA_07       = CASE RTRIM(DATA_01) WHEN '3'  THEN  '0'      -- 코너
															 WHEN '4'  THEN  '0'      -- 부분    
								                             ELSE RTRIM(DATA_07) END 
                       ,@DATA_08       = CASE RTRIM(DATA_01) WHEN '2'  THEN  '0'      -- 면
															 WHEN '4'  THEN  '0'      -- 부분    
								                             ELSE RTRIM(DATA_08) END 
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
                       ,@DATA_27       = RTRIM(DATA_27)
                       ,@DATA_28       = RTRIM(DATA_28)
                       ,@DATA_29       = RTRIM(DATA_29)
                       ,@DATA_30       = RTRIM(DATA_30)
                       ,@DATA_31       = RTRIM(DATA_31)
                       ,@DATA_32       = RTRIM(DATA_32)
                       ,@DATA_33       = RTRIM(DATA_33)
                       ,@DATA_34       = RTRIM(DATA_34)
                       ,@DATA_35       = RTRIM(DATA_35)
                       ,@DATA_36       = RTRIM(DATA_36)
                       ,@DATA_37       = RTRIM(DATA_37)
                       ,@DATA_38       = RTRIM(DATA_38)
                       ,@DATA_39       = RTRIM(DATA_39)
                       ,@DATA_40       = RTRIM(DATA_40)
                       ,@DATA_41       = RTRIM(DATA_41)
                       ,@DATA_42       = RTRIM(DATA_42)
                       --,@DATA_43       = RTRIM(DATA_43) --2018-09-07 박무진 의미없는 값이 입력됨.
                FROM   TB_L1_RECEIVE
                WHERE  IF_SEQ       = @P_IF_SEQ;
                IF  @@ROWCOUNT = 0
                    BEGIN
                        SET  @P_PROC_STAT  = 'ERR';
                        SET  @P_PROC_MSG   = CONVERT(VARCHAR(10),@P_IF_SEQ) + '에 작업할 정보가 없습니다. ';
                        RAISERROR(@P_PROC_MSG, 16, 1);
                    END;
                ---- 조업정보 INSERT
                INSERT INTO [dbo].[TB_GRD_OPER_INFO]
                           ([WORK_DDTT]
                           ,[GRD_DRV_MODE]
                           ,[HEIGHT_SETV]
                           ,[WIDTH_SETV]
                           ,[HEIGHT_MV]
                           ,[WIDTH_MV]
                           ,[LENGTH_MV]
                           ,[FACE_PASS_CNT]
                           ,[CORN_PASS_CNT]
                           ,[WORK_SEQ]
                           ,[WORK_Track_NO]
                           ,[RBGW_SPEED_SETV]
                           ,[RBGW_ADV_SETV]
                           ,[RBGW_DIA_MV]
                           ,[RBGW_ENGY_MV]
                           ,[RBGW_VIBR_MV]
                           ,[ROTANGLE_SETV]
                           ,[ROTANGLE_MV]
                           ,[GRD_PRES_SURFACE_SETV]
                           ,[GRD_PRES_ENDP_SETV]
                           ,[GRD_PRES_CORN_SETV]
                           ,[SET_GRD_Force]
                           ,[GRD_PRES_MV]
                           ,[GRD_MTR_ELEC]
                           ,[TRUCKSPEED_SETV]
                           ,[TRUCKSPEED_MV]
                           ,[GRD_LOC]
                           ,[COOL_DOWN_HR_SETV]
                           ,[COOL_DOWN_SPEED_SETV]
                           ,[BELT_SLIP_WARN_SETV]
                           ,[CORN_GRD_3TRK1bs]
                           ,[CORN_GRD_3TRK2bs]
                           ,[CORN_GRD_3TRK3fs]
                           ,[CORN_GRD_5TRK1bs]
                           ,[CORN_GRD_5TRK2bs]
                           ,[CORN_GRD_5TRK3bs]
                           ,[CORN_GRD_5TRK4fs]
                           ,[CORN_GRD_5TRK5fs]
                           ,[OutsIDe_back]
                           ,[OutsIDe_front]
                           ,[WORK_SETV_Top]
                           ,[WORK_SETV_Right]
                           ,[WORK_SETV_Bottom]
                           ,[WORK_SETV_Left]
                           ,[REGISTER]
                           ,[REG_DDTT]
                          ) VALUES ( 
                            CONVERT(datetime,@REG_DDTT)
                            ,convert(int,@DATA_01)           --<GRD_DRV_MODE, int,>
                            ,convert(int,@DATA_02)           --<HEIGHT_SETV, int,>
                            ,convert(int,@DATA_03)           --<WIDTH_SETV, int,>
                            ,convert(int,@DATA_04)           --<HEIGHT_MV, int,>
                            ,convert(int,@DATA_05)           --<WIDTH_MV, int,>
                            ,convert(int,@DATA_06)           --<LENGTH_MV, int,>
                            ,convert(int,@DATA_07)           --<FACE_PASS_CNT, int,>
                            ,convert(int,@DATA_08)           --<CORN_PASS_CNT, int,>
                            ,convert(int,@DATA_09)           --<WORK_SEQ, int,>
                            ,substring(@DATA_10,1,10)        --<WORK_Track_NO, varchar(10),>
                            ,convert(int,@DATA_11)           --<RBGW_SPEED_SETV, int,>
                            ,convert(int,@DATA_12)           --<RBGW_ADV_SETV, int,>
                            ,convert(int,@DATA_13)           --<RBGW_DIA_MV, int,>
                            ,convert(int,@DATA_14)           --<RBGW_ENGY_MV, int,>
                            ,convert(int,@DATA_15)           --<RBGW_VIBR_MV, int,>
                            ,convert(int,@DATA_16)           --<ROTANGLE_SETV, int,>
                            ,convert(int,@DATA_17)           --<ROTANGLE_MV, int,>
                            ,convert(int,@DATA_18)           --<GRD_PRES_SURFACE_SETV, int,>
                            ,convert(int,@DATA_19)           --<GRD_PRES_ENDP_SETV, int,>
                            ,convert(int,@DATA_20)           --<GRD_PRES_CORN_SETV, int,>
                            ,convert(int,@DATA_21)           --<SET_GRD_Force, integer>
                            ,convert(int,@DATA_22)           --<GRD_PRES_MV, int,>
                            ,convert(int,@DATA_14)           --<GRD_MTR_ELEC, integer> DATA_14 -- 2018-09-03 박도우 대리: RBGW_ENGY_MV(DATA_14) 값과 동일
                            ,convert(int,@DATA_23)           --<TRUCKSPEED_SETV, int,>
                            ,convert(int,@DATA_24)           --<TRUCKSPEED_MV, int,>
                            ,convert(int,@DATA_25)           --<GRD_LOC, varchar(10),>
                            ,substring(@DATA_26,1,10)        --<COOL_DOWN_HR_SETV, int,>
                            ,convert(int,@DATA_27)           --<COOL_DOWN_SPEED_SETV, int,>
                            ,convert(int,@DATA_28)           --<BELT_SLIP_WARN_SETV, int,>
                            ,convert(int,@DATA_29)           --<CORN_GRD_3TRK1bs, int,>
                            ,convert(int,@DATA_30)           --<CORN_GRD_3TRK2bs, int,>
                            ,convert(int,@DATA_31)           --<CORN_GRD_3TRK3fs, int,>
                            ,convert(int,@DATA_32)           --<CORN_GRD_5TRK1bs, int,>
                            ,convert(int,@DATA_33)           --<CORN_GRD_5TRK2bs, int,>
                            ,convert(int,@DATA_34)           --<CORN_GRD_5TRK3bs, int,>
                            ,convert(int,@DATA_35)           --<CORN_GRD_5TRK4fs, int,>
                            ,convert(int,@DATA_36)           --<CORN_GRD_5TRK5fs, int,>
                            ,convert(int,@DATA_37)           --<OutsIDe_back, int,>
                            ,convert(int,@DATA_38)           --<OutsIDe_front, int,>
                            ,substring(@DATA_39,1,5)         --<WORK_SETV_Top, varchar(5),>
                            ,substring(@DATA_40,1,5)         --<WORK_SETV_Right, varchar(5),>
                            ,substring(@DATA_41,1,5)         --<WORK_SETV_Bottom, varchar(5),>
                            ,substring(@DATA_42,1,5)         --<WORK_SETV_Left, varchar(5),>
                           ,'SYSTEM'                         --<REGISTER, varchar(20),>
                           ,GETDATE()                        --<REG_DDTT, datetime,>
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
            SET  @P_PROC_STAT  = 'ERR';
            SET  @P_PROC_MSG   = @ERR_MSG;
        END
    END CATCH
    
END





























