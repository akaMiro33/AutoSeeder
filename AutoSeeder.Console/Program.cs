// See https://aka.ms/new-console-template for more information

//string tableSchema = "CREATE TABLE Persons (\r\n    PersonID int,\r\n    LastName varchar(255),\r\n    FirstName varchar(255),\r\n    Address varchar(255),\r\n    City varchar(255)\r\n);";
using AutoSeeder.Data.Common.DataTypeFactory;
using AutoSeeder.Services;
using AutoSeeder.Services.Common.ConstraintParsing;
using AutoSeeder.Services.Parser;
using AutoSeeder.Services.Seed;
using System;

//string inputTableSchema = "CREATE TABLE Persons (\r\n    PersonID DECIMAL(19,4)  NOT NULL,\r\n    LastName varchar(255)  NOT NULL ,\r\n    FirstName varchar(255)  ,\r\n    Address varchar(255),\r\n    City varchar(255)\r\n);  CREATE TABLE Cars (\r\n    CarID int,\r\n    Type varchar(255),\r\n    Color varchar(255),\r\n    Year varchar(255),\r\n    Country varchar(255)\r\n);";
//string inputTableSchema = """
//    CREATE TABLE Customers (
//        CustomerId INT IDENTITY(1,1) PRIMARY KEY,
//        Name NVARCHAR(100) NOT NULL,
//        Email NVARCHAR(255) UNIQUE
//    );

//    CREATE TABLE Orders (
//        OrderId INT IDENTITY(1,1) PRIMARY KEY,
//        CustomerId INT NOT NULL FOREIGN KEY REFERENCES Customers (CustomerId),
//        OrderDate DATETIME2 NOT NULL,
//        TotalAmount DECIMAL(10, 2) NOT NULL 
//    );
//    """;


//string inputTableSchema = """
//    CREATE TABLE Customers (
//        CustomerId INT PRIMARY KEY,
//        Name NVARCHAR(100) NOT NULL,
//        Email NVARCHAR(255) UNIQUE
//    );

//    CREATE TABLE Orders (
//        OrderId INT PRIMARY KEY,
//        CustomerId INT NOT NULL FOREIGN KEY REFERENCES Customers (CustomerId),
//        OrderDate DATETIME2 NOT NULL,
//        TotalAmount DECIMAL(10, 2) NOT NULL 
//    );

//    CREATE TABLE OrderItems (
//        OrderItemId INT PRIMARY KEY,
//        OrderId INT NOT NULL FOREIGN KEY REFERENCES Orders (OrderId),
//        ProductName NVARCHAR(200) NOT NULL,
//        Quantity INT NOT NULL,
//        UnitPrice DECIMAL(10, 2) NOT NULL    
//    );



//    """;


//string inputTableSchema = """
//    CREATE TABLE Customers (
//        CustomerId INT PRIMARY KEY,
//        Name NVARCHAR(100) NOT NULL,
//        Email NVARCHAR(255) UNIQUE
//    );

//    CREATE TABLE Orders (
//        OrderId INT PRIMARY KEY,
//        CustomerId INT NOT NULL FOREIGN KEY REFERENCES Customers (CustomerId),
//        OrderDate DATETIME2 NOT NULL,
//        TotalAmount DECIMAL(10, 2) NOT NULL 
//    );



//         CREATE TABLE OrderItems (
//         OrderItemId INT PRIMARY KEY,
//         OrderId INT NOT NULL ,
//         ProductName NVARCHAR(200) NOT NULL,
//         Quantity INT NOT NULL,
//         UnitPrice DECIMAL(10, 2) NOT NULL,    
//    	 CONSTRAINT  FK_OrderItems_Orders FOREIGN KEY (OrderId) REFERENCES Orders (OrderId) ON DELETE CASCADE ON UPDATE NO ACTION
//     );

//    """;

//string inputTableSchema = """
//    CREATE TABLE Customers (
//        CustomerId INT PRIMARY KEY,
//        Name NVARCHAR(100) NOT NULL,
//        Email NVARCHAR(255) UNIQUE
//    );

//    CREATE TABLE Orders (
//        OrderId INT PRIMARY KEY,
//        CustomerId INT NOT NULL FOREIGN KEY REFERENCES Customers (CustomerId),
//        OrderDate DATETIME2 NOT NULL,
//        TotalAmount DECIMAL(10, 2) NOT NULL 
//    );

//    CREATE TABLE OrderItems (
//         OrderItemId INT,
//         ItemId INT,
//         OrderId INT NOT NULL,
//         ProductName NVARCHAR(200) NOT NULL,
//         Quantity INT NOT NULL,
//         UnitPrice DECIMAL(10, 2) NOT NULL,   
//         PRIMARY KEY (OrderItemId, ItemId),
//    	 CONSTRAINT  FK_OrderItems_Orders FOREIGN KEY (OrderId) REFERENCES Orders (OrderId) ON DELETE CASCADE ON UPDATE NO ACTION
//     );

//    CREATE TABLE OrderItemParts (
//         OrderItemPartId INT,
//         OrderItemId INT,
//         ItemId INT,
//         ProductName NVARCHAR(200) NOT NULL,
//         Quantity INT NOT NULL,
//         UnitPrice DECIMAL(10, 2) NOT NULL,   
//         PRIMARY KEY (OrderItemPartId),
//    	 CONSTRAINT FK_OrderItemParts_OrderItems FOREIGN KEY (OrderItemId, ItemId) REFERENCES OrderItems (OrderItemId, ItemId) ON DELETE CASCADE ON UPDATE NO ACTION
//     );


//    """;

string inputTableSchema = """
    CREATE TABLE OrderItems (
         OrderItemId INT,
         ItemId INT,
         OrderId INT NOT NULL,
         ProductName NVARCHAR(200) NOT NULL,
         Quantity INT NOT NULL,
         UnitPrice DECIMAL(10, 2) NOT NULL,   
         PRIMARY KEY (OrderItemId, ItemId)
    	 
     );

    CREATE TABLE OrderItemParts (
         OrderItemPartId INT,
         OrderItemId INT,
         ItemId INT,
         ProductName NVARCHAR(200) NOT NULL,
         Quantity INT NOT NULL,
         UnitPrice DECIMAL(10, 2) NOT NULL,   
         PRIMARY KEY (OrderItemPartId),
    	 CONSTRAINT FK_OrderItemParts_OrderItems FOREIGN KEY (OrderItemId, ItemId) REFERENCES OrderItems (OrderItemId, ItemId) ON DELETE CASCADE ON UPDATE NO ACTION
     );


    """;


//string inputTableSchema = """
//    CREATE TABLE OrderItems (
//         OrderItemId INT,
//         ItemId INT,
//         OrderId INT NOT NULL,
//         ProductName NVARCHAR(200) NOT NULL,
//         Quantity INT NOT NULL,
//         UnitPrice DECIMAL(10, 2) NOT NULL,   
//         PRIMARY KEY (OrderItemId)

//     );

//    CREATE TABLE OrderItemParts (
//         OrderItemPartId INT,
//         OrderItemId INT,
//         ProductName NVARCHAR(200) NOT NULL,
//         Quantity INT NOT NULL,
//         UnitPrice DECIMAL(10, 2) NOT NULL,   
//         PRIMARY KEY (OrderItemPartId),
//    	 CONSTRAINT FK_OrderItemParts_OrderItems FOREIGN KEY (OrderItemId) REFERENCES OrderItems (OrderItemId) ON DELETE CASCADE ON UPDATE NO ACTION
//     );


//    """;



//string inputTableSchema = """
//    CREATE TABLE Customers (
//        CustomerId INT PRIMARY KEY,
//        Name NVARCHAR(100) NOT NULL,
//        Email NVARCHAR(255) UNIQUE
//    );

//    CREATE TABLE Orders (
//        OrderId INT PRIMARY KEY,
//        CustomerId INT NOT NULL FOREIGN KEY REFERENCES Customers (CustomerId),
//        OrderDate DATETIME2 NOT NULL,
//        TotalAmount DECIMAL(10, 2) NOT NULL 
//    );



//         CREATE TABLE OrderItems (
//         OrderItemId INT PRIMARY KEY,
//         OrderId INT NOT NULL ,
//         ProductName NVARCHAR(200) NOT NULL,
//         Quantity INT NOT NULL,
//         UnitPrice DECIMAL(10, 2) NOT NULL,    
//         FOREIGN KEY (OrderId) REFERENCES Orders (OrderId)
//     );

//    """;



var parsers = new List<IColumnConstraintParser>()
{
   new NotNullConstraintParser(),
   new PrimaryKeyConstraintParser(),
   new ForeignKeyConstraintParser(),
   new UniqueConstraintParser(),
   new DefaultConstraintParser(),
   new IdentityConstraintParser()
};

IDataTypeFactory dataTypeFactory = new SqlTypeFactory();
var parserService = new ParserService();
var seedingCreationService = new SeedCreationService();
var createTablesService = new TableRepresentationService(parserService, parsers, dataTypeFactory);
var seedService = new SeedService(createTablesService, seedingCreationService);
seedService.Create(inputTableSchema);

// to-do - composite primary keys and foreign keys
// to-do - everything that is not involved in CREATE TABLE - that is things like ADD CONSTRAINTS









