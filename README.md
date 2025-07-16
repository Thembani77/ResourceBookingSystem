# 🛠️ Resource Booking System

This is an ASP.NET Core MVC web application for managing resource bookings (e.g. rooms, equipment, etc). Built with C#, Entity Framework Core, and Razor Views.

---

## 🚀 Features

- ✅ Add/Edit/Delete bookings
  
- ✅ Bookings linked to resources
  
- ✅ Robust **booking conflict logic** preventing overlapping bookings
  
- ✅ Filter bookings by date & resource
  
- ✅ View upcoming bookings per resource

- ✅ Fully responsive using Bootstrap 5
  
- ✅ Entity Framework Core + Migrations
  
- ✅ TempData toast notifications for feedback  

---

## 💻 Tech Stack

- ASP.NET Core MVC  
- Razor Views (CSHTML)  
- Entity Framework Core  
- SQL Server  
- Bootstrap 5  
- C#  

---

## 🏁 How To Run

1. **Clone the repository:**  
   ```bash
    git clone https://github.com/Thembani77/ResourceBookingSystem.git
    cd ResourceBookingSystem

2. **Install dependencies & EF CLI (if needed)**:
    dotnet tool install --global dotnet-ef
   
3. **Apply migrations and create the database:**
    dotnet ef database update

---
🧠 **Booking Conflict Logic Explained**
When a new booking is created, the system checks for any overlapping bookings on the selected resource:
 
var hasConflict = await _context.Bookings.AnyAsync(b =>
    b.ResourceId == booking.ResourceId &&
    (
        (booking.StartTime >= b.StartTime && booking.StartTime < b.EndTime) ||
        (booking.EndTime > b.StartTime && booking.EndTime <= b.EndTime) ||
        (booking.StartTime <= b.StartTime && booking.EndTime >= b.EndTime)
    ));

---
🧱 **Database Setup**
Uses ApplicationDbContext with DbSet<Resource> and DbSet<Booking>.

Booking entity has a foreign key relationship with Resource.

Initial seed data for resources added in OnModelCreating.
