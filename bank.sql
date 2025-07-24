CREATE DATABASE Bank
GO
USE Bank
GO

CREATE TABLE People (
    Id NVARCHAR(450) NOT NULL PRIMARY KEY,
    Address NVARCHAR(MAX) NOT NULL,
    BirthDate DATETIMEOFFSET NOT NULL,
    Deleted BIT NOT NULL,
    Discriminator NVARCHAR(8) NOT NULL,
    Gender INT NOT NULL,
    Identification NVARCHAR(MAX) NOT NULL,
    LastName NVARCHAR(MAX) NOT NULL,
    Name NVARCHAR(MAX) NOT NULL,
    Phone NVARCHAR(MAX) NOT NULL,
    Password NVARCHAR(MAX), -- Solo aplica si Discriminator = 'Client'
    Status BIT              -- Solo aplica si Discriminator = 'Client'
);

CREATE TABLE Accounts (
    Id NVARCHAR(450) NOT NULL PRIMARY KEY,
    AccountNumber NVARCHAR(MAX) NOT NULL,
    Balance DECIMAL(18,2) NOT NULL,
    ClientId NVARCHAR(450) NOT NULL,
    Deleted BIT NOT NULL,
    Status BIT NOT NULL,
    Type INT NOT NULL,
    CONSTRAINT FK_Accounts_Client FOREIGN KEY (ClientId) REFERENCES People(Id) ON DELETE CASCADE
);

CREATE INDEX IX_Accounts_ClientId ON Accounts(ClientId);

CREATE TABLE Movements (
    Id NVARCHAR(450) NOT NULL PRIMARY KEY,
    AccountId NVARCHAR(450) NOT NULL,
    Date DATETIME2 NOT NULL,
    Deleted BIT NOT NULL,
    PreviousBalance DECIMAL(18,2) NOT NULL,
    Type INT NOT NULL,
    Value DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_Movements_Account FOREIGN KEY (AccountId) REFERENCES Accounts(Id) ON DELETE CASCADE
);

CREATE INDEX IX_Movements_AccountId ON Movements(AccountId);

GO

INSERT INTO People (Id, Address, BirthDate, Deleted, Discriminator, Gender, Identification, LastName, Name, Phone, Password, Status)
VALUES 
('client-001', 'Calle 1', '1990-01-01T00:00:00+00:00', 0, 'Client', 1, '00100000001', 'Gómez', 'Ana', '8091111111', 'pass1', 1),
('client-002', 'Calle 2', '1985-03-15T00:00:00+00:00', 0, 'Client', 2, '00100000002', 'Rodríguez', 'Luis', '8092222222', 'pass2', 1),
('client-003', 'Calle 3', '1978-07-20T00:00:00+00:00', 0, 'Client', 1, '00100000003', 'Pérez', 'María', '8093333333', 'pass3', 1),
('client-004', 'Calle 4', '1995-10-30T00:00:00+00:00', 0, 'Client', 1, '00100000004', 'Fernández', 'Carlos', '8094444444', 'pass4', 1),
('client-005', 'Calle 5', '2000-12-05T00:00:00+00:00', 0, 'Client', 2, '00100000005', 'Ramírez', 'Lucía', '8095555555', 'pass5', 1);

INSERT INTO Accounts (Id, AccountNumber, Balance, ClientId, Deleted, Status, Type)
VALUES 
('acc-001', 'AC10001', 1200.00, 'client-001', 0, 1, 0),
('acc-002', 'AC10002', 800.00, 'client-001', 0, 1, 1),
('acc-003', 'AC20001', 5000.00, 'client-002', 0, 1, 0),
('acc-004', 'AC30001', 3200.00, 'client-003', 0, 1, 0),
('acc-005', 'AC40001', 150.00, 'client-004', 0, 1, 1),
('acc-006', 'AC50001', 2500.00, 'client-005', 0, 1, 0),
('acc-007', 'AC50002', 340.00, 'client-005', 0, 1, 1);

INSERT INTO Movements (Id, AccountId, Date, Deleted, PreviousBalance, Type, Value)
VALUES
-- acc-001 (client-001)
('mov-001', 'acc-001', '2025-07-20T09:00:00', 0, 1000.00, 0, 200.00),
('mov-002', 'acc-001', '2025-07-21T10:00:00', 0, 1200.00, 1, 100.00),
('mov-003', 'acc-001', '2025-07-22T11:00:00', 0, 1100.00, 0, 100.00),

-- acc-002 (client-001)
('mov-004', 'acc-002', '2025-07-20T14:00:00', 0, 900.00, 1, 100.00),
('mov-005', 'acc-002', '2025-07-21T15:00:00', 0, 800.00, 0, 50.00),

-- acc-003 (client-002)
('mov-006', 'acc-003', '2025-07-20T08:00:00', 0, 5000.00, 1, 1000.00),
('mov-007', 'acc-003', '2025-07-21T09:30:00', 0, 4000.00, 0, 300.00),

-- acc-004 (client-003)
('mov-008', 'acc-004', '2025-07-20T10:00:00', 0, 3200.00, 1, 500.00),
('mov-009', 'acc-004', '2025-07-22T11:30:00', 0, 2700.00, 0, 100.00),

-- acc-005 (client-004)
('mov-010', 'acc-005', '2025-07-20T13:00:00', 0, 150.00, 0, 50.00),

-- acc-006 (client-005)
('mov-011', 'acc-006', '2025-07-21T08:00:00', 0, 2500.00, 1, 200.00),
('mov-012', 'acc-006', '2025-07-22T10:00:00', 0, 2300.00, 1, 100.00),

-- acc-007 (client-005)
('mov-013', 'acc-007', '2025-07-20T09:00:00', 0, 340.00, 0, 60.00),
('mov-014', 'acc-007', '2025-07-21T09:30:00', 0, 400.00, 1, 100.00);