# Enterprise Management System - ASP.NET Core

## Overview

A professional ASP.NET Core MVC web application with SQLite database integration, featuring four comprehensive business modules: Product Management, Student Grade Tracking, Employee Directory, and User Registration.

## Features

- **Product Management** - Complete CRUD operations for inventory management
- **Student Grade Tracking** - Grade recording with performance analytics
- **Employee Directory** - Employee records with searchable database
- **User Registration** - Secure user management with password hashing

## Technology Stack

- ASP.NET Core MVC (.NET 10.0.102)
- C# 13
- SQLite with ADO.NET
- Bootstrap 5 & Font Awesome 6.4.0
- Kestrel Web Server

## Quick Start

```bash
cd WebApp
dotnet build
dotnet run
```

Visit: `http://localhost:5099`

## Documentation

See [PROJECT_DOCUMENTATION.md](PROJECT_DOCUMENTATION.md) for comprehensive feature documentation with images.

## Security

- Parameterized SQL queries for SQL injection prevention
- SHA256 password hashing for user authentication
- Input validation on all forms
- UNIQUE constraints on usernames and emails

## License

MIT License - See LICENSE file for details

---

**Author:** Ranjeet Choudhary  
**Email:** ranjeetjat00001@gmail.com
