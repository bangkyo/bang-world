CREATE PROCEDURE [dbo].[SP_IF_GRD_END]
 @P_IF_SEQ    NUMERIC(12)
,@P_TC_CD     VARCHAR(4)  = 'F050'
,@P_PROC_STAT VARCHAR(3)     OUTPUT
,@P_PROC_MSG  VARCHAR(1000)  OUTPUT
AS
--------------------------------------------------------
---- 그라인딩 종료( 실적발생 )
---- 입력변수 : IF_SEQ, TC_CD
---- 출력변수 : 처리상태(OK,ERR), 처리결과MSG
--------------------------------------------------------
SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_IF_GRD_END';
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

DECLARE @ZONE_CD        VARCHAR(3) = 'Z80';
DECLARE @BEF_ZONE_CD    VARCHAR(3) = 'Z71';
DECLARE @S_HEAT_NO      VARCHAR(6) = '';
DECLARE @S_HEAT_SEQ     INT        = 0;
DECLARE @S_REWORK       VARCHAR(1) = 'N';

DECLARE @REG_DDTT       VARCHAR(17);
DECLARE @DATA_01        VARCHAR(30);
DECLARE @DATA_02        VARCHAR(30);
DECLARE @START_DDTT     DATETIME;
DECLARE @END_DDTT       DATETIME;

DECLARE @GRD_DRV_MODE          int;
DECLARE @WORK_SETV_Top         varchar(5);
DECLARE @WORK_SETV_Right       varchar(5);
DECLARE @WORK_SETV_Bottom      varchar(5);
DECLARE @WORK_SETV_Left        varchar(5);
DECLARE @HEIGHT_MV             int;
DECLARE @WIDTH_MV              int;
DECLARE @LENGTH_MV             int;
DECLARE @FACE_PASS_CNT         int;
DECLARE @CORN_PASS_CNT         int;
DECLARE @RBGW_SPEED_SETV       int;
DECLARE @RBGW_ADV_SETV         int;
DECLARE @RBGW_DIA_MV           int;
DECLARE @RBGW_ENGY_MV          int;
DECLARE @RBGW_VIBR_MV          int;
DECLARE @ROTANGLE_SETV         int;
DECLARE @ROTANGLE_MV           int;
DECLARE @GRD_PRES_SURFACE_SETV int;
DECLARE @GRD_PRES_ENDP_SETV    int;
DECLARE @GRD_PRES_CORN_SETV    int;
DECLARE @GRD_PRES_MV           int;
DECLARE @TRUCKSPEED_SETV       int;
DECLARE @TRUCKSPEED_MV         int;
DECLARE @S_EQP_CD              VARCHAR(10);
DECLARE @S_ITEM_CD             VARCHAR(10);
DECLARE @S_CHECK_SEQ           int;
DECLARE @S_USECNT              int; 

BEGIN
    SET  @P_PROC_STAT = 'OK';
    SET  @P_PROC_MSG  = '정상적으로 처리되었습니다. ';

    BEGIN TRY
        
        SET @CUR_DATE = CONVERT(VARCHAR(8),GETDATE()-6/24,112);
        IF  @P_TC_CD  <> 'F050'
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
                        @S_HEAT_NO       = HEAT_NO
                       ,@S_HEAT_SEQ      = HEAT_SEQ
					   ,@S_REWORK        = REWORK_YN
                FROM   TB_RL_TM_TRACKING
                WHERE  ZONE_CD       = @BEF_ZONE_CD
                AND    PROG_STAT     = 'RUN'
                ORDER BY HEAT_NO, WORK_SEQ;
                IF  @@ROWCOUNT = 0
                    BEGIN
                        SET  @P_PROC_STAT  = 'ERR';
                        SET  @P_PROC_MSG   = @BEF_ZONE_CD + '에 작업할 정보가 없습니다. ';
                        RAISERROR(@P_PROC_MSG, 16, 1);
                    END;
                ----이동할 위치에 진행중인 BLOOM이 있으면 오류처리
                SELECT @SELECT_CNT = COUNT(*)
                FROM   TB_RL_TM_TRACKING
                WHERE  ZONE_CD       = @ZONE_CD
                AND    PROG_STAT     = 'RUN';
                IF  @SELECT_CNT > 0
                    BEGIN
                        SET  @P_PROC_STAT  = 'ERR';
                        SET  @P_PROC_MSG   = @ZONE_CD + '에 작업중인 정보가 있습니다. ';
                        EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @P_PROC_STAT, @P_PROC_MSG;
                        RAISERROR('작업중인 정보가 없습니다.', 16, 1);
                    END;
                ----위치 이동 UPDATE
                UPDATE  TB_RL_TM_TRACKING
                SET     ZONE_CD           = @ZONE_CD
                       ,ZONE_CD_UPD_DDTT  = GETDATE()
                       ,BEF_ZONE_CD       = ZONE_CD
                       ,BEF_ZONE_UPD_DDTT = ZONE_CD_UPD_DDTT
                       ,PROG_STAT         = 'RUN'
                       ,BEF_PROG_STAT     = PROG_STAT
                       ,GRD_END_DDTT      = CONVERT(DATETIME,@REG_DDTT)
                WHERE  HEAT_NO    = @S_HEAT_NO
                AND    HEAT_SEQ   = @S_HEAT_SEQ;
                IF  @@ERROR = 0
                    SET @UPDATE_CNT = @UPDATE_CNT + 1;
                ----조업정보를 위해 시작,종료일시를 읽어 온다.
                SELECT  @START_DDTT = GRD_START_DDTT
                       ,@END_DDTT   = GRD_END_DDTT
                FROM   TB_RL_TM_TRACKING
                WHERE  HEAT_NO    = @S_HEAT_NO
                AND    HEAT_SEQ   = @S_HEAT_SEQ;
                ----조업정보 평균값 SELECT
                SELECT  @GRD_DRV_MODE           = AVG(GRD_DRV_MODE         )
                       ,@WORK_SETV_Top          = MAX(WORK_SETV_Top        )
                       ,@WORK_SETV_Right        = MAX(WORK_SETV_Right      )
                       ,@WORK_SETV_Bottom       = MAX(WORK_SETV_Bottom     )
                       ,@WORK_SETV_Left         = MAX(WORK_SETV_Left       )
                       ,@HEIGHT_MV              = AVG(HEIGHT_MV            )
                       ,@WIDTH_MV               = AVG(WIDTH_MV             )
                       ,@LENGTH_MV              = AVG(LENGTH_MV            )
                       ,@FACE_PASS_CNT          = AVG(FACE_PASS_CNT        )
                       ,@CORN_PASS_CNT          = AVG(CORN_PASS_CNT        )
                       ,@RBGW_SPEED_SETV        = AVG(RBGW_SPEED_SETV      )
                       ,@RBGW_ADV_SETV          = AVG(RBGW_ADV_SETV        )
                       ,@RBGW_DIA_MV            = AVG(RBGW_DIA_MV          )
                       ,@RBGW_ENGY_MV           = AVG(RBGW_ENGY_MV         )
                       ,@RBGW_VIBR_MV           = AVG(RBGW_VIBR_MV         )
                       ,@ROTANGLE_SETV          = AVG(ROTANGLE_SETV        )
                       ,@ROTANGLE_MV            = AVG(ROTANGLE_MV          )
                       ,@GRD_PRES_SURFACE_SETV  = AVG(GRD_PRES_SURFACE_SETV)
                       ,@GRD_PRES_ENDP_SETV     = AVG(GRD_PRES_ENDP_SETV   )
                       ,@GRD_PRES_CORN_SETV     = AVG(GRD_PRES_CORN_SETV   )
                       ,@GRD_PRES_MV            = AVG(GRD_PRES_MV          )
                       ,@TRUCKSPEED_SETV        = AVG(TRUCKSPEED_SETV      )
                       ,@TRUCKSPEED_MV          = AVG(TRUCKSPEED_MV        )
                FROM   TB_GRD_OPER_INFO
                WHERE  WORK_DDTT >= @START_DDTT
                AND    WORK_DDTT <= @END_DDTT
                --------------------------------------------------------------
                ----실적생성    
                --------------------------------------------------------------
                ----조업정보르 평균값으로 SELECT
                INSERT INTO [dbo].[TB_GRD_WR]
                           ([HEAT_NO]
                           ,[HEAT_SEQ]
                           ,[REWORK_SNO]
                           ,[STEEL]
                           ,[ITEM]
                           ,[ITEM_SIZE]
                           ,[STEEL_TYPE]
                           ,[MFG_DATE]
                           ,[QR_SCAN_INFO]
                           ,[WORK_DATE]
                           ,[GRD_START_DDTT]
                           ,[GRD_END_DDTT]
                           ,[GRD_DRV_MODE]
                           ,[WORK_SETV_Top]
                           ,[WORK_SETV_Right]
                           ,[WORK_SETV_Bottom]
                           ,[WORK_SETV_Left]
                           ,[HEIGHT_MV]
                           ,[WIDTH_MV]
                           ,[LENGTH_MV]
                           ,[FACE_PASS_CNT]
                           ,[CORN_PASS_CNT]
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
                           ,[GRD_PRES_MV]
                           ,[TRUCKSPEED_SETV]
                           ,[TRUCKSPEED_MV]
                           ,[SURFACE_STAT]
                           ,[REWORK_YN]
                           ,[SEND_STAT]
                           ,[REGISTER]
                           ,[REG_DDTT] )
                   SELECT 
                            A.HEAT_NO
                           ,A.HEAT_SEQ
                           ,A.REWORK_SNO
                           ,A.STEEL
                           ,A.ITEM
                           ,A.ITEM_SIZE
                           ,A.STEEL_TYPE
                           ,A.MFG_DATE
                           ,A.QR_SCAN_INFO
                           ,@CUR_DATE
                           ,A.GRD_START_DDTT
                           ,A.GRD_END_DDTT
                           ,@GRD_DRV_MODE         
                           ,@WORK_SETV_Top        
                           ,@WORK_SETV_Right      
                           ,@WORK_SETV_Bottom     
                           ,@WORK_SETV_Left       
                           ,@HEIGHT_MV            
                           ,@WIDTH_MV             
                           ,@LENGTH_MV            
                           ,@FACE_PASS_CNT        
                           ,@CORN_PASS_CNT        
                           ,@RBGW_SPEED_SETV      
                           ,@RBGW_ADV_SETV        
                           ,@RBGW_DIA_MV          
                           ,@RBGW_ENGY_MV         
                           ,@RBGW_VIBR_MV         
                           ,@ROTANGLE_SETV        
                           ,@ROTANGLE_MV          
                           ,@GRD_PRES_SURFACE_SETV
                           ,@GRD_PRES_ENDP_SETV   
                           ,@GRD_PRES_CORN_SETV   
                           ,@GRD_PRES_MV          
                           ,@TRUCKSPEED_SETV      
                           ,@TRUCKSPEED_MV
                           ,''  --표면상태
                           ,A.REWORK_YN
                           ,'N'    --L2 IF 여부
                           ,'SYSTEM'
                           ,GETDATE()
                   FROM    TB_RL_TM_TRACKING  A
                   WHERE  A.HEAT_NO   = @S_HEAT_NO
                   AND    A.HEAT_SEQ  = @S_HEAT_SEQ;
                IF  @@ERROR <> 0
                    BEGIN
                        SET  @P_PROC_STAT  = 'ERR';
                        SET  @P_PROC_MSG   = @S_HEAT_NO + ' : ' + CONVERT(VARCHAR(5),@S_HEAT_SEQ) + ' INSERT 작업중 오류 ' + SUBSTRING(ERROR_MESSAGE(),1,200);
                        RAISERROR('INSERT 작업중 오류가 발생했습니다.', 16, 1);
                    END;
                ----정지시간 계산을 위해 종료시각 관리
                UPDATE  TB_GRD_RBGW_MGMT
                SET     STOP_START_DDTT  = @END_DDTT
                       ,STOP_END_DDTT    = NULL
                       ,GRD_RBGW_USE_CNT = isnull(GRD_RBGW_USE_CNT,0) + 1;
                IF  @@ROWCOUNT = 0
                    BEGIN
                        INSERT INTO TB_GRD_RBGW_MGMT ( STOP_START_DDTT, STOP_END_DDTT,GRD_RBGW_USE_CNT )
                               VALUES ( @END_DDTT, NULL,0 );
                    END;

				---그라인드 사용횟수 설비관리실적 2018-08-29 (박도우 대리 요청)
                SELECT TOP 1
                       @S_EQP_CD     = EQP_CD
                     , @S_ITEM_CD    = ITEM_CD
                     , @S_CHECK_SEQ  = MAX(CHECK_SEQ)
					 , @S_USECNT     = MAX(USE_CNT)
                  FROM [dbo].[TB_EQP_CHECK_WR]
			   GROUP BY EQP_CD
			          , ITEM_CD
			   HAVING   EQP_CD  = 'C010'
                   AND ITEM_CD  = '2020';
				 IF  @@ROWCOUNT = 0
                  BEGIN
                      SET  @P_PROC_STAT  = 'ERR';
                      SET  @P_PROC_MSG   = 'EQP_CD = C010, ITEM_CD = 2020 설비점검실적 코드가 존재하지않습니다.';
					  EXEC DBO.SP_ERR_LOG @ERR_PGM, @P_PROC_STAT, @P_PROC_STAT, @P_PROC_MSG;
                      --RAISERROR(@P_PROC_MSG, 16, 1);
                  END;
				UPDATE [dbo].[TB_EQP_CHECK_WR]
				   SET [USE_CNT] = @S_USECNT +1
				      ,[MODIFIER] = 'GRD_END'
				      ,[MOD_DDTT] = GETDATE()
				WHERE EQP_CD     = @S_EQP_CD
                  AND ITEM_CD    = @S_ITEM_CD
				  AND CHECK_SEQ  = @S_CHECK_SEQ
				IF  @@ERROR <> 0
                  BEGIN
                      SET  @P_PROC_STAT  = 'ERR';
                      SET  @P_PROC_MSG   = @S_HEAT_NO + ' 설비점검 CNT 증가 실패 ' + SUBSTRING(ERROR_MESSAGE(),1,200);
                      RAISERROR(@P_PROC_MSG, 16, 1);
                  END;

                ----스케줄에 작업본수 갱신
                UPDATE TB_SCHEDULE
                SET    WORK_PCS = ISNULL(WORK_PCS,0) + 1
                      ,WORK_STAT = 'RUN'
                      ,MODIFIER = 'GRD_END'
                      ,MOD_DDTT = GETDATE()
                WHERE  HEAT_NO  = @S_HEAT_NO
				  AND  ISNULL(@S_REWORK, 'N') = 'N';
                IF  @@ERROR <> 0
                    BEGIN
                        SET  @P_PROC_STAT  = 'ERR';
                        SET  @P_PROC_MSG   = @S_HEAT_NO + ' UPDATE 작업중 오류 ' + SUBSTRING(ERROR_MESSAGE(),1,200);
                        RAISERROR('UPDATE 작업중 오류가 발생했습니다.', 16, 1);
                    END;
                IF  EXISTS ( SELECT HEAT_NO FROM TB_SCHEDULE WHERE HEAT_NO = @S_HEAT_NO AND TAKE_OVER_PCS = WORK_PCS )
                    BEGIN
                        UPDATE TB_SCHEDULE
                        SET    WORK_STAT = 'END'
                              ,MODIFIER = 'GRD_END'
                              ,MOD_DDTT = GETDATE()
                        WHERE  HEAT_NO       = @S_HEAT_NO;
                    END;
            END;
            
        COMMIT TRAN
        
        SET  @P_PROC_STAT  = 'OK';
        SET  @P_PROC_MSG   = '정상처리완료 ' + CONVERT(VARCHAR(5),@UPDATE_CNT);
        
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
            SET  @P_PROC_STAT  = 'ERR';
            SET  @P_PROC_MSG   = @ERR_MSG;
			EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @ERR_MSG;
        END
    END CATCH
    
END