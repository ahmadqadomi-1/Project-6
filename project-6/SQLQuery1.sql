USE MiniProject;
------------------------------------------------------Break------------------------------------------------------!
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Category;
DROP TABLE IF EXISTS Products;
DROP TABLE IF EXISTS Cart;
DROP TABLE IF EXISTS ShoppingCartItems;
------------------------------------------------------Break------------------------------------------------------!
CREATE TABLE Users (
UserID INT PRIMARY KEY IDENTITY(1,1),
Fname_User varchar (50) NOT NULL,
Lname_User varchar (50) NOT NULL,
User_Email varchar (50) NOT NULL UNIQUE,
User_Password VARCHAR (100) NOT NULL
);
------------------------------------------------------Break------------------------------------------------------!
CREATE TABLE Category (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);
------------------------------------------------------Break------------------------------------------------------!
CREATE TABLE Products (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Price FLOAT NOT NULL,
    Image_URL NVARCHAR(MAX) DEFAULT 'DefaultProduct.png',
    Description NVARCHAR(MAX),
    CategoryID INT NOT NULL,
    FOREIGN KEY (CategoryID) REFERENCES Category(ID)
);

------------------------------------------------------Break------------------------------------------------------!
CREATE TABLE Cart (
    CartID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    CreateDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

------------------------------------------------------Break------------------------------------------------------!
CREATE TABLE ShoppingCartItems (
    CartItemID INT IDENTITY(1,1) PRIMARY KEY,
    CartID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    CreateDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (CartID) REFERENCES Cart(CartID),
    FOREIGN KEY (ProductID) REFERENCES Products(ID)
);
------------------------------------------------------Break------------------------------------------------------!



