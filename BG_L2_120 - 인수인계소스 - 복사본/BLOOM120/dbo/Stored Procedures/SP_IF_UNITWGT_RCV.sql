CREATE PROCEDURE dbo.SP_IF_UNITWGT_RCV

AS

SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_IF_UNITWGT_RCV';
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
DECLARE @BEF_DATE       DATETIME;

DECLARE @ITEM           VARCHAR(2);
DECLARE @ITEM_SIZE      VARCHAR(4);
DECLARE @STEEL_TYPE     VARCHAR(3);
DECLARE @UOM_WEIGHT     NUMERIC(15,5);
DECLARE @TMP1           VARCHAR(100);
DECLARE @TMP2           VARCHAR(100);


BEGIN

    BEGIN TRY
        
        SET @BEF_DATE = GETDATE() - 1;
        BEGIN TRAN
            IF EXISTS ( SELECT ITEM FROM TB_CM_UOM_WGT )
                BEGIN
                    DELETE FROM TB_CM_UOM_WGT;
                END;

            DECLARE cur_C1 CURSOR FOR
                    SELECT ITEM
                          ,ITEM_SIZE
                          ,STEEL_TYPE
                          ,UOM_WEIGHT 
                    FROM   L2P120..LEVEL2.TB_UOM_WEIGHT;
                    
            OPEN cur_C1
            
            FETCH NEXT FROM cur_C1 INTO @ITEM
                                       ,@ITEM_SIZE
                                       ,@STEEL_TYPE  
                                       ,@UOM_WEIGHT;
            WHILE(@@FETCH_STATUS = 0)
                BEGIN
                
                    IF EXISTS ( SELECT  ITEM FROM TB_CM_UOM_WGT 
                                WHERE ITEM = @ITEM AND ITEM_SIZE = @ITEM_SIZE AND STEEL_TYPE = @STEEL_TYPE )
                        BEGIN
                            SET @UPDATE_CNT = @UPDATE_CNT + 1;
                            UPDATE TB_CM_UOM_WGT
                            SET    UOM_WGT = @UOM_WEIGHT
                            WHERE ITEM = @ITEM AND ITEM_SIZE = @ITEM_SIZE AND STEEL_TYPE = @STEEL_TYPE;
                        END
                    ELSE
                        BEGIN
                            SET @INSERT_CNT = @INSERT_CNT + 1;
                            INSERT INTO TB_CM_UOM_WGT
                                  (ITEM
                                  ,ITEM_SIZE
                                  ,STEEL_TYPE
                                  ,UOM_WGT
                                  ,REGISTER
                                  ,REG_DDTT 
                                 ) VALUES (
                                   @ITEM
                                  ,@ITEM_SIZE
                                  ,@STEEL_TYPE  
                                  ,@UOM_WEIGHT
                                  ,'SYSTEM' 
                                  ,GETDATE());
                                  
                        END;
                    FETCH NEXT FROM cur_C1 INTO @ITEM
                                               ,@ITEM_SIZE
                                               ,@STEEL_TYPE  
                                               ,@UOM_WEIGHT;;
                END
            CLOSE cur_C1;
            DEALLOCATE cur_C1;
            ----규격명칭 일괄갱신
            BEGIN
                UPDATE TB_CM_UOM_WGT
                SET    ITEM_SIZE_NM = B.CD_NM
                FROM   TB_CM_UOM_WGT A
                    INNER JOIN TB_CM_COM_CD B
                    ON (A.ITEM + A.ITEM_SIZE = B.CD_ID
                        AND B.CATEGORY = 'ITEM_SIZE');    
            END

        COMMIT TRAN;
        SET  @ERR_TP  = 'INFO';
        SET  @ERR_CD  = '정상'; 
        SET  @ERR_MSG = 'INSERT_CNT : ' + CONVERT(VARCHAR(5),@INSERT_CNT) + '  UPDATE_CNT : ' + CONVERT(VARCHAR(5),@UPDATE_CNT);
        EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @ERR_MSG;
    END TRY
    BEGIN CATCH --//EXCEPTION 처리부분 : 쿼리문이 오류가 났을 경우 실행할 쿼리문

        ROLLBACK TRAN -- 실패!
 
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
