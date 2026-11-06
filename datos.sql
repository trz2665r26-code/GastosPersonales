-- Práctica 10: Control de gastos personales
CREATE DATABASE GastosPersonalesDB;
GO
USE GastosPersonalesDB;
GO

CREATE TABLE Categoria (
    IdCategoria INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre      NVARCHAR(50) NOT NULL UNIQUE
);
GO

CREATE TABLE Gasto (
    IdGasto     INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Descripcion NVARCHAR(100) NOT NULL,
    Monto       DECIMAL(10,2) NOT NULL CHECK (Monto > 0),
    Fecha       DATE NOT NULL,
    IdCategoria INT NOT NULL,
    CONSTRAINT FK_Gasto_Categoria FOREIGN KEY (IdCategoria) REFERENCES Categoria(IdCategoria)
);
GO

INSERT INTO Categoria (Nombre) VALUES
('Alimentación'), ('Transporte'), ('Educación'), ('Entretenimiento'), ('Salud'), ('Servicios');
GO

INSERT INTO Gasto (Descripcion, Monto, Fecha, IdCategoria) VALUES
('Supermercado semanal',      2500.00, '2026-10-03', 1),
('Almuerzo en la universidad', 350.00, '2026-10-05', 1),
('Pasaje de guagua',           100.00, '2026-10-06', 2),
('Gasolina',                  1800.00, '2026-10-08', 2),
('Libros de programación',    3200.00, '2026-10-10', 3),
('Cine con amigos',            700.00, '2026-10-12', 4),
('Consulta médica',           1500.00, '2026-10-15', 5),
('Factura de internet',       1200.00, '2026-10-18', 6),
('Pago de luz',               2100.00, '2026-10-20', 6),
('Cena familiar',             1600.00, '2026-09-25', 1),
('Suscripción de streaming',   450.00, '2026-09-28', 4);
GO
