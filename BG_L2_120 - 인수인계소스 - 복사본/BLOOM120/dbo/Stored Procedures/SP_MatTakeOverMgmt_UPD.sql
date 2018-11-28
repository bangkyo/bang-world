CREATE PROCEDURE [dbo].[SP_MatTakeOverMgmt_UPD]
 @P_INSU_GP  VARCHAR(5)
,@P_HEAT_NO  VARCHAR(8)
,@P_SURFACE_STAT  VARCHAR(20)
,@P_HEAT_SEQ  VARCHAR(4000)
,@P_USER_ID   VARCHAR(20)
,@P_PROC_STAT VARCHAR(3)     OUTPUT
,@P_PROC_MSG  VARCHAR(1000)  OUTPUT

AS
-----------------------------
----인수처리 SP
-----------------------------

SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_MatTakeOverMgmt_UPD';
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
DECLARE @S_PROC_STAT   VARCHAR(3);
DECLARE @S_PROC_MSG    VARCHAR(1000);
DECLARE @MFG_PCS INT;
DECLARE @S_MFG_PCS INT;
DECLARE @S_HEAT_NO     VARCHAR(6);
DECLARE @S_GRADE       VARCHAR(10);
DECLARE @S_ITEM_CODE   VARCHAR(6);
DECLARE @S_STEEL_TYPE  VARCHAR(3);
DECLARE @S_DAILY_DATE  VARCHAR(8);

BEGIN
    
    SET @CUR_DATE = CONVERT(VARCHAR(8), GETDATE()-6/24, 112);
    SET  @ERR_TP  = 'INFO';
    SET  @ERR_MSG = @P_INSU_GP + '_' + @P_HEAT_NO + '_' + @P_SURFACE_STAT + '_' + @P_HEAT_SEQ;
    EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @ERR_MSG;

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
    ------------------------------------------------------------------
    --HEAT 인수정보 INSERT ( 있는 경우는 갱신 : 표면상태 )
    ------------------------------------------------------------------
    IF EXISTS ( SELECT HEAT_NO FROM TB_MAT_TAKE_OVER_HEAT WHERE HEAT_NO = @P_HEAT_NO )
        BEGIN
            UPDATE TB_MAT_TAKE_OVER_HEAT
            SET    SURFACE_STAT = @P_SURFACE_STAT
                  ,MODIFIER     = @P_USER_ID
                  ,MOD_DDTT     = GETDATE()
            WHERE HEAT_NO  = @P_HEAT_NO;
            SELECT  @S_HEAT_NO    = HEAT_NO
                   ,@S_GRADE      = STEEL
                   ,@S_ITEM_CODE  = ITEM + ITEM_SIZE
                   ,@S_DAILY_DATE = MFG_DATE
                   ,@S_MFG_PCS    = MFG_PCS
            FROM   TB_MAT_TAKE_OVER_HEAT
            WHERE  HEAT_NO  = @P_HEAT_NO;
        END;     
    ELSE
        BEGIN
            ----BLOOM생산본수 정확한것 가져오기
            ----SELECT  @S_MFG_PCS  = SUM(CUT_PIECE_1) + SUM(CUT_PIECE_2)
            ----FROM    L2P120..LEVEL2.TB_CCM_CAST 
            ----WHERE   HEAT_NO  = SUBSTRING(@P_HEAT_NO,1,1) + '-' + SUBSTRING(@P_HEAT_NO,2,5);
            SELECT @S_HEAT_NO = REPLACE(HEAT_NO,'-','') 
                  ,@S_GRADE   = GRADE 
                  ,@S_ITEM_CODE = ITEM_CODE
                  ,@S_DAILY_DATE = DAILY_DATE 
                  ,@S_MFG_PCS  = (select SUM(CUT_PIECE_1) + SUM(CUT_PIECE_2)  FROM L2P120..LEVEL2.TB_CCM_CAST WHERE HEAT_NO = A.HEAT_NO)
            FROM   L2P120..LEVEL2.TB_CCM_MAIN A
            WHERE  HEAT_NO = SUBSTRING(@P_HEAT_NO,1,1) + '-' + SUBSTRING(@P_HEAT_NO,2,5); 
            
            INSERT INTO TB_MAT_TAKE_OVER_HEAT
                   ( HEAT_NO
                    ,STEEL
                    ,ITEM
                    ,ITEM_SIZE
                    ,STEEL_TYPE
                    ,MFG_DATE
                    ,MFG_PCS
                    ,TAKE_OVER_PCS
                    ,TAKE_OVER_DATE
                    ,SURFACE_STAT
                    ,REGISTER
                    ,REG_DDTT  )
            SELECT   @S_HEAT_NO 
                    ,@S_GRADE
                    ,SUBSTRING(@S_ITEM_CODE,1,2) ITEM
                    ,SUBSTRING(@S_ITEM_CODE,3,4) ITEM_SIZE
                    ,(SELECT COLUMNA FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = @S_GRADE )
                    ,@S_DAILY_DATE
                    ,@S_MFG_PCS
                    ,0
                    ,@CUR_DATE
                    ,@P_SURFACE_STAT
                    ,@P_USER_ID
                    ,GETDATE();
        END;
    -------------------------------------------------------------------
    --BLOOM별 인수정보 INSERT 
    -------------------------------------------------------------------
    IF EXISTS ( SELECT HEAT_NO FROM TB_MAT_TAKE_OVER_BLOOM WHERE HEAT_NO = @P_HEAT_NO 
                AND HEAT_SEQ IN (SELECT HEAT_SEQ FROM #PARA_HEAT_SEQ WHERE HEAT_NO = @P_HEAT_NO) )
        BEGIN
            DELETE FROM TB_MAT_TAKE_OVER_BLOOM
            WHERE  HEAT_NO  = @P_HEAT_NO
            AND    HEAT_SEQ IN (SELECT HEAT_SEQ FROM #PARA_HEAT_SEQ WHERE HEAT_NO = @P_HEAT_NO); 
        END;     
    -----------------------------------------------
    ----BLOOM본수정보 생성 만큼 만든다.
    ---- TEMP TABLE에 본수정보 INSERT
    CREATE TABLE #tmp_HEAT_SEQ
         ( HEAT_NO  VARCHAR(6),
           HEAT_SEQ INT );
    SET    @MFG_PCS      = @S_MFG_PCS
    SET    @HEAT_SEQ_INT = 0;
    WHILE  @MFG_PCS > @HEAT_SEQ_INT 
    BEGIN
        SET  @HEAT_SEQ_INT = @HEAT_SEQ_INT + 1;
        INSERT INTO #tmp_HEAT_SEQ ( HEAT_NO, HEAT_SEQ )
        SELECT @P_HEAT_NO, @HEAT_SEQ_INT; 
    END;   
    -----------------------------------------------
    --BLOOM INSERT
    -----------------------------------------------
    BEGIN
        INSERT INTO TB_MAT_TAKE_OVER_BLOOM
                ( HEAT_NO
                ,HEAT_SEQ
                ,STEEL
                ,STRAND_NO
                ,ITEM
                ,ITEM_SIZE
                ,STEEL_TYPE
                ,MFG_DATE
                ,TAKE_OVER_DATE
                ,THEORY_LENGTH
                ,REAL_LENGTH
                ,Marking_Code
                ,REGISTER
                ,REG_DDTT  )
        SELECT   A.HEAT_NO   AS HEAT_NO
                ,A.HEAT_SEQ  AS HEAT_SEQ
                ,@S_GRADE       AS STEEL
                ,C.STRAND_NO
                ,SUBSTRING(@S_ITEM_CODE,1,2)  AS ITEM
                ,SUBSTRING(@S_ITEM_CODE,3,4)  AS ITEM_SIZE
                ,(SELECT COLUMNA FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = @S_GRADE )
                ,@S_DAILY_DATE as mfg_date
                ,@CUR_DATE
                ,C.LEGNTH
                ,C.LEGNTH
                ,C.MARKING_CODE
                ,@P_USER_ID
                ,GETDATE()
        FROM   #tmp_HEAT_SEQ   A 
               LEFT OUTER JOIN
               (SELECT HEAT_NO, HEAT_CNT, STRAND_NO, MARKING_CODE, LEGNTH
                FROM   L2P120..LEVEL2.TB_CCM_BLOOM
                WHERE  HEAT_NO = @P_HEAT_NO) C
                ON A.HEAT_NO = C.HEAT_NO AND A.HEAT_SEQ = C.HEAT_CNT
        WHERE A.HEAT_NO  = @P_HEAT_NO
        AND   A.HEAT_SEQ IN ( SELECT HEAT_SEQ FROM #PARA_HEAT_SEQ WHERE HEAT_NO = @P_HEAT_NO );
        ----PRINT @@ROWCOUNT;
        IF  @@ERROR <> 0
            BEGIN
                SET  @P_PROC_STAT = 'ERR';
                SET  @P_PROC_MSG  = 'BLOOM정보 INSERT 오류.';
                RAISERROR('BLOOM정보 INSERT 오류.', 16, 1);
            END;
    END;
    ----본수 집계 및 규격 SELECT & SET
    BEGIN
        SELECT @TAKE_OVER_PCS = COUNT(*)
              ,@ITEM          = MAX(ITEM)
              ,@ITEM_SIZE     = MAX(ITEM_SIZE)
              ,@STEEL_TYPE    = MAX(STEEL_TYPE)
        FROM   TB_MAT_TAKE_OVER_BLOOM
        WHERE  HEAT_NO  = @P_HEAT_NO;
    END;
    ----HEAT에 인수본수 갱신
    BEGIN
        UPDATE TB_MAT_TAKE_OVER_HEAT 
        SET    TAKE_OVER_PCS = @TAKE_OVER_PCS
              ,MODIFIER      = @P_USER_ID
              ,MOD_DDTT      = GETDATE()
        WHERE HEAT_NO  = @P_HEAT_NO;
    END;

    -------------------------------------------------------------------
    --스케줄 자동 편성
    -------------------------------------------------------------------
    IF EXISTS ( SELECT HEAT_NO FROM TB_SCHEDULE WHERE HEAT_NO = @P_HEAT_NO )
       BEGIN
            UPDATE  TB_SCHEDULE
            SET    TAKE_OVER_PCS = @TAKE_OVER_PCS
                  ,MODIFIER      = @P_USER_ID
                  ,MOD_DDTT      = GETDATE()
            WHERE HEAT_NO  = @P_HEAT_NO;
       END;
    ELSE
       BEGIN
            INSERT INTO TB_SCHEDULE (
                   HEAT_NO
                  ,WORK_PLN_DATE
                  ,WORK_SEQ
                  ,STEEL
                  ,ITEM
                  ,ITEM_SIZE
                  ,STEEL_TYPE
                  ,MFG_DATE
                  ,TAKE_OVER_DATE
                  ,TAKE_OVER_PCS
                  ,WORK_STAT
                  ,REGISTER
                  ,REG_DDTT  )
            SELECT A.HEAT_NO
                  ,TAKE_OVER_DATE
                  ,(SELECT isnull(MAX(WORK_SEQ),0) + 1 FROM TB_SCHEDULE WHERE WORK_PLN_DATE = A.TAKE_OVER_DATE)
                  ,STEEL
                  ,ITEM
                  ,ITEM_SIZE
                  ,STEEL_TYPE
                  ,MFG_DATE
                  ,TAKE_OVER_DATE
                  ,TAKE_OVER_PCS
                  ,'WAT'
                  ,@P_USER_ID
                  ,GETDATE()
            FROM   TB_MAT_TAKE_OVER_HEAT A
            WHERE  HEAT_NO  = @P_HEAT_NO;
       END;   
    ----트래킹정보 편성
    EXEC dbo.SP_TRACKING_CRE @P_HEAT_NO, @P_USER_ID, @S_PROC_STAT output, @S_PROC_MSG output;
    IF  @S_PROC_STAT = 'ERR'
        BEGIN
            SET  @P_PROC_STAT = 'ERR';
            SET  @P_PROC_MSG  = @S_PROC_MSG;
            EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @P_PROC_STAT, @P_PROC_MSG;
        END;
    COMMIT TRAN;
    SET @P_PROC_STAT = 'OK';
    SET  @P_PROC_MSG  = '정상처리 완료 !!!';
    
  END TRY
    BEGIN CATCH --//EXCEPTION 처리부분 : 쿼리문이 오류가 났을 경우 실행할 쿼리문

        ROLLBACK TRAN; -- 실패!

        SET  @P_PROC_STAT = 'ERR';
        SET  @P_PROC_MSG  = '처리중 오류가 발생했습니다. 전산담당자에 문의바랍니다.';
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
    
    