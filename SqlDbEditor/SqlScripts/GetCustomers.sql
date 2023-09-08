USE [LinkTekTest]
GO
/****** Object:  StoredProcedure [dbo].[GetCustomers]    Script Date: 08-09-2023 22:22:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetCustomers]
    @Offset INT,
    @PageSize INT
AS
BEGIN
    SELECT CustomerId, FirstName, LastName, Address1, Address2, City, State, Zip, Phone, Age, Sales, CreatedTime, UpdatedTime 
	FROM dbo.Customer 
	ORDER BY CustomerId OFFSET @Offset 
	ROWS FETCH NEXT @PageSize ROWS ONLY
END