-- Создание базы данных
USE master
GO
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'MedicalLaboratory20')
BEGIN
    ALTER DATABASE MedicalLaboratory20 SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE MedicalLaboratory20;
END
GO
CREATE DATABASE MedicalLaboratory20
GO
USE MedicalLaboratory20
GO

-- Создание таблицы Roles
CREATE TABLE [dbo].[Roles](
    [RoleId] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([RoleId] ASC),
    CONSTRAINT [UQ_Roles_Name] UNIQUE NONCLUSTERED ([Name] ASC)
)
GO

-- Вставка данных в Roles
INSERT INTO [dbo].[Roles] ([Name]) VALUES 
('Администратор'),
('Лаборант'),
('Лаборант-исследователь'),
('Бухгалтер')
GO

-- Создание таблицы InsuranceCompanies
CREATE TABLE [dbo].[InsuranceCompanies](
    [CompanyId] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [Address] [nvarchar](200) NOT NULL,
    [INN] [nvarchar](12) NOT NULL,
    [AccountNumber] [nvarchar](20) NOT NULL,
    [BIC] [nvarchar](9) NOT NULL,
    [IsArchived] [bit] NOT NULL DEFAULT 0,
    CONSTRAINT [PK_InsuranceCompanies] PRIMARY KEY CLUSTERED ([CompanyId] ASC)
)
GO

-- Создание таблицы Patients
CREATE TABLE [dbo].[Patients](
    [PatientId] [int] IDENTITY(1,1) NOT NULL,
    [Login] [nvarchar](50) NOT NULL,
    [Password] [nvarchar](100) NOT NULL,
    [FirstName] [nvarchar](50) NOT NULL,
    [LastName] [nvarchar](50) NOT NULL,
    [MiddleName] [nvarchar](50) NULL,
    [BirthDate] [date] NOT NULL,
    [PassportSeries] [nvarchar](4) NOT NULL,
    [PassportNumber] [nvarchar](6) NOT NULL,
    [Phone] [nvarchar](15) NOT NULL,
    [Email] [nvarchar](100) NULL,
    [InsurancePolicyNumber] [nvarchar](20) NOT NULL,
    [InsurancePolicyType] [nvarchar](50) NOT NULL,
    [InsuranceCompanyId] [int] NOT NULL,
    [Photo] [varbinary](max) NULL,
    [IsArchived] [bit] NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Patients] PRIMARY KEY CLUSTERED ([PatientId] ASC),
    CONSTRAINT [UQ_Patients_Login] UNIQUE NONCLUSTERED ([Login] ASC),
    CONSTRAINT [FK_Patients_InsuranceCompanies] FOREIGN KEY([InsuranceCompanyId]) REFERENCES [dbo].[InsuranceCompanies] ([CompanyId])
)
GO

-- Создание таблицы SystemUsers
CREATE TABLE [dbo].[SystemUsers](
    [UserId] [int] IDENTITY(1,1) NOT NULL,
    [Login] [nvarchar](50) NOT NULL,
    [Password] [nvarchar](100) NOT NULL,
    [FirstName] [nvarchar](50) NOT NULL,
    [LastName] [nvarchar](50) NOT NULL,
    [MiddleName] [nvarchar](50) NULL,
    [RoleId] [int] NOT NULL,
    [LastLogin] [datetime] NULL,
    [Photo] [varbinary](max) NULL,
    [IsBlocked] [bit] NOT NULL DEFAULT 0,
    [BlockedUntil] [datetime] NULL,
    [IsArchived] [bit] NOT NULL DEFAULT 0,
    [LastEnterTime] [datetime] NULL,
    [SessionStartTime] [datetime] NULL,
    [SessionDuration] [int] NULL,
    CONSTRAINT [PK_SystemUsers] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [UQ_SystemUsers_Login] UNIQUE NONCLUSTERED ([Login] ASC),
    CONSTRAINT [FK_SystemUsers_Roles] FOREIGN KEY([RoleId]) REFERENCES [dbo].[Roles] ([RoleId])
)
GO

-- Создание таблицы LoginHistory
CREATE TABLE [dbo].[LoginHistory](
    [LoginHistoryId] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NULL,
    [Login] [nvarchar](50) NOT NULL,
    [AttemptTime] [datetime] NOT NULL DEFAULT GETDATE(),
    [IsSuccess] [bit] NOT NULL,
    [IPAddress] [nvarchar](15) NULL,
    [UserAgent] [nvarchar](500) NULL,
    [DeviceType] [nvarchar](50) NULL,
    CONSTRAINT [PK_LoginHistory] PRIMARY KEY CLUSTERED ([LoginHistoryId] ASC),
    CONSTRAINT [FK_LoginHistory_SystemUsers] FOREIGN KEY([UserId]) REFERENCES [dbo].[SystemUsers] ([UserId])
)
GO

-- Создание таблицы Services
CREATE TABLE [dbo].[Services](
    [ServiceId] [int] IDENTITY(1,1) NOT NULL,
    [Code] [nvarchar](10) NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [Price] [decimal](10, 2) NOT NULL,
    [ExecutionTimeDays] [int] NOT NULL,
    [AverageDeviation] [decimal](10, 2) NULL,
    [IsArchived] [bit] NOT NULL DEFAULT 0,
    [AvailableAnalyzers] [nvarchar](max) NULL,
    CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED ([ServiceId] ASC),
    CONSTRAINT [UQ_Services_Code] UNIQUE NONCLUSTERED ([Code] ASC)
)
GO

-- Вставка данных в Services
INSERT INTO [dbo].[Services] ([Code], [Name], [Price], [ExecutionTimeDays], [AverageDeviation], [AvailableAnalyzers]) VALUES 
('619', 'TSH', 262.71, 1, NULL, 'Ledetect | Biorad'),
('311', 'Амилаза', 361.88, 1, NULL, 'Ledetect'),
('548', 'Альбумин', 234.09, 1, NULL, 'Biorad'),
('258', 'Креатинин', 143.22, 1, NULL, 'Biorad | Ledetect'),
('176', 'Билирубин общий', 102.85, 1, NULL, 'Biorad'),
('501', 'Гепатит В', 176.83, 1, NULL, 'Ledetect'),
('543', 'Гепатит С', 289.99, 1, NULL, 'Ledetect | Biorad'),
('557', 'ВИЧ', 490.77, 1, NULL, 'Ledetect'),
('229', 'СПИД', 341.78, 1, NULL, 'Ledetect'),
('415', 'Кальций общий', 419.90, 1, NULL, 'Ledetect'),
('323', 'Глюкоза', 447.65, 1, NULL, 'Ledetect'),
('855', 'Ковид IgM', 209.78, 1, NULL, 'Biorad'),
('346', 'Общий белок', 396.03, 1, NULL, 'Ledetect'),
('836', 'Железо', 105.32, 1, NULL, 'Biorad'),
('659', 'Сифилис RPR', 443.66, 1, NULL, 'Ledetect | Biorad'),
('797', 'АТ и АГ к ВИЧ 1/2', 370.62, 1, NULL, 'Biorad'),
('287', 'Волчаночный антикоагулянт', 290.11, 1, NULL, 'Biorad')
GO

-- Создание таблицы UserServices
CREATE TABLE [dbo].[UserServices](
    [UserServiceId] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NOT NULL,
    [ServiceId] [int] NOT NULL,
    CONSTRAINT [PK_UserServices] PRIMARY KEY CLUSTERED ([UserServiceId] ASC),
    CONSTRAINT [FK_UserServices_SystemUsers] FOREIGN KEY([UserId]) REFERENCES [dbo].[SystemUsers] ([UserId]),
    CONSTRAINT [FK_UserServices_Services] FOREIGN KEY([ServiceId]) REFERENCES [dbo].[Services] ([ServiceId])
)
GO

-- Создание таблицы Analyzers
CREATE TABLE [dbo].[Analyzers](
    [AnalyzerId] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [IsArchived] [bit] NOT NULL DEFAULT 0,
    [AnalyzerModel] [nvarchar](100) NULL,
    [AnalyzerManufacturer] [nvarchar](100) NULL,
    [LastCalibrationDate] [datetime] NULL,
    CONSTRAINT [PK_Analyzers] PRIMARY KEY CLUSTERED ([AnalyzerId] ASC)
)
GO

-- Вставка данных в Analyzers
INSERT INTO [dbo].[Analyzers] ([Name], [AnalyzerModel], [AnalyzerManufacturer], [LastCalibrationDate]) VALUES 
('Ledetect', 'LD-2000', 'MediTech', GETDATE()),
('Biorad', 'BR-500', 'BioSystems', GETDATE())
GO

-- Создание таблицы Orders
CREATE TABLE [dbo].[Orders](
    [OrderId] [int] IDENTITY(1,1) NOT NULL,
    [PatientId] [int] NOT NULL,
    [Barcode] [nvarchar](20) NOT NULL,
    [CreationDate] [datetime] NOT NULL DEFAULT GETDATE(),
    [Status] [nvarchar](20) NOT NULL DEFAULT 'New',
    [IsArchived] [bit] NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([OrderId] ASC),
    CONSTRAINT [FK_Orders_Patients] FOREIGN KEY([PatientId]) REFERENCES [dbo].[Patients] ([PatientId])
)
GO

-- Создание таблицы OrderServices
CREATE TABLE [dbo].[OrderServices](
    [OrderServiceId] [int] IDENTITY(1,1) NOT NULL,
    [OrderId] [int] NOT NULL,
    [ServiceId] [int] NOT NULL,
    [Status] [nvarchar](20) NOT NULL DEFAULT 'Pending',
    [ExecutionTimeSeconds] [int] NULL,
    [IsArchived] [bit] NOT NULL DEFAULT 0,
    CONSTRAINT [PK_OrderServices] PRIMARY KEY CLUSTERED ([OrderServiceId] ASC),
    CONSTRAINT [FK_OrderServices_Orders] FOREIGN KEY([OrderId]) REFERENCES [dbo].[Orders] ([OrderId]),
    CONSTRAINT [FK_OrderServices_Services] FOREIGN KEY([ServiceId]) REFERENCES [dbo].[Services] ([ServiceId])
)
GO

-- Создание таблицы PerformedServices
CREATE TABLE [dbo].[PerformedServices](
    [PerformedServiceId] [int] IDENTITY(1,1) NOT NULL,
    [OrderServiceId] [int] NOT NULL,
    [PerformedDate] [datetime] NOT NULL DEFAULT GETDATE(),
    [AnalyzerId] [int] NOT NULL,
    [Result] [nvarchar](max) NULL,
    [IsArchived] [bit] NOT NULL DEFAULT 0,
    CONSTRAINT [PK_PerformedServices] PRIMARY KEY CLUSTERED ([PerformedServiceId] ASC),
    CONSTRAINT [FK_PerformedServices_Analyzers] FOREIGN KEY([AnalyzerId]) REFERENCES [dbo].[Analyzers] ([AnalyzerId]),
    CONSTRAINT [FK_PerformedServices_OrderServices] FOREIGN KEY([OrderServiceId]) REFERENCES [dbo].[OrderServices] ([OrderServiceId])
)
GO

-- Создание таблицы InsuranceInvoices
CREATE TABLE [dbo].[InsuranceInvoices](
    [InvoiceId] [int] IDENTITY(1,1) NOT NULL,
    [InsuranceCompanyId] [int] NOT NULL,
    [OrderId] [int] NOT NULL,
    [CreationDate] [datetime] NOT NULL DEFAULT GETDATE(),
    [Amount] [decimal](10, 2) NOT NULL,
    [IsPaid] [bit] NOT NULL DEFAULT 0,
    [IsArchived] [bit] NOT NULL DEFAULT 0,
    CONSTRAINT [PK_InsuranceInvoices] PRIMARY KEY CLUSTERED ([InvoiceId] ASC),
    CONSTRAINT [FK_InsuranceInvoices_InsuranceCompanies] FOREIGN KEY([InsuranceCompanyId]) REFERENCES [dbo].[InsuranceCompanies] ([CompanyId]),
    CONSTRAINT [FK_InsuranceInvoices_Orders] FOREIGN KEY([OrderId]) REFERENCES [dbo].[Orders] ([OrderId])
)
GO

-- Создание таблицы Biomaterials
CREATE TABLE [dbo].[Biomaterials](
    [BiomaterialId] [int] IDENTITY(1,1) NOT NULL,
    [Barcode] [nvarchar](50) NOT NULL,
    [PatientId] [int] NULL,
    [Type] [nvarchar](50) NOT NULL,
    [CollectionDateTime] [datetime] NOT NULL,
    [Status] [nvarchar](50) NOT NULL DEFAULT 'Зарегистрирован',
    [Notes] [nvarchar](max) NULL,
    [IsArchived] [bit] NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Biomaterials] PRIMARY KEY CLUSTERED ([BiomaterialId] ASC),
    CONSTRAINT [UQ_Biomaterials_Barcode] UNIQUE NONCLUSTERED ([Barcode] ASC),
    CONSTRAINT [FK_Biomaterials_Patients] FOREIGN KEY([PatientId]) REFERENCES [dbo].[Patients] ([PatientId])
)
GO

-- Создание таблицы Supplies
CREATE TABLE [dbo].[Supplies](
    [SupplyId] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [Quantity] [int] NOT NULL,
    [Unit] [nvarchar](20) NOT NULL,
    [IsArchived] [bit] NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Supplies] PRIMARY KEY CLUSTERED ([SupplyId] ASC)
)
GO

-- Вставка тестовых данных для SystemUsers
INSERT INTO [dbo].[SystemUsers] 
([Login], [Password], [FirstName], [LastName], [MiddleName], [RoleId], [LastLogin], [IsBlocked], [IsArchived])
VALUES 
('admin', 'admin123', 'Иван', 'Иванов', 'Иванович', 1, GETDATE(), 0, 0),
('lab1', 'lab1123', 'Петр', 'Петров', 'Петрович', 2, GETDATE(), 0, 0),
('lab2', 'lab2123', 'Сергей', 'Сергеев', 'Сергеевич', 3, GETDATE(), 0, 0),
('accountant', 'acc123', 'Анна', 'Сидорова', 'Алексеевна', 4, GETDATE(), 0, 0)
GO

-- Вставка тестовых данных для InsuranceCompanies
INSERT INTO [dbo].[InsuranceCompanies] 
([Name], [Address], [INN], [AccountNumber], [BIC], [IsArchived])
VALUES 
('Страховая Компания 1', 'г. Москва, ул. Ленина, 1', '123456789012', '40702810100010000001', '044525201', 0),
('Страховая Компания 2', 'г. Санкт-Петербург, Невский пр., 10', '987654321098', '40702810200020000002', '044525202', 0)
GO

-- Вставка тестовых данных для Patients
INSERT INTO [dbo].[Patients] 
([Login], [Password], [FirstName], [LastName], [MiddleName], [BirthDate], [PassportSeries], [PassportNumber], 
 [Phone], [Email], [InsurancePolicyNumber], [InsurancePolicyType], [InsuranceCompanyId], [IsArchived])
VALUES 
('patient1', 'patient123', 'Ольга', 'Смирнова', 'Владимировна', '1980-05-15', '1234', '567890', 
 '79101234567', 'olga@mail.ru', '1234567890', 'ОМС', 1, 0),
('patient2', 'patient234', 'Алексей', 'Кузнецов', 'Сергеевич', '1975-10-22', '5678', '123456', 
 '79209876543', 'alex@mail.ru', '0987654321', 'ДМС', 2, 0)
GO

-- Вставка тестовых данных для Orders
INSERT INTO [dbo].[Orders] 
([PatientId], [Barcode], [CreationDate], [Status], [IsArchived])
VALUES 
(1, '7270614', '2020-06-01', 'Completed', 0),
(2, '7881540', '2020-06-02', 'InProgress', 0)
GO

-- Вставка тестовых данных для OrderServices
INSERT INTO [dbo].[OrderServices] 
([OrderId], [ServiceId], [Status], [ExecutionTimeSeconds], [IsArchived])
VALUES 
(1, 1, 'Completed', 3600, 0),
(1, 2, 'Completed', 1800, 0),
(2, 3, 'InProgress', NULL, 0)
GO

-- Вставка тестовых данных для PerformedServices
INSERT INTO [dbo].[PerformedServices] 
([OrderServiceId], [PerformedDate], [AnalyzerId], [Result], [IsArchived])
VALUES 
(1, '2020-06-02', 1, '0.20596', 0),
(2, '2020-06-02', 2, '0.32315', 0)
GO

-- Вставка тестовых данных для Biomaterials
INSERT INTO [dbo].[Biomaterials] 
([Barcode], [PatientId], [Type], [CollectionDateTime], [Status], [Notes], [IsArchived])
VALUES 
('7270614', 1, 'Кровь', '2020-06-01', 'Зарегистрирован', 'Первичный забор', 0),
('7881540', 2, 'Моча', '2020-06-02', 'Зарегистрирован', 'Плановый забор', 0)
GO

-- Вставка тестовых данных для Supplies
INSERT INTO [dbo].[Supplies] 
([Name], [Quantity], [Unit], [IsArchived])
VALUES 
('Пробирки', 1000, 'шт', 0),
('Перчатки', 500, 'пар', 0),
('Реактивы для анализа', 200, 'комплект', 0)
GO

-- Вставка тестовых данных для LoginHistory
INSERT INTO [dbo].[LoginHistory] 
([UserId], [Login], [AttemptTime], [IsSuccess], [IPAddress], [UserAgent], [DeviceType])
VALUES 
(1, 'admin', '2023-05-01 09:00:00', 1, '192.168.1.1', 'Chrome/Windows', 'Desktop'),
(2, 'lab1', '2023-05-01 09:05:00', 1, '192.168.1.2', 'Firefox/Windows', 'Desktop'),
(3, 'lab2', '2023-05-01 09:10:00', 1, '192.168.1.3', 'Edge/Windows', 'Desktop'),
(4, 'accountant', '2023-05-01 09:15:00', 1, '192.168.1.4', 'Chrome/Windows', 'Desktop')
GO