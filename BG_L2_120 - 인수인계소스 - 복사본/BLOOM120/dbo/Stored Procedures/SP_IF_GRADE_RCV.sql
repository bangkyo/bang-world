CREATE PROCEDURE dbo.SP_IF_GRADE_RCV

AS

SET NOCOUNT ON;

------//프로시져 내부 변수정의//------
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_IF_GRADE_RCV';
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

DECLARE @CD_ID          VARCHAR(22);
DECLARE @CD_NM          VARCHAR(100);
DECLARE @COLUMNA        VARCHAR(100);
DECLARE @COLUMN1        NUMERIC(15,5);

BEGIN

    BEGIN TRY
        
        SET @BEF_DATE = GETDATE() - 1;
        BEGIN TRAN

            DECLARE cur_C1 CURSOR FOR
                    SELECT GRADE
                          ,GRADE_DESC
                          ,GRP_GRADE
                          ,SOLID_VALUE  
                    FROM   L2P120..LEVEL2.TB_GRADE_MASTER A
                    WHERE  ( CREATE_TIME >= @BEF_DATE OR UPDATE_TIME >= @BEF_DATE )
                    OR     NOT EXISTS ( SELECT  CD_ID FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.GRADE );
            OPEN cur_C1
            
            FETCH NEXT FROM cur_C1 INTO @CD_ID  
                                       ,@CD_NM  
                                       ,@COLUMNA
                                       ,@COLUMN1;
            WHILE(@@FETCH_STATUS = 0)
                BEGIN
                
                    IF EXISTS ( SELECT  CD_ID FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = @CD_ID )
                        BEGIN
                            SET @UPDATE_CNT = @UPDATE_CNT + 1;
                            UPDATE TB_CM_COM_CD
                            SET    CD_NM   = @CD_NM
                                  ,COLUMNA = @COLUMNA
                                  ,COLUMN1 = @COLUMN1
                            WHERE CATEGORY = 'STEEL'
                            AND   CD_ID    = @CD_ID;
                        END
                    ELSE
                        BEGIN
                            SET @INSERT_CNT = @INSERT_CNT + 1;
                            INSERT INTO TB_CM_COM_CD
                                  (CATEGORY
                                  ,CD_ID
                                  ,CD_NM
                                  ,COLUMNA
                                  ,COLUMN1
                                  ,USE_YN 
                                 ) VALUES (
                                  'STEEL'
                                  ,@CD_ID
                                  ,@CD_NM
                                  ,@COLUMNA
                                  ,@COLUMN1
                                  ,'Y' );
                                  
                        END;
                    FETCH NEXT FROM cur_C1 INTO @CD_ID  
                                               ,@CD_NM  
                                               ,@COLUMNA
                                               ,@COLUMN1;
                END
            CLOSE cur_C1;
            DEALLOCATE cur_C1;

        COMMIT TRAN;
        SET  @ERR_TP  = 'INFO';
        SET  @ERR_CD  = '정상'; 
        SET  @ERR_MSG = 'INSERT_CNT : ' + CONVERT(VARCHAR(5),@INSERT_CNT) + '  UPDATE_CNT : ' + CONVERT(VARCHAR(5),@UPDATE_CNT);
        EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @ERR_MSG;
        
    END TRY
    BEGIN CATCH --//EXCEPTION 처리부분 : 쿼리문이 오류가 났을 경우 실행할 쿼리문

        ROLLBACK TRAN; -- 실패!
 
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
