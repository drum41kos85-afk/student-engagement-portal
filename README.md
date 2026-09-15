# Student Engagement Portal

An ASP.NET Core MVC web application built for SWE5307 Assignment 2, allowing students to browse and register for campus events, submit support messages, and allowing admins to manage events, sign-ups, and announcements.

## Tech Stack
- ASP.NET Core MVC (.NET 10)
- Entity Framework Core (code-first)
- ASP.NET Core Identity
- Bootstrap 5
- SQL Server LocalDB

## Features
- Student registration/login
- Browse and filter events by category
- Register/unregister for events (with capacity limits)
- Submit and track support messages
- Admin panel: create/edit/delete events, view sign-up lists, post announcements, resolve support messages

## Running Locally
1. `dotnet restore`
2. `dotnet ef database update`
3. `dotnet run`

Default admin login (seeded automatically): `admin@portal.local` / `Admin123!`

## Testing
Unit and functional tests are in `StudentEngagementPortal.Tests/`. Run with:
```
dotnet test
```
