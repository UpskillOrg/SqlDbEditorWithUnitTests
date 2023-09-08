SqlDbEditorWithUnitTests

Introduction

SqlDbEditorWithUnitTests is a .NET application that allows you to edit and view data stored in a SQL Server database. This README provides step-by-step instructions on how to prepare your environment, restore the necessary database, and configure the application.

Prerequisites

Before you begin, ensure you have the following components installed on your system:

- SQL Server 2022 Express Edition
- SQL Server Management Studio
- Microsoft Visual Studio 2022 (or a compatible version)
- .NET Framework 4.6.1

Database Setup

Step 1: Install SQL Server 2022 Express Edition

Download and install SQL Server 2022 Express Edition from the official Microsoft website.

Step 2: Install SQL Server Management Studio

Install SQL Server Management Studio (SSMS) to manage your SQL Server instance effectively.

Step 3: Restore the Database

Restore the provided LinkTekTest.bak database backup file to your SQL Server instance using SSMS or SQL Server Management Studio Express.

Step 4: Create a Stored Procedure

Execute the following SQL script in SSMS to create the GetCustomers stored procedure within your LinkTekTest database:

USE [LinkTekTest]
GO

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

Application Configuration

Step 5: Update Connection String

Open the SqlDbEditor\SqlDbEditor\App.config file in your project and update the connection string to point to your SQL Server database. Modify the following connection string:

<add name="SqlDbEditor.Properties.Settings.LinkTekTestConnectionString"
     connectionString="Data Source=YOUR_SERVER_NAME;Initial Catalog=LinkTekTest;Persist Security Info=True;User ID=YOUR_USERNAME;Password=YOUR_PASSWORD;MultipleActiveResultSets=True"
     providerName="System.Data.SqlClient" />

Replace YOUR_SERVER_NAME, YOUR_USERNAME, and YOUR_PASSWORD with the appropriate values for your SQL Server instance.

Build and Run

Step 6: Build the Application

Open the solution in Visual Studio and build the application.

Step 7: Run the Application

Run the application. You should now be able to connect to the LinkTekTest database, view and edit its data using this application.

Note : If the application run on debug mode there will be Validation Exceptions are expected to be thrown so continue running application by click the F% button or in the exception dialog uncheck and say dont break for the Validation Exception type. The Validation exception strategy used to validate the UI fields in the WPF application.

Contributing

If you'd like to contribute to this project, please follow our contributing guidelines (CONTRIBUTING.md).

License

This project is licensed under the MIT License (LICENSE).
