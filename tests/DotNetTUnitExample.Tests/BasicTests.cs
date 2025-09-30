using TUnit.Core;
using DotNetTUnitExample.Services;

namespace DotNetTUnitExample.Tests;

/// <summary>
/// Basic TUnit tests demonstrating fundamental testing features.
/// Shows simple test methods, basic assertions, and exception handling.
/// </summary>
public class BasicTests
{
    private readonly CalculatorService _calculatorService;

    public BasicTests()
    {
        _calculatorService = new CalculatorService();
    }

    #region Addition Tests

    /// <summary>
    /// Tests basic addition operation with positive numbers.
    /// Demonstrates simple TUnit test method and Assert.That() syntax.
    /// </summary>
    [Test]
    public async Task Add_WithPositiveNumbers_ReturnsCorrectSum()
    {
        // Arrange
        double a = 5.0;
        double b = 3.0;
        double expected = 8.0;

        // Act
        double result = _calculatorService.Add(a, b);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    /// <summary>
    /// Tests addition with negative numbers.
    /// Demonstrates assertion with negative values.
    /// </summary>
    [Test]
    public async Task Add_WithNegativeNumbers_ReturnsCorrectSum()
    {
        // Arrange
        double a = -5.0;
        double b = -3.0;
        double expected = -8.0;

        // Act
        double result = _calculatorService.Add(a, b);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    /// <summary>
    /// Tests addition with zero.
    /// Demonstrates edge case testing.
    /// </summary>
    [Test]
    public async Task Add_WithZero_ReturnsOtherNumber()
    {
        // Arrange
        double a = 10.0;
        double b = 0.0;
        double expected = 10.0;

        // Act
        double result = _calculatorService.Add(a, b);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    #endregion

    #region Subtraction Tests

    /// <summary>
    /// Tests basic subtraction operation.
    /// Demonstrates subtraction testing patterns.
    /// </summary>
    [Test]
    public async Task Subtract_WithPositiveNumbers_ReturnsCorrectDifference()
    {
        // Arrange
        double a = 10.0;
        double b = 4.0;
        double expected = 6.0;

        // Act
        double result = _calculatorService.Subtract(a, b);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    /// <summary>
    /// Tests subtraction resulting in negative number.
    /// Demonstrates handling of negative results.
    /// </summary>
    [Test]
    public async Task Subtract_ResultingInNegative_ReturnsCorrectDifference()
    {
        // Arrange
        double a = 3.0;
        double b = 7.0;
        double expected = -4.0;

        // Act
        double result = _calculatorService.Subtract(a, b);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    #endregion

    #region Multiplication Tests

    /// <summary>
    /// Tests basic multiplication operation.
    /// Demonstrates multiplication testing.
    /// </summary>
    [Test]
    public async Task Multiply_WithPositiveNumbers_ReturnsCorrectProduct()
    {
        // Arrange
        double a = 4.0;
        double b = 5.0;
        double expected = 20.0;

        // Act
        double result = _calculatorService.Multiply(a, b);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    /// <summary>
    /// Tests multiplication by zero.
    /// Demonstrates zero multiplication edge case.
    /// </summary>
    [Test]
    public async Task Multiply_ByZero_ReturnsZero()
    {
        // Arrange
        double a = 15.0;
        double b = 0.0;
        double expected = 0.0;

        // Act
        double result = _calculatorService.Multiply(a, b);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    /// <summary>
    /// Tests multiplication with negative numbers.
    /// Demonstrates negative number multiplication.
    /// </summary>
    [Test]
    public async Task Multiply_WithNegativeNumbers_ReturnsCorrectProduct()
    {
        // Arrange
        double a = -3.0;
        double b = 4.0;
        double expected = -12.0;

        // Act
        double result = _calculatorService.Multiply(a, b);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    #endregion

    #region Division Tests

    /// <summary>
    /// Tests basic division operation.
    /// Demonstrates division testing patterns.
    /// </summary>
    [Test]
    public async Task Divide_WithValidNumbers_ReturnsCorrectQuotient()
    {
        // Arrange
        double a = 15.0;
        double b = 3.0;
        double expected = 5.0;

        // Act
        double result = _calculatorService.Divide(a, b);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    /// <summary>
    /// Tests division with decimal result.
    /// Demonstrates floating-point result testing.
    /// </summary>
    [Test]
    public async Task Divide_WithDecimalResult_ReturnsCorrectQuotient()
    {
        // Arrange
        double a = 7.0;
        double b = 2.0;
        double expected = 3.5;

        // Act
        double result = _calculatorService.Divide(a, b);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    /// <summary>
    /// Tests division by zero throws exception.
    /// Demonstrates exception testing using Assert.Throws().
    /// </summary>
    [Test]
    public async Task Divide_ByZero_ThrowsDivideByZeroException()
    {
        // Arrange
        double a = 10.0;
        double b = 0.0;

        // Act & Assert
        await Assert.ThrowsAsync<DivideByZeroException>(() => Task.FromResult(_calculatorService.Divide(a, b)));
    }

    #endregion

    #region Input Validation Tests

    /// <summary>
    /// Tests that NaN input throws ArgumentException.
    /// Demonstrates exception testing for invalid inputs.
    /// </summary>
    [Test]
    public async Task Add_WithNaNInput_ThrowsArgumentException()
    {
        // Arrange
        double a = double.NaN;
        double b = 5.0;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => Task.FromResult(_calculatorService.Add(a, b)));
    }

    /// <summary>
    /// Tests that Infinity input throws ArgumentException.
    /// Demonstrates exception testing for infinity values.
    /// </summary>
    [Test]
    public async Task Multiply_WithInfinityInput_ThrowsArgumentException()
    {
        // Arrange
        double a = double.PositiveInfinity;
        double b = 2.0;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => Task.FromResult(_calculatorService.Multiply(a, b)));
    }

    /// <summary>
    /// Tests that negative infinity input throws ArgumentException.
    /// Demonstrates exception testing for negative infinity.
    /// </summary>
    [Test]
    public async Task Subtract_WithNegativeInfinityInput_ThrowsArgumentException()
    {
        // Arrange
        double a = 10.0;
        double b = double.NegativeInfinity;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => Task.FromResult(_calculatorService.Subtract(a, b)));
    }

    #endregion

    #region Assertion Pattern Demonstrations

    /// <summary>
    /// Demonstrates various assertion patterns available in TUnit.
    /// Shows different ways to verify test results.
    /// </summary>
    [Test]
    public async Task AssertionPatterns_Demonstration()
    {
        // Arrange
        double result = _calculatorService.Add(2.0, 3.0);

        // Demonstrate different assertion patterns
        await Assert.That(result).IsEqualTo(5.0);                    // Equality assertion
        await Assert.That(result).IsGreaterThan(4.0);                // Comparison assertion
        await Assert.That(result).IsLessThan(6.0);                   // Comparison assertion
        await Assert.That(result).IsGreaterThanOrEqualTo(5.0);       // Comparison assertion
        await Assert.That(result).IsLessThanOrEqualTo(5.0);          // Comparison assertion
        await Assert.That(result).IsNotEqualTo(4.0);                 // Inequality assertion
    }

    /// <summary>
    /// Demonstrates boolean assertion patterns.
    /// Shows how to test boolean conditions.
    /// </summary>
    [Test]
    public async Task BooleanAssertions_Demonstration()
    {
        // Arrange
        double positiveResult = _calculatorService.Add(2.0, 3.0);
        double negativeResult = _calculatorService.Subtract(2.0, 5.0);

        // Demonstrate boolean assertions
        await Assert.That(positiveResult > 0).IsTrue();              // Boolean true assertion
        await Assert.That(negativeResult > 0).IsFalse();             // Boolean false assertion
    }

    /// <summary>
    /// Demonstrates null and type assertion patterns.
    /// Shows how to verify object state and types.
    /// </summary>
    [Test]
    public async Task ObjectAssertions_Demonstration()
    {
        // Arrange
        var service = _calculatorService;
        CalculatorService? nullService = null;

        // Demonstrate object assertions
        await Assert.That(service).IsNotNull();                      // Not null assertion
        await Assert.That(nullService).IsNull();                     // Null assertion
        await Assert.That(service).IsTypeOf<CalculatorService>();    // Type assertion
    }

    #endregion
}