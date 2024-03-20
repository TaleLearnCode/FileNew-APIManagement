CREATE TABLE dbo.QuoteCategory
(
  QuoteCategoryId INT NOT NULL IDENTITY(1,1),
  QuoteId         INT NOT NULL,
  CategoryId      INT NOT NULL,
  CONSTRAINT pkcQuoteCategory PRIMARY KEY (QuoteCategoryId),
  CONSTRAINT fkQuoteCategory_Quote FOREIGN KEY (QuoteId) REFERENCES dbo.Quote (QuoteId),
  CONSTRAINT fkQuoteCategory_QuoteCategory FOREIGN KEY (CategoryId) REFERENCES dbo.Category (CategoryId)
)