CREATE PROCEDURE dbo.SP_IF_GRD_SND

AS

SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_IF_GRD_SND';
DECLARE @Err_Msg        VARCHAR(300); 
DECLARE @Err_CD         VARCHAR(20); 
DECLARE @Err_TP         VARCHAR(10); 
DECLARE @ERR_LINE       NUMERIC(10,0); 
DECLARE @ERR_NUMBER     NUMERIC(10,0);
DECLARE @REMARK         VARCHAR(20);

DECLARE @DELETE_CNT     NUMERIC(10) = 0;
DECLARE @INSERT_CNT     NUMERIC(10) = 0;
DECLARE @UPDATE_CNT     NUMERIC(10) = 0;

DECLARE @SQL            NVARCHAR(4000)
DECLARE @CUR_DATE       VARCHAR(8);
DECLARE @BEF_DATE       VARCHAR(8);
DECLARE @SIM_NO         NUMERIC(12) = 0;
DECLARE @SIM_NO_MAX     NUMERIC(12) = 0;

DECLARE @HEAT_NO        VARCHAR(6);
DECLARE @HEAT_SEQ       integer;
DECLARE @QR_SCAN_INFO   VARCHAR(16);
DECLARE @WORK_DATE      VARCHAR(8);
DECLARE @MFG_DATE       VARCHAR(8);
DECLARE @STEEL          VARCHAR(10);
DECLARE @FULL_CNT       integer;
DECLARE @FACE_CNT       integer;
DECLARE @CORN_CNT       integer;
DECLARE @GRD_PRES_MV    integer;
DECLARE @FACE_PASS_CNT  integer;
DECLARE @CORN_PASS_CNT  integer;
DECLARE @TRUCKSPEED_MV  integer;

BEGIN

    BEGIN TRY
        
        SET @BEF_DATE = CONVERT(VARCHAR(8),GETDATE() - 1,112);
        --BEGIN TRAN

            DECLARE cur_C1 CURSOR FOR
                    SELECT HEAT_NO
                          ,HEAT_SEQ
                          ,QR_SCAN_INFO   
                          ,WORK_DATE      --작업일자
                          ,MFG_DATE       --제강생산일자
                          ,STEEL
                          ,CASE WHEN GRD_DRV_MODE = 1 THEN 1 ELSE 0 END AS FULL_CNT --Full본수
                          ,CASE WHEN GRD_DRV_MODE = 2 THEN 1 ELSE 0 END AS FACE_CNT --Surface본수
                          ,CASE WHEN GRD_DRV_MODE = 3 THEN 1 ELSE 0 END AS CORN_CNT --Corner본수
                          ,GRD_PRES_MV    --표면압력
                          ,FACE_PASS_CNT  --Pass수 면
                          ,CORN_PASS_CNT  --Pass수 코너
                          ,TRUCKSPEED_MV  --대차속도 
                    FROM   TB_GRD_WR
                    WHERE  ISNULL(SEND_STAT,'N') <> 'Y'
                    FOR UPDATE OF SEND_STAT;
                    
            OPEN cur_C1;
            
            FETCH NEXT FROM cur_C1 INTO  @HEAT_NO      
                                        ,@HEAT_SEQ     
                                        ,@QR_SCAN_INFO 
                                        ,@WORK_DATE    
                                        ,@MFG_DATE     
                                        ,@STEEL        
                                        ,@FULL_CNT     
                                        ,@FACE_CNT     
                                        ,@CORN_CNT     
                                        ,@GRD_PRES_MV  
                                        ,@FACE_PASS_CNT
                                        ,@CORN_PASS_CNT
                                        ,@TRUCKSPEED_MV;
            WHILE(@@FETCH_STATUS = 0)
                BEGIN
                    IF @SIM_NO = 0 
                        BEGIN
                            SELECT @SIM_NO_MAX = ISNULL(MAX(SIM_NO),0)
                            FROM   L2P120..LEVEL2.L2_RECEIVE_NEW;
                            SET @SIM_NO  = @SIM_NO_MAX + 1;
                        END
                    ELSE
                        BEGIN
                            SET @SIM_NO  = @SIM_NO + 1;
                        END
                    BEGIN
                        SET @INSERT_CNT = @INSERT_CNT + 1;
                        ----INSERT INTO L2P120..LEVEL2.L2_RECEIVE_NEW
                        INSERT INTO OPENQUERY ( L2P120, 'SELECT
                                                           SIM_NO
                                                          ,SIM_CODE
                                                          ,SIM_DATE
                                                          ,SIM_TIME
                                                          ,SIM_STATUS
                                                          ,SIM_IP
                                                          ,SIM_IUD
                                                          ,SIM_DATA1
                                                          ,SIM_DATA2
                                                          ,SIM_DATA3
                                                          ,SIM_DATA4
                                                          ,SIM_DATA5
                                                          ,SIM_DATA6
                                                          ,SIM_DATA7
                                                          ,SIM_DATA8
                                                          ,SIM_DATA9
                                                          ,SIM_DATA10
                                                          ,SIM_DATA11
                                                          ,SIM_DATA12
                                                          ,SIM_DATA13
                                                          FROM  L2_RECEIVE_NEW'
                             ) VALUES (
                               @SIM_NO
                              ,'S010'
                              ,CONVERT(VARCHAR(8), GETDATE(), 112)
                              ,substring(CONVERT(VARCHAR(8), GETDATE(), 114),1,2) + substring(CONVERT(VARCHAR(8), GETDATE(), 114),4,2) + substring(CONVERT(VARCHAR(8), GETDATE(), 114),7,2)
                              ,'N' 
                              ,'192.168.250.165'
                              ,'I'
                              ,@HEAT_NO      
                              ,@HEAT_SEQ     
                              ,@QR_SCAN_INFO 
                              ,@WORK_DATE    
                              ,@MFG_DATE     
                              ,@STEEL        
                              ,CONVERT(VARCHAR(10),@FULL_CNT)     
                              ,CONVERT(VARCHAR(10),@FACE_CNT)     
                              ,CONVERT(VARCHAR(10),@CORN_CNT)     
                              ,CONVERT(VARCHAR(10),@GRD_PRES_MV)  
                              ,CONVERT(VARCHAR(10),@FACE_PASS_CNT)
                              ,CONVERT(VARCHAR(10),@CORN_PASS_CNT)
                              ,CONVERT(VARCHAR(10),@TRUCKSPEED_MV)
                             ); 
                        IF  @@ERROR <> 0
                            BEGIN
                                RAISERROR('INSERT 오류', 16, 1);
                            END;
                    END;      
                    BEGIN
                        UPDATE TB_GRD_WR
                        SET    SEND_STAT  = 'Y'
                        WHERE  CURRENT OF cur_C1;
                    END;
                    FETCH NEXT FROM cur_C1 INTO  @HEAT_NO      
                                                ,@HEAT_SEQ     
                                                ,@QR_SCAN_INFO 
                                                ,@WORK_DATE    
                                                ,@MFG_DATE     
                                                ,@STEEL        
                                                ,@FULL_CNT     
                                                ,@FACE_CNT     
                                                ,@CORN_CNT     
                                                ,@GRD_PRES_MV  
                                                ,@FACE_PASS_CNT
                                                ,@CORN_PASS_CNT
                                                ,@TRUCKSPEED_MV;
                END;
            CLOSE cur_C1;
            DEALLOCATE cur_C1;

        --COMMIT TRAN;
        SET  @ERR_TP  = 'INFO';
        SET  @ERR_CD  = '정상'; 
        SET  @ERR_MSG = 'INSERT_CNT : ' + CONVERT(VARCHAR(5),@INSERT_CNT);
        EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @ERR_MSG;
    END TRY
    BEGIN CATCH --//EXCEPTION 처리부분 : 쿼리문이 오류가 났을 경우 실행할 쿼리문

        CLOSE cur_C1;
        DEALLOCATE cur_C1;

        --ROLLBACK TRAN; -- 실패!
 
        -- 해당 시스템 함수는 반드시 CATCH문에서 사용해야한다. (CATCH블록 외부에서 호출되는 경우 NULL값 반환)
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
