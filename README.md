# Student Management

An ASP.NET Core MVC application for listing, adding, viewing, editing, and deleting student records. It includes form validation and Razor views.

## Run locally

Install the .NET 8 SDK and run the following from the repository folder.

    dotnet run -- --environment Development --urls http://127.0.0.1:5000

Open [the student list](http://127.0.0.1:5000). The app initializes a local SQLite database at App_Data/students.db on first run and saves records there between sessions. Use sample student details for the demo.

## SQL Server

The original Entity Framework Core SQL Server migrations are in Migrations. To use them, supply a SQL Server connection string through the ConnectionStrings__dbcs environment variable. Apply the migrations with dotnet ef database update (using the EF Core CLI) before starting the app.
