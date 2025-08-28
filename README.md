# 📌 VNeed — Demand Management System  

**VNeed** is a **demand/request management system** built with **ASP.NET Core** and **PostgreSQL**, designed to streamline company-wide product and service requests.  
The platform follows **enterprise-grade backend practices** including role-based authorization, exception handling, custom response patterns, and batch operations.  

---

## 🚀 Features  

### 🔑 Authentication & Authorization  
- JWT-based authentication and **role-based authorization** (User, TeamLead, Admin).  
- Secure login and token management.  

### 📦 Demand Management  
- Create, update, and track demands.  
- Approval workflows: **TeamLead approval → Admin approval**.  
- Batch operations for **approve/reject/order by role**.  

### 📊 Admin Panel  
- Manage **users, roles, products, and categories**.  
- Track **demand histories and status transitions**.  

### ⚙️ Backend Infrastructure  
- **Layered architecture** (API, Services, Repositories, Data, Common).  
- **Entity Framework Core 9** & **PostgreSQL** with Fluent API configurations.  
- **Generic repository pattern** for data access.  
- **Custom API Response Wrapper** (success, timestamp, operationId, message, result).  
- **Global Exception Handling Middleware**.  
- **Pagination & Validation** for scalable endpoints.  

### 🧪 Testing  
- All API endpoints tested with **Postman collections** (functional + error scenarios).  
- Supports both **single and batch requests**.  

---

## 🛠️ Technologies  

- **Backend:** ASP.NET Core 9, C#, Entity Framework Core 9, PostgreSQL  
- **Architecture:** Layered Architecture, Repository Pattern, DTOs, Middleware  
- **Other Tools:** Swagger, Postman  

---

 

