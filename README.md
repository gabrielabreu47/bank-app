# BankApp

This project is an API for banking management, built with ASP.NET Core and SQL Server (using Azure SQL Edge in a container). It includes a `bank.sql` file with database scripts and a Postman collection for testing.

---

## 🚀 Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker](https://www.docker.com/products/docker-desktop/)
- [Postman](https://www.postman.com/downloads/)

---

## ⚙️ Setup and Run

### 1. Clone the repository

```bash
git clone https://github.com/your-user/BankApp.git
cd BankApp
```

### 2. Start the database and the API

Run:

```bash
docker-compose up --build
```

This will start two services:

- **db**: SQL Server on Azure SQL Edge listening on port `26`.
- **web**: The ASP.NET Core API listening on `http://localhost:8080`.

---

## 🛠 Database Setup

Once the `db` container is running:

1. Connect to the database (port `26`, user: `sa`, password: `P@ssword1`, database: `Bank`).
   You can use [Azure Data Studio](https://learn.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio) or SQL Server Management Studio.

2. Run the `bank.sql` script located at the root of the project to create the necessary tables and data.

---

## 📬 Postman Testing

The file `BankApp.postman_collection.json` is included to facilitate testing.

1. Import the collection into Postman.
2. Ensure the API is running at `http://localhost:8080`.
3. Execute the requests according to the desired flow.

---

## 🧪 Development Environment

- Environment: `Development`
- Database: SQL Server (Azure SQL Edge container)
- Ports:
  - API: `8080`
  - SQL Server: `26`

---

## 📂 Key Structure

- `docker-compose.yml`: Defines the `web` and `db` services.
- `bank.sql`: Script with the database structure.
- `BankApp.postman_collection.json`: Postman testing collection.
- `Api/appsettings.json`: API configuration file.

---

## 🪩 Shut down the services

```bash
docker-compose down
```
