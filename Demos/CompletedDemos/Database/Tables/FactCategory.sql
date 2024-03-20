CREATE TABLE dbo.FactCategory
(
  FactCategoryId INT NOT NULL IDENTITY(1,1),
  FactId         INT NOT NULL,
  CategoryId     INT NOT NULL,
  CONSTRAINT pkcFactCategory PRIMARY KEY CLUSTERED (FactCategoryId),
  CONSTRAINT fkFactCategory_Fact         FOREIGN KEY (FactId)     REFERENCES dbo.Fact (FactId),
  CONSTRAINT fkFactCategory_FactCategory FOREIGN KEY (CategoryId) REFERENCES dbo.Category (CategoryId)
)