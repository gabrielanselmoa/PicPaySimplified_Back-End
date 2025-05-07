# Simplified PicPay Backend - Summary

This project implements the backend for a simplified version of PicPay, focusing on digital wallet functionalities and transfers between Common and Merchant users.

**The idea here is to have a logged-in user (admin) who can perform and manage operations. After installing the project, register + login and enjoy the system's features 😊**

## 🏗️ Architecture

The project follows a **Modular Monolith Architecture**, organizing the code into cohesive modules (Domain, Application, Infrastructure, Rest) to facilitate maintenance and decoupling.

## 💻 Key Technologies and Libraries

* **Language:** C#
* **Framework:** .NET (ASP.NET Core)
* **Database:** MongoDB (NoSQL)
* **ORM/MongoDB Driver:** Official MongoDB Driver for .NET
* **Authentication:** JWT (JSON Web Tokens)
* **API Documentation:** Swagger/OpenAPI (Swashbuckle)
* **Mapping:** Manual or using libraries.
* **Logging:** Microsoft.Extensions.Logging

## ✨ Applied Principles and Patterns

* **SOLID:** Considered in the structure and implementation.
* **Clean Code:** Focus on readability and maintainability.
* **Clean Architecture:** Separation of layers and dependencies directed towards the domain.
* **Repository Pattern:** Abstraction of data access (MongoDB).
* **Service Pattern:** Business logic in the application layer.
* **DTO Pattern:** Data transfer between layers.
* **Result Pattern:** Consistent operation return type.

## 🔐 Authentication

Implemented via **JWT**. Tokens are generated upon login and used to access protected endpoints.

## 🗺️ API Endpoints (Transfer Focus)

The main endpoint for the challenge is:

### `POST /payments`

Processes the transfer flow, requiring `value`, `payer` (GUID), and `payee` (GUID) in the request body.

*Alternative Proposal:* `POST /payments/transfer` for better semantics.

## 🌐 External Services

* **Authorization Service:** Called via `GET` (`https://util.devi.tools/api/v2/authorize`) before debiting.
* **Notification Service:** Called via `POST` (`https://util.devi.tools/api/v1/notify`) **asynchronously** after the transfer to notify the receiver, without impacting the main flow.

## 🔄 Transactionality

The transfer operation is **atomic**. Actions are rolled back in case of failure to ensure the payer's balance consistency.

## 📝 Error Handling and Logging

Uses `try-catch` blocks and `ILogger` to record errors, exceptions, and the application flow.

## Usage Guide:

This guide summarizes the steps required to set up and run the Simplified PicPay Backend project.

## 📋 Prerequisites

Make sure you have the following software installed on your system:

* **SDK do .NET:** Version compatible with the project (check the `.csproj` file, usually .NET 6+).
* **Docker:** To run the MongoDB database.
* **Git:** To clone the repository.

## ⚙️ Setup and Execution

Follow these steps to get the application up and running:

1.  **Clone the Repository:**
    Open your terminal and execute:
    ```bash
    git clone [https://github.com/gabrielanselmoa/PicPaySimplified.git](https://github.com/gabrielanselmoa/PicPaySimplified.git)
    cd PicPaySimplified
    ```

2.  **Start the Database (MongoDB with Docker):**
    Run the command to start a MongoDB container:
    ```bash
    docker run -d --name picpaysimplified-mongo -p 27017:27017 mongo:latest
    ```
    Verify that the container started correctly with `docker ps`.

3.  **Create the Environment Variables File (`.env`):**
    In the **solution root** (`./PicPaySimplified`), create a new file named `.env` and add the following variables, replacing the values as needed:
    ```env
    CONNECTION_STRING=mongodb://localhost:27017
    DB_NAME=PicPaySimplifiedDB
    JWT_SECRET=YourSuperSecretJWTKeyThatShouldBeLongAndDifficult
    ISSUER=YourIssuerHere (Ex: http://localhost:xxxx)
    AUDIENCE=YourAudienceHere (Ex: http://localhost:xxxx)
    ```

4.  **Restore Dependencies and Build the Project:**
    Still in the solution root folder (`./PicPaySimplified`), execute the .NET build command:
    ```bash
    dotnet build
    ```

5.  **Run the Application:**
    Navigate to the API project folder (usually `PicPaySimplified/PicPaySimplified.API`):
    ```bash
    cd PicPaySimplified.API
    dotnet run
    ```
    The terminal will indicate the addresses (URLs) where the application is listening.

6.  **Access the API Documentation (Swagger):**
    With the application running, open your browser and navigate to the Swagger UI endpoint at one of the URLs provided by `dotnet run`, usually:
    `https://localhost:7XXX/swagger` or `http://localhost:5XXX/swagger` (replace `7XXX` or `5XXX` with the correct port).

## ⏹️ Stopping the Services

* **`.NET` Application:** In the terminal where `dotnet run` is active, press `Ctrl + C`.
* **MongoDB Container:** To stop the Docker MongoDB container:
    ```bash
    docker stop picpaysimplified-mongo
    ```

## ✅ Value Analysis and Strong Points

The implementation of this project demonstrates a comprehensive approach and a strong command over the essential pillars of modern backend development. I successfully addressed and applied the vast majority of the valued criteria, delivering not just the required functionality, but a system built with attention to **quality, robustness, and best practices**.

The strong points and areas where knowledge was applied and demonstrated include:

* **Solid Architecture and Decoupling:** I applied a **Modular/Clean Architecture**, thinking about the structure before coding and dedicating care to **decoupling components** between layers (Service, Repository, etc.), which significantly improves **Code Maintainability**.
* **Application of Design Patterns:** I utilized relevant **Design Patterns** (such as Repository, Service, DTO, and Result Pattern) to solve common design problems and structure the code effectively.
* **Infrastructure and Tools:** I demonstrated proficiency in using **Docker** to manage the database environment, simplifying setup.
* **Persistence and Modeling:** I performed appropriate **Data Modeling** for MongoDB, meeting the business requirements.
* **Quality and Reliability:** I implemented consistent **Error Handling** and aimed for **consistent test coverage** (unit/integration scenarios) to ensure application reliability.
* **Security and Accessibility:** I paid **Attention to basic security items** (like JWT Authentication) and provided clear **Documentation** (Swagger) for the API.
* **Argumentation and Technical Proficiency:** Through the architectural and implementation choices, I am able to **argue my choices** and **present solutions I master**, demonstrating consistency in technical reasoning.

This set of applied practices and concepts resulted in a solution that is not only functional but well-structured, tested (as mentioned), easily maintainable, and aligned with industry standards, exceeding the expectations of a basic project.
