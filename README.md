# Tasty Treat Backend API

A .NET 8 Web API project for the Tasty Treat food ordering system with complete CRUD operations for all entities.

## Project Structure

```
Tasty_Treat_be/
├── Controllers/          # API Controllers with CRUD endpoints
├── Data/                # Database context
├── DTOs/                # Data Transfer Objects
├── Interfaces/
│   ├── Repository/      # Repository interfaces
│   └── Service/         # Service interfaces
├── Models/              # Entity models
├── Profiles/            # AutoMapper profiles
├── Repositories/        # Repository implementations
└── Services/            # Service implementations
```

## Technologies Used

- **.NET 8.0** - Latest .NET framework
- **Entity Framework Core 8.0** - ORM for database operations
- **SQL Server** - Database
- **AutoMapper 13.0** - Object-to-object mapping
- **Swagger/OpenAPI** - API documentation

## Entities

1. **User** - System users (customers, delivery personnel, etc.)
2. **ChatMsg** - Chat messages between users
3. **Order** - Customer orders
4. **Item** - Food items in the menu
5. **CustomizationOption** - Customization options for items
6. **OrderItem** - Items within an order
7. **Review** - Customer reviews for items
8. **Delivery** - Delivery information for orders
9. **InstantQuote** - Quick price quotes for orders
10. **Payment** - Payment information for orders

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server or LocalDB

### Installation

1. Navigate to the project directory
2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

### Database Setup

1. Update the connection string in `appsettings.json` to match your SQL Server instance
2. Run migrations to create the database:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

### Running the Application

```bash
dotnet run
```

The API will be available at:
- HTTPS: `https://localhost:7xxx`
- HTTP: `http://localhost:5xxx`

Swagger UI will be available at: `https://localhost:7xxx/swagger`

## API Endpoints

All controllers follow RESTful conventions with complete CRUD operations for:
- Users (`/api/users`)
- Chat Messages (`/api/chatmessages`)
- Orders (`/api/orders`)
- Items (`/api/items`)
- Customization Options (`/api/customizationoptions`)
- Order Items (`/api/orderitems`)
- Reviews (`/api/reviews`)
- Deliveries (`/api/deliveries`)
- Instant Quotes (`/api/instantquotes`)
- Payments (`/api/payments`)

Check Swagger UI for detailed endpoint documentation with all available operations.

## Architecture

### Separation of Concerns

The project follows a clean architecture approach:

1. **Controllers** - Handle HTTP requests and responses
2. **Services** - Business logic layer
3. **Repositories** - Data access layer
4. **DTOs** - Data transfer objects for API communication
5. **Models** - Entity models representing database tables

### Key Features

- **Dependency Injection** - All dependencies registered in Program.cs
- **AutoMapper** - Automatic entity-to-DTO conversions
- **Repository Pattern** - Generic and specific repositories
- **Service Layer** - Business logic separation
- **DTOs** - Separate Create, Update, and Read DTOs

## Configuration

Update `appsettings.json` for your environment:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=TastyTreatDb;Trusted_Connection=true"
  }
}
```

## Running Migrations

```bash
# Add new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```