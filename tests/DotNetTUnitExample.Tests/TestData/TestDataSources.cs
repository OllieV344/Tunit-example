using DotNetTUnitExample.Models;

namespace DotNetTUnitExample.Tests.TestData;

/// <summary>
/// Static class providing test data sources for parameterized tests.
/// Demonstrates various ways to provide test data for TUnit parameterized tests.
/// </summary>
public static class TestDataSources
{
    #region Calculator Test Data

    /// <summary>
    /// Provides test data for addition operations with various number combinations.
    /// Returns tuples of (a, b, expected) for testing calculator addition.
    /// </summary>
    /// <returns>Enumerable of test data for addition tests</returns>
    public static IEnumerable<(double a, double b, double expected)> AdditionTestData()
    {
        yield return (1.0, 2.0, 3.0);
        yield return (0.0, 5.0, 5.0);
        yield return (-3.0, 7.0, 4.0);
        yield return (-5.0, -2.0, -7.0);
        yield return (10.5, 2.3, 12.8);
        yield return (100.0, -50.0, 50.0);
        yield return (0.1, 0.2, 0.30000000000000004); // Floating point precision
    }

    /// <summary>
    /// Provides test data for multiplication operations including edge cases.
    /// Returns tuples of (a, b, expected) for testing calculator multiplication.
    /// </summary>
    /// <returns>Enumerable of test data for multiplication tests</returns>
    public static IEnumerable<(double a, double b, double expected)> MultiplicationTestData()
    {
        yield return (2.0, 3.0, 6.0);
        yield return (0.0, 10.0, 0.0);
        yield return (-4.0, 5.0, -20.0);
        yield return (-3.0, -7.0, 21.0);
        yield return (1.5, 2.0, 3.0);
        yield return (10.0, 0.1, 1.0);
    }

    /// <summary>
    /// Provides test data for division operations with valid inputs.
    /// Returns tuples of (dividend, divisor, expected) for testing calculator division.
    /// </summary>
    /// <returns>Enumerable of test data for division tests</returns>
    public static IEnumerable<(double dividend, double divisor, double expected)> DivisionTestData()
    {
        yield return (10.0, 2.0, 5.0);
        yield return (15.0, 3.0, 5.0);
        yield return (-12.0, 4.0, -3.0);
        yield return (20.0, -5.0, -4.0);
        yield return (7.0, 2.0, 3.5);
        yield return (1.0, 4.0, 0.25);
    }

    /// <summary>
    /// Provides test data for operations that should throw exceptions.
    /// Returns tuples of (a, b, operationType) for testing exception scenarios.
    /// </summary>
    /// <returns>Enumerable of test data for exception tests</returns>
    public static IEnumerable<(double a, double b, string operation)> InvalidInputTestData()
    {
        yield return (double.NaN, 5.0, "Add");
        yield return (3.0, double.NaN, "Subtract");
        yield return (double.PositiveInfinity, 2.0, "Multiply");
        yield return (1.0, double.NegativeInfinity, "Divide");
        yield return (double.NaN, double.NaN, "Add");
    }

    #endregion

    #region Data Processing Test Data

    /// <summary>
    /// Provides test data for number processing operations.
    /// Returns tuples of (numbers, multiplier, expected) for testing data processing.
    /// </summary>
    /// <returns>Enumerable of test data for number processing tests</returns>
    public static IEnumerable<(int[] numbers, int multiplier, int[] expected)> NumberProcessingTestData()
    {
        yield return (new[] { 1, 2, 3 }, 2, new[] { 2, 4, 6 });
        yield return (new[] { 5, 10, 15 }, 3, new[] { 15, 30, 45 });
        yield return (new[] { -2, -4, 6 }, -1, new[] { 2, 4, -6 });
        yield return (new[] { 0, 1, -1 }, 5, new[] { 0, 5, -5 });
        yield return (new int[] { }, 10, new int[] { });
    }

    /// <summary>
    /// Provides test data for string processing operations.
    /// Returns tuples of (strings, toUpperCase, expected) for testing string processing.
    /// </summary>
    /// <returns>Enumerable of test data for string processing tests</returns>
    public static IEnumerable<(string[] strings, bool toUpperCase, string[] expected)> StringProcessingTestData()
    {
        yield return (new[] { "hello", "world" }, true, new[] { "HELLO", "WORLD" });
        yield return (new[] { "TEST", "Data" }, false, new[] { "test", "data" });
        yield return (new[] { "Mixed", "CASE", "text" }, true, new[] { "MIXED", "CASE", "TEXT" });
        yield return (new[] { "", "empty" }, true, new[] { "", "EMPTY" });
        yield return (new string[] { }, true, new string[] { });
    }

    /// <summary>
    /// Provides test data for number filtering operations.
    /// Returns tuples of (numbers, minValue, expected) for testing filtering logic.
    /// </summary>
    /// <returns>Enumerable of test data for filtering tests</returns>
    public static IEnumerable<(int[] numbers, int minValue, int[] expected)> FilteringTestData()
    {
        yield return (new[] { 1, 5, 10, 15, 20 }, 10, new[] { 10, 15, 20 });
        yield return (new[] { -5, -2, 0, 3, 7 }, 0, new[] { 0, 3, 7 });
        yield return (new[] { 100, 200, 300 }, 150, new[] { 200, 300 });
        yield return (new[] { 1, 2, 3 }, 5, new int[] { });
        yield return (new int[] { }, 0, new int[] { });
    }

    /// <summary>
    /// Provides test data for aggregation operations.
    /// Returns tuples of (numbers, operation, expected) for testing aggregation logic.
    /// </summary>
    /// <returns>Enumerable of test data for aggregation tests</returns>
    public static IEnumerable<(int[] numbers, string operation, double expected)> AggregationTestData()
    {
        yield return (new[] { 1, 2, 3, 4, 5 }, "sum", 15.0);
        yield return (new[] { 2, 4, 6, 8 }, "average", 5.0);
        yield return (new[] { 10, 5, 20, 15 }, "max", 20.0);
        yield return (new[] { 10, 5, 20, 15 }, "min", 5.0);
        yield return (new[] { -3, -1, -5 }, "sum", -9.0);
        yield return (new[] { 100 }, "average", 100.0);
    }

    #endregion

    #region Complex Object Test Data

    /// <summary>
    /// Provides test data for processing result scenarios.
    /// Returns ProcessingResult objects with various success and failure states.
    /// </summary>
    /// <returns>Enumerable of ProcessingResult test data</returns>
    public static IEnumerable<ProcessingResult<List<int>>> ProcessingResultTestData()
    {
        yield return ProcessingResult<List<int>>.CreateSuccess(new List<int> { 1, 2, 3 }, TimeSpan.FromMilliseconds(100));
        yield return ProcessingResult<List<int>>.CreateSuccess(new List<int>(), TimeSpan.FromMilliseconds(50));
        yield return ProcessingResult<List<int>>.CreateFailure("Test failure", TimeSpan.FromMilliseconds(200));
        yield return ProcessingResult<List<int>>.CreateFailure("Null input error", TimeSpan.FromMilliseconds(10));
    }

    /// <summary>
    /// Provides test data for boolean flag combinations.
    /// Returns tuples of (shouldFail, expectedSuccess) for testing conditional logic.
    /// </summary>
    /// <returns>Enumerable of boolean test data</returns>
    public static IEnumerable<(bool shouldFail, bool expectedSuccess)> BooleanFlagTestData()
    {
        yield return (false, true);
        yield return (true, false);
    }

    /// <summary>
    /// Provides test data for string validation scenarios.
    /// Returns tuples of (input, isValid, description) for testing string validation.
    /// </summary>
    /// <returns>Enumerable of string validation test data</returns>
    public static IEnumerable<(string input, bool isValid, string description)> StringValidationTestData()
    {
        yield return ("valid@email.com", true, "Valid email format");
        yield return ("", false, "Empty string");
        yield return (null, false, "Null string");
        yield return ("no-at-symbol", false, "Missing @ symbol");
        yield return ("test@", false, "Missing domain");
        yield return ("@domain.com", false, "Missing local part");
        yield return ("valid.email+tag@example.org", true, "Complex valid email");
    }

    #endregion

    #region Edge Case Test Data

    /// <summary>
    /// Provides test data for boundary value testing.
    /// Returns various boundary values for testing edge cases.
    /// </summary>
    /// <returns>Enumerable of boundary value test data</returns>
    public static IEnumerable<(int value, string category)> BoundaryValueTestData()
    {
        yield return (int.MinValue, "Minimum integer");
        yield return (int.MaxValue, "Maximum integer");
        yield return (0, "Zero");
        yield return (-1, "Negative one");
        yield return (1, "Positive one");
    }

    /// <summary>
    /// Provides test data for null and empty collection scenarios.
    /// Returns various collection states for testing null/empty handling.
    /// </summary>
    /// <returns>Enumerable of collection state test data</returns>
    public static IEnumerable<(int[]? collection, string description)> CollectionStateTestData()
    {
        yield return (null, "Null collection");
        yield return (new int[] { }, "Empty collection");
        yield return (new[] { 42 }, "Single item collection");
        yield return (new[] { 1, 2, 3, 4, 5 }, "Multiple items collection");
    }

    #endregion
}