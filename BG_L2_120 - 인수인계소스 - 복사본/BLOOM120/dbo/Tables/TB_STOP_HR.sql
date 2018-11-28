﻿CREATE TABLE [dbo].[TB_STOP_HR] (
    [START_DDTT]  DATETIME      NOT NULL,
    [END_DDTT]    DATETIME      NULL,
    [STOP_RSN]    VARCHAR (200) NULL,
    [DATA_OCC_GP] VARCHAR (10)  NULL,
    [REGISTER]    VARCHAR (20)  NULL,
    [REG_DDTT]    DATETIME      NULL,
    [MODIFIER]    VARCHAR (20)  NULL,
    [MOD_DDTT]    DATETIME      NULL,
    CONSTRAINT [PK_TB_STOP_HR] PRIMARY KEY CLUSTERED ([START_DDTT] ASC)
);

