# Diamond.Core-Patterns — Architecture Overview

## What This Project Is

**Diamond.Core-Patterns** is a C# library that provides **production-ready implementations of enterprise design patterns** for .NET applications. It is published as a set of NuGet packages authored by Daniel M. Porrey, targeting **.NET 10.0**, and licensed under LGPL-3.0-or-later. The entire library lives under the `Diamond.Core.*` namespace.

---

## Directory Structure

Everything meaningful lives under `/Src`, organized into 18 functional areas with **67 projects** total:

| Folder | Purpose |
|--------|---------|
| `Shared/` | Core abstractions (`Diamond.Core.Abstractions`) shared by all patterns |
| `Repository/` | Generic data access layer with read/write separation |
| `Unit of Work/` | Transaction management across aggregates/repos |
| `Decorator/` | Composable decorator base templates |
| `Specification/` | Query object encapsulation with LinqKit |
| `Rules/` | Configurable business rules engine |
| `Workflow/` | State machine / workflow orchestration |
| `Dependency-Injection/` | Extensions for `IServiceCollection` with multi-DB support |
| `Hosting/` | Extensions for `IHost` / `IHostBuilder` |
| `Command Line/` | CLI argument parsing and command execution |
| `AspNetCore/` | DataTables, RFC 7807 problem details, Swagger/OpenAPI helpers |
| `AutoMapper/` | AutoMapper integration utilities |
| `Clonable/` | Deep/shallow object cloning (JSON-based and reflection-based) |
| `System/` | Utilities: Base36 encoding, text helpers, temp folders, performance |
| `Wpf/` | WPF/MVVM command binding support |
| `Examples/` | Sample console, web, WPF, and DI demo apps |
| `Tests/` | Unit test projects (NUnit) |

---

## Consistent Project Architecture

Every major pattern follows the same **three-layer structure**:

```
Diamond.Core.[Pattern].Abstractions       ← Interfaces/contracts only
Diamond.Core.[Pattern]                    ← Core implementation
Diamond.Core.[Pattern].[Specialization]   ← e.g., EntityFrameworkCore, SqlServer
```

**Namespace convention:** `Diamond.Core.[Pattern].[Specialization]`

**Common internal structure per project:**

```
/Services/       — Business logic services
/Decorators/     — Decorator implementations
/Factories/      — Factory pattern objects
/Models/         — Data models and entities
/Exceptions/     — Custom exceptions
/Interfaces/     — Contracts (in abstraction projects)
```

---

## Key Technologies

| Category | Technology / Package |
|----------|----------------------|
| **Framework** | .NET 10.0 (SDK-style projects) |
| **DI / Hosting** | `Microsoft.Extensions.Hosting`, `Microsoft.Extensions.DependencyInjection`, `Microsoft.Extensions.Logging` |
| **ORM** | Entity Framework Core (generic repository implementations) |
| **Database Providers** | SQL Server, PostgreSQL, SQLite, MySQL, Oracle, Cosmos DB, In-Memory |
| **Mapping** | AutoMapper 16.x |
| **Querying** | LinqKit.Core (for Specification pattern predicates) |
| **CLI** | `System.CommandLine` (beta/alpha) |
| **Serialization** | Newtonsoft.Json |
| **Disposable helpers** | `System.DisposableObject`, `TryDisposable` |
| **Testing** | NUnit 4.x, NUnit3TestAdapter, coverlet |
| **NuGet / Symbols** | Microsoft.SourceLink.GitHub, `.snupkg` symbol packages |

---

## Design Patterns Implemented

| Pattern | Key Base Class / Interface |
|---------|---------------------------|
| **Repository** | Generic read/write repo with `IReadOnlyRepository<T>` / `IWritableRepository<T>` |
| **Unit of Work** | Coordinates transactions across multiple repositories |
| **Decorator** | `DecoratorTemplate<TDecoratedItem, TResult>` |
| **Specification** | `SpecificationTemplate<TParameter, TResult>` using LinqKit predicates |
| **Rules Engine** | Pluggable business rules evaluated against a model |
| **Workflow / State Machine** | Linear and branching workflow orchestration with step factories |
| **Factory** | Abstractions for repository, workflow step, and decorator factories |
| **Clonable** | JSON-serialization and reflection-based deep clone |
| **Command** | CLI command abstraction with naming-convention binding |

---

## Testing & Build

- **Test runner:** NUnit 4.3.2 with `NUnit3TestAdapter`
- **Coverage:** `coverlet.collector`
- **Test projects exist for:** Clonable, Decorator, Rules, Specification, System utilities
- **Version:** All projects share version `10.1.0` (aligned to the .NET 10 target)
- **NuGet publishing:** Manual via `/Src/NuGet.Publish.cmd` batch script (no CI/CD pipeline files present)
- **Extras:** SourceLink enabled so consumers can step through source code when debugging NuGet packages; XML documentation generated for all projects

---

## Key Design Philosophy

1. **Abstraction-first** — every pattern has a separate `*.Abstractions` NuGet so consumers depend only on interfaces, not implementations.
2. **Database-agnostic repositories** — the same repository pattern works with any EF Core provider, selected at DI registration time.
3. **Consistent extension-method-based DI setup** — patterns register themselves into `IServiceCollection` via fluent extension methods.
4. **Template base classes** — reduce boilerplate by providing abstract base classes (e.g., `DecoratorTemplate`, `SpecificationTemplate`) that consumers simply subclass.
5. **Composed, not inherited** — heavy use of the decorator and factory patterns to compose behaviors at runtime rather than through deep inheritance.
