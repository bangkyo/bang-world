CREATE PROCEDURE [dbo].[SP_IF_MPI_OUT]
 @P_IF_SEQ    NUMERIC(12)
,@P_TC_CD     VARCHAR(4)  = 'A060'
,@P_PROC_STAT VARCHAR(3)     OUTPUT
,@P_PROC_MSG  VARCHAR(1000)  OUTPUT
AS
--------------------------------------------------------
---- 입력변수 : IF_SEQ, TC_CD
---- 출력변수 : 처리상태(OK,ERR), 처리결과MSG
--------------------------------------------------------
SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_IF_MPI_OUT';
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
DECLARE @S_REWORK_SNO   INT        =0; --20180820 추가 MJ

DECLARE @REG_DDTT       VARCHAR(17);
DECLARE @DATA_01        VARCHAR(30);
DECLARE @DATA_02        VARCHAR(30);
DECLARE @START_DDTT  DATETIME;
DECLARE @END_DDTT    DATETIME;

DECLARE @Yoke_MTR_NO1_CURRENT    INT;
DECLARE @Yoke_MTR_NO2_CURRENT    INT;
DECLARE @Yoke_MTR_NO3_CURRENT    INT;
DECLARE @Yoke_MTR_NO4_CURRENT    INT;
DECLARE @COIL_NO1_2_CURRENT      INT;

BEGIN
    SET  @P_PROC_STAT = 'OK';
    SET  @P_PROC_MSG  = '정상적으로 처리되었습니다. ';

    BEGIN TRY
        
        BEGIN TRAN
            SET @CUR_DATE = CONVERT(VARCHAR(8),GETDATE()-6/24,112);
            IF  @P_TC_CD  <> 'A060'
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
                        @S_HEAT_NO       = HEAT_NO
                       ,@S_HEAT_SEQ      = HEAT_SEQ
					   ,@S_REWORK_SNO    = REWORK_SNO  -- 20180820 추가 MJ
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
                       ,MPI_END_DDTT      = CONVERT(DATETIME,@REG_DDTT)
                WHERE  HEAT_NO    = @S_HEAT_NO
                AND    HEAT_SEQ   = @S_HEAT_SEQ;
                IF  @@ERROR = 0
                    SET @UPDATE_CNT = @UPDATE_CNT + 1;
                ----조업정보를 위해 시작,종료일시를 읽어 온다.
                SELECT  @START_DDTT = MPI_START_DDTT
                       ,@END_DDTT   = MPI_END_DDTT
                FROM   TB_RL_TM_TRACKING
                WHERE  HEAT_NO    = @S_HEAT_NO
                AND    HEAT_SEQ   = @S_HEAT_SEQ;
                ----조업정보 평균값 SELECT
                SELECT  @Yoke_MTR_NO1_CURRENT     =  AVG(@Yoke_MTR_NO1_CURRENT)
                       ,@Yoke_MTR_NO2_CURRENT     =  AVG(@Yoke_MTR_NO2_CURRENT)
                       ,@Yoke_MTR_NO3_CURRENT     =  AVG(@Yoke_MTR_NO3_CURRENT)
                       ,@Yoke_MTR_NO4_CURRENT     =  AVG(@Yoke_MTR_NO4_CURRENT)
                       ,@COIL_NO1_2_CURRENT       =  AVG(@COIL_NO1_2_CURRENT  )
                FROM   TB_MPI_OPER_INFO
                WHERE  WORK_DDTT >= @START_DDTT
                AND    WORK_DDTT <= @END_DDTT
                --------------------------------------------------------------
                ----실적생성    
                --------------------------------------------------------------
                ----조업정보르 평균값으로 SELECT
                -- UPSERT START
				IF NOT EXISTS( SELECT * FROM  [dbo].[TB_MPI_WR] WHERE HEAT_NO = @S_HEAT_NO AND HEAT_SEQ = @S_HEAT_SEQ AND REWORK_SNO = @S_REWORK_SNO)
					INSERT INTO [dbo].[TB_MPI_WR]
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
                           ,[MPI_START_DDTT]
                           ,[MPI_END_DDTT]
                           ,[Yoke_MTR_NO1_CURRENT]
                           ,[Yoke_MTR_NO2_CURRENT]
                           ,[Yoke_MTR_NO3_CURRENT]
                           ,[Yoke_MTR_NO4_CURRENT]
                           ,[COIL_NO1_2_CURRENT]
                           ,[REWORK_YN]
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
                           ,A.MPI_START_DDTT
                           ,A.MPI_END_DDTT
                           ,@Yoke_MTR_NO1_CURRENT
                           ,@Yoke_MTR_NO2_CURRENT
                           ,@Yoke_MTR_NO3_CURRENT
                           ,@Yoke_MTR_NO4_CURRENT
                           ,@COIL_NO1_2_CURRENT  
                           ,A.REWORK_YN
                           ,'SYSTEM'
                           ,GETDATE()
                   FROM    TB_RL_TM_TRACKING  A
                   WHERE  A.HEAT_NO   = @S_HEAT_NO
                   AND    A.HEAT_SEQ  = @S_HEAT_SEQ;
					IF  @@ERROR <> 0
						BEGIN
							SET  @P_PROC_STAT  = 'ERR';
							SET  @P_PROC_MSG   = @S_HEAT_NO + ' : ' + CONVERT(VARCHAR(5),@S_HEAT_SEQ) + ' INSERT 작업중 오류 ' + SUBSTRING(ERROR_MESSAGE(),1,200);
							RAISERROR(@P_PROC_MSG, 16, 1);
						END;
				ELSE
					UPDATE [dbo].[TB_MPI_WR] 
					   SET [HEAT_NO]				 = A.HEAT_NO
						  ,[HEAT_SEQ]				 = A.HEAT_SEQ
						  ,[REWORK_SNO]				 = A.REWORK_SNO
						  ,[STEEL]					 = A.STEEL
						  ,[ITEM]					 = A.ITEM
						  ,[ITEM_SIZE]				 = A.ITEM_SIZE
						  ,[STEEL_TYPE]				 = A.STEEL_TYPE
						  ,[MFG_DATE]				 = A.MFG_DATE
						  ,[QR_SCAN_INFO]			 = A.QR_SCAN_INFO
						  ,[WORK_DATE]				 = @CUR_DATE
						  ,[MPI_START_DDTT]			 = A.MPI_START_DDTT
						  ,[MPI_END_DDTT]			 = A.MPI_END_DDTT
						  ,[Yoke_MTR_NO1_CURRENT]    = [Yoke_MTR_NO1_CURRENT]
						  ,[Yoke_MTR_NO2_CURRENT]    = [Yoke_MTR_NO2_CURRENT]
						  ,[Yoke_MTR_NO3_CURRENT]    = [Yoke_MTR_NO3_CURRENT]
						  ,[Yoke_MTR_NO4_CURRENT]    = [Yoke_MTR_NO4_CURRENT]
						  ,[COIL_NO1_2_CURRENT]      = [COIL_NO1_2_CURRENT]
						  ,[REWORK_YN]               = A.REWORK_YN
						  ,[MODIFIER]                = 'SYSTEM'
						  ,[MOD_DDTT]                = GETDATE()
				      FROM TB_MPI_WR 
					     , TB_RL_TM_TRACKING A
					 WHERE TB_MPI_WR.HEAT_NO  = A.HEAT_NO
					   AND TB_MPI_WR.HEAT_SEQ = A.HEAT_SEQ
					   AND A.HEAT_NO   = @S_HEAT_NO
                       AND A.HEAT_SEQ  = @S_HEAT_SEQ
					   AND TB_MPI_WR.REWORK_SNO = @S_REWORK_SNO;
					   IF  @@ERROR <> 0
							BEGIN
							    SET  @P_PROC_STAT  = 'ERR';
							    SET  @P_PROC_MSG   = @S_HEAT_NO + ' : ' + CONVERT(VARCHAR(5),@S_HEAT_SEQ) + ' UPDATE 작업중 오류 ' + SUBSTRING(ERROR_MESSAGE(),1,200);
							    RAISERROR(@P_PROC_MSG, 16, 1);
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
            EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @ERR_MSG;
        END
    END CATCH
    
END
