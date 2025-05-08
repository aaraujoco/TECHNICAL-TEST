CREATE DATABASE RealEstateDB;
GO

CREATE DATABASE RealEstateDBSecurity;
GO

-- Usar la base de datos
USE RealEstateDB;
GO


CREATE TABLE Owners (
    IdOwner INT IDENTITY(1,1) PRIMARY KEY,  -- Identificador único, autoincremental
    Name NVARCHAR(255) NULL,                 -- Nombre completo del propietario
    Address NVARCHAR(255) NULL,              -- Dirección del propietario
    Photo NVARCHAR(MAX) NULL,                -- URL o cadena base64 de la foto del propietario
    Birthday DATE NULL,                       -- Fecha de nacimiento del propietario
    CreatedBy NVARCHAR(255) NULL,            -- Usuario que creó el registro
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),  -- Fecha y hora de creación
    UpdatedBy NVARCHAR(255) NULL,            -- Usuario que actualizó el registro por última vez
    UpdatedDate DATETIME NOT NULL DEFAULT GETDATE()   -- Fecha y hora de la última actualización
);
GO

CREATE TABLE Properties (
    IdProperty INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Address NVARCHAR(255) NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    CodeInternal NVARCHAR(50) NOT NULL,
    Year INT NOT NULL,
    IdOwner INT NOT NULL,
	CreatedBy NVARCHAR(255) NULL,            -- Usuario que creó el registro
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),  -- Fecha y hora de creación
    UpdatedBy NVARCHAR(255) NULL,            -- Usuario que actualizó el registro por última vez
    UpdatedDate DATETIME NOT NULL DEFAULT GETDATE(),   -- Fecha y hora de la última actualización
    FOREIGN KEY (IdOwner) REFERENCES Owners(IdOwner) -- Asegúrate de que la tabla Owners existe
);
GO

CREATE TABLE PropertyTraces (
    IdPropertyTrace INT IDENTITY(1,1) PRIMARY KEY,
    DateSale DATETIME NOT NULL,
    Name NVARCHAR(255) NOT NULL,
    Value DECIMAL(18, 2) NOT NULL,
    Tax DECIMAL(18, 2) NOT NULL,
    IdProperty INT NOT NULL,
    CreatedBy NVARCHAR(255) NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedBy NVARCHAR(255) NULL,
    UpdatedDate DATETIME NOT NULL DEFAULT GETDATE(),
	CONSTRAINT FK_PropertyTrace_Property FOREIGN KEY (IdProperty) REFERENCES Properties(IdProperty)
);
GO

CREATE TABLE PropertyImage (
    IdPropertyImage INT IDENTITY(1,1) PRIMARY KEY,
    IdProperty INT NOT NULL,
    [File] NVARCHAR(MAX) NOT NULL,
    Enabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_PropertyImage_Property FOREIGN KEY (IdProperty)
        REFERENCES Properties(IdProperty)
);
GO

CREATE PROCEDURE GetOwnerById
    @IdOwner INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Select the owner record by IdOwner
    SELECT IdOwner, Name, Address, Photo, Birthday, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    FROM Owners
    WHERE IdOwner = @IdOwner;
END
GO

CREATE PROCEDURE GetAll_Owners
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        IdOwner,
        Name,
        Address,
        Photo,
        Birthday,
        CreatedBy,
        CreatedDate,
        UpdatedBy,
        UpdatedDate
    FROM Owners;
END;
GO

CREATE OR ALTER PROCEDURE Create_Owner_Async
    @Name NVARCHAR(255),
    @Address NVARCHAR(255),
    @Photo NVARCHAR(MAX),
    @Birthday DATE,
    @CreatedBy NVARCHAR(255),
    @CreatedDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    -- Inserta un nuevo propietario en la tabla Owners
    INSERT INTO Owners (Name, Address, Photo, Birthday, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate)
    VALUES (@Name, @Address, @Photo, @Birthday, @CreatedBy, GETDATE(), @CreatedBy, GETDATE());

    -- Captura el ID generado automáticamente
    DECLARE @IdOwner INT;
    SET @IdOwner = SCOPE_IDENTITY();

    -- Devuelve el ID generado
    SELECT @IdOwner AS IdOwner;

END
GO

CREATE PROCEDURE Update_Owner_Async
    @IdOwner INT,
    @Name NVARCHAR(255),
    @Address NVARCHAR(255),
    @Photo NVARCHAR(MAX),
    @Birthday DATE,
    @UpdatedBy NVARCHAR(255),
    @UpdatedDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    -- Update the existing owner record
    UPDATE Owners
    SET Name = @Name,
        Address = @Address,
        Photo = @Photo,
        Birthday = @Birthday,
        UpdatedBy = @UpdatedBy,
        UpdatedDate = @UpdatedDate
    WHERE IdOwner = @IdOwner;
END
GO

CREATE OR ALTER PROCEDURE Get_PropertyById
    @IdProperty INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [IdProperty]
      ,[Name]
      ,[Address]
      ,[Price]
      ,[CodeInternal]
      ,[Year]
      ,[IdOwner]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[UpdatedBy]
      ,[UpdatedDate]
    FROM Properties
    WHERE IdProperty = @IdProperty;
END
GO

CREATE OR ALTER PROCEDURE GetProperties_With_Filters
    @Name NVARCHAR(100) = NULL,
    @Address NVARCHAR(200) = NULL,
    @Price DECIMAL(18,2) = NULL,
    @CodeInternal NVARCHAR(50) = NULL,
    @Year INT = NULL,
    @Page INT = 1,
    @Size INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    -- Calcular el offset para la paginación
    DECLARE @Offset INT = (@Page - 1) * @Size;

    SELECT 
        [IdProperty],
        [Name],
        [Address],
        [Price],
        [CodeInternal],
        [Year],
        [IdOwner],
        [CreatedBy],
        [CreatedDate],
        [UpdatedBy],
        [UpdatedDate]
    FROM Properties
    WHERE
        (@Name IS NULL OR LTRIM(RTRIM(@Name)) = '' OR Name LIKE '%' + @Name + '%')
        AND (@Address IS NULL OR LTRIM(RTRIM(@Address)) = '' OR Address LIKE '%' + @Address + '%')
        AND (@Price IS NULL OR @Price = 0 OR Price = @Price)
        AND (@CodeInternal IS NULL OR LTRIM(RTRIM(@CodeInternal)) = '' OR CodeInternal LIKE '%' + @CodeInternal + '%')
        AND (@Year IS NULL OR @Year = 0 OR Year = @Year)
    ORDER BY IdProperty
    OFFSET @Offset ROWS
    FETCH NEXT @Size ROWS ONLY;
END
GO

CREATE OR ALTER PROCEDURE GetPropertiesCount_With_Filters
    @Name NVARCHAR(100) = NULL,
    @Address NVARCHAR(200) = NULL,
    @Price DECIMAL(18,2) = NULL,
    @CodeInternal NVARCHAR(50) = NULL,
    @Year INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS TotalProperties
    FROM Properties
    WHERE
        (@Name IS NULL OR LTRIM(RTRIM(@Name)) = '' OR Name LIKE '%' + @Name + '%')
        AND (@Address IS NULL OR LTRIM(RTRIM(@Address)) = '' OR Address LIKE '%' + @Address + '%')
        AND (@Price IS NULL OR @Price = 0 OR Price = @Price)
        AND (@CodeInternal IS NULL OR LTRIM(RTRIM(@CodeInternal)) = '' OR CodeInternal LIKE '%' + @CodeInternal + '%')
        AND (@Year IS NULL OR @Year = 0 OR Year = @Year)
END
GO

CREATE OR ALTER PROCEDURE Create_Property_Async
    @Name NVARCHAR(100),
    @Address NVARCHAR(255),
    @Price DECIMAL(18, 2),
    @CodeInternal NVARCHAR(50),
    @Year INT,
    @IdOwner INT,
	@CreatedBy NVARCHAR(255),
    @CreatedDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    -- Inserta la nueva propiedad
    INSERT INTO Properties (Name, Address, Price, CodeInternal, Year, IdOwner,CreatedBy, CreatedDate, UpdatedBy, UpdatedDate)
    VALUES (@Name, @Address, @Price, @CodeInternal, @Year, @IdOwner,@CreatedBy, GETDATE(), @CreatedBy, GETDATE());

    -- Captura el ID generado automáticamente
    DECLARE @IdProperty INT;
    SET @IdProperty = SCOPE_IDENTITY();

    -- Devuelve el ID generado
    SELECT @IdProperty AS IdProperty;
END
GO

CREATE OR ALTER PROCEDURE Update_Property_Async
    @IdProperty INT,
    @Name NVARCHAR(100),
    @Address NVARCHAR(200),
    @Price DECIMAL(18, 2),
    @CodeInternal NVARCHAR(50),
    @Year INT,
    @IdOwner INT,
    @UpdatedBy NVARCHAR(50),
    @UpdatedDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Properties
    SET Name = @Name,
        Address = @Address,
        Price = @Price,
        CodeInternal = @CodeInternal,
        Year = @Year,
        IdOwner = @IdOwner,
        UpdatedBy = @UpdatedBy,
        UpdatedDate = @UpdatedDate
    WHERE IdProperty = @IdProperty;
END
GO

CREATE OR ALTER PROCEDURE Update_Property_Price
    @IdProperty INT,
    @NewPrice DECIMAL(18, 2),
    @UpdatedBy NVARCHAR(50),
    @UpdatedDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Properties
    SET Price = @NewPrice,
        UpdatedBy = @UpdatedBy,
        UpdatedDate = @UpdatedDate
    WHERE IdProperty = @IdProperty;
END
GO

CREATE PROCEDURE GetPropertyTraces_By_PropertyId
    @IdProperty INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        IdPropertyTrace,
        DateSale,
        Name,
        Value,
        Tax,
        IdProperty,
        CreatedBy,
        CreatedDate,
        UpdatedBy,
        UpdatedDate
    FROM 
        PropertyTraces
    WHERE 
        IdProperty = @IdProperty;
END;
GO

CREATE OR ALTER PROCEDURE Create_PropertyTrace_Async
    @DateSale DATETIME,
    @Name NVARCHAR(255),
    @Value DECIMAL(18, 2),
    @Tax DECIMAL(18, 2),
    @IdProperty INT,
    @CreatedBy NVARCHAR(255)
AS
BEGIN
    INSERT INTO PropertyTraces (DateSale, Name, Value, Tax, IdProperty, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate)
    VALUES (@DateSale, @Name, @Value, @Tax, @IdProperty, @CreatedBy, GETDATE(), @CreatedBy, GETDATE());

    SELECT SCOPE_IDENTITY() AS IdPropertyTrace; -- Return the newly created Id
END
GO

CREATE OR ALTER PROCEDURE Create_PropertyImage_Async
    @IdProperty INT,
    @File NVARCHAR(MAX),
    @Enabled BIT
AS
BEGIN
    INSERT INTO PropertyImage (IdProperty, [File], Enabled)
    VALUES (@IdProperty, @File, @Enabled);

	SELECT SCOPE_IDENTITY() AS IdPropertyImage;
END;
GO

CREATE OR ALTER PROCEDURE Get_PropertyImages_By_PropertyId
    @IdProperty INT
AS
BEGIN
    SELECT IdPropertyImage, IdProperty, [File], Enabled, CreatedDate
    FROM PropertyImage
    WHERE IdProperty = @IdProperty;
END;
GO