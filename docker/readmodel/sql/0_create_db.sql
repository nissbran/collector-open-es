CREATE DATABASE ReadModel
GO

IF SERVERPROPERTY('EngineEdition') <> 5
BEGIN
    ALTER DATABASE ReadModel SET READ_COMMITTED_SNAPSHOT ON;
END;
GO

USE ReadModel
GO