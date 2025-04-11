
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/09/2025 16:35:56
-- Generated from EDMX file: C:\Users\Diego Alejandro\OneDrive - SENA\Documentos\C#\NeoMarke\Diagram\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [baseDatos];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'FormSet'
CREATE TABLE [dbo].[FormSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [NameForm] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Status] bit  NOT NULL,
    [IdModule] int  NOT NULL
);
GO

-- Creating table 'ModuleSet'
CREATE TABLE [dbo].[ModuleSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [NameModule] nvarchar(max)  NOT NULL,
    [Status] bit  NOT NULL
);
GO

-- Creating table 'RolFormSet'
CREATE TABLE [dbo].[RolFormSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Permision] int  NOT NULL,
    [IdForm] int  NOT NULL,
    [IdRol] int  NOT NULL
);
GO

-- Creating table 'RolSet'
CREATE TABLE [dbo].[RolSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [NameRol] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Status] bit  NOT NULL
);
GO

-- Creating table 'UserSet'
CREATE TABLE [dbo].[UserSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [CreateAt] nvarchar(max)  NOT NULL,
    [DeleteAt] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [UpdateAt] nvarchar(max)  NOT NULL,
    [CompanyId] int  NOT NULL,
    [Person_Id] int  NOT NULL
);
GO

-- Creating table 'PersonSet'
CREATE TABLE [dbo].[PersonSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [first_name] nvarchar(max)  NOT NULL,
    [last_name] nvarchar(max)  NOT NULL,
    [phone_number] smallint  NOT NULL,
    [email] nvarchar(max)  NOT NULL,
    [type_identification] nvarchar(max)  NOT NULL,
    [number_identification] smallint  NOT NULL,
    [status] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SedeSet'
CREATE TABLE [dbo].[SedeSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [NameSede] nvarchar(max)  NOT NULL,
    [CodeSede] bigint  NOT NULL,
    [AddressSede] nvarchar(max)  NOT NULL,
    [PhoneSede] smallint  NOT NULL,
    [EmailSede] nvarchar(max)  NOT NULL,
    [Status] bit  NOT NULL,
    [CreateAt] datetime  NOT NULL,
    [DeleteAt] datetime  NOT NULL,
    [UpdateAt] datetime  NOT NULL,
    [IdCompany] int  NOT NULL
);
GO

-- Creating table 'CompanySet'
CREATE TABLE [dbo].[CompanySet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CreateAt] datetime  NOT NULL,
    [DeleteAt] datetime  NOT NULL,
    [UpdateAt] datetime  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [NameCompany] nvarchar(max)  NOT NULL,
    [PhoneCompany] smallint  NOT NULL,
    [EmailCompany] nvarchar(max)  NOT NULL,
    [NitCompany] smallint  NOT NULL,
    [Status] bit  NOT NULL,
    [Logo] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Items'
CREATE TABLE [dbo].[Items] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [NameItem] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Price] nvarchar(max)  NOT NULL,
    [CreateAt] nvarchar(max)  NOT NULL,
    [DeleteAt] nvarchar(max)  NOT NULL,
    [UpdateAt] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [IdInventory] int  NOT NULL,
    [ImageItem_Id] int  NOT NULL,
    [Category_Id] int  NOT NULL
);
GO

-- Creating table 'ImageItemSet'
CREATE TABLE [dbo].[ImageItemSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UrlImage] nvarchar(max)  NOT NULL,
    [IdProduct] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Inventorys'
CREATE TABLE [dbo].[Inventorys] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [NameInventory] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [Observation] nvarchar(max)  NOT NULL,
    [CreateAt] nvarchar(max)  NOT NULL,
    [DeleteAt] nvarchar(max)  NOT NULL,
    [UpdateAt] nvarchar(max)  NOT NULL,
    [StockActual] nvarchar(max)  NOT NULL,
    [ZoneProduct] nvarchar(max)  NOT NULL,
    [IdProduct] int  NOT NULL
);
GO

-- Creating table 'CategorySet'
CREATE TABLE [dbo].[CategorySet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [IdProduct] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'MovimientoInventorySet'
CREATE TABLE [dbo].[MovimientoInventorySet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TypeMoviment] int  NOT NULL,
    [Quantity] int  NOT NULL,
    [Date] int  NOT NULL,
    [Description] int  NOT NULL,
    [IdInventory] nvarchar(max)  NOT NULL,
    [UserId] int  NOT NULL,
    [ProductId] int  NOT NULL
);
GO

-- Creating table 'Buyouts'
CREATE TABLE [dbo].[Buyouts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Quantity] nvarchar(max)  NOT NULL,
    [Date] nvarchar(max)  NOT NULL,
    [IdUser] int  NOT NULL,
    [ProductId] int  NOT NULL
);
GO

-- Creating table 'Sales'
CREATE TABLE [dbo].[Sales] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] nvarchar(max)  NOT NULL,
    [totaly] nvarchar(max)  NOT NULL,
    [IdUser] int  NOT NULL
);
GO

-- Creating table 'SaleDetailSet'
CREATE TABLE [dbo].[SaleDetailSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Quantity] nvarchar(max)  NOT NULL,
    [Price] nvarchar(max)  NOT NULL,
    [IdSale] int  NOT NULL,
    [IdProduct] int  NOT NULL
);
GO

-- Creating table 'Notifications'
CREATE TABLE [dbo].[Notifications] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TypeAction] nvarchar(max)  NOT NULL,
    [IdReferece] nvarchar(max)  NOT NULL,
    [Mensage] nvarchar(max)  NOT NULL,
    [Read] nvarchar(max)  NOT NULL,
    [Date] nvarchar(max)  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'FormSet'
ALTER TABLE [dbo].[FormSet]
ADD CONSTRAINT [PK_FormSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ModuleSet'
ALTER TABLE [dbo].[ModuleSet]
ADD CONSTRAINT [PK_ModuleSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RolFormSet'
ALTER TABLE [dbo].[RolFormSet]
ADD CONSTRAINT [PK_RolFormSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RolSet'
ALTER TABLE [dbo].[RolSet]
ADD CONSTRAINT [PK_RolSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [PK_UserSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PersonSet'
ALTER TABLE [dbo].[PersonSet]
ADD CONSTRAINT [PK_PersonSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SedeSet'
ALTER TABLE [dbo].[SedeSet]
ADD CONSTRAINT [PK_SedeSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CompanySet'
ALTER TABLE [dbo].[CompanySet]
ADD CONSTRAINT [PK_CompanySet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [PK_Items]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ImageItemSet'
ALTER TABLE [dbo].[ImageItemSet]
ADD CONSTRAINT [PK_ImageItemSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Inventorys'
ALTER TABLE [dbo].[Inventorys]
ADD CONSTRAINT [PK_Inventorys]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CategorySet'
ALTER TABLE [dbo].[CategorySet]
ADD CONSTRAINT [PK_CategorySet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MovimientoInventorySet'
ALTER TABLE [dbo].[MovimientoInventorySet]
ADD CONSTRAINT [PK_MovimientoInventorySet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Buyouts'
ALTER TABLE [dbo].[Buyouts]
ADD CONSTRAINT [PK_Buyouts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Sales'
ALTER TABLE [dbo].[Sales]
ADD CONSTRAINT [PK_Sales]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SaleDetailSet'
ALTER TABLE [dbo].[SaleDetailSet]
ADD CONSTRAINT [PK_SaleDetailSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Notifications'
ALTER TABLE [dbo].[Notifications]
ADD CONSTRAINT [PK_Notifications]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Id] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [FK_UserRol]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[RolSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdCompany] in table 'SedeSet'
ALTER TABLE [dbo].[SedeSet]
ADD CONSTRAINT [FK_CompanySede]
    FOREIGN KEY ([IdCompany])
    REFERENCES [dbo].[CompanySet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanySede'
CREATE INDEX [IX_FK_CompanySede]
ON [dbo].[SedeSet]
    ([IdCompany]);
GO

-- Creating foreign key on [ImageItem_Id] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [FK_ItemImageItem]
    FOREIGN KEY ([ImageItem_Id])
    REFERENCES [dbo].[ImageItemSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ItemImageItem'
CREATE INDEX [IX_FK_ItemImageItem]
ON [dbo].[Items]
    ([ImageItem_Id]);
GO

-- Creating foreign key on [Category_Id] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [FK_ItemCategory]
    FOREIGN KEY ([Category_Id])
    REFERENCES [dbo].[CategorySet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ItemCategory'
CREATE INDEX [IX_FK_ItemCategory]
ON [dbo].[Items]
    ([Category_Id]);
GO

-- Creating foreign key on [TypeMoviment] in table 'MovimientoInventorySet'
ALTER TABLE [dbo].[MovimientoInventorySet]
ADD CONSTRAINT [FK_InventoryMovimientoItemInventory]
    FOREIGN KEY ([TypeMoviment])
    REFERENCES [dbo].[Inventorys]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InventoryMovimientoItemInventory'
CREATE INDEX [IX_FK_InventoryMovimientoItemInventory]
ON [dbo].[MovimientoInventorySet]
    ([TypeMoviment]);
GO

-- Creating foreign key on [Person_Id] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [FK_UserPerson]
    FOREIGN KEY ([Person_Id])
    REFERENCES [dbo].[PersonSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserPerson'
CREATE INDEX [IX_FK_UserPerson]
ON [dbo].[UserSet]
    ([Person_Id]);
GO

-- Creating foreign key on [IdForm] in table 'RolFormSet'
ALTER TABLE [dbo].[RolFormSet]
ADD CONSTRAINT [FK_FormRolForm]
    FOREIGN KEY ([IdForm])
    REFERENCES [dbo].[FormSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FormRolForm'
CREATE INDEX [IX_FK_FormRolForm]
ON [dbo].[RolFormSet]
    ([IdForm]);
GO

-- Creating foreign key on [IdRol] in table 'RolFormSet'
ALTER TABLE [dbo].[RolFormSet]
ADD CONSTRAINT [FK_RolRolForm]
    FOREIGN KEY ([IdRol])
    REFERENCES [dbo].[RolSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RolRolForm'
CREATE INDEX [IX_FK_RolRolForm]
ON [dbo].[RolFormSet]
    ([IdRol]);
GO

-- Creating foreign key on [IdModule] in table 'FormSet'
ALTER TABLE [dbo].[FormSet]
ADD CONSTRAINT [FK_ModuleForm]
    FOREIGN KEY ([IdModule])
    REFERENCES [dbo].[ModuleSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModuleForm'
CREATE INDEX [IX_FK_ModuleForm]
ON [dbo].[FormSet]
    ([IdModule]);
GO

-- Creating foreign key on [IdUser] in table 'Buyouts'
ALTER TABLE [dbo].[Buyouts]
ADD CONSTRAINT [FK_UserBuyout]
    FOREIGN KEY ([IdUser])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserBuyout'
CREATE INDEX [IX_FK_UserBuyout]
ON [dbo].[Buyouts]
    ([IdUser]);
GO

-- Creating foreign key on [ProductId] in table 'Buyouts'
ALTER TABLE [dbo].[Buyouts]
ADD CONSTRAINT [FK_ProductBuyout]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Items]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductBuyout'
CREATE INDEX [IX_FK_ProductBuyout]
ON [dbo].[Buyouts]
    ([ProductId]);
GO

-- Creating foreign key on [UserId] in table 'MovimientoInventorySet'
ALTER TABLE [dbo].[MovimientoInventorySet]
ADD CONSTRAINT [FK_UserMovimientoInventory]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserMovimientoInventory'
CREATE INDEX [IX_FK_UserMovimientoInventory]
ON [dbo].[MovimientoInventorySet]
    ([UserId]);
GO

-- Creating foreign key on [ProductId] in table 'MovimientoInventorySet'
ALTER TABLE [dbo].[MovimientoInventorySet]
ADD CONSTRAINT [FK_ProductMovimientoInventory]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Items]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductMovimientoInventory'
CREATE INDEX [IX_FK_ProductMovimientoInventory]
ON [dbo].[MovimientoInventorySet]
    ([ProductId]);
GO

-- Creating foreign key on [IdProduct] in table 'Inventorys'
ALTER TABLE [dbo].[Inventorys]
ADD CONSTRAINT [FK_ProductInventory]
    FOREIGN KEY ([IdProduct])
    REFERENCES [dbo].[Items]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductInventory'
CREATE INDEX [IX_FK_ProductInventory]
ON [dbo].[Inventorys]
    ([IdProduct]);
GO

-- Creating foreign key on [IdUser] in table 'Sales'
ALTER TABLE [dbo].[Sales]
ADD CONSTRAINT [FK_UserSale]
    FOREIGN KEY ([IdUser])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserSale'
CREATE INDEX [IX_FK_UserSale]
ON [dbo].[Sales]
    ([IdUser]);
GO

-- Creating foreign key on [IdSale] in table 'SaleDetailSet'
ALTER TABLE [dbo].[SaleDetailSet]
ADD CONSTRAINT [FK_SaleSaleDetail]
    FOREIGN KEY ([IdSale])
    REFERENCES [dbo].[Sales]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SaleSaleDetail'
CREATE INDEX [IX_FK_SaleSaleDetail]
ON [dbo].[SaleDetailSet]
    ([IdSale]);
GO

-- Creating foreign key on [IdProduct] in table 'SaleDetailSet'
ALTER TABLE [dbo].[SaleDetailSet]
ADD CONSTRAINT [FK_ProductSaleDetail]
    FOREIGN KEY ([IdProduct])
    REFERENCES [dbo].[Items]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductSaleDetail'
CREATE INDEX [IX_FK_ProductSaleDetail]
ON [dbo].[SaleDetailSet]
    ([IdProduct]);
GO

-- Creating foreign key on [UserId] in table 'Notifications'
ALTER TABLE [dbo].[Notifications]
ADD CONSTRAINT [FK_UserNotification]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserNotification'
CREATE INDEX [IX_FK_UserNotification]
ON [dbo].[Notifications]
    ([UserId]);
GO

-- Creating foreign key on [CompanyId] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [FK_CompanyUser]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[CompanySet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyUser'
CREATE INDEX [IX_FK_CompanyUser]
ON [dbo].[UserSet]
    ([CompanyId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------