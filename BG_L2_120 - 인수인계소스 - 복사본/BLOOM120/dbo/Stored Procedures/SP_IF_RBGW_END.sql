CREATE PROCEDURE [dbo].[SP_IF_RBGW_END]
 @P_IF_SEQ    NUMERIC(12)
,@P_TC_CD     VARCHAR(4)  = 'F110'
,@P_PROC_STAT VARCHAR(3)     OUTPUT
,@P_PROC_MSG  VARCHAR(1000)  OUTPUT
AS
--------------------------------------------------------
---- 입력변수 : IF_SEQ, TC_CD
---- 출력변수 : 처리상태(OK,ERR), 처리결과MSG
--------------------------------------------------------
SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_IF_RBGW_END';
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

DECLARE @ZONE_CD        VARCHAR(3) = 'Z90';
DECLARE @BEF_ZONE_CD    VARCHAR(3) = 'Z80';
DECLARE @S_HEAT_NO      VARCHAR(6) = '';
DECLARE @S_HEAT_SEQ     INT        = 0;

DECLARE @REG_DDTT       VARCHAR(17);
DECLARE @DATA_01        VARCHAR(30);
DECLARE @DATA_02        VARCHAR(30);
DECLARE @STOP_RSN       VARCHAR(150);
DECLARE @START_DDTT     DATETIME;
DECLARE @END_DDTT       DATETIME;
DECLARE @GRD_RBGW_USE_CNT int;

BEGIN
    SET  @P_PROC_STAT = 'OK';
    SET  @P_PROC_MSG  = '정상적으로 처리되었습니다. ';

    BEGIN TRY
        
        BEGIN TRAN
            SET @CUR_DATE = CONVERT(VARCHAR(8),GETDATE()-6/24,112);
            IF  @P_TC_CD  <> 'F110'
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
		         -- START *
		        -- 2018-09-11 박도우 대리 요청
		        -- 연마석 종료신호를 이용해서 교체시작, 종료 이벤트 생성
		        -- DATA_01  0 인경우 교체시작,
		        --          1 인경우 교체종료
		        IF @DATA_01 = '0'
					BEGIN
					    ----연마지석 교체 시작 UPDATE
					    UPDATE  TB_GRD_RBGW_MGMT
					    SET     GRD_RBGW_CHG_START_DDTT = GETDATE()
					            ,GRD_RBGW_CHG_END_DDTT   = NULL
          						,GRD_RBGW_CHG_YN         ='Y'
								,GRD_RBGW_USE_CNT        = 0;  -- 2018-09-13 박도우 대리 요청 연마지석 교체시 사용 0 으로 SET
					    IF  @@ROWCOUNT = 0
					        BEGIN
					            INSERT INTO TB_GRD_RBGW_MGMT ( GRD_RBGW_CHG_START_DDTT, GRD_RBGW_CHG_END_DDTT, GRD_RBGW_USE_CNT )
					                    VALUES ( GETDATE(), NULL, 0 );
					        END;
					    
					END;
		        ELSE
					BEGIN
						SELECT  @START_DDTT = GRD_RBGW_CHG_START_DDTT
								,@END_DDTT   = GETDATE()
								,@GRD_RBGW_USE_CNT = GRD_RBGW_USE_CNT
							FROM   TB_GRD_RBGW_MGMT;
							IF  @@ROWCOUNT = 0
								BEGIN
									----없는 경우는 오류 처리안함
									SET  @P_PROC_STAT  = 'ERR';
									SET  @P_PROC_MSG   = CONVERT(VARCHAR(10),@P_IF_SEQ) + '에 작업할 정보가 없습니다. ';
								END;
							ELSE
								BEGIN
									----그라인딩 연마지석 교체 정지시간 등록
									SET  @STOP_RSN = '연마지석 교체';
									EXEC dbo.SP_STOP_TIME_CALC @START_DDTT, @END_DDTT, @STOP_RSN;
									----연마지석 사용량 --> 설비점검관리에 INSERT
									----설비코드, 항목코드 정의 필요.
									begin
										INSERT INTO [dbo].[TB_EQP_CHECK_WR]
												([EQP_CD]
												,[ITEM_CD]
												,[CHECK_SEQ]
												,[EQP_NM]
												,[ROUTING_NM]
												,[CHECK_ITEM]
												,[CHECK_PLN_DATE]
												,[CHECK_WR_DATE]
												,[CHECK_GAP]
												,[CHECK_CYCLE]
												,[CHECK_YN]
												,[CHECK_CNTS]
												,[USE_CNT]
												,[PROG_STAT]
												,[REGISTER]
												,[REG_DDTT])
											SELECT  A.EQP_CD
												,A.ITEM_CD
												,(select ISNULL(MAX(CHECK_SEQ),0) + 1 FROM TB_EQP_CHECK_WR WHERE EQP_CD = A.EQP_CD AND ITEM_CD = A.ITEM_CD)
												,B.EQP_NM
												,B.ROUTING_NM
												,A.CHECK_ITEM
												,@CUR_DATE
												,@CUR_DATE
												,A.CHECK_GAP
												,A.CHECK_CYCLE
												,'Y'
												,''
												,0
												,'END'
												,'RBGW_END'
												,GETDATE()
											FROM   TB_EQP_CHECK_ITEM A
												,TB_EQP_INFO       B
											WHERE  A.EQP_CD     = B.EQP_CD
											AND    A.EQP_CD     = 'C010'
											AND    A.ITEM_CD    = '2020'; 
									end;
                       
								END;
							----그라인딩연마석교체 작업 끝. UPDATE
							UPDATE  TB_GRD_RBGW_MGMT
							SET     GRD_RBGW_CHG_END_DDTT  = GETDATE()
								--,GRD_RBGW_USE_CNT       = 0  /*--2018-08-28 박도우 대리: 연마석 교체 신호 이상으로 추후 수동으로 종료처리할지 검토후 적용.
								,STOP_START_DDTT        = GETDATE()
								,GRD_RBGW_CHG_YN        = 'N'
				 
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
