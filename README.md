# Library Management API

## Getting Started

This project provides a Library Management API to manage library books, users, and borrowing records. The setup includes automatic database creation, seeding, and migration using Entity Framework Core, so the process is streamlined. Follow the steps below to get up and running.

### Prerequisites

- **.NET SDK**: Ensure that the .NET SDK is installed.
- **SQL Server**: A running instance of SQL Server is required for this application.

### Setting Up the Database

The `LibraryManagement.API` project is configured to handle the database setup and seeding automatically when you build and run the project. This is achieved through Entity Framework Core migrations, which are executed when the API starts.

The migrations create necessary tables and seed initial data for `Books`, `Users`, and `BorrowingRecords` in the database.

### Steps to Run the Application

1. **Clone the Repository**:
   Clone this repository to your local machine.

2. **Configure the Connection String**:

   - In `appsettings.json`, configure the `DefaultConnection` string to point to your SQL Server instance.

3. **Build and Run the Application**:

   - Open the solution in Visual Studio or your preferred editor.
   - Set `LibraryManagement.API` as the startup project.
   - Build and run the application.

   **Note**: On startup, the application will:

   - Create the `LibraryManagement` database if it doesn’t already exist.
   - Run any pending migrations to ensure the database schema is up-to-date.
   - Seed initial data for books, users, and borrowing records.

4. **Access the API**:
   - Once running, you can access the API at the specified localhost URL.
   - Swagger documentation is available at `/swagger`, providing an interactive interface to explore and test the API endpoints.

### Additional Details

For more detailed configuration and service setup, refer to the following files:

- `LibraryManagement.API/Program.cs`: Contains the application’s main configuration, including the setup for logging, dependency injection, and automatic database migration.
- `LibraryManagement.Infrastructure/Data/LibraryDbContext.cs`: Defines the database context and includes the initial data seeding.

The application is now fully operational and ready for development, testing, or production deployment.

## There is No need to setup database using script, as it is taken care in .net application itself however if anyone wants to do it. following are the instructions

## Database Setup Instructions

1. **Open SQL Server Management Studio (SSMS)** and connect to your SQL Server instance.
2. **Open a New Query** window.
3. **Copy and paste the SQL script** into the query window.
4. **Execute the script** by clicking the **Execute** button or pressing **F5**.
5. **Verify the database and tables** by expanding the `LibraryManagement` database in the Object Explorer.

This will create the `LibraryManagement` database along with the `Books`, `Users`, and `BorrowingRecords` tables.
