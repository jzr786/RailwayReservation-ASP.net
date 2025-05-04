# 🚆 Railway Reservation Management System

An ASP.NET Core MVC 8.0-based web application for managing train reservations, schedules, stations, and users. The system supports role-based access, seat availability tracking, and secure user management.

---

## 🔧 Technologies Used

- **Backend**: ASP.NET Core MVC 8.0
- **Frontend**: Razor Views (Bootstrap UI)
- **Database**: SQL Server (EF Core Code-First)
- **Authentication**: ASP.NET Core Identity
- **Architecture**: MVC Pattern

---

## 👥 User Roles

- **Admin**
  - Manages Trains, Stations, Train Schedules, and Users.
  - Creates reservations and monitors seat availability.
  - Assigns roles and controls user activation status.
  
- **Customer**
  - Registers/logs in to book and cancel train reservations.
  - Views available train schedules and personal booking history.

---

## ✅ Features

### 🔐 User Management (Admin Only)

- Create users (Admin or Customer)
- Assign default password: `Default@123`
- Passwords must be alphanumeric & hashed securely
- Activate/deactivate users
- Track user `CreatedAt` and `LastLoginDate`

### 🚉 Train & Schedule Management

- Add, edit, and delete:
  - Stations
  - Trains
  - Train Schedules (linked to stations and trains)

### 🎟️ Reservation Module

- Customers:
  - Book seats from available train schedules
  - View and cancel their own bookings
- Seat availability is checked dynamically before booking
- Admin can monitor all reservations

---

## 🛡️ Security & Validation

- Session-based login/logout with ASP.NET Identity
- Roles and access are protected using `[Authorize]` attributes
- Passwords are hashed using Identity's built-in security
- Data annotations & server-side validation included

---

## 🗃️ Database Structure (Entities)

- `Station`
- `Train`
- `TrainSchedule`
- `Reservation`
- `ApplicationUser` (inherits from `IdentityUser`)

---

## 🚀 Getting Started

### 1. Clone the Repo

```bash
git clone https://github.com/yourusername/railway-reservation.git
cd railway-reservation

## 2. Configure the Database

"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=RailwayReservationDB;Trusted_Connection=True;"
}

## 3. Run Migrations

```bash
dotnet ef database update

## 4. Seed Roles and Admin User

await RoleInitializer.SeedRolesAndAdminAsync(serviceProvider);

  ## Default Admin Credentials:
      Username: admin
      Email: admin@railway.com
      Password: Admin@123

## Project Structure

├── Controllers/
│   ├── AccountController.cs
│   ├── ReservationController.cs
│   ├── TrainScheduleController.cs
│   └── ...
├── Models/
│   ├── ApplicationUser.cs
│   ├── Reservation.cs
│   └── ...
├── Views/
│   ├── Account/
│   ├── Reservation/
│   └── ...
├── Data/
│   └── RailwayContext.cs
├── Services/
│   └── RoleInitializer.cs
└── Program.cs / Startup.cs


## Screenshots

![Alt text](D:\asp practice\RailwayReservation)


## ✍️ Author
Juzer (Karachi, Pakistan)
MBA in Supply Chain | Aspiring Full Stack Developer


## 📃 License
MIT License – Open for learning and educational use.
---

Let me know if you'd like me to include a **deployment guide**, **Docker support**, or **screenshots section**.








  





