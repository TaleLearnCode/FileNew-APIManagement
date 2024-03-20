MERGE INTO dbo.CategoryType AS TARGET
USING (VALUES ( 1, 'Fact'),
              ( 2, 'Quote'))
AS SOURCE (CategoryTypeId, CategoryTypeName)
ON TARGET.CategoryTypeId = SOURCE.CategoryTypeId
WHEN MATCHED THEN UPDATE SET TARGET.CategoryTypeName = SOURCE.CategoryTypeName
WHEN NOT MATCHED BY TARGET THEN INSERT (CategoryTypeId,
                                        CategoryTypeName)
                                 VALUES (SOURCE.CategoryTypeId,
                                         SOURCE.CategoryTypeName)
WHEN NOT MATCHED BY SOURCE THEN DELETE;
GO