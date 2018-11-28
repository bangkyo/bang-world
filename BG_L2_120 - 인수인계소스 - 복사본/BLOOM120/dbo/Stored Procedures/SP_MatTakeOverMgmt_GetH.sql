CREATE PROCEDURE [dbo].[SP_MatTakeOverMgmt_GetH]
 @P_INSU_GP  VARCHAR(5)
,@P_FR_DATE  VARCHAR(8)
,@P_TO_DATE  VARCHAR(8)
,@P_HEAT_NO  VARCHAR(8) = ''

AS

SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_MatTakeOverMgmt_GetH';
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
    SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER (ORDER BY HEAT_NO ASC)) AS L_NUM 
          ,HEAT_NO
          ,STEEL
          ,(select cd_nm from tb_cm_com_cd where CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM
          ,ITEM
          ,ITEM_SIZE
          ,(select cd_nm from tb_cm_com_cd where CATEGORY = 'ITEM_SIZE' AND CD_ID = A.ITEM + A.ITEM_SIZE) AS ITEM_SIZE_NM
          ,STEEL_TYPE
          ,CONVERT(DATE, MFG_DATE) AS MFG_DATE
          ,MFG_PCS
          ,CONVERT(DATE, TAKE_OVER_DATE) AS TAKE_OVER_DATE
          ,ISNULL(TAKE_OVER_PCS, 0)      AS TAKE_OVER_PCS
          ,SURFACE_STAT
    FROM  TB_MAT_TAKE_OVER_HEAT A
    WHERE HEAT_NO  LIKE '%' + @P_HEAT_NO + '%'
    AND   TAKE_OVER_DATE  >= @V_FR_DATE
    AND   TAKE_OVER_DATE  <= @V_TO_DATE; 
    END;     
ELSE
    BEGIN    
        SELECT * INTO #tmp_CCM_MAIN
          FROM (SELECT REPLACE(HEAT_NO,'-','') AS HEAT_NO
                      ,GRADE STEEL
                      ,SUBSTRING(ITEM_CODE,1,2)  ITEM
                      ,SUBSTRING(ITEM_CODE,3,4)  ITEM_SIZE
                      ,'' STEEL_TYPE
                      ,DAILY_DATE               AS MFG_DATE
                      --,LONG_PIECE + SHORT_PIECE  AS MFG_PCS
                      ,(SELECT SUM(CUT_PIECE_1) + SUM(CUT_PIECE_2) FROM L2P120..LEVEL2.TB_CCM_CAST WHERE HEAT_NO = A.HEAT_NO) AS MFG_PCS
                FROM   L2P120..LEVEL2.TB_CCM_MAIN A
                WHERE  REPLACE(HEAT_NO,'-','') LIKE @P_HEAT_NO + '%'
                AND    DAILY_DATE  >= @V_FR_DATE
                AND    DAILY_DATE  <= @V_TO_DATE ) a;
      
      
/*    OPENQUERY(L2P120, ' SELECT  REPLACE(HEAT_NO,''-'','''') AS HEAT_NO
                                      ,GRADE STEEL
                                      ,'''' ITEM_SIZE
                                      ,DAILY_DATE               AS MFG_DATE
                                      ,LONG_PIECE + SHORT_PIECE AS MFG_PCS
                               FROM   TB_CCM_MAIN
                               WHERE  REPLACE(HEAT_NO,''-'','''') LIKE @P_HEAT_NO || ''%''
                               AND    DAILY_DATE  >= @V_FR_DATE
                               AND    DAILY_DATE  <= @V_TO_DATE ');
 */   
    SELECT CONVERT(VARCHAR,ROW_NUMBER() OVER (ORDER BY HEAT_NO ASC)) AS L_NUM 
          ,HEAT_NO
          ,STEEL
          ,(select cd_nm from tb_cm_com_cd where CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM
          ,ITEM
          ,ITEM_SIZE
          ,(select cd_nm from tb_cm_com_cd where CATEGORY = 'ITEM_SIZE' AND CD_ID = A.ITEM + a.ITEM_SIZE) AS ITEM_SIZE_NM
          ,STEEL_TYPE
          ,CONVERT(DATE, MFG_DATE) AS MFG_DATE
          ,MFG_PCS
          ,'' TAKE_OVER_DATE
          ,(SELECT ISNULL(TAKE_OVER_PCS, 0) AS TAKE_OVER_PCS  FROM TB_MAT_TAKE_OVER_HEAT  WHERE  HEAT_NO = A.HEAT_NO) AS TAKE_OVER_PCS
          ,'' SURFACE_STAT
    FROM  #tmp_CCM_MAIN A
    WHERE NOT EXISTS ( SELECT HEAT_NO FROM TB_MAT_TAKE_OVER_HEAT
                       WHERE  HEAT_NO        = A.HEAT_NO
                       AND    TAKE_OVER_PCS >= A.MFG_PCS );
    END;
END;
