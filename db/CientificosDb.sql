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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261026031322_InitialCreate'
)
BEGIN
    CREATE TABLE [Cientificos] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(100) NOT NULL,
        [Apellido] nvarchar(100) NOT NULL,
        [Nacionalidad] nvarchar(80) NOT NULL,
        [FechaNacimiento] date NOT NULL,
        [Email] nvarchar(150) NOT NULL,
        CONSTRAINT [PK_Cientificos] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261026031322_InitialCreate'
)
BEGIN
    CREATE TABLE [Especialidades] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(100) NOT NULL,
        [Descripcion] nvarchar(500) NULL,
        CONSTRAINT [PK_Especialidades] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261026031322_InitialCreate'
)
BEGIN
    CREATE TABLE [Descubrimientos] (
        [Id] int NOT NULL IDENTITY,
        [Titulo] nvarchar(200) NOT NULL,
        [Descripcion] nvarchar(1000) NULL,
        [FechaDescubrimiento] date NOT NULL,
        [Lugar] nvarchar(150) NOT NULL,
        [CientificoId] int NOT NULL,
        CONSTRAINT [PK_Descubrimientos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Descubrimientos_Cientificos_CientificoId] FOREIGN KEY ([CientificoId]) REFERENCES [Cientificos] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261026031322_InitialCreate'
)
BEGIN
    CREATE TABLE [Investigaciones] (
        [Id] int NOT NULL IDENTITY,
        [Titulo] nvarchar(200) NOT NULL,
        [AreaEstudio] nvarchar(150) NOT NULL,
        [FechaInicio] date NOT NULL,
        [FechaFin] date NULL,
        [Estado] nvarchar(20) NOT NULL,
        [CientificoId] int NOT NULL,
        CONSTRAINT [PK_Investigaciones] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_Investigaciones_Estado] CHECK ([Estado] IN ('En curso', 'Finalizada', 'Publicada')),
        CONSTRAINT [FK_Investigaciones_Cientificos_CientificoId] FOREIGN KEY ([CientificoId]) REFERENCES [Cientificos] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261026031322_InitialCreate'
)
BEGIN
    CREATE TABLE [CientificoEspecialidades] (
        [CientificoId] int NOT NULL,
        [EspecialidadId] int NOT NULL,
        CONSTRAINT [PK_CientificoEspecialidades] PRIMARY KEY ([CientificoId], [EspecialidadId]),
        CONSTRAINT [FK_CientificoEspecialidades_Cientificos_CientificoId] FOREIGN KEY ([CientificoId]) REFERENCES [Cientificos] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CientificoEspecialidades_Especialidades_EspecialidadId] FOREIGN KEY ([EspecialidadId]) REFERENCES [Especialidades] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261026031322_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Descripcion', N'Nombre') AND [object_id] = OBJECT_ID(N'[Especialidades]'))
        SET IDENTITY_INSERT [Especialidades] ON;
    EXEC(N'INSERT INTO [Especialidades] ([Id], [Descripcion], [Nombre])
    VALUES (1, N''Estudio de la materia, energía e interacciones'', N''Física''),
    (2, N''Estudio de la composición de la materia'', N''Química''),
    (3, N''Estudio de los seres vivos'', N''Biología''),
    (4, N''Estudio de números, estructuras y patrones'', N''Matemáticas''),
    (5, N''Estudio del procesamiento de información'', N''Informática'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Descripcion', N'Nombre') AND [object_id] = OBJECT_ID(N'[Especialidades]'))
        SET IDENTITY_INSERT [Especialidades] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261026031322_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_CientificoEspecialidades_EspecialidadId] ON [CientificoEspecialidades] ([EspecialidadId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261026031322_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Cientificos_Email] ON [Cientificos] ([Email]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261026031322_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Descubrimientos_CientificoId] ON [Descubrimientos] ([CientificoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261026031322_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Especialidades_Nombre] ON [Especialidades] ([Nombre]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261026031322_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Investigaciones_CientificoId] ON [Investigaciones] ([CientificoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261026031322_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261026031322_InitialCreate', N'8.0.11');
END;
GO

COMMIT;
GO

