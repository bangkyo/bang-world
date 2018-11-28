CREATE PROCEDURE [dbo].[SP_L1_RECEIVE_ALL]

AS
--------------------------------------------------------
---- L1 RECEIVE DATA 처리
---- 커서선언하여 한건씩 처리
--------------------------------------------------------
SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_L1_RECEIVE_ALL';
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

DECLARE @IF_SEQ         NUMERIC(12);
DECLARE @TC_CD          VARCHAR(10);
DECLARE @PROC_STAT      VARCHAR(2);
DECLARE @REG_DDTT       VARCHAR(17);
DECLARE @RECEIVE_DDTT   VARCHAR(17);

DECLARE @P_STAT       VARCHAR(3);
DECLARE @P_MSG        VARCHAR(200);



BEGIN

    BEGIN TRAN

    DECLARE CUR_C1  CURSOR FOR 
        SELECT  IF_SEQ
               ,TC_CD
               ,PROC_STAT
               ,REG_DDTT
        FROM   TB_L1_RECEIVE 
        WHERE  PROC_STAT = 'AD'
        ORDER  BY IF_SEQ;

    SET  @PROC_STAT = 'RD';
    SET  @CUR_DATE = CONVERT(VARCHAR(8),GETDATE()-6/24,112);
    SET  @P_STAT   = '';
    SET  @P_MSG    = '';

    OPEN CUR_C1;
    FETCH NEXT FROM  CUR_C1 INTO @IF_SEQ, @TC_CD, @PROC_STAT, @REG_DDTT;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        
        IF  @TC_CD  = 'A010'
            EXEC  dbo.SP_IF_Z01_IN @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
        ELSE 
        begin
            IF  @TC_CD = 'A020'
                EXEC  dbo.SP_IF_QRCD @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
            ELSE
            begin
                IF  @TC_CD = 'A030'
                    EXEC  dbo.SP_IF_SHT_IN @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
                ELSE
                begin
                    IF  @TC_CD = 'A040'
                        EXEC  dbo.SP_IF_SHT_OUT @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
                    ELSE
                    begin
                        IF  @TC_CD = 'A050'
                            EXEC  dbo.SP_IF_MPI_IN @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
                        ELSE
                        begin
                            IF  @TC_CD = 'A060'
                                EXEC  dbo.SP_IF_MPI_OUT @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
                            ELSE
                            begin
                                IF  @TC_CD = 'A070'
								    --2018-08-29 박도우 대리: ADDRESS 확인후 동작 검토.
                                    --EXEC  dbo.SP_IF_Z50_IN @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
									SELECT 'NOTHING'
                                ELSE
                                begin
                                    IF  @TC_CD = 'A080'
                                        EXEC  dbo.SP_IF_Z60_IN @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
                                    ELSE
									begin
									    IF  @TC_CD = 'A090'
									        EXEC  dbo.SP_IF_Z20_IN @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
									    ELSE
										begin
											IF  @TC_CD = 'B010'
											    EXEC  dbo.SP_IF_SHT_OPER @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
											ELSE
											begin
											    IF  @TC_CD = 'C010'
											        EXEC  dbo.SP_IF_MPI_OPER @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
											    ELSE
											    begin
											        IF  @TC_CD = 'F010'
											            EXEC  dbo.SP_IF_GRD_OPER @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
											        ELSE
											        begin
											            IF  @TC_CD = 'F030'
											                EXEC  dbo.SP_IF_TRK_IN @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
											            ELSE
											            begin
											                IF  @TC_CD = 'F040'
											                    EXEC  dbo.SP_IF_GRD_STR @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
											                ELSE
											                begin
											                    IF  @TC_CD = 'F050'
											                        EXEC  dbo.SP_IF_GRD_END @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
											                    ELSE
											                    begin
											                        IF  @TC_CD = 'F060'
											                            EXEC  dbo.SP_IF_Z91_IN @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
											                        ELSE
											                        begin
											                            IF  @TC_CD = 'F070'
											                                EXEC  dbo.SP_IF_Z90_IN @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
											                            ELSE
											                            begin
											                                IF  @TC_CD = 'F080'
											                                    EXEC  dbo.SP_IF_CHIP_STR @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
											                                ELSE
											                                begin
											                                    IF  @TC_CD = 'F090'
											                                        EXEC  dbo.SP_IF_CHIP_END @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
											                                    ELSE
											                                    begin
											                                        IF  @TC_CD = 'F100'
																					--    2018-09-11 박도우 대리: 연마지석 시작 이벤트 사용 안함.
											                                        --    EXEC  dbo.SP_IF_RBGW_STR @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
																						SELECT 'NOTHING'
											                                        ELSE
											                                        begin
											                                            IF  @TC_CD = 'F110'
											                                                EXEC  dbo.SP_IF_RBGW_END @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
											                                            ELSE
											                                            begin
											                                                IF  @TC_CD = 'H010'
											                                                    EXEC  dbo.SP_IF_SHT_DUST_ON @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
											                                                ELSE
											                                                begin
											                                                    IF  @TC_CD = 'H020'
											                                                        EXEC  dbo.SP_IF_SHT_DUST_OFF @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
											                                                    ELSE
											                                                    begin
											                                                        IF  @TC_CD = 'H030'
											                                                            EXEC  dbo.SP_IF_SHT_DUST_OPER @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
											                                                        ELSE
											                                                        begin
											                                                            IF  @TC_CD = 'K010'
											                                                                EXEC  dbo.SP_IF_GRD_DUST_ON @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
											                                                            ELSE
											                                                            begin
											                                                                IF  @TC_CD = 'K020'
											                                                                    EXEC  dbo.SP_IF_GRD_DUST_OFF @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
											                                                                ELSE
											                                                                begin
											                                                                    IF  @TC_CD = 'K030'
											                                                                        EXEC  dbo.SP_IF_GRD_DUST_OPER @IF_SEQ, @TC_CD, @P_STAT OUTPUT, @P_MSG OUTPUT;
											                                                                    ELSE
											                                                                    begin
											                                                                        SET  @P_STAT = 'ERR';
											                                                                        SET  @P_MSG  = 'TC CD 없음';
											                                                                    end;
											                                                                end;
											                                                            end;
											                                                        end;
											                                                    end;
											                                                end;
											                                            end;
											                                        end;
											                                    end;
											                                end;
											                            end;
											                        end;
											                    end;
											                end;
											            end;
											        end;
											    end;
											end;
										end;
									end;
                                end;
                            end;
                        end;
                    end;    
                end;    
            end;    
        end;
        ----L1 IF 갱신
        IF  @P_STAT  = 'ERR'
            BEGIN
                UPDATE TB_L1_RECEIVE
                SET    PROC_STAT    = 'ER'
                      ,RECEIVE_DDTT = FORMAT(GETDATE(),'yyyyMMdd HH:mm:ss')
                      ,ERR_MSG      = @P_MSG
                WHERE IF_SEQ        = @IF_SEQ;
            END;
        ELSE
            BEGIN
                UPDATE TB_L1_RECEIVE
                SET    PROC_STAT    = 'RD'
                      ,RECEIVE_DDTT = FORMAT(GETDATE(),'yyyyMMdd HH:mm:ss')
                WHERE IF_SEQ        = @IF_SEQ;
            END;
        FETCH NEXT FROM  CUR_C1 INTO @IF_SEQ, @TC_CD, @PROC_STAT, @REG_DDTT;
    END;
    CLOSE CUR_C1;
    DEALLOCATE CUR_C1;
    
    COMMIT TRAN;
END
