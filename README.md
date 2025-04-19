# FUNewsManagementSystem 📰

**FUNewsManagementSystem** is a web-based News Management System built using **ASP.NET Core Razor Pages** and **SignalR**. The system helps universities manage and publish news content efficiently with real-time collaboration and role-based access control.

---

## 🎯 Assignment Objectives

This project was developed to fulfill the following key objectives:

- ✅ Use ASP.NET Core Razor Pages and SignalR
- ✅ Apply a 3-layer architecture (Presentation - Business Logic - Data Access)
- ✅ Use Repository & Singleton patterns
- ✅ Perform CRUD and search with EF Core
- ✅ Use LINQ for data sorting and querying
- ✅ Validate data types and user inputs
- ✅ Use popup dialogs for Create and Update actions
- ✅ Real-time updates via SignalR

---

## 🧱 Architecture Overview

The solution uses a **3-layer architecture**:

- **Presentation Layer** (`*.RazorPages`): Razor Pages, UI, SignalR Hub
- **Business Logic Layer (BLL)**: Interfaces, services
- **Data Access Layer (DAL)**: Repository pattern, database models using EF Core

---

## 📌 Technologies Used

- ASP.NET Core Razor Pages (.NET 8)
- SignalR (for real-time functionality)
- Entity Framework Core
- SQL Server
- LINQ
- Bootstrap
- Repository & Singleton patterns

---

## 🧩 Functional Overview

### 🔓 Public (No Login Required)
- View news articles (only if status = active)

### 👩‍🏫 Lecturer
- View **only active** news articles

### 👩‍💼 Staff
- Manage categories (CRUD + Search)  
  > Cannot delete categories assigned to any news  
- Manage own profile
- Manage news articles (CRUD + Search + Tags) with **real-time updates**
- View history of own created news

### 🧑‍💼 Admin
- Manage all user accounts
- Generate statistical reports by date range (StartDate to EndDate), sorted descending

---

## 🧪 Default Admin Credentials

Stored in `appsettings.json`:

```json
{
  "AdminAccount": {
    "Email": "admin@FUNewsManagementSystem.org",
    "Password": "@@abc123@@"
  }
}
