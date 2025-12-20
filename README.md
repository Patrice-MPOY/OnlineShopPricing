# Online Shop Pricing – Technical Exercise

## Overview

This repository implements a cart pricing calculation for an online shop, as per the provided technical exercise.

The solution computes the total cost of a customer's cart using one of three pricing grids based on customer type:
- Individual customers
- Business customers with annual revenue < €10M
- Business customers with annual revenue ≥ €10M

## Solution Structure

Multi-project solution for clarity and reusability:

- **OnlineShopPricing.Core** (.NET Class Library)  
  Core domain and business logic:
  - `Domain`: `Customer` (abstract base), `IndividualCustomer`, `BusinessCustomer`, `ProductType`, `Cart` (aggregate root)
  - `Services`: `IPricingStrategy`, concrete strategies, injectable `PricingStrategyFactory`

- **OnlineShopPricing.Tests** (xUnit Test Project)  
  Comprehensive test suite using xUnit, FluentAssertions, and Moq:
  - End-to-end integration tests covering all pricing scenarios and edge cases
  - Isolated unit tests demonstrating dependency injection and mocking
  - Test helpers for unsupported scenarios

## Key Design Decisions

- **Strategy pattern** for pricing variability – easily extensible for new segments or promotions.
- **Dependency injection** in `Cart` and `PricingStrategyFactory` for testability and adherence to DIP.
- **No console application** – the library is the deliverable; behavior is fully validated through tests (executable documentation).
- **Pure domain** – no external dependencies or persistence, focused on the exercise while remaining production-ready in structure.

## Running the Tests
``bash
dotnet test

All tests should pass, covering:
- The three pricing grids
- Defensive cases (negative quantity, null client, unsupported client type, unknown product)
- Isolated cart behavior with mocked pricing strategy

## Potential Extensions

- Configurable pricing (JSON, database)
- Discount and promotion rules
- Cart persistence (Redis, relational database)
- Integration into an ASP.NET Core API or microservice

Thank you for the exercise – I look forward to discussing the design choices and trade-offs 
during the technical interview.

Patrice MPOY

