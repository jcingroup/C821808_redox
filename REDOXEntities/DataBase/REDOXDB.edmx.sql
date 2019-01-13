
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/13/2019 10:24:03
-- Generated from EDMX file: D:\Git\Redox\REDOXEntities\DataBase\REDOXDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [REDOX];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_NEWS_USER]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NEWS] DROP CONSTRAINT [FK_NEWS_USER];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FILEBASE]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FILEBASE];
GO
IF OBJECT_ID(N'[dbo].[LISTEDITOR]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LISTEDITOR];
GO
IF OBJECT_ID(N'[dbo].[LOGERR]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LOGERR];
GO
IF OBJECT_ID(N'[dbo].[NEWS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NEWS];
GO
IF OBJECT_ID(N'[dbo].[PRODUCT]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PRODUCT];
GO
IF OBJECT_ID(N'[dbo].[USER]', 'U') IS NOT NULL
    DROP TABLE [dbo].[USER];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'FILEBASE'
CREATE TABLE [dbo].[FILEBASE] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [FILE_RANDOM_NM] nvarchar(50)  NOT NULL,
    [FILE_REL_NM] nvarchar(50)  NOT NULL,
    [FILE_PATH] nvarchar(500)  NOT NULL,
    [IDENTIFY_KEY] int  NULL,
    [SQ] float  NULL,
    [BUD_DT] datetime  NOT NULL,
    [BUD_ID] int  NOT NULL,
    [MAP_ID] int  NOT NULL,
    [MAP_SITE] nvarchar(50)  NOT NULL,
    [FILE_TP] char(1)  NOT NULL,
    [URL_PATH] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'LISTEDITOR'
CREATE TABLE [dbo].[LISTEDITOR] (
    [OBJ_NAME] nvarchar(50)  NOT NULL,
    [OBJ_CONTENT] nvarchar(max)  NULL,
    [UP_DT] datetime  NOT NULL,
    [UP_ID] int  NOT NULL,
    [BUD_DT] datetime  NOT NULL,
    [BUD_ID] int  NOT NULL,
    [STATUS] bit  NOT NULL
);
GO

-- Creating table 'LOGERR'
CREATE TABLE [dbo].[LOGERR] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ERR_GID] int  NOT NULL,
    [ERR_SMRY] nvarchar(max)  NOT NULL,
    [ERR_DESC] nvarchar(max)  NOT NULL,
    [ERR_SRC] varchar(max)  NOT NULL,
    [LOG_DTM] datetime  NOT NULL
);
GO

-- Creating table 'NEWS'
CREATE TABLE [dbo].[NEWS] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [PUB_DT_STR] nvarchar(10)  NOT NULL,
    [TITLE] nvarchar(200)  NOT NULL,
    [CONTENT] nvarchar(max)  NOT NULL,
    [SQ] float  NOT NULL,
    [DISABLE] bit  NOT NULL,
    [HOME_PAGE_DISPLAY] bit  NOT NULL,
    [BUD_DT] datetime  NULL,
    [BUD_ID] int  NOT NULL,
    [UPT_DT] datetime  NOT NULL,
    [UPT_ID] int  NOT NULL
);
GO

-- Creating table 'PRODUCT'
CREATE TABLE [dbo].[PRODUCT] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TITLE] nvarchar(200)  NOT NULL,
    [SPEC] nchar(10)  NOT NULL,
    [DESC] nvarchar(max)  NOT NULL,
    [HOME_INFO] nvarchar(max)  NULL,
    [CONTENT] nvarchar(max)  NULL,
    [SQ] float  NOT NULL,
    [DISABLE] bit  NOT NULL,
    [HOME_PAGE_DISPLAY] bit  NOT NULL,
    [BUD_DT] datetime  NOT NULL,
    [BUD_ID] int  NOT NULL,
    [UPT_DT] datetime  NOT NULL,
    [UPT_ID] int  NOT NULL,
    [EDIT1] nvarchar(max)  NULL,
    [EDIT1_MOBILE] nvarchar(max)  NULL,
    [EDIT2] nvarchar(max)  NULL,
    [EDIT2_MOBILE] nvarchar(max)  NULL,
    [EDIT3] nvarchar(max)  NULL,
    [EDIT3_MOBILE] nvarchar(max)  NULL,
    [QA] nvarchar(max)  NULL
);
GO

-- Creating table 'USER'
CREATE TABLE [dbo].[USER] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [USR_ID] nvarchar(50)  NOT NULL,
    [USR_PWD] nvarchar(50)  NOT NULL,
    [BUD_DT] datetime  NOT NULL,
    [BUD_ID] int  NOT NULL,
    [UPD_DT] datetime  NOT NULL,
    [UPD_ID] int  NOT NULL,
    [USR_NM] nvarchar(50)  NULL,
    [DISABLE] bit  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'FILEBASE'
ALTER TABLE [dbo].[FILEBASE]
ADD CONSTRAINT [PK_FILEBASE]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [OBJ_NAME] in table 'LISTEDITOR'
ALTER TABLE [dbo].[LISTEDITOR]
ADD CONSTRAINT [PK_LISTEDITOR]
    PRIMARY KEY CLUSTERED ([OBJ_NAME] ASC);
GO

-- Creating primary key on [ID] in table 'LOGERR'
ALTER TABLE [dbo].[LOGERR]
ADD CONSTRAINT [PK_LOGERR]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'NEWS'
ALTER TABLE [dbo].[NEWS]
ADD CONSTRAINT [PK_NEWS]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'PRODUCT'
ALTER TABLE [dbo].[PRODUCT]
ADD CONSTRAINT [PK_PRODUCT]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'USER'
ALTER TABLE [dbo].[USER]
ADD CONSTRAINT [PK_USER]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UPT_ID] in table 'NEWS'
ALTER TABLE [dbo].[NEWS]
ADD CONSTRAINT [FK_NEWS_USER]
    FOREIGN KEY ([UPT_ID])
    REFERENCES [dbo].[USER]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NEWS_USER'
CREATE INDEX [IX_FK_NEWS_USER]
ON [dbo].[NEWS]
    ([UPT_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------