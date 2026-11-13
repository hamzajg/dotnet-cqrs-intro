# CQRS and Event Sourcing Intro for Developers

We live in a world of dynamically changing technologies. New ways of architecturing our solutions, new frameworks and libraries seem to appear on almost daily basis. 


**But good software engineering is not about fancy frameworks and solutions aggressively promoted by their vendors.** It is not about doing something because Netflix or Google did it. It is about taking well-thought-out decisions based on facts and knowledge. That’s why it is important to be familiar basic architectural concepts like CQRS. It is one of the tools we use in our software house every day. We mentioned CQRS in the article which is part of the series about [Microservices on .NET Core](https://altkomsoftware.pl/en/blog/building-microservices-on-net-core-1/), but it was presented from technical perspective and here we want to focus on basics concepts explanation with visualisation and examples.


[Check our article!](https://altkomsoftware.pl/en/blog/cqrs-event-sourcing/)

## No CQRS

<p align="center">
    <img alt="No CQRS" src="https://raw.githubusercontent.com/asc-lab/dotnet-cqrs-intro/master/readme-images/1_no_cqrs.png" />
</p>

## Separate Commands and Queries

<p align="center">
    <img alt="Separate Commands and Queries" src="https://raw.githubusercontent.com/asc-lab/dotnet-cqrs-intro/master/readme-images/2_separe_commands_queries.png" />
</p>

## Separate Models Commands and Queries

<p align="center">
    <img alt="Separate Models Commands and Queries" src="https://raw.githubusercontent.com/asc-lab/dotnet-cqrs-intro/master/readme-images/3_separate_models_commands_queries.png" />
</p>

## Separate Storage Engines

<p align="center">
    <img alt="Separate Storage Engines" src="https://raw.githubusercontent.com/asc-lab/dotnet-cqrs-intro/master/readme-images/4_separate_storage_engines.png" />
</p>

## Event Sourcing

<p align="center">
    <img alt="Event Sourcing" src="https://raw.githubusercontent.com/asc-lab/dotnet-cqrs-intro/master/readme-images/5_event_sourcing.png" />
</p>

## Running locally without a database server

The solution targets **.NET 10** and defaults to an in-memory storage profile, so
`dotnet run` works on a clean machine with no PostgreSQL, no Docker and no
connection strings:

```bash
dotnet run --project CqrsWithEs        # event-sourced, in-memory event store + read model
dotnet run --project SeparateModels    # Marten-style domain, in-memory write store + read models
dotnet run --project NoCqrs            # EF Core in-memory
dotnet test                            # 30 tests, none of them need a server
```

Each app reads `Storage:Provider` from configuration:

| value | meaning |
|---|---|
| `InMemory` (default) | everything is held in process memory and disappears on exit |
| `Marten` / `Postgres` | uses the `DefaultConnection` connection string and a real PostgreSQL server |

```bash
Storage__Provider=Postgres dotnet run --project CqrsWithEs
```

In `SeparateModels` the `Marten` profile keeps the document store (Marten 9);
the in-memory profile uses `IDataStore`/read-model implementations that mirror
the same tables, so switching profiles does not change the application code.
