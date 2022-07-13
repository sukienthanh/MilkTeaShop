IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [About] (
    [ID] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NULL,
    [Address] nvarchar(200) NULL,
    [Phone] varchar(11) NULL,
    [Image] varchar(200) NULL,
    [Status] nvarchar(200) NULL,
    [Created_Date] datetime NULL DEFAULT ((getdate())),
    [Modified_Date] datetime NULL,
    [Created_By] nvarchar(50) NULL,
    [Modified_By] nvarchar(50) NULL,
    CONSTRAINT [PK_About] PRIMARY KEY ([ID])
);
GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Catalog] (
    [Cata_Id] int NOT NULL IDENTITY,
    [Cata_Name] nvarchar(50) NULL,
    [Created_Date] datetime NULL DEFAULT ((getdate())),
    [Modified_Date] datetime NULL,
    [Created_By] nvarchar(50) NULL,
    [Modified_By] nvarchar(50) NULL,
    CONSTRAINT [PK__Catalog__CC584C7652B9100B] PRIMARY KEY ([Cata_Id])
);
GO

CREATE TABLE [Orders] (
    [Order_Id] int NOT NULL IDENTITY,
    [Order_Status] bit NULL,
    [Total_Amount] int NULL,
    [Created_Date] datetime NULL DEFAULT ((getdate())),
    [Modified_Date] datetime NULL,
    [Created_By] nvarchar(50) NULL,
    [Modified_By] nvarchar(50) NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([Order_Id])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Product] (
    [Product_Id] int NOT NULL IDENTITY,
    [Product_Name] nvarchar(50) NULL,
    [Catalog_Id] int NULL,
    [Price] int NULL,
    [Discount] int NULL,
    [Image] varchar(200) NULL,
    [Created_Date] datetime NULL DEFAULT ((getdate())),
    [Modified_Date] datetime NULL,
    [Created_By] nvarchar(50) NULL,
    [Modified_By] nvarchar(50) NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY ([Product_Id]),
    CONSTRAINT [FK__Product__Catalog__30F848ED] FOREIGN KEY ([Catalog_Id]) REFERENCES [Catalog] ([Cata_Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Payment] (
    [Payment_Id] int NOT NULL IDENTITY,
    [Order_Id] int NULL,
    [Pay_Date] datetime NULL DEFAULT ((getdate())),
    [Created_Date] datetime NULL DEFAULT ((getdate())),
    [Modified_Date] datetime NULL,
    [Created_By] nvarchar(50) NULL,
    [Modified_By] nvarchar(50) NULL,
    CONSTRAINT [PK_Payment] PRIMARY KEY ([Payment_Id]),
    CONSTRAINT [FK__Payment__Order_I__3C69FB99] FOREIGN KEY ([Order_Id]) REFERENCES [Orders] ([Order_Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [OrderDetail] (
    [ID] int NOT NULL IDENTITY,
    [Order_Id] int NULL,
    [Product_Id] int NULL,
    [Quantity] int NULL,
    [Created_Date] datetime NULL DEFAULT ((getdate())),
    [Modified_Date] datetime NULL,
    [Created_By] nvarchar(50) NULL,
    [Modified_By] nvarchar(50) NULL,
    CONSTRAINT [PK_OrderDetail] PRIMARY KEY ([ID]),
    CONSTRAINT [FK__OrderDeta__Order__37A5467C] FOREIGN KEY ([Order_Id]) REFERENCES [Orders] ([Order_Id]) ON DELETE CASCADE,
    CONSTRAINT [FK__OrderDeta__Produ__38996AB5] FOREIGN KEY ([Product_Id]) REFERENCES [Product] ([Product_Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

CREATE INDEX [IX_OrderDetail_Order_Id] ON [OrderDetail] ([Order_Id]);
GO

CREATE INDEX [IX_OrderDetail_Product_Id] ON [OrderDetail] ([Product_Id]);
GO

CREATE INDEX [IX_Payment_Order_Id] ON [Payment] ([Order_Id]);
GO

CREATE INDEX [IX_Product_Catalog_Id] ON [Product] ([Catalog_Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220512072504_Initial', N'6.0.4');
GO

COMMIT;
GO

