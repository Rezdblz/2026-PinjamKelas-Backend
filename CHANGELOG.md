# Changelog

## [1.0.0] - 2026-02-17

### Features
- **Authentication:** Implement JWT token generation in login method
- **User Authentication:** Implement user authentication with JWT and password handling
- **Posts API:** Add Posts API with CRUD operations
- **CORS Support:** Implement CORS support for cross-origin requests
- **Classroom CRUD:** Add basic CRUD operations for classroom and users management
- **Database Schema:** Implement new initial database schema and seeding logic
- **Status Logging:** Add status log trigger and setup for classroom status tracking
- **StatusLogsController:** Add StatusLogsController with GetAllStatusLogs method
- **Database Triggers:** Update triggers to log classroom status changes with text descriptions and handle post deletions

### Fixes
- **Controller Values:** Fix controller returning incorrect values

### Improvements
- **Soft Delete System:** Implement soft delete system with BaseEntity and update models
- **Database Migrations:** Setup EF Core migrations and initial database schema
- **Swagger:** Add Swagger/OpenAPI documentation support
- **Environment Config:** Setup EF Core and environment configuration
- **Project Setup:** Initial ASP.NET Core setup with .env example

### Refactoring
- **Migration Cleanup:** Remove unused AddStatusLogTrigger migration file

### Removed
- **Buggy Trigger Migration:** Delete buggy trigger migration and adjust db seeder

## Summary

Initial release of PinjamKelas Backend API with complete REST endpoints for managing:
- User authentication and profiles
- Classroom resource sharing
- Posts and announcements
- Status logging and audit trails

**Stats:**
- 37 files added/modified
- 2,343 lines of code
- 20+ commits during development
