﻿CREATE TABLE [dbo].[TB_MAT_TAKE_OVER_BLOOM] (
    [HEAT_NO]        VARCHAR (6)  NOT NULL,
    [HEAT_SEQ]       INT          NOT NULL,
    [STEEL]          VARCHAR (10) NULL,
    [STRAND_NO]      VARCHAR (1)  NULL,
    [ITEM]           VARCHAR (2)  NULL,
    [ITEM_SIZE]      VARCHAR (4)  NULL,
    [STEEL_TYPE]     VARCHAR (3)  NULL,
    [MFG_DATE]       VARCHAR (8)  NULL,
    [TAKE_OVER_DATE] VARCHAR (8)  NULL,
    [THEORY_LENGTH]  INT          NULL,
    [REAL_LENGTH]    INT          NULL,
    [THEORY_WGT]     NUMERIC (6)  NULL,
    [REAL_WGT]       NUMERIC (6)  NULL,
    [Marking_Code]   VARCHAR (16) NULL,
    [REGISTER]       VARCHAR (20) NULL,
    [REG_DDTT]       DATETIME     NULL,
    [MODIFIER]       VARCHAR (20) NULL,
    [MOD_DDTT]       DATETIME     NULL,
    CONSTRAINT [PK_TB_MAT_TAKE_OVER_BLOOM] PRIMARY KEY CLUSTERED ([HEAT_NO] ASC, [HEAT_SEQ] ASC)
);

