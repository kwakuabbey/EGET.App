# EGET Project Guide

## 1. Project Purpose

EGET is a mobile vehicle marketplace designed for Ghana.

The goal is to provide a modern platform where users can:

- Discover vehicles for sale
- Search and filter vehicles
- View detailed vehicle information
- Save vehicles to favourites
- Contact sellers
- List vehicles for sale
- Manage their own vehicle listings

The application is intended to grow into a complete vehicle marketplace.

---

## 2. Current Application

This repository contains the EGET mobile application.

The previous EGET website is a separate project and should not be mixed into this repository.

### Current technology

- .NET MAUI
- C#
- XAML
- Visual Studio 2026
- Android first
- iOS planned later

The application should remain a .NET MAUI project unless there is a deliberate architectural decision to change it.

---

## 3. Current Development Stage

The project is currently in the early mobile application development stage.

The current home screen contains:

- EGET branding
- Vehicle search
- Buy a Car action
- Sell a Car action
- Latest Vehicles section
- Vehicle listing cards
- Bumblebee-inspired black and yellow design

The home screen is functional and should be treated as existing work.

Do not replace the existing home screen unnecessarily.

---

## 4. Planned Application Features

The planned application will eventually include:

### Vehicle marketplace

- Browse vehicles
- Search vehicles
- Filter vehicles
- Vehicle details
- Multiple vehicle photos
- Vehicle image gallery
- Seller information
- Vehicle location
- Price
- Mileage
- Fuel type
- Transmission
- Vehicle condition

### User accounts

- Register
- Login
- Logout
- User profile
- My Vehicles

### Favourites

Users should be able to save vehicles and view their saved vehicles.

### Selling vehicles

Users should eventually be able to:

- Add a vehicle
- Upload multiple photos
- Enter vehicle information
- Set a price
- Publish a listing
- Edit their listing
- Remove their listing

### Communication

Planned future functionality includes:

- Buyer/seller messaging
- Notifications
- Contact options

### Verification

Future versions may include seller verification and other trust and safety features.

### Payments

Payment functionality may be added later.

---

## 5. Backend Architecture

The planned backend architecture is:

Mobile App
    ↓
ASP.NET Core Web API
    ↓
SQL Server Database

The mobile application should communicate with the backend through APIs rather than directly connecting to the SQL Server database.

The existing EGET website/backend work may provide useful reference material, but the mobile application should have a clean and appropriate API architecture.

---

## 6. Database

The planned database is Microsoft SQL Server.

The existing EGET project already contains database and vehicle marketplace concepts that may be reused or adapted when the API is developed.

Do not duplicate database logic unnecessarily inside the mobile application.

---

## 7. Identity and Authentication

The planned backend will use ASP.NET Core Identity for user authentication and account management.

The mobile application should communicate with the authentication system through the API.

Authentication implementation should be designed securely and should not store sensitive credentials directly in the mobile application's source code.

---

## 8. Design and Branding

EGET uses a premium Bumblebee-inspired visual identity.

Primary visual direction:

- Black
- Bumblebee Yellow
- White where appropriate

The application should feel:

- Modern
- Premium
- Clean
- Professional
- Easy to navigate

The design should avoid unnecessary visual clutter.

EGET should be written as:

**EGET**

Do not change the brand name without discussing it first.

---

## 9. Development Rules

### Preserve existing working features

Before changing an existing feature:

1. Understand how it currently works.
2. Check related files.
3. Make the smallest reasonable change.
4. Build the project.
5. Test the affected feature.

Do not replace large sections of working code simply to implement a small change.

### Avoid unnecessary architecture changes

Do not introduce a completely different architecture, framework, navigation system, or project structure without discussing the reason first.

### Keep the project buildable

After significant changes:

- Build the project.
- Check for compiler errors.
- Check XAML errors.
- Run the application when appropriate.

### Keep files organized

Use clear folders and meaningful file names.

Do not create duplicate copies of the application inside the repository.

---

## 10. GitHub Workflow

The GitHub repository is the shared source of truth.

Repository:

https://github.com/kwakuabbey/EGET.App

The main branch is:

**master**

The repository should remain clean and buildable.

Before pushing major changes:

- Build the application.
- Review changed files.
- Commit with a clear message.
- Push to GitHub.

For larger experimental features, a feature branch may be used before merging into `master`.

---

## 11. Team Roles

### Primary developer

**Kwaku**

Kwaku is responsible for the primary development of the EGET mobile application.

### Technical reviewer / advisor

**darkwaphil**

darkwaphil is an experienced full-time developer who will mainly:

- Review the project
- Cross-check implementation
- Review architecture
- Identify potential problems
- Give technical advice
- Suggest improvements when appropriate

He is not expected to be the primary developer of the application.

---

## 12. Working With AI Assistance

AI assistance may be used during development.

Before making changes, the existing project should be inspected and understood.

AI assistance should:

- Preserve existing working functionality.
- Avoid unnecessary rewrites.
- Explain significant architectural decisions.
- Keep changes consistent with the project structure.
- Build and check the project after important changes.

When a requested change affects architecture, authentication, database design, security, or major application structure, the change should be reviewed carefully before implementation.

---

## 13. Important Project History

The original EGET project was developed as a web application.

That website project is separate from this mobile application repository.

The mobile application was created as a fresh .NET MAUI project so that the mobile architecture could be developed independently.

Some concepts and design ideas from the original website may be reused where appropriate.

---

## 14. Current Priority

The immediate priority is to build the EGET mobile application systematically.

The development order should generally be:

1. Establish the application structure
2. Complete the core UI
3. Build navigation
4. Build vehicle browsing and search
5. Build vehicle details
6. Build authentication
7. Build favourites
8. Build vehicle listing/selling
9. Connect the backend API
10. Connect the database
11. Add messaging and notifications
12. Add additional marketplace features

This order may change when there is a clear technical reason.

---

## 15. Important Rule

Do not restart the project unnecessarily.

The existing EGET mobile application is the project being developed.

When adding a feature, build on the existing application unless there is a documented reason to change the architecture.

The goal is to develop EGET progressively into a reliable vehicle marketplace for Ghana.