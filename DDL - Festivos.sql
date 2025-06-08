--Crear la base de datos
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
			WHERE TABLE_CATALOG='Festivos')
	CREATE DATABASE Festivos
ELSE
	PRINT 'Ya existe la BD [Festivos]'
GO

--Abrir la base de datos
USE Festivos
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_NAME='Tipo')

	--Crear la tabla TIPO
	CREATE TABLE Tipo(
		Id int IDENTITY(1,1) NOT NULL,
		CONSTRAINT pkTipo_Id PRIMARY KEY (Id),
		Tipo VARCHAR(100) NOT NULL
		);
ELSE
	PRINT 'Ya existe la tabla [Tipo]'

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_NAME='Festivo')
	--Crear la tabla FESTIVO
	CREATE TABLE Festivo(
		Id int IDENTITY(1,1) NOT NULL,
		CONSTRAINT pkFestivo_Id PRIMARY KEY (Id),
		Nombre VARCHAR(100) NOT NULL,
		Dia INT NOT NULL,
		Mes INT NOT NULL,
		DiasPascua INT NOT NULL,
		IdTipo INT NOT NULL,
		CONSTRAINT fkFestivo_Tipo FOREIGN KEY (IdTipo) REFERENCES Tipo(Id)
		);
ELSE
	PRINT 'Ya existe la tabla [Festivo]'