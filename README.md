# 🛒 E-Commerce API (.NET Core)

## 📌 Overview
This project is a **Back-End E-Commerce API** built with **.NET Core 8** and **Entity Framework Core**.  
It provides a secure and scalable system for managing products, categories, users, orders, and payments.  
The API is designed following **Clean Architecture** and **Repository/Unit of Work Pattern**, ensuring maintainability and flexibility for future improvements.  

---

## ⚙️ Features
- 🔐 **Authentication & Authorization**
  - ASP.NET Core Identity for user registration & login

- 📦 **Product & Category Management**
  - CRUD operations for products & categories
  - Image upload support for products

- 🛍️ **Shopping Basket**
  - Add / remove  items in basket
  - Stored in **Redis** for fast performance

- 📑 **Orders**
  - Create & manage customer orders
  - Order tracking with statuses

- 💳 **Payments Integration**
  - Stripe payment gateway
  - Secure checkout process

- 📊 **Pagination, Filtering, Sorting & Searching**
  - Flexible queries for products and categories

---

## 🏗️ Architecture
The project follows **Clean Architecture** with layered structure:

- **Core Layer**  
  - Entities, DTOs, Interfaces, Business Logic
- **Infrastructure Layer**  
  - Data access with EF Core, Identity, Repositories, Redis, Stripe Integration
- **API Layer**  
  - Controllers, Middlewares, Endpoints, Dependency Injection

---

## 🛠️ Tech Stack
- **Framework**: .NET Core 8 (C#)  
- **Database**: SQL Server + Entity Framework Core  
- **Authentication**: Identity Framework  
- **Cache**: Redis  
- **Payment Gateway**: Stripe    
- **Design Pattern**: Repository & Unit of Work + AutoMapper  

---

## 📂 Project Structure
EcommerceAPI/
│── Ecommerce.Core/ # Entities, DTOs, Interfaces, Services
│── Ecommerce.Infrastructure/ # Data Access, Repositories, EF Configurations
│── Ecommerce.API/ # Controllers, Middlewares, Helper, Mapping
