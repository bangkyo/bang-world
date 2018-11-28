CREATE PROCEDURE [dbo].[SP_MatTakeOverMgmt_CAN]
 @P_INSU_GP  VARCHAR(5)
,@P_HEAT_NO  VARCHAR(8)
,@P_SURFACE_STAT  VARCHAR(20)
,@P_HEAT_SEQ  VARCHAR(4000)
,@P_USER_ID   VARCHAR(20)
,@P_PROC_STAT VARCHAR(3)     OUTPUT
,@P_PROC_MSG  VARCHAR(1000)  OUTPUT

AS

SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_MatTakeOverMgmt_CAN';
DECLARE @Err_Msg        VARCHAR(300); 
DECLARE @Err_CD         VARCHAR(20); 
DECLARE @Err_TP         VARCHAR(10); 

DECLARE @ERR_LINE       NUMERIC(10,0); 
DECLARE @ERR_NUMBER     NUMERIC(10,0);
DECLARE @REMARK         VARCHAR(20);

DECLARE @DELETE_CNT     NUMERIC(10)
DECLARE @INSERT_CNT     NUMERIC(10)
DECLARE @UPDATE_CNT     NUMERIC(10);

DECLARE @SQL            NVARCHAR(4000)
DECLARE @CUR_DATE       VARCHAR(8);

DECLARE @V_FR_DATE  VARCHAR(8)
DECLARE @V_TO_DATE  VARCHAR(8);
DECLARE @HEAT_SEQ_LIST VARCHAR(4000);
DECLARE @HEAT_SEQ_TMP  VARCHAR(10);
DECLARE @HEAT_SEQ_INT  INT;
DECLARE @TAKE_OVER_PCS INT;
DECLARE @ITEM          VARCHAR(2);
DECLARE @ITEM_SIZE     VARCHAR(4);
DECLARE @STEEL_TYPE    VARCHAR(3);

BEGIN
    
    SET @CUR_DATE = CONVERT(VARCHAR(8), GETDATE(), 112);
  BEGIN TRY
    BEGIN TRAN
------Heat순번 컴마로 이루어진것 Temp Table에 저장
------ TABLE #PARA_HEAT_SEQ
    CREATE TABLE #PARA_HEAT_SEQ
    ( HEAT_NO  VARCHAR(8),
      HEAT_SEQ INT );
      
    SET @HEAT_SEQ_LIST = @P_HEAT_SEQ;
    ----마지막에 컴마가 없는 경우 붙여 주기
    IF  RIGHT(@HEAT_SEQ_LIST,1) <> ','  AND LEN(@HEAT_SEQ_LIST) >= 1
        SET @HEAT_SEQ_LIST = @HEAT_SEQ_LIST + ',';
    ----컴마의 갯수 만큼 처리
    WHILE CHARINDEX(',', @HEAT_SEQ_LIST) > 0
    BEGIN
         SELECT @HEAT_SEQ_TMP  = Ltrim(SUBSTRING(@HEAT_SEQ_LIST, 1, CHARINDEX(',', @HEAT_SEQ_LIST) - 1));
         SELECT @HEAT_SEQ_LIST = SUBSTRING(@HEAT_SEQ_LIST, CHARINDEX(',', @HEAT_SEQ_LIST) + 1, LEN(@HEAT_SEQ_LIST));
         SET    @HEAT_SEQ_INT  = CONVERT(INT, @HEAT_SEQ_TMP,1);
         INSERT INTO #PARA_HEAT_SEQ ( HEAT_NO, HEAT_SEQ )
         SELECT @P_HEAT_NO, @HEAT_SEQ_INT; 
    END;
    -------------------------------------------------------------------
    ----트래킹에 진행중, 완료된것이 있으면 취소불가
    -------------------------------------------------------------------
    IF  EXISTS ( SELECT HEAT_SEQ FROM TB_RL_TM_TRACKING WHERE HEAT_NO = @P_HEAT_NO AND PROG_STAT IN ('RUN','END')
                 AND    HEAT_SEQ IN (SELECT HEAT_SEQ FROM #PARA_HEAT_SEQ WHERE HEAT_NO = @P_HEAT_NO) )
        BEGIN
            SET  @P_PROC_STAT = 'ERR';
            SET  @P_PROC_MSG  = '취소 불가[이미 실적이 발생했습니다.] 전산담당자에 문의바랍니다.';
            RAISERROR(@P_PROC_MSG, 16, 1);
        END;
    -------------------------------------------------------------------
    --스케줄에 작업상태가 변경되었거나 실적본수가 있으면 취소 불가 msg
    -------------------------------------------------------------------
    /*
    IF  EXISTS ( SELECT HEAT_NO FROM TB_SCHEDULE WHERE HEAT_NO = @P_HEAT_NO AND WORK_PCS > 0 )
        BEGIN
            --raiserror (15250, -1,-1);
            SET  @P_PROC_STAT = 'ERR';
            SET  @P_PROC_MSG  = '취소 불가[이미 실적이 발생했습니다.] 전산담당자에 문의바랍니다.';
            --ROLLBACK TRAN;
            --RETURN 1;
            RAISERROR('취소 불가[이미 실적이 발생했습니다.]', 16, 1);
        END;
    */
    -------------------------------------------------------------------
    --BLOOM별 인수정보 삭제 
    -------------------------------------------------------------------
    IF EXISTS ( SELECT HEAT_NO FROM TB_MAT_TAKE_OVER_BLOOM WHERE HEAT_NO = @P_HEAT_NO 
                AND HEAT_SEQ IN (SELECT HEAT_SEQ FROM #PARA_HEAT_SEQ WHERE HEAT_NO = @P_HEAT_NO) )
        BEGIN
            DELETE FROM TB_MAT_TAKE_OVER_BLOOM
            WHERE  HEAT_NO  = @P_HEAT_NO
            AND    HEAT_SEQ IN (SELECT HEAT_SEQ FROM #PARA_HEAT_SEQ WHERE HEAT_NO = @P_HEAT_NO); 
        END;     
    ------------------------------------------------------------------
    --HEAT 인수정보 삭제
    ------------------------------------------------------------------
    IF EXISTS ( SELECT HEAT_NO FROM TB_MAT_TAKE_OVER_HEAT WHERE HEAT_NO = @P_HEAT_NO )
        BEGIN
            DELETE FROM TB_MAT_TAKE_OVER_HEAT
            WHERE HEAT_NO  = @P_HEAT_NO
            AND   NOT EXISTS ( SELECT HEAT_NO FROM TB_MAT_TAKE_OVER_BLOOM WHERE HEAT_NO = @P_HEAT_NO ); 
        END;     
    ----본수 집계 및 규격 SELECT & SET
    ----BLOOM정보가 있는 경우 집계하고 갱신
    SET @TAKE_OVER_PCS = 0;
    IF EXISTS ( SELECT HEAT_NO FROM TB_MAT_TAKE_OVER_BLOOM WHERE HEAT_NO = @P_HEAT_NO )
    BEGIN
        SELECT @TAKE_OVER_PCS = COUNT(*)
              ,@ITEM          = MAX(ITEM)
              ,@ITEM_SIZE     = MAX(ITEM_SIZE)
              ,@STEEL_TYPE    = MAX(STEEL_TYPE)
        FROM   TB_MAT_TAKE_OVER_BLOOM
        WHERE  HEAT_NO  = @P_HEAT_NO;
       ----HEAT에 인수본수 갱신
        UPDATE TB_MAT_TAKE_OVER_HEAT
        SET    TAKE_OVER_PCS = @TAKE_OVER_PCS
              ,MODIFIER      = @P_USER_ID
              ,MOD_DDTT      = GETDATE()
        WHERE HEAT_NO  = @P_HEAT_NO;
        --스케줄 인수본수 갱신
        UPDATE TB_SCHEDULE
        SET    TAKE_OVER_PCS = @TAKE_OVER_PCS
              ,MODIFIER      = @P_USER_ID
              ,MOD_DDTT      = GETDATE()
        WHERE HEAT_NO  = @P_HEAT_NO;
    END;
    ----스케줄 삭제    
    IF  EXISTS ( SELECT HEAT_NO FROM TB_SCHEDULE WHERE HEAT_NO = @P_HEAT_NO AND @TAKE_OVER_PCS = 0 )
        BEGIN
            DELETE FROM TB_SCHEDULE
            WHERE  HEAT_NO  = @P_HEAT_NO;
        END;
    ----TRACKING정보 삭제
    IF  EXISTS ( SELECT HEAT_NO FROM TB_RL_TM_TRACKING WHERE HEAT_NO = @P_HEAT_NO AND HEAT_SEQ > @TAKE_OVER_PCS )
    BEGIN
        DELETE FROM TB_RL_TM_TRACKING
        WHERE  HEAT_NO  = @P_HEAT_NO
        AND    HEAT_SEQ > @TAKE_OVER_PCS;
    END;
    COMMIT TRAN;
    SET  @P_PROC_STAT = 'OK';
    SET  @P_PROC_MSG  = '정상처리 완료 !!!';
  END TRY
    BEGIN CATCH --//EXCEPTION 처리부분 : 쿼리문이 오류가 났을 경우 실행할 쿼리문

        ROLLBACK TRAN; -- 실패!

        --SET  @P_PROC_STAT = 'ERR';
        --SET  @P_PROC_MSG  = '처리중 오류가 발생했습니다. 전산담당자에 문의바랍니다.';
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

END;    
    