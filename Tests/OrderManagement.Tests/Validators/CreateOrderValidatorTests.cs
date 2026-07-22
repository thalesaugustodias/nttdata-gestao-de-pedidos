using FluentAssertions;
using OrderManagement.Application.CQRS.Commands.Orders;

namespace OrderManagement.Tests.Validators;

public class CreateOrderValidatorTests
{
    private readonly CreateOrderValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_PassesValidation()
    {
        var command = new CreateOrderCommand(
            CustomerId: Guid.NewGuid(),
            Items: [new OrderItemInput("Product A", 1, 10.00m)]
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_EmptyItems_FailsValidation()
    {
        var command = new CreateOrderCommand(
            CustomerId: Guid.NewGuid(),
            Items: []
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Order must have at least 1 item.");
    }

    [Fact]
    public void Validate_ZeroQuantity_FailsValidation()
    {
        var command = new CreateOrderCommand(
            CustomerId: Guid.NewGuid(),
            Items: [new OrderItemInput("Product A", 0, 10.00m)]
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Quantity must be greater than zero.");
    }

    [Fact]
    public void Validate_ZeroUnitPrice_FailsValidation()
    {
        var command = new CreateOrderCommand(
            CustomerId: Guid.NewGuid(),
            Items: [new OrderItemInput("Product A", 1, 0.00m)]
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "UnitPrice must be greater than zero.");
    }

    [Fact]
    public void Validate_EmptyCustomerId_FailsValidation()
    {
        var command = new CreateOrderCommand(
            CustomerId: Guid.Empty,
            Items: [new OrderItemInput("Product A", 1, 10.00m)]
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "CustomerId is required.");
    }
}
