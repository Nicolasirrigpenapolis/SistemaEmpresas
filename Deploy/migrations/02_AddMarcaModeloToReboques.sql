BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251210205540_AddMarcaModeloToReboques'
)
BEGIN
    ALTER TABLE [Reboques] ADD [AnoFabricacao] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251210205540_AddMarcaModeloToReboques'
)
BEGIN
    ALTER TABLE [Reboques] ADD [Marca] nvarchar(100) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251210205540_AddMarcaModeloToReboques'
)
BEGIN
    ALTER TABLE [Reboques] ADD [Modelo] nvarchar(100) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251210205540_AddMarcaModeloToReboques'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251210205540_AddMarcaModeloToReboques', N'8.0.0');
END;
GO

COMMIT;
GO

