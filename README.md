# PinjamKelas Backend

A modern .NET ASP.NET Core REST API for managing classroom borrowing requests and resource sharing. Built with Entity Framework Core, PostgreSQL, and JWT authentication.

📋 **Table of Contents**
- Features
- Tech Stack
- Getting Started
- Project Structure
- Available Scripts
- Key Endpoints
- Authentication
- Database Schema
- Troubleshooting
- License

✨ **Features**

**User Features**
- User Authentication: Secure JWT-based login with password hashing
- Create Borrowing Posts: Submit new classroom borrowing requests
- Manage Posts: View, update, and delete personal borrowing requests
- Search & Filter: Query posts by classroom, status, and other criteria
- User Profile: Manage user information

**Admin Features**
- Approve/Reject Requests: Review and manage borrowing requests
- View All Posts: Access all borrowing requests from all users
- User Management: Create and manage user accounts
- Status Monitoring: Track all classroom status changes via audit logs
- Complete Access: Full control over all resources

**System Features**
- Automatic Status Logging: Triggers track all classroom status changes
- Soft Delete System: Data protection with soft delete implementation
- CORS Support: Cross-origin resource sharing enabled
- API Documentation: Comprehensive Swagger/OpenAPI documentation
- Database Seeding: Sample data for development

🛠 **Tech Stack**
- **Framework:** .NET 10.0 with ASP.NET Core
- **ORM:** Entity Framework Core (EF Core)
- **Database:** PostgreSQL
- **Authentication:** JWT (JSON Web Tokens)
- **API Documentation:** Swagger/OpenAPI
- **Language:** C#

🚀 **Getting Started**

**Prerequisites**
- .NET 10.0 SDK or higher
- PostgreSQL 12 or higher
- Git

**Installation**

1. Clone the repository
```bash
git clone https://github.com/Rezdblz/2026-PinjamKelas-Backend.git
cd 2026-PinjamKelas-Backend
```

2. Navigate to API directory
```bash
cd PinjamKelas.Api
```

3. Create environment configuration
```bash
# Copy example file
cp .env.example .env

# Edit .env and configure:
# - Database connection string
# - JWT secret key
# - API port
```

4. Restore dependencies
```bash
dotnet restore
```

5. Apply database migrations
```bash
dotnet ef database update
```

6. Run the application
```bash
dotnet run
```

The API will be available at `http://localhost:5000`

7. Access Swagger UI
```
http://localhost:5000/swagger/index.html
```

📁 **Project Structure**

```
PinjamKelas.Api/
├── Controllers/
│   ├── AuthController.cs           # Authentication endpoints
│   ├── UsersController.cs          # User management
│   ├── ClassroomsController.cs     # Classroom management
│   ├── PostsController.cs          # Borrowing posts CRUD
│   ├── StatusLogsController.cs     # Status tracking logs
│   └── BaseController.cs           # Base controller with common logic
├── Models/
│   ├── User.cs                     # User entity
│   ├── Classroom.cs                # Classroom entity
│   ├── Post.cs                     # Borrowing post entity
│   ├── StatusLog.cs                # Status log entity
│   └── BaseEntity.cs               # Base entity with soft delete
├── Dtos/
│   ├── LoginDto.cs                 # Login request
│   ├── Users/                      # User DTOs
│   ├── Classrooms/                 # Classroom DTOs
│   ├── Posts/                      # Post DTOs
│   └── StatusLog/                  # Status log DTOs
├── Migrations/                     # EF Core migrations
├── Seeder/
│   └── DbSeeder.cs                 # Database seeding logic
├── AppDbContext.cs                 # EF Core database context
├── Program.cs                      # Application configuration
└── appsettings.json               # Configuration settings
```

📝 **Available Scripts**

```bash
# Run application
dotnet run

# Build project
dotnet build

# Run tests (if added)
dotnet test

# Create new migration
dotnet ef migrations add <MigrationName>

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

🔑 **Key Endpoints**

**Authentication**
- `POST /api/auth/login` - User login

**Users**
- `GET /api/users` - Get all users (Admin)
- `GET /api/users/{id}` - Get user details
- `POST /api/users` - Create user (Admin)
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Soft delete user

**Classrooms**
- `GET /api/classrooms` - Get all classrooms
- `GET /api/classrooms/{id}` - Get classroom details
- `POST /api/classrooms` - Create classroom
- `PUT /api/classrooms/{id}` - Update classroom
- `DELETE /api/classrooms/{id}` - Soft delete classroom

**Posts (Borrowing Requests)**
- `GET /api/posts` - Get all posts (Admin)
- `GET /api/posts/user/{userId}` - Get user's posts
- `GET /api/posts/{id}` - Get post details
- `POST /api/posts` - Create post
- `PUT /api/posts/{id}` - Update post
- `DELETE /api/posts/{id}` - Soft delete post
- `PUT /api/posts/{id}/approval` - Approve/reject post (Admin)

**Status Logs**
- `GET /api/statuslogs` - Get all status logs
- `GET /api/statuslogs/{classroomId}` - Get logs for specific classroom

🔐 **Authentication**

The API uses JWT (JSON Web Token) for authentication:

**Login Request**
```json
POST /api/auth/login
{
  "username": "student@example.com",
  "password": "password123"
}
```

**Login Response**
```json
{
  "id": 1,
  "username": "student@example.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "role": "Student"
}
```

**Using Token**
```
Authorization: Bearer <token>
```

**Token Claims**
- User ID
- Username
- Role (Student/Admin)
- Expiration time

📊 **Database Schema**

**Users Table**
- Id (Primary Key)
- Username (Unique)
- Password (Hashed)
- Role (Student/Admin)
- CreatedAt
- UpdatedAt
- DeletedAt (Soft Delete)

**Classrooms Table**
- Id (Primary Key)
- Name
- Location
- Capacity
- Status
- CreatedAt
- UpdatedAt
- DeletedAt (Soft Delete)

**Posts Table**
- Id (Primary Key)
- UserId (Foreign Key)
- ClassroomId (Foreign Key)
- Title
- Description
- BorrowDate
- ReturnDate
- Status (Pending/Approved/Rejected)
- CreatedAt
- UpdatedAt
- DeletedAt (Soft Delete)

**StatusLogs Table**
- Id (Primary Key)
- ClassroomId (Foreign Key)
- OldStatus
- NewStatus
- Reason
- CreatedAt

🐛 **Troubleshooting**

**Database Connection Issues**
```bash
# Check PostgreSQL is running
# Update connection string in appsettings.json or .env
# Verify database exists or create it
```

**Migration Issues**
```bash
# Reset database (careful!)
dotnet ef database drop
dotnet ef database update

# Or rollback specific migration
dotnet ef migrations remove
```

**Port Already in Use**
```bash
# Change port in launchSettings.json or environment variables
# Or find and kill existing process on port 5000
```

**JWT Token Errors**
```bash
# Ensure JWT secret is configured in appsettings.json
# Check token hasn't expired
# Verify Authorization header format: "Bearer <token>"
```

📄 **License**

This project is licensed under the LICENSE file in the root directory.

👥 **Contributors**
- Rezdblz

**Last Updated:** February 17, 2026
