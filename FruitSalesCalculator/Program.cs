using FruitSalesCalculator.Models;
using FruitSalesCalculator.PricingService;
using Spectre.Console;

var repository = new FruitSalesCalculator.Repositories.JsonFruitRepository("Data/fruits.json");
var calculator = new FruitCalculator(repository);

var fruit = calculator.GetFruits();

if (fruit.Count == 0)
{
    AnsiConsole.MarkupLine("[red]No fruits available.[/]");
    return;
}

var purchases = new List<(FreshProduce Fruit, decimal Quantity, decimal Price)>();
bool continueShopping = true;

while (continueShopping)
{
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold yellow]🍎 Welcome to the Fruit shop![/]");
    AnsiConsole.WriteLine();

    var selectedName = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Select a [green]fruit[/]:")
            .PageSize(10)
        .AddChoices(fruit.Select(f => f.Name))
);

    var selectedFruit = fruit.First(f => f.Name == selectedName);

    string quantityPrompt = selectedFruit.PricingMethod == PricingMethod.PerKg
        ? $"Enter the [green]weight in kg[/] of {selectedFruit.Name} to purchase:"
        : $"Enter the [green]number of items[/] of {selectedFruit.Name} to purchase:";

    decimal quantity = AnsiConsole.Prompt(
        new TextPrompt<decimal>(quantityPrompt)
            .Validate(q => q > 0 ? ValidationResult.Success() : ValidationResult.Error("[red]Quantity must be greater than zero.[/]"))
    );

    decimal price = calculator.CalculatePrice(selectedFruit, quantity);

    purchases.Add((selectedFruit, quantity, price));

    AnsiConsole.MarkupLine($"[green]✓[/] Added {quantity} {(selectedFruit.PricingMethod == PricingMethod.PerKg ? "kg" : "items")} of {selectedFruit.Name} to cart");
    AnsiConsole.WriteLine();

    continueShopping = AnsiConsole.Confirm("Would you like to add another fruit?", true);
}

AnsiConsole.Clear();
AnsiConsole.WriteLine();
AnsiConsole.MarkupLine("[bold underline yellow]📜 RECEIPT[/]");
AnsiConsole.WriteLine();

var table = new Table();
table.AddColumn("Item");
table.AddColumn("Quantity");
table.AddColumn(new TableColumn("Price").RightAligned());

decimal totalPrice = 0;

foreach (var (purchasedFruit, qty, itemPrice) in purchases)
{
    string unit = purchasedFruit.PricingMethod == PricingMethod.PerKg ? "kg" : "items";
    table.AddRow(
        purchasedFruit.Name,
        $"{qty} {unit}",
        $"{itemPrice:C}"
    );
    totalPrice += itemPrice;
}

AnsiConsole.Write(table);

AnsiConsole.WriteLine();
var rule = new Rule($"[bold green]TOTAL: {totalPrice:C}[/]");
rule.RightJustified();
AnsiConsole.Write(rule);

AnsiConsole.WriteLine();
AnsiConsole.MarkupLine("[yellow]Thank you for shopping! 👋[/]");