CREATE TABLE dbo.Category
(
  CategoryId     INT           NOT NULL IDENTITY(1,1),
  CategoryTypeId INT           NOT NULL,
  CategoryName   NVARCHAR(100) NOT NULL,
  CONSTRAINT pkcCategory PRIMARY KEY CLUSTERED (CategoryId),
  CONSTRAINT fkCategory_CategoryType FOREIGN KEY (CategoryTypeId) REFERENCES dbo.CategoryType (CategoryTypeId)
)