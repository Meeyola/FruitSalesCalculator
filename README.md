# Fruit Sales Calculator

A small system for calculating the price of fruit orders, supporting multiple pricing methods (per kg, per item) 
and conditional bulk discounts.

## Design Decisions

The system is split into four independent areas: 

- **Models**: Contains the data structures representing fruits and their properties.
- **Repositories**: Responsible for data access, currently implemented with a JSON file repository.
- **Services**: Contains the business logic for calculating prices based on different pricing strategies.
- **Pricing**: Contains the pricing strategies that can be applied to calculate the final price of fruits.

## Design Patterns Used

- **Strategy Pattern**: The pricing strategies are implemented using the Strategy pattern. 
Currently there is only one implementation - StandardPricing, since both PerItem and PerKg have the same calculation logic.
However, if there is a need to add a new pricing method, it can be done easily, without modifying existing code.

- **Decorator Pattern**: The bulk discount is implemented using the Decorator pattern. This decorator wraps around the existing
and future pricing strategies, applying the bulk discount logic on top of the base pricing strategy.

## Ways to Extend

- **Adding a new fruit type**: To add a new fruit type, simply add a new entry in the `fruits.json` file with the appropriate properties.
- **Adding a new pricing strategy**: To add a new pricing strategy, create a new class that implements the `IPricingStrategy` interface.
- **Adding a new discount**: If a new discount needs to be added on top of the existing one, you can create a new decorator and stack it with the previous one.
 
