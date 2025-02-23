# 📚 BookManagementApi

BookManagementApi is an **ASP.NET Core Web API** project designed for managing books. The project follows a **3-layered architecture** and utilizes **C#**, **Entity Framework Core**, and **SQL Server**.

## 🚀 Technologies Used

- **Programming Language**: C#  
- **Framework**: .NET 9  
- **Database**: SQL Server (with EF Core)
- **Architecture**: 3-layered architecture (Models, DataAccess, API)  

## 📁 Project Structure

```
BookManagementApi/
│-- API/
│   ├── Controllers/
│   |-- Program.cs
│   |-- appsettings.json
│-- DataAccess/
│   |-- <db_contex>.cs
│   ├── Migrations/
│-- Models/
│   ├── DTOs/
|   |-- <mode_name>.cs
|-- .gitignore
|-- BookMananement.sln
|-- README.md
```

## 📦 Installation

1. **Clone the repository**  
   ```sh
   git clone https://github.com/yourusername/BookManagementApi.git
   cd BookManagementApi
   ```

2. **Install dependencies**  
   ```sh
   dotnet restore
   ```

3. **Configure the database**  
   - Update the `ConnectionString` in `appsettings.json` for **SQL Server**.

4. **Apply migrations and create the database (for EF Core)**  
   ```sh
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. **Run the project**  
   ```sh
   dotnet run
   ```

## 📌 API Endpoints

| HTTP Method | Endpoint | Description |
|------------|---------|-------------|
| `POST` | `/api/Books` | Add a new book |
| `GET` | `/api/Books` | Get all books |
| `DELETE` | `/api/Books` | Delete books bulk |
| `POST` | `/api/Books/bulk` | Add a new books |
| `GET` | `/api/Books/{id}` | Get a book by ID |
| `PUT` | `/api/Books/{id}` | Update a book |
| `DELETE` | `/api/Books/{id}` | Delete a book |

## 📜 Author

👤 **Sarvar Azodov**  
- GitHub: [sarvar-swe](https://github.com/sarvar-swe)  
- LinkedIn: [sarvarbek-azodov](https://linkedin.com/in/sarvarbek-azodov)

