# UserManagementAPI

A minimal ASP.NET Core Web API for user management, featuring CRUD endpoints, authentication, logging, error handling, and Swagger documentation.  
This project is an **assignment for the course: Back-End Development with .NET on Coursera** and is designed as a showcase/portfolio example.

---

## Features

- **User CRUD**: Create, read, update, and delete users.
- **Input Validation**: Ensures only valid user data is processed.
- **Authentication Middleware**: Requires a valid token for protected endpoints.
- **Global Exception Handling**: Returns consistent JSON error responses and logs errors.
- **Request Logging**: Logs HTTP method, path, and status code to `logs.txt` using Serilog.
- **Swagger UI**: Interactive API documentation and testing.
- **In-memory Storage**: Users are stored in a thread-safe dictionary.

---

## Technologies Used

- ASP.NET Core Minimal API
- Serilog (file logging)
- Swagger / Swashbuckle
- C#

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)

### Setup & Run

1. **Clone the repository:**

    ```sh
    git clone https://github.com/ionsper/UserManagementAPI.git
    cd UserManagementAPI
    ```

2. **Restore dependencies:**

    ```sh
    dotnet restore
    ```

3. **Run the API:**

    ```sh
    dotnet run
    ```

    The API will listen on `http://localhost:5000`.

4. **Access Swagger UI:**
    - Open [http://localhost:5000/swagger](http://localhost:5000/swagger) in your browser.

---

## Authentication

> **Note:** The authentication in this project is a **placeholder** and is **not secure**.  
> It uses a hardcoded token comparison for demonstration purposes only.  
> **Do not use this approach in production or for sensitive data.**

- All endpoints except `/` require an `Authorization` header in the format:

    ```text
    Authorization: Bearer TestToken
    ```

- The token (`TestToken`) is checked against a hardcoded value in the middleware.
- There is **no user management, encryption, or secure token validation**.
- This implementation is for learning and showcase only.  
  For real applications, use proper authentication.

Replace `TestToken` with your token if you change it in the code.

---

## Endpoints

| Method | Path              | Description         | Auth Required |
|--------|-------------------|--------------------|--------------|
| GET    | `/`               | Root path check    | No           |
| POST   | `/users`          | Create user        | Yes          |
| GET    | `/users`          | List all users     | Yes          |
| GET    | `/users/{id}`     | Get user by id     | Yes          |
| PUT    | `/users/{id}`     | Update user        | Yes          |
| DELETE | `/users/{id}`     | Delete user        | Yes          |

---

## Testing

- Use the included [`api-tests.http`](http-tests/api-tests.http) file for manual endpoint testing in VS Code or compatible editors.
- The file covers:
  - Valid/invalid requests
  - Authentication scenarios
  - Input validation
  - Error handling

---

## Logging

- All requests and errors are logged to `logs.txt` in the project root.

---

## Example User Object

```json
{
  "name": "Alice",
  "age": 30
}
```

---

## Folder Structure

```text
UserManagementAPI/
  Program.cs
  Models/
    User.cs
  http-tests/
    api-tests.http
  README.md
  LICENSE
  UserManagementAPI.csproj
```

---

## License

This project is open source and available under the [MIT License](LICENSE).

---
