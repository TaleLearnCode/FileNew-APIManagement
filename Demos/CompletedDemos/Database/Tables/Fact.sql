﻿CREATE TABLE dbo.Fact
(
  FactId  INT           NOT NULL IDENTITY(1,1),
  Content NVARCHAR(200) NOT NULL,
  CONSTRAINT pkcFact PRIMARY KEY CLUSTERED (FactId)
)