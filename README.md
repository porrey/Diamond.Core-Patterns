![Diamond.Core Logo](https://github.com/porrey/Diamond.Core-Patterns/raw/master/Images/Diamond.Core.128x128.png)

# Diamond.Core Patterns

[![NuGet](https://img.shields.io/nuget/v/Diamond.Core.Abstractions.svg)](https://www.nuget.org/packages/Diamond.Core.Abstractions)
[![License: LGPL-3.0-or-later](https://img.shields.io/badge/License-LGPL--3.0--or--later-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple)](https://dotnet.microsoft.com/)

**Diamond.Core Patterns** is a production-ready C# library that provides enterprise-grade implementations of common design patterns for .NET applications. Published as a suite of focused NuGet packages under the `Diamond.Core.*` namespace, it is authored by **Daniel M. Porrey** and targets **.NET 10.0**.

---

## Table of Contents

- [Overview](#overview)
- [NuGet Packages](#nuget-packages)
- [Getting Started](#getting-started)
- [Design Patterns](#design-patterns)
  - [Repository](#repository-pattern)
  - [Unit of Work](#unit-of-work-pattern)
  - [Decorator](#decorator-pattern)
  - [Specification](#specification-pattern)
  - [Rules Engine](#rules-engine)
  - [Workflow / State Machine](#workflow--state-machine)
  - [Dependency Injection Extensions](#dependency-injection-extensions)
  - [Hosting Extensions](#hosting-extensions)
  - [Command Line](#command-line)
  - [ASP.NET Core Utilities](#aspnet-core-utilities)
  - [AutoMapper Extensions](#automapper-extensions)
  - [Clonable](#clonable-pattern)
  - [System Utilities](#system-utilities)
  - [WPF Support](#wpf-support)
- [Examples](#examples)
- [Design Philosophy](#design-philosophy)
- [Requirements](#requirements)
- [License](#license)

---

## Overview

Diamond.Core Patterns provides a cohesive set of **67 projects** organized into 18 functional areas. Every pattern follows the same three-layer structure:

```
Diamond.Core.[Pattern].Abstractions    ← Interfaces and contracts only
Diamond.Core.[Pattern]                 ← Core implementation
Diamond.Core.[Pattern].[Provider]      ← Provider-specific (e.g., EntityFrameworkCore, SqlServer)
```

This separation ensures your application code can depend only on abstractions, keeping it testable and decoupled from implementation details.

---

## NuGet Packages

All packages are published to [NuGet.org](https://www.nuget.org/) and versioned at **10.1.0** (aligned with the .NET 10 target framework).

### Core Abstractions

| Package | Description |
|---------|-------------|
| [`Diamond.Core.Abstractions`](https://www.nuget.org/packages/Diamond.Core.Abstractions) | Shared interfaces used across all patterns |

### Repository

| Package | Description |
|---------|-------------|
| [`Diamond.Core.Repository.Abstractions`](https://www.nuget.org/packages/Diamond.Core.Repository.Abstractions) | Repository interfaces (`IReadOnlyRepository`, `IWritableRepository`, etc.) |
| [`Diamond.Core.Repository`](https://www.nuget.org/packages/Diamond.Core.Repository) | Core repository base classes |
| [`Diamond.Core.Repository.EntityFrameworkCore`](https://www.nuget.org/packages/Diamond.Core.Repository.EntityFrameworkCore) | EF Core repository implementation |
| [`Diamond.Core.Repository.DateTimeModel`](https://www.nuget.org/packages/Diamond.Core.Repository.DateTimeModel) | DateTime-aware entity model base |

### Unit of Work

| Package | Description |
|---------|-------------|
| [`Diamond.Core.UnitOfWork.Abstractions`](https://www.nuget.org/packages/Diamond.Core.UnitOfWork.Abstractions) | `IUnitOfWork` and `IUnitOfWorkFactory` interfaces |
| [`Diamond.Core.UnitOfWork`](https://www.nuget.org/packages/Diamond.Core.UnitOfWork) | Unit of Work base implementation |

### Rules Engine

| Package | Description |
|---------|-------------|
| [`Diamond.Core.Rules.Abstractions`](https://www.nuget.org/packages/Diamond.Core.Rules.Abstractions) | `IRule`, `IRuleResult`, `IRulesFactory` interfaces |
| [`Diamond.Core.Rules`](https://www.nuget.org/packages/Diamond.Core.Rules) | `RuleTemplate` base class and engine |

### Workflow

| Package | Description |
|---------|-------------|
| [`Diamond.Core.Workflow.Abstractions`](https://www.nuget.org/packages/Diamond.Core.Workflow.Abstractions) | `IWorkflowManager`, `IWorkflowItem`, `IContext` interfaces |
| [`Diamond.Core.Workflow.State.Abstractions`](https://www.nuget.org/packages/Diamond.Core.Workflow.State.Abstractions) | State-based workflow interfaces |
| [`Diamond.Core.Workflow`](https://www.nuget.org/packages/Diamond.Core.Workflow) | Workflow engine implementation |
| [`Diamond.Core.Workflow.Steps`](https://www.nuget.org/packages/Diamond.Core.Workflow.Steps) | Built-in workflow step base classes |

### Decorator

| Package | Description |
|---------|-------------|
| [`Diamond.Core.Decorator.Abstractions`](https://www.nuget.org/packages/Diamond.Core.Decorator.Abstractions) | `IDecorator` interface and factory |
| [`Diamond.Core.Decorator`](https://www.nuget.org/packages/Diamond.Core.Decorator) | `DecoratorTemplate` base class |

### Specification

| Package | Description |
|---------|-------------|
| [`Diamond.Core.Specification.Abstractions`](https://www.nuget.org/packages/Diamond.Core.Specification.Abstractions) | `ISpecification<TResult>` and `ISpecification<TParameter, TResult>` interfaces |
| [`Diamond.Core.Specification`](https://www.nuget.org/packages/Diamond.Core.Specification) | Specification base classes with LinqKit support |

### Dependency Injection Extensions

| Package | Description |
|---------|-------------|
| [`Diamond.Core.Extensions.DependencyInjection.Abstractions`](https://www.nuget.org/packages/Diamond.Core.Extensions.DependencyInjection.Abstractions) | `IDependencyFactory` interface |
| [`Diamond.Core.Extensions.DependencyInjection`](https://www.nuget.org/packages/Diamond.Core.Extensions.DependencyInjection) | `IServiceCollection` extension methods |
| [`Diamond.Core.Extensions.DependencyInjection.EntityFrameworkCore`](https://www.nuget.org/packages/Diamond.Core.Extensions.DependencyInjection.EntityFrameworkCore) | EF Core DI helpers |
| [`Diamond.Core.Extensions.DependencyInjection.SqlServer`](https://www.nuget.org/packages/Diamond.Core.Extensions.DependencyInjection.SqlServer) | SQL Server DI helpers |
| [`Diamond.Core.Extensions.DependencyInjection.PostgreSQL`](https://www.nuget.org/packages/Diamond.Core.Extensions.DependencyInjection.PostgreSQL) | PostgreSQL DI helpers |
| [`Diamond.Core.Extensions.DependencyInjection.MySql`](https://www.nuget.org/packages/Diamond.Core.Extensions.DependencyInjection.MySql) | MySQL DI helpers |
| [`Diamond.Core.Extensions.DependencyInjection.Sqlite`](https://www.nuget.org/packages/Diamond.Core.Extensions.DependencyInjection.Sqlite) | SQLite DI helpers |
| [`Diamond.Core.Extensions.DependencyInjection.Oracle`](https://www.nuget.org/packages/Diamond.Core.Extensions.DependencyInjection.Oracle) | Oracle DI helpers |
| [`Diamond.Core.Extensions.DependencyInjection.Cosmos`](https://www.nuget.org/packages/Diamond.Core.Extensions.DependencyInjection.Cosmos) | Azure Cosmos DB DI helpers |
| [`Diamond.Core.Extensions.DependencyInjection.InMemory`](https://www.nuget.org/packages/Diamond.Core.Extensions.DependencyInjection.InMemory) | In-memory database DI helpers |

### Hosting

| Package | Description |
|---------|-------------|
| [`Diamond.Core.Extensions.Hosting.Abstractions`](https://www.nuget.org/packages/Diamond.Core.Extensions.Hosting.Abstractions) | `IStartup` and startup lifecycle interfaces |
| [`Diamond.Core.Extensions.Hosting`](https://www.nuget.org/packages/Diamond.Core.Extensions.Hosting) | `IHost` / `IHostBuilder` extension methods |

### Command Line

| Package | Description |
|---------|-------------|
| [`Diamond.Core.CommandLine.Abstractions`](https://www.nuget.org/packages/Diamond.Core.CommandLine.Abstractions) | `ICommand`, `IRootCommand` interfaces |
| [`Diamond.Core.CommandLine.Model`](https://www.nuget.org/packages/Diamond.Core.CommandLine.Model) | CLI option and argument models |
| [`Diamond.Core.CommandLine`](https://www.nuget.org/packages/Diamond.Core.CommandLine) | `System.CommandLine`-based implementation |

### ASP.NET Core

| Package | Description |
|---------|-------------|
| [`Diamond.Core.AspNetCore.DoAction.Abstractions`](https://www.nuget.org/packages/Diamond.Core.AspNetCore.DoAction.Abstractions) | `IDoAction<TInputs, TResult>` controller action interfaces |
| [`Diamond.Core.AspNetCore.DoAction`](https://www.nuget.org/packages/Diamond.Core.AspNetCore.DoAction) | DoAction base implementation |
| [`Diamond.Core.AspNetCore.DataTables`](https://www.nuget.org/packages/Diamond.Core.AspNetCore.DataTables) | DataTables server-side processing |
| [`Diamond.Core.AspNetCore.Rfc7807`](https://www.nuget.org/packages/Diamond.Core.AspNetCore.Rfc7807) | RFC 7807 Problem Details support |
| [`Diamond.Core.AspNetCore.Hosting`](https://www.nuget.org/packages/Diamond.Core.AspNetCore.Hosting) | ASP.NET Core hosting integration |
| [`Diamond.Core.AspNetCore.Swagger`](https://www.nuget.org/packages/Diamond.Core.AspNetCore.Swagger) | Swagger / OpenAPI helpers |

### Other Utilities

| Package | Description |
|---------|-------------|
| [`Diamond.Core.AutoMapperExtensions`](https://www.nuget.org/packages/Diamond.Core.AutoMapperExtensions) | AutoMapper DI integration |
| [`Diamond.Core.Clonable.Abstractions`](https://www.nuget.org/packages/Diamond.Core.Clonable.Abstractions) | `IObjectCloneFactory` interface |
| [`Diamond.Core.Clonable`](https://www.nuget.org/packages/Diamond.Core.Clonable) | Base cloning implementation |
| [`Diamond.Core.Clonable.Microsoft`](https://www.nuget.org/packages/Diamond.Core.Clonable.Microsoft) | Microsoft serializer-based deep clone |
| [`Diamond.Core.Clonable.Newtonsoft`](https://www.nuget.org/packages/Diamond.Core.Clonable.Newtonsoft) | Newtonsoft.Json-based deep clone |
| [`Diamond.Core.Performance`](https://www.nuget.org/packages/Diamond.Core.Performance) | Performance measurement utilities |
| [`Diamond.Core.System.Base36`](https://www.nuget.org/packages/Diamond.Core.System.Base36) | Base36 encoding/decoding |
| [`Diamond.Core.System.Text`](https://www.nuget.org/packages/Diamond.Core.System.Text) | Text manipulation helpers |
| [`Diamond.Core.System.TemporaryFolder`](https://www.nuget.org/packages/Diamond.Core.System.TemporaryFolder) | Temporary folder lifecycle management |
| [`Diamond.Core.Wpf.Abstractions`](https://www.nuget.org/packages/Diamond.Core.Wpf.Abstractions) | WPF `IWindow`, `IMainWindow` interfaces |
| [`Diamond.Core.Wpf`](https://www.nuget.org/packages/Diamond.Core.Wpf) | WPF MVVM command helpers |

---

## Getting Started

Install the packages you need via the .NET CLI. It is recommended to reference only the `*.Abstractions` packages in your domain/business logic projects and reference the implementation packages in your composition root (startup project).

```bash
# Core abstractions
dotnet add package Diamond.Core.Abstractions

# Example: Repository pattern with EF Core
dotnet add package Diamond.Core.Repository.Abstractions
dotnet add package Diamond.Core.Repository.EntityFrameworkCore
dotnet add package Diamond.Core.Extensions.DependencyInjection.SqlServer

# Example: Rules engine
dotnet add package Diamond.Core.Rules.Abstractions
dotnet add package Diamond.Core.Rules
```

### Basic Host Setup

```csharp
using Diamond.Core.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureDiamondCore()          // registers Diamond.Core services
    .ConfigureServices((ctx, services) =>
    {
        services.AddTransient<IMyRepository, MyRepository>();
        services.AddTransient<IMyRule, MyValidationRule>();
    })
    .Build();

await host.RunAsync();
```

---

## Design Patterns

### Repository Pattern

The Repository pattern separates data-access logic from business logic. The library provides read/write-separated interfaces and a full Entity Framework Core implementation.

**Key interfaces** (`Diamond.Core.Repository.Abstractions`):

```csharp
public interface IReadOnlyRepository<TInterface> : IRepository<TInterface>
    where TInterface : IEntity
{
    Task<IEnumerable<TInterface>> GetAllAsync();
    Task<IEnumerable<TInterface>> GetAsync(Expression<Func<TInterface, bool>> predicate);
}

public interface IWritableRepository<TInterface> : IRepository<TInterface>
    where TInterface : IEntity
{
    Task<TInterface> AddAsync(TInterface item);
    Task<TInterface> UpdateAsync(TInterface item);
    Task<bool> DeleteAsync(TInterface item);
}
```

**Implementing a repository** with Entity Framework Core:

```csharp
// 1. Define your entity interface
public interface IEmployeeEntity : IEntity
{
    string Name { get; set; }
    bool Active { get; set; }
}

// 2. Implement the entity
public class EmployeeEntity : IEmployeeEntity { ... }

// 3. Create the repository
public class EmployeeRepository
    : EntityFrameworkRepository<IEmployeeEntity, EmployeeEntity, MyDbContext>
{
    public EmployeeRepository(MyDbContext context, IEntityFactory<IEmployeeEntity> factory)
        : base(context, factory) { }

    protected override DbSet<EmployeeEntity> MyDbSet(MyDbContext context)
        => context.Employees;
}

// 4. Register in DI
services.AddTransient<IReadOnlyRepository<IEmployeeEntity>, EmployeeRepository>();

// 5. Use via IRepositoryFactory
IReadOnlyRepository<IEmployeeEntity> repo =
    await repositoryFactory.GetReadOnlyAsync<IEmployeeEntity>();

IEnumerable<IEmployeeEntity> active = await repo.GetAsync(e => e.Active);
```

---

### Unit of Work Pattern

The Unit of Work pattern coordinates transactions that span multiple repositories or aggregates into a single atomic operation.

**Key interfaces** (`Diamond.Core.UnitOfWork.Abstractions`):

```csharp
public interface IUnitOfWork<TResult, TSourceItem> : IUnitOfWork
{
    Task<TResult> CommitAsync(TSourceItem item);
}
```

**Example usage**:

```csharp
// Define your unit of work
public class PromoteEmployeeUnitOfWork
    : IUnitOfWork<(bool Success, IEmployeeEntity Employee),
                  (int EmployeeId, string NewTitle, decimal PercentRaise)>
{
    public async Task<(bool, IEmployeeEntity)> CommitAsync(
        (int EmployeeId, string NewTitle, decimal PercentRaise) input)
    {
        // Retrieve, update, and save in a single transaction
    }
}

// Resolve and execute
var uow = await unitOfWorkFactory
    .GetAsync<(bool, IEmployeeEntity), (int, string, decimal)>(WellKnown.UnitOfWork.PromoteEmployee);

var (success, employee) = await uow.CommitAsync((employeeId, "Senior Engineer", 0.10m));
```

---

### Decorator Pattern

The Decorator pattern wraps a subject to add behavior. Diamond.Core provides a generic decorator abstraction and factory for resolving decorators by key.

**Key interfaces** (`Diamond.Core.Decorator.Abstractions`):

```csharp
public interface IDecorator<TDecoratedItem, TResult> : IDecorator
{
    TDecoratedItem Item { get; set; }
    Task<TResult> TakeActionAsync();
    Task<TResult> TakeActionAsync(TDecoratedItem item);
}
```

**Implementing a decorator**:

```csharp
public class EmployeePromotionDecorator
    : DecoratorTemplate<IEmployeeEntity, (bool Success, IEmployeeEntity Result, string Message)>
{
    public EmployeePromotionDecorator(ILogger<EmployeePromotionDecorator> logger,
        IUnitOfWorkFactory uowFactory, IRulesFactory rulesFactory)
        : base(logger) { ... }

    protected override async Task<(bool, IEmployeeEntity, string)> OnTakeActionAsync(
        IEmployeeEntity employee)
    {
        // validate rules, execute unit of work, return result
    }
}

// Resolve via factory and execute
IDecorator<IEmployeeEntity, (bool, IEmployeeEntity, string)> decorator =
    await decoratorFactory.GetAsync<IEmployeeEntity, (bool, IEmployeeEntity, string)>(
        WellKnown.Decorator.EmployeePromotion);

var (ok, updated, message) = await decorator.TakeActionAsync(employee);
```

---

### Specification Pattern

The Specification pattern encapsulates a query as an object. This keeps query logic out of repositories and controllers and makes it reusable and testable.

**Key interfaces** (`Diamond.Core.Specification.Abstractions`):

```csharp
// No input parameters
public interface ISpecification<TResult> : ISpecification
{
    Task<TResult> ExecuteSelectionAsync();
}

// With input parameters
public interface ISpecification<TParameter, TResult> : ISpecification
{
    Task<TResult> ExecuteSelectionAsync(TParameter inputs);
}
```

**Implementing a specification**:

```csharp
public class ActiveEmployeeIdsSpecification
    : SpecificationTemplate<IEnumerable<int>>
{
    public ActiveEmployeeIdsSpecification(IRepositoryFactory repositoryFactory)
        => this.RepositoryFactory = repositoryFactory;

    public override async Task<IEnumerable<int>> ExecuteSelectionAsync()
    {
        using IReadOnlyRepository<IEmployeeEntity> repo =
            await this.RepositoryFactory.GetReadOnlyAsync<IEmployeeEntity>();
        return (await repo.GetAsync(e => e.Active)).Select(e => e.Id);
    }
}
```

---

### Rules Engine

The Rules Engine evaluates one or more business rules against a model, returning pass/fail results and error messages.

**Key interfaces** (`Diamond.Core.Rules.Abstractions`):

```csharp
public interface IRule<TItem, TResult> : IRule
{
    Task<TResult> ValidateAsync(TItem item);
}

public interface IRule<TItem> : IRule<TItem, IRuleResult> { }

public interface IRuleResult
{
    bool Passed { get; set; }
    string ErrorMessage { get; set; }
}
```

**Implementing a rule**:

```csharp
public class GoodStandingRule : RuleTemplate<IEmployeeEntity>
{
    // "EmployeePromotion" is the group key used to batch-evaluate all related rules
    public GoodStandingRule() : base(WellKnown.Rules.EmployeePromotion) { }

    protected override Task<IRuleResult> OnValidateAsync(IEmployeeEntity employee)
    {
        bool passed = !employee.HasRecentWarnings;
        return Task.FromResult<IRuleResult>(new RuleResult
        {
            Passed = passed,
            ErrorMessage = passed ? null : "Employee has recent disciplinary warnings."
        });
    }
}

// Evaluate all rules in a group
string failureMessage = await rulesFactory.EvaluateAsync(
    WellKnown.Rules.EmployeePromotion, employee);
```

---

### Workflow / State Machine

The Workflow engine executes a series of ordered steps (each implementing `IWorkflowItem`) against a shared `IContext`. Two built-in managers control execution behavior:

- **`LinearCompleteWorkflowManager`** — runs all steps even if one fails
- **`LinearHaltWorkflowManager`** — stops on the first failed step

**Key interfaces** (`Diamond.Core.Workflow.Abstractions`):

```csharp
public interface IWorkflowManager
{
    string ServiceKey { get; set; }
    IWorkflowItem[] Steps { get; }
    Task<bool> ExecuteWorkflowAsync(IContext context);
}

public interface IWorkflowItem
{
    int Ordinal { get; set; }
    string Name { get; set; }
    double Weight { get; set; }
    bool AlwaysExecute { get; set; }
    Task<bool> ExecuteStepAsync(IContext context);
}
```

**Creating a workflow**:

```csharp
// Define the manager
public class OnboardingWorkflow : LinearHaltWorkflowManager
{
    public OnboardingWorkflow(ILogger<OnboardingWorkflow> logger,
        IWorkflowItemFactory stepFactory)
        : base(logger, stepFactory)
    {
        this.ServiceKey = WellKnown.Workflow.Onboarding;
    }
}

// Define a step
public class CreateAccountStep : WorkflowItemTemplate
{
    public override string Name => "Create User Account";
    public override int Ordinal => 1;

    protected override async Task<bool> OnExecuteStepAsync(IContext context)
    {
        string username = context.Properties.Get<string>("username");
        // ... create account ...
        return true;
    }
}

// Execute
IWorkflowManager workflow = await workflowFactory.GetAsync(WellKnown.Workflow.Onboarding);
bool success = await workflow.ExecuteWorkflowAsync(context);
```

---

### Dependency Injection Extensions

Diamond.Core extends `IServiceCollection` with helpers for registering repositories and database contexts across all major EF Core providers.

```csharp
// Register EF Core with SQL Server
services.AddDiamondSqlServer<MyDbContext>(options =>
    options.UseSqlServer(connectionString));

// Register EF Core with SQLite
services.AddDiamondSqlite<MyDbContext>(options =>
    options.UseSqlite(connectionString));

// Register EF Core with PostgreSQL
services.AddDiamondPostgreSQL<MyDbContext>(options =>
    options.UseNpgsql(connectionString));

// Supported providers: SqlServer, PostgreSQL, MySql, Sqlite, Oracle, Cosmos, InMemory
```

---

### Hosting Extensions

Diamond.Core extends the generic host with a modular startup pipeline, enabling separate classes for each lifecycle phase.

**Key interfaces** (`Diamond.Core.Extensions.Hosting.Abstractions`):

```csharp
public interface IStartupConfigureServices : IStartup
{
    void ConfigureServices(HostBuilderContext context, IServiceCollection services);
}

public interface IStartupConfigureLogging : IStartup
{
    void ConfigureLogging(HostBuilderContext context, ILoggingBuilder logging);
}
```

**Usage**:

```csharp
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureDiamondCore()
    .Build();
```

---

### Command Line

Built on top of `System.CommandLine`, this module provides a DI-friendly command abstraction with naming-convention binding.

```csharp
// Implement a command
public class GreetCommand : ICommand
{
    public string Name => "greet";
    public string Description => "Greets a user by name";

    public async Task<int> ExecuteAsync(string name)
    {
        Console.WriteLine($"Hello, {name}!");
        return 0;
    }
}
```

---

### ASP.NET Core Utilities

#### DoAction Pattern

Moves controller logic out of action methods into dedicated, DI-resolved action classes, keeping controllers thin.

```csharp
// Define the action
public class GetEmployeeAction
    : IDoAction<int, IEmployeeEntity>
{
    public async Task<IControllerActionResult<IEmployeeEntity>> ExecuteActionAsync(int id)
    {
        IEmployeeEntity employee = await this.repository.GetByIdAsync(id);
        return employee is not null
            ? this.Ok(employee)
            : this.NotFound();
    }
}

// In the controller
[HttpGet("{id}")]
public async Task<IActionResult> GetEmployee(int id)
{
    IDoAction<int, IEmployeeEntity> action =
        await this.doActionFactory.GetAsync<int, IEmployeeEntity>(WellKnown.DoAction.GetEmployee);
    IControllerActionResult<IEmployeeEntity> result = await action.ExecuteActionAsync(id);
    return this.StatusCode(result.StatusCode, result.Result);
}
```

#### Other ASP.NET Core Packages

- **`Diamond.Core.AspNetCore.DataTables`** — Server-side processing for jQuery DataTables
- **`Diamond.Core.AspNetCore.Rfc7807`** — Standardized error responses per [RFC 7807](https://datatracker.ietf.org/doc/html/rfc7807)
- **`Diamond.Core.AspNetCore.Swagger`** — Swagger/OpenAPI document configuration helpers

---

### AutoMapper Extensions

Simplifies AutoMapper registration within the Diamond.Core DI container.

```csharp
services.AddAutoMapper(typeof(MyMappingProfile).Assembly);
// Diamond.Core wires AutoMapper profiles discovered via DI
```

---

### Clonable Pattern

Provides deep object cloning with pluggable serialization backends.

**Key interface** (`Diamond.Core.Clonable.Abstractions`):

```csharp
public interface IObjectCloneFactory
{
    T CloneInstance<T>(T instance) where T : ICloneable;
}
```

**Usage**:

```csharp
// Register the Newtonsoft.Json-backed implementation
services.AddNewtonsoftClonable();

// Clone an object
IEmployeeEntity copy = cloneFactory.CloneInstance(employee);
```

Available backends:
- `Diamond.Core.Clonable.Newtonsoft` — Newtonsoft.Json serialization
- `Diamond.Core.Clonable.Microsoft` — `System.Text.Json` serialization

---

### System Utilities

#### Performance Measurement

```csharp
IMeasureAction measure = serviceProvider.GetRequiredService<IMeasureAction>();
await measure.ExecuteAsync("LongOperation", async () =>
{
    await DoSomethingExpensiveAsync();
});
```

#### Base36 Encoding

```csharp
string encoded = Base36.Encode(12345678L);  // "7CLZI"
long decoded   = Base36.Decode("7CLZI");    // 12345678
```

#### Temporary Folders

```csharp
ITemporaryFolder folder = await tempFolderFactory.GetAsync();
// folder.Path is an auto-created temp directory
// folder is IDisposable — deletes itself on Dispose()
```

#### Text Utilities

`Diamond.Core.System.Text` provides string manipulation and formatting helpers used throughout the library.

---

### WPF Support

`Diamond.Core.Wpf` and `Diamond.Core.Wpf.Abstractions` provide WPF-friendly DI integration including `IWindow`/`IMainWindow` interfaces, enabling window resolution from the DI container.

```csharp
// Register main window
services.AddTransient<IMainWindow, MainWindow>();

// Resolve and show
IMainWindow window = serviceProvider.GetRequiredService<IMainWindow>();
((Window)window).Show();
```

---

## Examples

The repository ships with fully working example projects under `Src/Examples/`:

| Example | Description |
|---------|-------------|
| **`Diamond.Core.Example.BasicConsole`** | Comprehensive console app demonstrating Repository, Rules, Unit of Work, Workflow, Decorator, and Specification patterns together using SQLite and SQL Server |
| **`Diamond.Core.Example.Web`** | ASP.NET Core web API using the DoAction pattern with EF Core |
| **`Diamond.Core.Example.Wpf`** | WPF application demonstrating DI-resolved windows and commands |
| **`Diamond.Core.Example.ConsoleCommand`** | Console app with `System.CommandLine`-based command routing |
| **`Diamond.Core.Example.AutoMapperDependency`** | AutoMapper + Diamond.Core DI integration |
| **`Diamond.Core.Example.DbContext`** | EF Core `DbContext` setup and multiple-provider configuration |
| **`Diamond.Core.Example.LoadServices`** | Scanning and auto-registering services from assemblies |
| **`Diamond.Core.Example.KeyedServices`** | Keyed / named service registration with the DI extensions |

To run the BasicConsole example:

```bash
cd Src/Examples/Diamond.Core.Example.BasicConsole
dotnet run
```

---

## Design Philosophy

1. **Abstraction-first** — every pattern ships a separate `*.Abstractions` package so application code can depend only on interfaces.
2. **Database-agnostic repositories** — the same repository pattern works across SQL Server, PostgreSQL, MySQL, SQLite, Oracle, Cosmos DB, and in-memory providers; the provider is selected at DI registration time.
3. **Consistent extension-method DI setup** — every pattern registers itself into `IServiceCollection` via fluent extension methods.
4. **Template base classes** — abstract base classes such as `DecoratorTemplate`, `RuleTemplate`, and `WorkflowItemTemplate` eliminate boilerplate, letting you focus on business logic.
5. **Composed, not inherited** — heavy use of the Decorator and Factory patterns composes behavior at runtime rather than through deep inheritance hierarchies.

---

## Requirements

| Requirement | Version |
|-------------|---------|
| .NET SDK | 10.0+ |
| C# | 13.0+ |
| Entity Framework Core | compatible with .NET 10 |
| AutoMapper *(optional)* | 16.x |

---

## License

This library is licensed under the **GNU Lesser General Public License v3.0 or later (LGPL-3.0-or-later)**.  
See the [LICENSE](LICENSE) file for full details.

Copyright © Daniel Porrey 2019–2026

