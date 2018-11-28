CREATE PROCEDURE [dbo].[SP_DAILY_JOB]

AS
--------------------------------------------------------
---- 매일 07시 처리
---- L2 인터페이스 수신( 강종, 규격, 단위중량 )
---- L2 인터페이스 송신( 그라인딩 실적 )
---- 설비점검계획편성
--------------------------------------------------------
SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_DAILY_JOB';
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

    SET  @CUR_DATE = CONVERT(VARCHAR(8),GETDATE()-6/24,112);
    SET  @P_STAT   = '';
    SET  @P_MSG    = '';
    ----강종코드정보
    EXEC dbo.SP_IF_GRADE_RCV;
    
    ----규격코드정보
    EXEC dbo.SP_IF_ITEM_RCV;
    
    ----단위중량
    EXEC dbo.SP_IF_UNITWGT_RCV;
    
    ----그라인딩 실적
    EXEC dbo.SP_IF_GRD_SND;
    
    ----설비점검계획생성
    EXEC dbo.SP_EQP_CHECK_PLN_CRE;
    
    ----전력량 I/F 일 1회
    EXEC dbo.SP_IF_ELEC_USE_SELECT;
    
    COMMIT TRAN;
END
