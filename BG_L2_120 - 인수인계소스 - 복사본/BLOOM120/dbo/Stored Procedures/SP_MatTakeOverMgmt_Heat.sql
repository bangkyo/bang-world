CREATE PROCEDURE [dbo].[SP_MatTakeOverMgmt_Heat]
 @P_INSU_GP  VARCHAR(5)
,@P_HEAT_NO  VARCHAR(8) = ''

AS

SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_MatTakeOverMgmt_Heat';
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

IF  @P_INSU_GP = 'Y' 
    BEGIN
         SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER (ORDER BY HEAT_NO ASC)) AS L_NUM 
               ,X.*
         FROM  (
            SELECT HEAT_NO
                  ,STEEL
                  ,(select cd_nm from tb_cm_com_cd where CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM
            FROM  TB_MAT_TAKE_OVER_HEAT A
            WHERE HEAT_NO  LIKE '%' + @P_HEAT_NO + '%'
            ) X; 
    END;     
ELSE
    BEGIN    
        SELECT * INTO #TMP_CCM_MAIN
          FROM (SELECT REPLACE(HEAT_NO,'-','') AS HEAT_NO
                      ,GRADE STEEL
                      ,'' ITEM
                      ,'' ITEM_SIZE
                      ,'' STEEL_TYPE
                      ,DAILY_DATE               AS MFG_DATE
                      ,LONG_PIECE + SHORT_PIECE AS MFG_PCS
                FROM   L2P120..LEVEL2.TB_CCM_MAIN
                WHERE  REPLACE(HEAT_NO,'-','') LIKE '%' + @P_HEAT_NO + '%' ) a;
      
      
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
        SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER (ORDER BY HEAT_NO ASC)) AS L_NUM 
              ,X.*
        FROM (
            SELECT HEAT_NO
                  ,STEEL
                  ,(select cd_nm from tb_cm_com_cd where CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM
            FROM  #TMP_CCM_MAIN A
            WHERE NOT EXISTS ( SELECT HEAT_NO FROM TB_MAT_TAKE_OVER_HEAT
                               WHERE  HEAT_NO = A.HEAT_NO )
            ) X;
    END;
END;
