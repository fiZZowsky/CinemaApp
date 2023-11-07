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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE TABLE [AgeRatings] (
        [Id] int NOT NULL IDENTITY,
        [MinimumAge] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_AgeRatings] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
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
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE TABLE [Halls] (
        [Id] int NOT NULL IDENTITY,
        [Number] int NOT NULL,
        [NumberOfRows] int NOT NULL,
        [PlacesInARow] int NOT NULL,
        CONSTRAINT [PK_Halls] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE TABLE [Movies] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(max) NOT NULL,
        [Genre] nvarchar(max) NOT NULL,
        [Country] nvarchar(max) NOT NULL,
        [Language] nvarchar(max) NOT NULL,
        [Duration] int NOT NULL,
        [AgeRatingId] int NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [Rating] float NOT NULL,
        [ReleaseDate] datetime2 NOT NULL,
        [PriceList_Id] int NOT NULL,
        [PriceList_NormalTicketPrice] int NOT NULL,
        [PriceList_ReducedTicketPrice] int NOT NULL,
        [EncodedTitle] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Movies] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Movies_AgeRatings_AgeRatingId] FOREIGN KEY ([AgeRatingId]) REFERENCES [AgeRatings] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(128) NOT NULL,
        [ProviderKey] nvarchar(128) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(128) NOT NULL,
        [Name] nvarchar(128) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE TABLE [Seats] (
        [Id] int NOT NULL IDENTITY,
        [Number] int NOT NULL,
        [RowNumber] int NOT NULL,
        [HallId] int NOT NULL,
        CONSTRAINT [PK_Seats] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Seats_Halls_HallId] FOREIGN KEY ([HallId]) REFERENCES [Halls] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE TABLE [MovieShows] (
        [Id] int NOT NULL IDENTITY,
        [MovieId] int NOT NULL,
        [HallId] int NOT NULL,
        [StartTime] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_MovieShows] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_MovieShows_Halls_HallId] FOREIGN KEY ([HallId]) REFERENCES [Halls] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_MovieShows_Movies_MovieId] FOREIGN KEY ([MovieId]) REFERENCES [Movies] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE TABLE [Ratings] (
        [Id] int NOT NULL IDENTITY,
        [RateValue] int NOT NULL,
        [Comment] nvarchar(max) NOT NULL,
        [CreatedByUserId] nvarchar(max) NOT NULL,
        [CreatedBy] nvarchar(max) NOT NULL,
        [MovieId] int NOT NULL,
        [UserId] nvarchar(450) NULL,
        CONSTRAINT [PK_Ratings] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Ratings_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_Ratings_Movies_MovieId] FOREIGN KEY ([MovieId]) REFERENCES [Movies] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE TABLE [Tickets] (
        [Uid] nvarchar(450) NOT NULL,
        [MovieShowId] int NOT NULL,
        [PurchaseDate] datetime2 NOT NULL,
        [QRCode] varbinary(max) NOT NULL,
        [IsScanned] bit NOT NULL,
        [PurchasedById] nvarchar(450) NOT NULL,
        [NormalPriceSeats] int NOT NULL,
        [ReducedPriceSeats] int NOT NULL,
        CONSTRAINT [PK_Tickets] PRIMARY KEY ([Uid]),
        CONSTRAINT [FK_Tickets_AspNetUsers_PurchasedById] FOREIGN KEY ([PurchasedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Tickets_MovieShows_MovieShowId] FOREIGN KEY ([MovieShowId]) REFERENCES [MovieShows] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE TABLE [Payments] (
        [Id] int NOT NULL IDENTITY,
        [SessionId] nvarchar(max) NOT NULL,
        [TicketId] nvarchar(450) NOT NULL,
        [PurchasedById] nvarchar(450) NOT NULL,
        [PurchaseDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Payments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Payments_AspNetUsers_PurchasedById] FOREIGN KEY ([PurchasedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Payments_Tickets_TicketId] FOREIGN KEY ([TicketId]) REFERENCES [Tickets] ([Uid]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE TABLE [SeatTicket] (
        [SeatsId] int NOT NULL,
        [TicketsUid] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_SeatTicket] PRIMARY KEY ([SeatsId], [TicketsUid]),
        CONSTRAINT [FK_SeatTicket_Seats_SeatsId] FOREIGN KEY ([SeatsId]) REFERENCES [Seats] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_SeatTicket_Tickets_TicketsUid] FOREIGN KEY ([TicketsUid]) REFERENCES [Tickets] ([Uid]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE INDEX [IX_Movies_AgeRatingId] ON [Movies] ([AgeRatingId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE INDEX [IX_MovieShows_HallId] ON [MovieShows] ([HallId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE INDEX [IX_MovieShows_MovieId] ON [MovieShows] ([MovieId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE INDEX [IX_Payments_PurchasedById] ON [Payments] ([PurchasedById]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE UNIQUE INDEX [IX_Payments_TicketId] ON [Payments] ([TicketId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE INDEX [IX_Ratings_MovieId] ON [Ratings] ([MovieId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE INDEX [IX_Ratings_UserId] ON [Ratings] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE INDEX [IX_Seats_HallId] ON [Seats] ([HallId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE INDEX [IX_SeatTicket_TicketsUid] ON [SeatTicket] ([TicketsUid]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE INDEX [IX_Tickets_MovieShowId] ON [Tickets] ([MovieShowId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    CREATE INDEX [IX_Tickets_PurchasedById] ON [Tickets] ([PurchasedById]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231104185721_Init')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231104185721_Init', N'7.0.13');
END;
GO

COMMIT;
GO

