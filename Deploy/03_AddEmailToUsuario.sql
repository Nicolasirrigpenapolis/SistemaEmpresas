BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251211125540_AddEmailToUsuario'
)
BEGIN
                    IF NOT EXISTS (
                        SELECT 1 
                        FROM INFORMATION_SCHEMA.COLUMNS 
                        WHERE TABLE_NAME = 'PW~Usuarios' 
                        AND COLUMN_NAME = 'Email'
                    )
                    BEGIN
                        ALTER TABLE [PW~Usuarios] ADD [Email] varchar(255) NULL
                    END
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251211125540_AddEmailToUsuario'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251211125540_AddEmailToUsuario', N'8.0.0');
END;
GO

COMMIT;
GO

