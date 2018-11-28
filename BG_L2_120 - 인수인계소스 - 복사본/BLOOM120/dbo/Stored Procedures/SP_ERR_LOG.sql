CREATE PROCEDURE [dbo].[SP_ERR_LOG] 
                 @PGM_ID     VARCHAR(50)
                ,@ERR_CD     VARCHAR(20)
                ,@ERR_TP     varchar(10)
                ,@ERR_MSG    varchar(300)
AS

SET NOCOUNT ON;

DECLARE @SEQ_NO bigint;

set TRANSACTION ISOLATION LEVEL READ COMMITTED;

BEGIN TRANSACTION;
    select @SEQ_NO = NEXT VALUE FOR ERR_LOG_SEQ_NO;
    INSERT INTO  TB_CM_ERR_LOG (
                 SEQ_NO
                ,PGM_ID 
                ,ERR_CD
                ,ERR_TP
                ,ERR_MSG 
                ,REG_DDTT 
               ) values ( 
                 @SEQ_NO
                ,@PGM_ID 
                ,@ERR_CD
                ,@ERR_TP
                ,@ERR_MSG 
                ,GETDATE()
               );

COMMIT TRANSACTION;
