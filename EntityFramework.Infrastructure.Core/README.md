# Entity Framework Core Repository and Unit of Work

## Overview
This library provides a robust implementation of the Repository and Unit of Work patterns using Entity Framework Core. It abstracts database operations and provides a clean and testable way to interact with the database.

### Features
- Generic repository implementation (`EntityRepository<TEntity>`)
- Unit of Work for managing transactions (`UnitOfWork`)
- Support for LINQ queries and raw SQL
- Easy-to-use API for CRUD operations

## Installation
To use this library, ensure you have Entity Framework Core installed in your project. You can install it via NuGet package manager or the .NET CLI:

```bash
dotnet add package Microsoft.EntityFrameworkCore
```

## Usage

### Setting Up the DbContext
First, ensure you have a `DbContext` implementation in your project.

```csharp
public class MyDbContext : DbContext
{
    // Define your DbSets
}
```

### Repository Usage
The `EntityRepository<TEntity>` can be used as follows:

```csharp
// Instantiate your DbContext
var dbContext = new MyDbContext();

// Create a repository for an entity
var repository = new EntityRepository<MyEntity>(dbContext);

// Adding an entity
repository.Add(new MyEntity { /* Set properties */ });

// Querying entities
var entities = repository.AsQueryable().Where(/* some conditions */);

// Save changes
dbContext.SaveChanges();
```

### Unit of Work Usage
The `UnitOfWork` pattern is used to manage transactions:

```csharp
var unitOfWork = new UnitOfWork(new MyDbContext());

// Start a transaction
unitOfWork.BeginTransaction();

try
{
    // Perform database operations
    var repository = unitOfWork.Repository<MyEntity>();
    repository.Add(new MyEntity { /* Set properties */ });

    // Commit the transaction
    unitOfWork.Commit();
}
catch
{
    // Rollback in case of an exception
    unitOfWork.RollBack();
}
```

## Additional Notes
- Ensure proper disposal of DbContext and UnitOfWork instances to release database connections.
- Customize the repository and unit of work classes to fit your specific needs.

---

## About the Author
**Selvin Medina**
- GitHub: [selvinmedina](https://github.com/selvinmedina)
- Linkedin: [selvinmedina](https://www.linkedin.com/in/selvinmedina)
- Contact: [Email](mailto:selvinmedina0016@gmail.com)

## Repository
- GitHub Repository: [EntityFramework.Infrastructure.Core](https://github.com/selvinmedina/EntityFramework.Infrastructure.Core)
- Issues and feature requests can be submitted through the GitHub issue tracker.

## Contributing
Contributions are welcome. Please follow the coding conventions and add tests for new features.

## License
MIT.
