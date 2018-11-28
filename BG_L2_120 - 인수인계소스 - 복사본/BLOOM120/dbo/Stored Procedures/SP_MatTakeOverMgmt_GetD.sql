CREATE PROCEDURE [dbo].[SP_MatTakeOverMgmt_GetD]
 @P_INSU_GP  VARCHAR(5)
,@P_FR_DATE  VARCHAR(8)
,@P_TO_DATE  VARCHAR(8)
,@P_HEAT_NO  VARCHAR(8) = ''

AS

SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_MatTakeOverMgmt_GetD';
DECLARE @Err_Msg        VARCHAR(300); 
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

DECLARE @S_HEAT_NO     VARCHAR(6);
DECLARE @S_GRADE       VARCHAR(10);
DECLARE @S_ITEM_CODE   VARCHAR(6);
DECLARE @S_STEEL_TYPE  VARCHAR(3);
DECLARE @S_DAILY_DATE  VARCHAR(8);
DECLARE @S_MFG_PCS     INT;

DECLARE @HEAT_SEQ_INT  INT = 0;
DECLARE @MFG_PCS       INT;       

BEGIN
    
    SET @CUR_DATE = CONVERT(VARCHAR(8), GETDATE(), 112);
    IF  len(@P_HEAT_NO) > 5  
        BEGIN
            SET @V_FR_DATE = '1';
            SET @V_TO_DATE = '99991231';
        END;
    ELSE
        BEGIN
            SET @V_FR_DATE = @P_FR_DATE;
            SET @V_TO_DATE = @P_TO_DATE;
        END;

IF  @P_INSU_GP = 'Y' 
    BEGIN
        SELECT CONVERT(VARCHAR,ROW_NUMBER() OVER (ORDER BY A.HEAT_NO ASC)) AS L_NUM 
              ,'True' AS SEL
              ,HEAT_NO
              ,HEAT_SEQ
              ,STRAND_NO
              ,STEEL
              ,(select cd_nm from tb_cm_com_cd where CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM
              ,ITEM
              ,ITEM_SIZE
              ,(select cd_nm from tb_cm_com_cd where CATEGORY = 'ITEM_SIZE' AND CD_ID = A.ITEM + a.ITEM_SIZE) AS ITEM_SIZE_NM
              ,CONVERT(DATE, MFG_DATE) AS MFG_DATE
              ,MARKING_CODE
        FROM  TB_MAT_TAKE_OVER_BLOOM A
        WHERE HEAT_NO  = @P_HEAT_NO; 
    END;     
ELSE
    BEGIN 
        
        SELECT @S_HEAT_NO = REPLACE(HEAT_NO,'-','') 
              ,@S_GRADE   = GRADE 
              ,@S_ITEM_CODE = ITEM_CODE
              ,@S_DAILY_DATE = DAILY_DATE 
              ----,@S_MFG_PCS    = (SELECT SUM(CUT_PIECE_1) + SUM(CUT_PIECE_2) FROM L2P120..LEVEL2.TB_CCM_CAST WHERE HEAT_NO = A.HEAT_NO) 
        FROM   L2P120..LEVEL2.TB_CCM_MAIN A
        WHERE  HEAT_NO = SUBSTRING(@P_HEAT_NO,1,1) + '-' + SUBSTRING(@P_HEAT_NO,2,5); 
        ----CAST에서 BLOOM본수
        SELECT @S_MFG_PCS = SUM(CUT_PIECE_1) + SUM(CUT_PIECE_2) 
        FROM   L2P120..LEVEL2.TB_CCM_CAST 
        WHERE  HEAT_NO = SUBSTRING(@P_HEAT_NO,1,1) + '-' + SUBSTRING(@P_HEAT_NO,2,5);
        
        SELECT * INTO #tmp_CCM_BLOOM
        FROM   (SELECT HEAT_NO, HEAT_CNT, STRAND_NO, MARKING_CODE
                FROM   L2P120..LEVEL2.TB_CCM_BLOOM
                WHERE  HEAT_NO = @P_HEAT_NO ) A;
        
        ----속도 개선 활동
        WITH TMP1( HEAT_NO, HEAT_SEQ ) AS
             ( SELECT @P_HEAT_NO  HEAT_NO, 1  HEAT_SEQ
               UNION ALL
               SELECT HEAT_NO, HEAT_SEQ + 1
               FROM   TMP1
               WHERE  HEAT_SEQ + 1 <= @S_MFG_PCS
             )
        SELECT * INTO #tmp_HEAT_SEQ
        FROM   ( SELECT * FROM TMP1 ) A;
             
        /*  위의 로직으로 수정
        CREATE TABLE #tmp_HEAT_SEQ
             ( HEAT_NO  VARCHAR(6),
               HEAT_SEQ INT );
        SET    @MFG_PCS  = @S_MFG_PCS
        
        WHILE  @MFG_PCS > @HEAT_SEQ_INT 
        BEGIN
            SET  @HEAT_SEQ_INT = @HEAT_SEQ_INT + 1;
            INSERT INTO #tmp_HEAT_SEQ ( HEAT_NO, HEAT_SEQ )
            SELECT @P_HEAT_NO, @HEAT_SEQ_INT; 
        END;   
        */
        SELECT L_NUM
               ,SEL
               ,HEAT_NO
               ,HEAT_SEQ
               ,STRAND_NO
               ,STEEL
               ,STEEL_NM
               ,ITEM
               ,ITEM_SIZE
               ,ITEM_SIZE_NM
               ,MFG_DATE
               ,MARKING_CODE
        FROM  (
            SELECT CONVERT(VARCHAR,ROW_NUMBER() OVER (ORDER BY A.HEAT_NO ASC)) AS L_NUM 
                  ,'False' AS SEL
                  ,A.HEAT_NO      AS HEAT_NO
                  ,A.HEAT_SEQ     AS HEAT_SEQ
                  ,C.STRAND_NO    AS STRAND_NO
                  ,@S_GRADE       AS STEEL
                  ,(select cd_nm from tb_cm_com_cd where CATEGORY = 'STEEL' AND CD_ID = @S_GRADE) AS STEEL_NM
                  ,SUBSTRING(@S_ITEM_CODE,1,2)  AS ITEM
                  ,SUBSTRING(@S_ITEM_CODE,3,4)  AS ITEM_SIZE
                  ,(select cd_nm from tb_cm_com_cd where CATEGORY = 'ITEM_SIZE' AND CD_ID = @S_ITEM_CODE) AS ITEM_SIZE_NM
                  ,CONVERT(DATE, @S_DAILY_DATE) AS MFG_DATE
                  ,C.MARKING_CODE AS MARKING_CODE
                  ,@S_MFG_PCS     AS HEAT_PCS
            FROM   #tmp_HEAT_SEQ   A 
                   LEFT OUTER JOIN
                   #tmp_CCM_BLOOM  C
                    ON A.HEAT_NO = C.HEAT_NO AND A.HEAT_SEQ = C.HEAT_CNT
            WHERE A.HEAT_NO = @P_HEAT_NO 
            AND   NOT EXISTS ( SELECT HEAT_SEQ FROM TB_MAT_TAKE_OVER_BLOOM WHERE HEAT_NO = A.HEAT_NO AND HEAT_SEQ = A.HEAT_SEQ )
             ) A;
    END;
END;

