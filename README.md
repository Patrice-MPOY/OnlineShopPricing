# Online Shop Pricing – Technical Exercise



## Overview

This repository implements a cart pricing calculation for an online shop, as part of a technical exercise.

The solution computes the total cost of a customer's cart by applying one of three pricing grids, determined by the customer type:
- Individual customers
- Small business customers (annual turnover < €10M)
- Large business customers (annual turnover ≥ €10M)

> Note: This solution deliberately applies a limited set of Domain-Driven Design (DDD) tactical patterns
> (Aggregates, Value Objects, Domain Events) to structure the pricing logic.
> Given the simplicity of the exercise, this is not presented as a mandatory approach,
> but as one possible design that could scale with increased business complexity.

---

## Tech Stack

- .NET 8 / .NET 9 (C# 12+, records, modern language features)
- xUnit + FluentAssertions (pure domain tests, no mocks)
- Domain-Driven Design (DDD) tactical patterns
- Polymorphic behaviour (Customer → PricingStrategy)
- Value Objects (`Money`)
- No external dependencies for the core domain (pure POCOs)

---

## Versions & Evolutions

- **v1.0-exercise-submission**  
  Exact version submitted as part of the recruitment exercise (January 2026).  
  Commit: [9088856](https://github.com/Patrice-MPOY/OnlineShopPricing/commit/9088856)  
  → [View code at this tag](https://github.com/Patrice-MPOY/OnlineShopPricing/tree/v1.0-exercise-submission)

- **Post-submission evolutions** (personal learning after the recruitment process was paused)
  - Introduction of a generic `Entity<T>` base class and `IAggregateRoot` marker interface
  - Refactoring of `Cart` to act as a true Aggregate Root
  - Implementation of Domain Events (e.g. `ProductAddedToCart`)
  - Internal collection of domain events within aggregates
  - Exhaustive unit and behavioural tests (consistent Should/When naming)
  - **Planned**: EF Core integration with domain event dispatching via interceptor / Outbox pattern

---

## Solution Structure

The solution is organised as a multi-project setup for clarity and maintainability:

- **OnlineShopPricing.Core** (.NET Class Library)  
  Core domain and business logic:
  - `Domain`: `Customer` (abstract base with polymorphic behaviour),  
    `IndividualCustomer`, `BusinessCustomer`, `ProductType`, `Cart` (aggregate root)
  - `Services`: `IPricingStrategy`, `PricingStrategyBase`, and concrete strategies  
    (`IndividualPricingStrategy`, `SmallBusinessPricingStrategy`, `LargeBusinessPricingStrategy`)
  - `ValueObjects`: `Money`

- **OnlineShopPricing.Tests** (xUnit Test Project)  
  Domain-focused test suite:
  - `PricingDomainTests`: pricing grid selection and strategy behaviour
  - `CartDomainTests`: cart invariants, behaviour, and edge cases
  - Tests rely exclusively on real domain objects (no mocks)

---

## Key Design Decisions

- **Polymorphic pricing strategy resolution**  
  The `Customer` is responsible for returning its appropriate `IPricingStrategy`
  via an abstract `GetPricingStrategy()` method.  
  This removes the need for an external factory, follows the **Tell, Don’t Ask**
  principle, and adheres to **Open/Closed** and **Single Responsibility** principles.

- **Domain-centric design**  
  Pricing rules are encapsulated within the customer hierarchy, resulting in high cohesion
  and natural extensibility.

- **Simplified Cart API**  
  The `Cart` only requires a `Customer`. Pricing strategy resolution is delegated
  to the domain model itself, avoiding orchestration logic in the application layer.

- **Domain Events (illustrative)**  
  Domain events (e.g. `ProductAddedToCart`) are emitted by the aggregate but not yet consumed.
  They illustrate how the model could evolve toward event-driven reactions
  (analytics, promotions, projections) without coupling the core domain logic.

- **Financial amounts encapsulated in `Money` Value Object**  
  Monetary values are never represented as raw `decimal` in the domain.
  - Enforces invariants (amount ≥ 0)
  - Prevents primitive obsession
  - Immutable, value-based equality
  - Natural operators (`+`, `*`)
  - Implicit EUR currency (YAGNI for multi-currency)

---

## Tests

All tests should pass and cover:
- Correct application of the three pricing grids (including the €10M boundary)
- Cart invariants (quantity > 0, known product, non-null customer)
- Edge cases (empty cart, very large quantities)

---

## Potential Extensions

- Externalised pricing configuration (database, feature flags, admin UI)
- Volume-based, time-limited, or customer-specific promotions
- Additional customer segments (VIP, nonprofit, etc.)
- Cart persistence and multi-device synchronisation
- Integration into an ASP.NET Core Web API
- CQRS-style read models for pricing summaries

---

Thank you for reviewing this exercise.  
I look forward to discussing the design choices (polymorphic resolution vs factory,
domain events, testing strategy), how this model could evolve in a larger system
(EF Core persistence, CQRS, microservices), and any feedback you may have.

**Patrice MPOY**
