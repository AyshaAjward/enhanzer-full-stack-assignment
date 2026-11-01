# Enhanzer Full Stack Assignment

A Purchase Bill Management application developed as part of the Enhanzer Full Stack Developer technical assignment.

## Technologies Used

### Frontend

- Angular 21
- TypeScript
- HTML and CSS

### Backend

- ASP.NET Core Web API (.NET 8)
- C#
- Entity Framework Core

### Database

- Microsoft SQL Server

## Project Structure

```text
enhanzer-full-stack-assignment/
│
├── backend/
│   └── Enhanzer.Api/
│       ├── Controllers/
│       ├── Data/
│       ├── Models/
│       ├── Services/
│       ├── Program.cs
│       └── appsettings.json
│
├── database/
│   └── EnhanzerAssignment.sql
│
├── frontend/
│   ├── src/
│   ├── angular.json
│   └── package.json
│
└── README.md
```

## Features

- User authentication using the Enhanzer external POS API.
- Session-based authentication and protected Purchase Bill access.
- Retrieval and storage of user locations in SQL Server.
- Dynamic Batch dropdown populated from saved locations.
- Purchase Bill creation with multiple items.
- Automatic margin, total cost and total selling calculations.
- Input validation and error handling.
- Loading indicators and success messages.
- Purchase Bill and item persistence in SQL Server.

## Prerequisites

Install the following before running the application:

- Node.js and npm
- Angular CLI
- .NET 8 SDK
- Microsoft SQL Server
- SQL Server Management Studio (SSMS)

## Database Setup

1. Open SQL Server Management Studio.
2. Connect to your SQL Server instance.
3. Create a database named:

   `EnhanzerAssignment`

4. Open `database/EnhanzerAssignment.sql`.
5. Execute the script against the newly created database.

The script creates the required tables:

- Location_Details
- Purchase_Bills
- Purchase_Bill_Items

## Backend Setup

1. Open a terminal in the project root.
2. Navigate to the backend project:

   `cd backend/Enhanzer.Api`

3. Check the connection string in `appsettings.json` and update it to match your SQL Server instance if necessary.

4. Restore dependencies:

   `dotnet restore`

5. Run the backend:

   `dotnet run`

The API runs at:

`http://localhost:5286`

## Frontend Setup

1. Open another terminal in the project root.
2. Navigate to the frontend:

   `cd frontend`

3. Install dependencies:

   `npm install`

4. Start the Angular development server:

   `npm start`

The application is available at:

`http://localhost:4200`

## Purchase Bill Calculations

The application calculates:

- Margin = Standard Price - Standard Cost
- Total Cost = Standard Cost × Quantity × (1 - Discount / 100)
- Total Selling = Standard Price × Quantity

For example:

| Field          | Value |
| -------------- | ----: |
| Standard Cost  |   100 |
| Standard Price |   150 |
| Quantity       |     5 |
| Discount       |   20% |
| Total Cost     |   400 |
| Total Selling  |   750 |

## Database Persistence

Purchase bills are stored in `Purchase_Bills`, while their individual items are stored in `Purchase_Bill_Items`.

Each purchase bill item references its parent bill through `Purchase_Bill_Id`.

## Notes

- The application uses the external Enhanzer POS API for login authentication.
- Users must have valid credentials accepted by the external API.
- The default SQL Server connection string is configured for a local SQL Server Express instance. Update it if your environment differs.
