CREATE PROCEDURE [dbo].[SP_SCR_TO_L1_RECEIVE_CRE]
 @P_TC_CD     VARCHAR(4)
,@P_DATA_01    VARCHAR(1)
,@P_PROC_STAT  VARCHAR(3)     OUTPUT
,@P_PROC_MSG   VARCHAR(1000)  OUTPUT
AS
--------------------------------------------------------
---- 입력변수 : TC_CD
---- 출력변수 : 처리상태(OK,ERR)
--------------------------------------------------------
SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_SCR_TO_L1_RECEIVE_CRE';
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

DECLARE @ZONE_CD        VARCHAR(3) = 'Z90';
DECLARE @BEF_ZONE_CD    VARCHAR(3) = 'Z80';
DECLARE @S_HEAT_NO      VARCHAR(6) = '';
DECLARE @S_HEAT_SEQ     INT        = 0;

DECLARE @REG_DDTT       VARCHAR(17);
DECLARE @DATA_01        VARCHAR(30);
DECLARE @DATA_02        VARCHAR(30);

BEGIN
    SET  @Err_TP = 'OK';

    BEGIN TRY
        
        SET @CUR_DATE = CONVERT(VARCHAR(8),GETDATE()-6/24,112);
        
        IF  @P_TC_CD  = ' ' 
            BEGIN
                SET  @Err_TP  = 'ERR';
                SET  @ERR_MSG   = 'TC CD ERROR : ' + @P_TC_CD;
                RAISERROR(@ERR_MSG, 16, 1);
            END;
            
        BEGIN TRAN
            BEGIN
                ----
                INSERT INTO TB_L1_RECEIVE (
                        IF_SEQ
                       ,TC_CD
                       ,PROC_STAT
                       ,REG_DDTT
                       ,DATA_01
                      ) VALUES (
                        NEXT VALUE FOR TB_L1_RECEIVE_SEQ
                       ,@P_TC_CD
                       ,'AD'
                       ,FORMAT(GETDATE(),'yyyyMMdd HH:mm:ss')
                       ,@P_DATA_01
                      ); 
                IF  @@ROWCOUNT = 0
                    BEGIN
                        SET  @Err_TP  = 'ERR';
                        SET  @ERR_MSG   = 'INSERT 오류입니다.';
                        RAISERROR(@ERR_MSG, 16, 1);
                    END;
            END;
            
        COMMIT TRAN
        
        SET  @Err_TP  = 'OK';
        SET  @ERR_MSG   = '정상처리완료 ' + CONVERT(VARCHAR(5),@UPDATE_CNT);
        
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

			GOTO ERROR_RTN;
        END
    END CATCH
ERROR_RTN:
    BEGIN
        SET  @P_PROC_STAT = @Err_TP;
        SET  @P_PROC_MSG  = @ERR_MSG;
        RETURN 1;
    END
END
