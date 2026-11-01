USE EnhanzerAssignment;
GO

CREATE TABLE Location_Details (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Location_Code NVARCHAR(100) NOT NULL,
    Location_Name NVARCHAR(200) NOT NULL,
    Created_At DATETIME2 DEFAULT GETDATE()
);
GO

CREATE TABLE Purchase_Bills (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Created_At DATETIME2 DEFAULT GETDATE()
);
GO

CREATE TABLE Purchase_Bill_Items (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Purchase_Bill_Id INT NOT NULL,
    Item_Name NVARCHAR(100) NOT NULL,
    Batch_Name NVARCHAR(200) NOT NULL,
    Standard_Cost DECIMAL(18,2) NOT NULL,
    Standard_Price DECIMAL(18,2) NOT NULL,
    Margin DECIMAL(18,2) NOT NULL,
    Quantity INT NOT NULL,
    Free_Quantity INT NOT NULL,
    Discount DECIMAL(5,2) NOT NULL,
    Total_Cost DECIMAL(18,2) NOT NULL,
    Total_Selling DECIMAL(18,2) NOT NULL,

    CONSTRAINT FK_PurchaseBillItems_PurchaseBills
    FOREIGN KEY (Purchase_Bill_Id)
    REFERENCES Purchase_Bills(Id)
);
GO