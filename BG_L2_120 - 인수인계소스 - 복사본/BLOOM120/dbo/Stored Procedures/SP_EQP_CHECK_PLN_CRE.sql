CREATE PROCEDURE [dbo].[SP_EQP_CHECK_PLN_CRE]

AS
/*---------------------------------------------------------
--설비점검계획생성
--
--처리내용 
--1. 설비점검주기(W)를 기준으로 편성
--   (마지막 점검한 일자를 기준으로) 처음은 시작일자 기준
--   커서 선언하여 한건씩 FETCH하여
--   설비점검실적(TB_EQP_CHECK_WR) 생성
----------------------------------------------------------*/
DECLARE @ERR_PGM        VARCHAR(50)   = 'SP_EQP_CHECK_PLN_CRE';
DECLARE @Err_Msg        VARCHAR(300); 
DECLARE @Err_CD         VARCHAR(20); 
DECLARE @Err_TP         VARCHAR(10); 
DECLARE @ERR_LINE       NUMERIC(10,0); 
DECLARE @ERR_NUMBER     NUMERIC(10,0);
DECLARE @REMARK         VARCHAR(20);

DECLARE @W_WORK_DATE    VARCHAR(8);

DECLARE @EQP_CD           VARCHAR(10);
DECLARE @ITEM_CD          VARCHAR(10);
DECLARE @CHECK_SEQ        NUMERIC(10);
DECLARE @EQP_NM           VARCHAR(50);
DECLARE @ROUTING_NM       VARCHAR(30);
DECLARE @CHECK_ITEM       VARCHAR(100);
DECLARE @CHECK_GAP        NUMERIC(4);
DECLARE @CHECK_CYCLE      VARCHAR(5);
DECLARE @CHECK_START_DATE VARCHAR(8);
DECLARE @CHECK_WR_DATE    VARCHAR(8);
DECLARE @DATA_GP          VARCHAR(5);

DECLARE @P_PROC_STAT VARCHAR(3)   ;
DECLARE @P_PROC_MSG  VARCHAR(1000);

DECLARE @T_DATE   DATE;
DECLARE @C_DATE   DATE;

DECLARE @T_CALC   INT;

        
BEGIN
    BEGIN TRAN

    SET @W_WORK_DATE = CONVERT(VARCHAR(8), GETDATE(), 112);

    DECLARE CUR_C1 CURSOR  FOR
        SELECT  A.EQP_CD
               ,A.ITEM_CD
               ,0  AS CHECK_SEQ
               ,B.EQP_NM
               ,B.ROUTING_NM
               ,A.CHECK_ITEM
               ,ISNULL(A.CHECK_GAP,1) AS CHECK_GAP
               ,A.CHECK_CYCLE
               ,A.CHECK_START_DATE
               ,A.CHECK_START_DATE  CHECK_WR_DATE
               ,'1'   AS DATA_GP       
        FROM    TB_EQP_CHECK_ITEM A 
               ,TB_EQP_INFO B
        WHERE  A.EQP_CD    = B.EQP_CD
        AND    A.CHECK_CYCLE <> 'C'
        AND    CHECK_START_DATE <= @W_WORK_DATE
        AND    ISNULL(CHECK_END_DATE,'39991231') >= @W_WORK_DATE
        AND    A.USE_YN      = 'Y'
        AND    NOT EXISTS ( SELECT CHECK_SEQ FROM TB_EQP_CHECK_WR WHERE EQP_CD = A.EQP_CD AND ITEM_CD = A.ITEM_CD )
        UNION ALL
        SELECT  A.EQP_CD
               ,A.ITEM_CD
               ,C.CHECK_SEQ
               ,B.EQP_NM
               ,B.ROUTING_NM
               ,A.CHECK_ITEM
               ,ISNULL(A.CHECK_GAP,1) AS CHECK_GAP
               ,A.CHECK_CYCLE
               ,A.CHECK_START_DATE
               ,C.CHECK_WR_DATE
               ,'2'   AS DATA_GP       
        FROM   TB_EQP_INFO B
               INNER JOIN  TB_EQP_CHECK_ITEM A
               ON B.EQP_CD  = A.EQP_CD
               LEFT OUTER JOIN TB_EQP_CHECK_WR C
               ON A.EQP_CD  = C.EQP_CD AND A.ITEM_CD = C.ITEM_CD
        WHERE  A.CHECK_CYCLE <> 'C'
        AND    A.CHECK_START_DATE                  <= @W_WORK_DATE
        AND    ISNULL(A.CHECK_END_DATE,'39991231') >= @W_WORK_DATE
        AND    A.USE_YN      = 'Y'
        AND    C.CHECK_YN    = 'Y'
        AND    C.CHECK_SEQ = ( SELECT MAX(CHECK_SEQ) FROM TB_EQP_CHECK_WR 
                               WHERE EQP_CD = A.EQP_CD AND ITEM_CD = A.ITEM_CD );


    OPEN CUR_C1;
    FETCH NEXT FROM  CUR_C1 INTO  @EQP_CD          
                                 ,@ITEM_CD         
                                 ,@CHECK_SEQ        
                                 ,@EQP_NM          
                                 ,@ROUTING_NM      
                                 ,@CHECK_ITEM      
                                 ,@CHECK_GAP       
                                 ,@CHECK_CYCLE     
                                 ,@CHECK_START_DATE
                                 ,@CHECK_WR_DATE   
                                 ,@DATA_GP;         
    WHILE @@FETCH_STATUS = 0
    BEGIN
        ----코드 등록 처음인 것 계획 생성
        ----실적 발생한것 계획 생성
        BEGIN
            ----작업일자에 주 계산
            SET @C_DATE = CONVERT(DATE,@CHECK_WR_DATE);
            SET @T_CALC = 0;
            if  @CHECK_CYCLE = 'W'
                SET @T_CALC = @CHECK_GAP * 7;
            SET @T_DATE = DATEADD(DAY, @T_CALC, @C_DATE);
            ----계획일자가 오늘보다 작은 경우 오늘일자 SET
            IF  @T_DATE < CONVERT(DATE,@W_WORK_DATE)
                SET @T_DATE = CONVERT(DATE,@W_WORK_DATE);
            INSERT INTO [dbo].[TB_EQP_CHECK_WR]
                       ([EQP_CD]
                       ,[ITEM_CD]
                       ,[CHECK_SEQ]
                       ,[EQP_NM]
                       ,[ROUTING_NM]
                       ,[CHECK_ITEM]
                       ,[CHECK_PLN_DATE]
                       ,[CHECK_WR_DATE]
                       ,[CHECK_GAP]
                       ,[CHECK_CYCLE]
                       ,[CHECK_YN]
                       ,[CHECK_CNTS]
                       ,[USE_CNT]
                       ,[PROG_STAT]
                       ,[REGISTER]
                       ,[REG_DDTT] )
                 VALUES
                       (@EQP_CD
                       ,@ITEM_CD
					   --,@CHECK_SEQ + 1
                       ,(SELECT ISNULL(MAX(CHECK_SEQ),0) + 1 FROM TB_EQP_CHECK_WR WHERE EQP_CD = @EQP_CD AND ITEM_CD = @ITEM_CD)
                       ,@EQP_NM
                       ,@ROUTING_NM
                       ,@CHECK_ITEM
                       ,CONVERT(VARCHAR(8), @T_DATE, 112)
                       ,NULL
                       ,@CHECK_GAP
                       ,@CHECK_CYCLE
                       ,'N'
                       ,NULL
                       ,0
                       ,'WAT'
                       ,'EQP_CHECK_PLN'
                       ,GETDATE() );
            IF  @@ERROR <> 0
                BEGIN
                    SET @P_PROC_MSG = 'INSERT ERROR TB_EQP_CHECK_WR_'+ @EQP_CD +'_'+@ITEM_CD +'_'+CONVERT(VARCHAR(10),(SELECT ISNULL(MAX(CHECK_SEQ),0) + 1 FROM TB_EQP_CHECK_WR WHERE EQP_CD = @EQP_CD AND ITEM_CD = @ITEM_CD));
                    EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @P_PROC_MSG;
                    GOTO ERROR_RTN;
                END; 
        END;
        FETCH NEXT FROM  CUR_C1 INTO  @EQP_CD          
                                     ,@ITEM_CD         
                                     ,@CHECK_SEQ       
                                     ,@EQP_NM          
                                     ,@ROUTING_NM      
                                     ,@CHECK_ITEM      
                                     ,@CHECK_GAP       
                                     ,@CHECK_CYCLE     
                                     ,@CHECK_START_DATE
                                     ,@CHECK_WR_DATE   
                                     ,@DATA_GP;         
    END;
    CLOSE CUR_C1;
    DEALLOCATE CUR_C1;
    
    COMMIT TRAN;
    RETURN 0;
        
ERROR_RTN:

        ROLLBACK TRAN; -- 실패!
        BEGIN
            SET  @ERR_LINE    = ERROR_LINE();
            SET  @ERR_NUMBER  = ERROR_NUMBER();
            SET  @ERR_MSG     = SUBSTRING(ERROR_MESSAGE(),1,200);
            SELECT CONVERT(VARCHAR(5),@ERR_LINE) + '-' +  @ERR_MSG;
            SET  @ERR_TP  = 'ERR';
            SET  @ERR_CD  = CONVERT(VARCHAR(10),@ERR_NUMBER); 
            EXEC DBO.SP_ERR_LOG @ERR_PGM, @ERR_CD, @ERR_TP, @P_PROC_MSG;
        END
END
