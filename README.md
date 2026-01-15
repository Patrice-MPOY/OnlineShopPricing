# Online Shop Pricing – Technical Exercise

## Overview

This repository implements a cart pricing calculation for an online shop, as part of a technical exercise.

The solution computes the total cost of a customer's cart by applying one of three pricing grids, determined by the customer type:
- Individual customers
- Small business customers (annual turnover < €10M)
- Large business customers (annual turnover ≥ €10M)

## Versions & Evolutions

- **v1.0-exercise-submission**  
  Exact version submitted as part of the recruitment exercise (January 2026).  
  Commit: [9088856](https://github.com/Patrice-MPOY/OnlineShopPricing/commit/9088856)  
  → [View code at this tag](https://github.com/Patrice-MPOY/OnlineShopPricing/tree/v1.0-exercise-submission)

- **Post-submission evolutions** (personal learning after the recruitment process was paused)  
  - Introduction of `Entity<T>` base class and `IAggregateRoot` marker interface  
  - Implementation of Domain Events (e.g. `ProductAddedToCart`)  
  - Refactoring of `Cart` to act as a true Aggregate Root with an internal collection of domain events  
  - Exhaustive unit and behavioral tests  


## Solution Structure

The solution is organised as a multi-project setup for clarity and maintainability:

- **OnlineShopPricing.Core** (.NET Class Library)  
  Core domain and business logic:
  - `Domain`: `Customer` (abstract base with polymorphic behaviour), `IndividualCustomer`, `BusinessCustomer`, `ProductType` (enum), `Cart` (aggregate root)
  - `Services`: `IPricingStrategy`, `PricingStrategyBase`, and concrete pricing strategies (`IndividualPricingStrategy`, `SmallBusinessPricingStrategy`, `LargeBusinessPricingStrategy`)

- **OnlineShopPricing.Tests** (xUnit Test Project)  
  Domain-focused test suite using xUnit and FluentAssertions:
  - `PricingDomainTests`: validates correct pricing grid selection and strategy behaviour
  - `CartDomainTests`: validates cart mechanics, invariants, and edge cases
  - All tests rely exclusively on real domain objects (no mocks)

## Key Design Decisions

- **Polymorphic pricing strategy resolution**  
  The `Customer` itself is responsible for returning its appropriate IPricingStrategy via an abstract `GetPricingStrategy()` method  
  This removes the need for an external factory, follows the **Tell, Don’t Ask** principle, and adheres to **Open/Closed** and **Single Responsibility** principles.

- **Domain-centric design**  
  Pricing rules are encapsulated within the customer hierarchy, resulting in high cohesion and natural extensibility.

- **Simplified Cart API**  
  The `Cart` only requires a `Customer`. Pricing strategy resolution is delegated to the domain model itself.

- **Clear test separation**  
  - `PricingDomainTests`: pricing rules and strategy behaviour
  - `CartDomainTests`: cart aggregate behaviour and invariants

- **No external dependencies**  
  The solution intentionally focuses on pure domain logic, remaining simple and focused on the exercise 
  while being production-ready from a design perspective.

- **Financial amounts encapsulated in `Money` Value Object**  
  All monetary values (unit prices, totals, subtotals, etc.) are represented using the immutable `Money` type 
  instead of raw `decimal`.  
  - **Strict rule**: Never use `decimal` directly for financial amounts in the domain  
  - Key benefits:  
    - Enforces domain invariants (amount ≥ 0) at construction time  
    - Prevents primitive obsession and confusion between prices, quantities, rates, etc.  
    - Provides value-based equality, immutability, and natural domain operators (`+`, `*`)  
    - Implicit EUR currency (multi-currency not needed for this exercise – YAGNI)  
  - Location: `OnlineShopPricing.Core/Domain/ValueObjects/Money.cs`

## Tests

All tests should pass and cover:
- Correct application of the three pricing grids (including the €10M boundary case)
- Cart invariants (quantity > 0, known product, non-null customer)
- Edge cases (empty cart, very large quantities)

## Potential Extensions

- Externalised pricing configuration (database, feature flags, admin UI)
- Volume-based, time-limited, or customer-specific promotions
- Additional customer segments (VIP, nonprofit, etc.)
- Cart persistence and multi-device synchronisation
- Integration into an ASP.NET Core Web API

--------

Thank you for reviewing this exercise.  
I look forward to discussing the design choices, the evolution from a factory-based approach to polymorphic 
resolution, the testing strategy, and how this domain model could evolve in a larger system.

**Patrice MPOY**
