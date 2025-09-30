using TUnit.Core;
using DotNetTUnitExample.Services;
using DotNetTUnitExample.Models;
using DotNetTUnitExample.Tests.TestData;

namespace DotNetTUnitExample.Tests;

/// <summary>
/// Parameterized tests demonstrating TUnit's data-driven testing capabilities.
/// Shows various ways to provide test data including inline arguments and method data sources.
/// </summary>
public class ParameterizedTests
{
    private readonly CalculatorService _calculatorService;
    private readonly DataProcessingService _dataProcessingService;

    public ParameterizedTests()
    {
        _calculatorService = new CalculatorService();
        _dataProcessingService = new DataProcessingService();
    }

    #region Inline Arguments Tests

    /// <summary>
    /// Tests calculator addition with inline arguments.
    /// Demonstrates [Arguments] attribute usage with various data types.
    /// </summary>
    /// <param name="a">First number</param>
    /// <param name="b">Second number</param>
    /// <param name="expected">Expected result</param>
    [Test]
    [Arguments(1.0, 2.0, 3.0)]
    [Arguments(0.0, 5.0, 5.0)]
    [Arguments(-3.0, 7.0, 4.0)]
    [Arguments(-5.0, -2.0, -7.0)]
    [Arguments(10.5, 2.3, 12.8)]
    public async Task Add_WithInlineArguments_ReturnsExpectedResult(double a, double b, double expected)
    {
        // Act
        double result = _calculatorService.Add(a, b);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    /// <summary>
    /// Tests calculator multiplication with inline arguments.
    /// Demonstrates parameterized testing with negative numbers and zero.
    /// </summary>
    /// <param name="a">First number</param>
    /// <param name="b">Second number</param>
    /// <param name="expected">Expected result</param>
    [Test]
    [Arguments(2.0, 3.0, 6.0)]
    [Arguments(0.0, 10.0, 0.0)]
    [Arguments(-4.0, 5.0, -20.0)]
    [Arguments(-3.0, -7.0, 21.0)]
    [Arguments(1.5, 2.0, 3.0)]
    public async Task Multiply_WithInlineArguments_ReturnsExpectedResult(double a, double b, double expected)
    {
        // Act
        double result = _calculatorService.Multiply(a, b);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    /// <summary>
    /// Tests string processing with inline arguments of different types.
    /// Demonstrates parameterized testing with strings and booleans.
    /// </summary>
    /// <param name="input">Input string to process</param>
    /// <param name="toUpperCase">Whether to convert to uppercase</param>
    /// <param name="expected">Expected result</param>
    [Test]
    [Arguments("hello", true, "HELLO")]
    [Arguments("WORLD", false, "world")]
    [Arguments("MiXeD", true, "MIXED")]
    [Arguments("", true, "")]
    [Arguments("test123", false, "test123")]
    public async Task ProcessSingleString_WithInlineArguments_ReturnsExpectedResult(string input, bool toUpperCase, string expected)
    {
        // Arrange
        var inputs = new[] { input };

        // Act
        var result = _dataProcessingService.ProcessStrings(inputs, toUpperCase);

        // Assert
        await Assert.That(result.Success).IsTrue();
        await Assert.That(result.Data).IsNotNull();
        await Assert.That(result.Data!.First()).IsEqualTo(expected);
    }

    /// <summary>
    /// Tests number filtering with inline arguments.
    /// Demonstrates parameterized testing with arrays and integers.
    /// </summary>
    /// <param name="minValue">Minimum value for filtering</param>
    /// <param name="expectedCount">Expected count of filtered results</param>
    [Test]
    [Arguments(5, 3)]   // Numbers >= 5: [5, 10, 15]
    [Arguments(10, 2)]  // Numbers >= 10: [10, 15]
    [Arguments(20, 0)]  // Numbers >= 20: []
    [Arguments(0, 4)]   // Numbers >= 0: [1, 5, 10, 15]
    [Arguments(-5, 4)]  // Numbers >= -5: [1, 5, 10, 15]
    public async Task FilterNumbers_WithInlineArguments_ReturnsExpectedCount(int minValue, int expectedCount)
    {
        // Arrange
        var numbers = new[] { 1, 5, 10, 15 };

        // Act
        var result = _dataProcessingService.FilterNumbers(numbers, minValue);

        // Assert
        await Assert.That(result.Success).IsTrue();
        await Assert.That(result.Data).IsNotNull();
        await Assert.That(result.Data!.Count).IsEqualTo(expectedCount);
    }

    #endregion

    #region Method Data Source Tests

    /// <summary>
    /// Tests calculator addition using method data source.
    /// Demonstrates [MethodDataSource] attribute with complex test data.
    /// </summary>
    /// <param name="a">First number</param>
    /// <param name="b">Second number</param>
    /// <param name="expected">Expected result</param>
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.AdditionTestData))]
    public async Task Add_WithMethodDataSource_ReturnsExpectedResult(double a, double b, double expected)
    {
        // Act
        double result = _calculatorService.Add(a, b);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    /// <summary>
    /// Tests calculator multiplication using method data source.
    /// Demonstrates method data source with edge cases and boundary values.
    /// </summary>
    /// <param name="a">First number</param>
    /// <param name="b">Second number</param>
    /// <param name="expected">Expected result</param>
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.MultiplicationTestData))]
    public async Task Multiply_WithMethodDataSource_ReturnsExpectedResult(double a, double b, double expected)
    {
        // Act
        double result = _calculatorService.Multiply(a, b);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    /// <summary>
    /// Tests calculator division using method data source.
    /// Demonstrates method data source with decimal results.
    /// </summary>
    /// <param name="dividend">Number to divide</param>
    /// <param name="divisor">Number to divide by</param>
    /// <param name="expected">Expected result</param>
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.DivisionTestData))]
    public async Task Divide_WithMethodDataSource_ReturnsExpectedResult(double dividend, double divisor, double expected)
    {
        // Act
        double result = _calculatorService.Divide(dividend, divisor);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    /// <summary>
    /// Tests number processing using method data source with arrays.
    /// Demonstrates method data source with complex data types (arrays).
    /// </summary>
    /// <param name="numbers">Input numbers to process</param>
    /// <param name="multiplier">Multiplier to apply</param>
    /// <param name="expected">Expected processed numbers</param>
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.NumberProcessingTestData))]
    public async Task ProcessNumbers_WithMethodDataSource_ReturnsExpectedResult(int[] numbers, int multiplier, int[] expected)
    {
        // Act
        var result = _dataProcessingService.ProcessNumbers(numbers, multiplier);

        // Assert
        await Assert.That(result.Success).IsTrue();
        await Assert.That(result.Data).IsNotNull();
        await Assert.That(result.Data!.ToArray()).IsEquivalentTo(expected);
    }

    /// <summary>
    /// Tests string processing using method data source with string arrays.
    /// Demonstrates method data source with string collections and boolean parameters.
    /// </summary>
    /// <param name="strings">Input strings to process</param>
    /// <param name="toUpperCase">Whether to convert to uppercase</param>
    /// <param name="expected">Expected processed strings</param>
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.StringProcessingTestData))]
    public async Task ProcessStrings_WithMethodDataSource_ReturnsExpectedResult(string[] strings, bool toUpperCase, string[] expected)
    {
        // Act
        var result = _dataProcessingService.ProcessStrings(strings, toUpperCase);

        // Assert
        await Assert.That(result.Success).IsTrue();
        await Assert.That(result.Data).IsNotNull();
        await Assert.That(result.Data!.ToArray()).IsEquivalentTo(expected);
    }

    /// <summary>
    /// Tests number filtering using method data source.
    /// Demonstrates method data source with filtering logic and empty results.
    /// </summary>
    /// <param name="numbers">Input numbers to filter</param>
    /// <param name="minValue">Minimum value for filtering</param>
    /// <param name="expected">Expected filtered numbers</param>
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.FilteringTestData))]
    public async Task FilterNumbers_WithMethodDataSource_ReturnsExpectedResult(int[] numbers, int minValue, int[] expected)
    {
        // Act
        var result = _dataProcessingService.FilterNumbers(numbers, minValue);

        // Assert
        await Assert.That(result.Success).IsTrue();
        await Assert.That(result.Data).IsNotNull();
        await Assert.That(result.Data!.ToArray()).IsEquivalentTo(expected);
    }

    #endregion

    #region Exception Testing with Parameters

    /// <summary>
    /// Tests that invalid inputs throw appropriate exceptions.
    /// Demonstrates parameterized exception testing with method data source.
    /// </summary>
    /// <param name="a">First number (potentially invalid)</param>
    /// <param name="b">Second number (potentially invalid)</param>
    /// <param name="operation">Operation to test</param>
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.InvalidInputTestData))]
    public async Task CalculatorOperations_WithInvalidInputs_ThrowsArgumentException(double a, double b, string operation)
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => Task.FromResult(operation switch
        {
            "Add" => _calculatorService.Add(a, b),
            "Subtract" => _calculatorService.Subtract(a, b),
            "Multiply" => _calculatorService.Multiply(a, b),
            "Divide" => _calculatorService.Divide(a, b),
            _ => throw new InvalidOperationException($"Unknown operation: {operation}")
        }));
    }

    /// <summary>
    /// Tests division by zero with inline arguments.
    /// Demonstrates parameterized exception testing with inline arguments.
    /// </summary>
    /// <param name="dividend">Number to divide</param>
    [Test]
    [Arguments(10.0)]
    [Arguments(-5.0)]
    [Arguments(0.0)]
    [Arguments(100.5)]
    public async Task Divide_ByZero_ThrowsDivideByZeroException(double dividend)
    {
        // Arrange
        double divisor = 0.0;

        // Act & Assert
        await Assert.ThrowsAsync<DivideByZeroException>(() => Task.FromResult(_calculatorService.Divide(dividend, divisor)));
    }

    #endregion

    #region Async Method Testing with Parameters

    /// <summary>
    /// Tests async aggregation operations using method data source.
    /// Demonstrates parameterized testing with async methods and various operations.
    /// </summary>
    /// <param name="numbers">Input numbers to aggregate</param>
    /// <param name="operation">Aggregation operation to perform</param>
    /// <param name="expected">Expected aggregated result</param>
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.AggregationTestData))]
    public async Task AggregateNumbersAsync_WithMethodDataSource_ReturnsExpectedResult(int[] numbers, string operation, double expected)
    {
        // Act
        var result = await _dataProcessingService.AggregateNumbersAsync(numbers, operation);

        // Assert
        await Assert.That(result.Success).IsTrue();
        await Assert.That(result.Data).IsEqualTo(expected);
    }

    /// <summary>
    /// Tests async processing with potential failures using inline arguments.
    /// Demonstrates parameterized testing with async methods and boolean flags.
    /// </summary>
    /// <param name="shouldFail">Whether the operation should fail</param>
    /// <param name="expectedSuccess">Expected success state</param>
    [Test]
    [Arguments(false, true)]
    [Arguments(true, false)]
    public async Task ProcessWithPotentialFailureAsync_WithInlineArguments_ReturnsExpectedResult(bool shouldFail, bool expectedSuccess)
    {
        // Arrange
        var data = new[] { 1, 2, 3 };

        // Act
        var result = await _dataProcessingService.ProcessWithPotentialFailureAsync(data, shouldFail);

        // Assert
        await Assert.That(result.Success).IsEqualTo(expectedSuccess);
        
        if (expectedSuccess)
        {
            await Assert.That(result.Data).IsNotNull();
            await Assert.That(result.Data!.ToArray()).IsEquivalentTo(data);
        }
        else
        {
            await Assert.That(result.ErrorMessage).IsNotNull();
            await Assert.That(result.ErrorMessage).Contains("failure");
        }
    }

    #endregion

    #region Complex Object Testing with Parameters

    /// <summary>
    /// Tests processing results using method data source with complex objects.
    /// Demonstrates parameterized testing with complex object types.
    /// </summary>
    /// <param name="processingResult">ProcessingResult to test</param>
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.ProcessingResultTestData))]
    public async Task ProcessingResult_WithMethodDataSource_HasExpectedProperties(ProcessingResult<List<int>> processingResult)
    {
        // Assert
        await Assert.That(processingResult).IsNotNull();
        await Assert.That(processingResult.ProcessingTime).IsGreaterThan(TimeSpan.Zero);
        
        if (processingResult.Success)
        {
            await Assert.That(processingResult.Data).IsNotNull();
            await Assert.That(processingResult.ErrorMessage).IsEqualTo(string.Empty);
        }
        else
        {
            await Assert.That(processingResult.ErrorMessage).IsNotNull();
            await Assert.That(processingResult.ErrorMessage).IsNotEmpty();
        }
    }

    /// <summary>
    /// Tests boolean flag combinations using method data source.
    /// Demonstrates parameterized testing with boolean logic.
    /// </summary>
    /// <param name="shouldFail">Input flag for failure simulation</param>
    /// <param name="expectedSuccess">Expected success state</param>
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.BooleanFlagTestData))]
    public async Task BooleanLogic_WithMethodDataSource_ReturnsExpectedResult(bool shouldFail, bool expectedSuccess)
    {
        // Arrange
        var data = new[] { "test", "data" };

        // Act
        var result = _dataProcessingService.ProcessWithPotentialFailure(data, shouldFail);

        // Assert
        await Assert.That(result.Success).IsEqualTo(expectedSuccess);
    }

    #endregion

    #region Edge Case Testing with Parameters

    /// <summary>
    /// Tests boundary values using method data source.
    /// Demonstrates parameterized testing with edge cases and boundary conditions.
    /// </summary>
    /// <param name="value">Boundary value to test</param>
    /// <param name="category">Category description for the boundary value</param>
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.BoundaryValueTestData))]
    public async Task BoundaryValues_WithMethodDataSource_HandledCorrectly(int value, string category)
    {
        // Arrange
        var numbers = new[] { value };

        // Act
        var result = _dataProcessingService.ProcessNumbers(numbers, 1);

        // Assert
        await Assert.That(result.Success).IsTrue();
        await Assert.That(result.Data).IsNotNull();
        await Assert.That(result.Data!.First()).IsEqualTo(value);
        
        // Additional assertion based on category
        if (category.Contains("Zero"))
        {
            await Assert.That(result.Data.First()).IsEqualTo(0);
        }
        else if (category.Contains("Minimum"))
        {
            await Assert.That(result.Data.First()).IsEqualTo(int.MinValue);
        }
        else if (category.Contains("Maximum"))
        {
            await Assert.That(result.Data.First()).IsEqualTo(int.MaxValue);
        }
    }

    /// <summary>
    /// Tests collection state handling using method data source.
    /// Demonstrates parameterized testing with null and empty collections.
    /// </summary>
    /// <param name="collection">Collection to test (may be null or empty)</param>
    /// <param name="description">Description of the collection state</param>
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.CollectionStateTestData))]
    public async Task CollectionStates_WithMethodDataSource_HandledCorrectly(int[]? collection, string description)
    {
        // Act
        var result = _dataProcessingService.ProcessNumbers(collection, 2);

        // Assert based on collection state
        if (collection == null)
        {
            await Assert.That(result.Success).IsFalse();
            await Assert.That(result.ErrorMessage).Contains("null");
        }
        else if (collection.Length == 0)
        {
            await Assert.That(result.Success).IsTrue();
            await Assert.That(result.Data).IsNotNull();
            await Assert.That(result.Data!.Count).IsEqualTo(0);
        }
        else
        {
            await Assert.That(result.Success).IsTrue();
            await Assert.That(result.Data).IsNotNull();
            await Assert.That(result.Data!.Count).IsEqualTo(collection.Length);
        }
    }

    #endregion

    #region Mixed Parameter Types

    /// <summary>
    /// Tests with mixed parameter types using inline arguments.
    /// Demonstrates parameterized testing with various data types in a single test.
    /// </summary>
    /// <param name="stringValue">String parameter</param>
    /// <param name="intValue">Integer parameter</param>
    /// <param name="boolValue">Boolean parameter</param>
    /// <param name="expectedLength">Expected result length</param>
    [Test]
    [Arguments("test", 3, true, 4)]
    [Arguments("hello", 2, false, 5)]
    [Arguments("", 5, true, 0)]
    [Arguments("world", 1, false, 5)]
    public async Task MixedParameterTypes_WithInlineArguments_ProcessedCorrectly(string stringValue, int intValue, bool boolValue, int expectedLength)
    {
        // Arrange
        var strings = new[] { stringValue };

        // Act
        var stringResult = _dataProcessingService.ProcessStrings(strings, boolValue);
        var numberResult = _dataProcessingService.ProcessNumbers(new[] { intValue }, 1);

        // Assert
        await Assert.That(stringResult.Success).IsTrue();
        await Assert.That(stringResult.Data).IsNotNull();
        await Assert.That(stringResult.Data!.First().Length).IsEqualTo(expectedLength);
        
        await Assert.That(numberResult.Success).IsTrue();
        await Assert.That(numberResult.Data).IsNotNull();
        await Assert.That(numberResult.Data!.First()).IsEqualTo(intValue);
    }

    #endregion
}