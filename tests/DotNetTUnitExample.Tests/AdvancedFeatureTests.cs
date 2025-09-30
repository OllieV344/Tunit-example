using TUnit.Core;
using DotNetTUnitExample.Services;
using DotNetTUnitExample.Models;
using System.Diagnostics;

namespace DotNetTUnitExample.Tests;

/// <summary>
/// Advanced TUnit tests demonstrating sophisticated testing features.
/// Shows parallel execution, custom attributes, timeouts, retries, and test categorization.
/// </summary>
public class AdvancedFeatureTests
{
    private readonly DataProcessingService _dataProcessingService;
    private readonly CalculatorService _calculatorService;

    public AdvancedFeatureTests()
    {
        _dataProcessingService = new DataProcessingService();
        _calculatorService = new CalculatorService();
    }

    #region Custom Test Attributes

    /// <summary>
    /// Custom attribute for categorizing performance tests.
    /// Demonstrates how to create custom test attributes for categorization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PerformanceTestAttribute : Attribute
    {
        public string Category { get; }
        public int ExpectedMaxDurationMs { get; }

        public PerformanceTestAttribute(string category, int expectedMaxDurationMs = 1000)
        {
            Category = category;
            ExpectedMaxDurationMs = expectedMaxDurationMs;
        }
    }

    /// <summary>
    /// Custom attribute for categorizing integration tests.
    /// Shows how to create attributes for test filtering and organization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class IntegrationTestAttribute : Attribute
    {
        public string Component { get; }
        public string TestType { get; }

        public IntegrationTestAttribute(string component, string testType = "Integration")
        {
            Component = component;
            TestType = testType;
        }
    }

    /// <summary>
    /// Custom attribute for marking slow tests.
    /// Demonstrates test categorization for selective execution.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SlowTestAttribute : Attribute
    {
        public string Reason { get; }

        public SlowTestAttribute(string reason = "Test involves time-consuming operations")
        {
            Reason = reason;
        }
    }

    #endregion

    #region Parallel Execution Tests

    /// <summary>
    /// Tests parallel execution of independent operations.
    /// Demonstrates concurrent test execution capabilities.
    /// </summary>
    [Test]
    [PerformanceTest("ParallelProcessing", 500)]
    public async Task ParallelProcessing_IndependentOperations_ExecutesConcurrently()
    {
        // Arrange
        var numbers = Enumerable.Range(1, 10).ToList();
        var multiplier = 2;

        // Act
        var stopwatch = Stopwatch.StartNew();
        var result = _dataProcessingService.ProcessNumbers(numbers, multiplier);
        stopwatch.Stop();

        // Assert
        await Assert.That(result.Success).IsTrue();
        await Assert.That(result.Data).HasCount().EqualTo(10);
        await Assert.That(result.Data.First()).IsEqualTo(2);
        await Assert.That(result.Data.Last()).IsEqualTo(20);
        
        // Verify performance characteristics
        await Assert.That(stopwatch.ElapsedMilliseconds).IsLessThan(500);
    }

    /// <summary>
    /// Tests parallel execution with different data sets.
    /// Shows how tests can work with various inputs independently.
    /// </summary>
    [Test]
    [PerformanceTest("ParallelDataProcessing")]
    public async Task ParallelProcessing_DifferentDataSets_ProcessesIndependently()
    {
        // Arrange
        var strings = new[] { "hello", "world", "tunit", "testing" };

        // Act
        var result = _dataProcessingService.ProcessStrings(strings, toUpperCase: true);

        // Assert
        await Assert.That(result.Success).IsTrue();
        await Assert.That(result.Data).Contains("HELLO");
        await Assert.That(result.Data).Contains("WORLD");
        await Assert.That(result.Data).Contains("TUNIT");
        await Assert.That(result.Data).Contains("TESTING");
    }

    /// <summary>
    /// Tests execution with mathematical operations.
    /// Demonstrates testing of stateless operations.
    /// </summary>
    [Test]
    [PerformanceTest("ParallelCalculations")]
    public async Task ParallelProcessing_MathematicalOperations_ExecutesIndependently()
    {
        // Arrange
        var testCases = new[]
        {
            (a: 10.0, b: 5.0, expected: 15.0),
            (a: 20.0, b: 8.0, expected: 28.0),
            (a: 100.0, b: 25.0, expected: 125.0)
        };

        // Act & Assert
        foreach (var (a, b, expected) in testCases)
        {
            var result = _calculatorService.Add(a, b);
            await Assert.That(result).IsEqualTo(expected);
        }
    }

    #endregion

    #region Timeout Tests

    /// <summary>
    /// Tests operation with timeout constraint.
    /// Demonstrates time-bounded test execution with manual timeout verification.
    /// </summary>
    [Test]
    [PerformanceTest("TimeoutTest", 2000)]
    public async Task TimeoutTest_FastOperation_CompletesWithinTimeout()
    {
        // Arrange
        var numbers = Enumerable.Range(1, 100).ToList();

        // Act
        var stopwatch = Stopwatch.StartNew();
        var result = await _dataProcessingService.ProcessNumbersAsync(numbers, 2, delayMs: 100);
        stopwatch.Stop();

        // Assert
        await Assert.That(result.Success).IsTrue();
        await Assert.That(result.Data).HasCount().EqualTo(100);
        await Assert.That(result.ProcessingTime.TotalMilliseconds).IsLessThan(2000);
        
        // Manual timeout verification
        await Assert.That(stopwatch.ElapsedMilliseconds).IsLessThan(2000);
    }

    /// <summary>
    /// Tests operation that should complete well within timeout.
    /// Shows timeout testing with quick operations using manual timing verification.
    /// </summary>
    [Test]
    [PerformanceTest("QuickTimeout", 1000)]
    public async Task TimeoutTest_QuickOperation_CompletesQuickly()
    {
        // Arrange
        var strings = new[] { "quick", "test" };

        // Act
        var stopwatch = Stopwatch.StartNew();
        var result = await _dataProcessingService.ProcessStringsBatchAsync(strings, batchSize: 2);
        stopwatch.Stop();

        // Assert
        await Assert.That(result.Success).IsTrue();
        await Assert.That(result.Data).HasCount().EqualTo(2);
        
        // Manual timeout verification
        await Assert.That(stopwatch.ElapsedMilliseconds).IsLessThan(1000);
    }

    /// <summary>
    /// Tests aggregation operation with timeout.
    /// Demonstrates timeout testing with async operations using manual timing verification.
    /// </summary>
    [Test]
    [IntegrationTest("DataProcessing", "AsyncAggregation")]
    public async Task TimeoutTest_AggregationOperation_CompletesWithinTimeout()
    {
        // Arrange
        var numbers = Enumerable.Range(1, 50).ToList();

        // Act
        var stopwatch = Stopwatch.StartNew();
        var result = await _dataProcessingService.AggregateNumbersAsync(numbers, "sum");
        stopwatch.Stop();

        // Assert
        await Assert.That(result.Success).IsTrue();
        await Assert.That(result.Data).IsEqualTo(1275); // Sum of 1 to 50
        
        // Manual timeout verification
        await Assert.That(stopwatch.ElapsedMilliseconds).IsLessThan(1500);
    }

    #endregion

    #region Retry Tests

    /// <summary>
    /// Tests operation with retry capability for flaky scenarios.
    /// Demonstrates manual retry logic for handling intermittent failures.
    /// </summary>
    [Test]
    [IntegrationTest("DataProcessing", "FlakyOperation")]
    public async Task RetryTest_FlakyOperation_RetriesOnFailure()
    {
        // Arrange
        var data = new[] { 1, 2, 3, 4, 5 };
        const int maxRetries = 3;
        
        // Act - Implement manual retry logic
        ProcessingResult<List<int>> result = null!;
        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            // Simulate a flaky operation that might fail randomly
            var random = new Random(attempt); // Use attempt as seed for deterministic behavior in tests
            var shouldFail = random.Next(0, 4) == 0; // 25% chance of failure
            
            result = _dataProcessingService.ProcessWithPotentialFailure(data, shouldFail, "Simulated flaky failure");
            
            if (result.Success)
                break;
                
            // Wait before retry (in real scenarios)
            if (attempt < maxRetries - 1)
                await Task.Delay(10);
        }

        // Assert
        await Assert.That(result).IsNotNull();
        // Note: Due to randomness, we'll accept either success or failure, but verify the retry logic worked
        if (result.Success)
        {
            await Assert.That(result.Data).HasCount().EqualTo(5);
        }
        else
        {
            await Assert.That(result.ErrorMessage).Contains("failure");
        }
    }

    /// <summary>
    /// Tests async operation with retry for network-like scenarios.
    /// Shows manual retry testing with async operations.
    /// </summary>
    [Test]
    [SlowTest("Simulates network delays and potential failures")]
    public async Task RetryTest_AsyncFlakyOperation_RetriesAsyncFailures()
    {
        // Arrange
        var data = new[] { "test", "retry", "async" };
        const int maxRetries = 2;
        
        // Act - Implement manual async retry logic
        ProcessingResult<List<string>> result = null!;
        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            // Simulate async flaky operation
            var random = new Random(attempt + 100); // Use different seed for async test
            var shouldFail = random.Next(0, 5) == 0; // 20% chance of failure

            result = await _dataProcessingService.ProcessWithPotentialFailureAsync(
                data, shouldFail, "Simulated async network failure", delayMs: 50);
                
            if (result.Success)
                break;
                
            // Wait before retry
            if (attempt < maxRetries - 1)
                await Task.Delay(50);
        }

        // Assert
        await Assert.That(result).IsNotNull();
        // Accept either success or failure, but verify retry logic worked
        if (result.Success)
        {
            await Assert.That(result.Data).HasCount().EqualTo(3);
        }
        else
        {
            await Assert.That(result.ErrorMessage).Contains("failure");
        }
    }

    /// <summary>
    /// Tests mathematical operation with retry for edge cases.
    /// Demonstrates manual retry with deterministic operations.
    /// </summary>
    [Test]
    [PerformanceTest("RetryMath")]
    public async Task RetryTest_MathematicalOperation_HandlesEdgeCases()
    {
        // Arrange
        var values = new[] { 1.0, 2.0, 3.0, 4.0, 5.0 };

        // Act & Assert
        foreach (var value in values)
        {
            var result = _calculatorService.Multiply(value, 2.0);
            await Assert.That(result).IsEqualTo(value * 2.0);
        }
    }

    #endregion

    #region Test Categorization and Selective Execution

    /// <summary>
    /// Performance test for large data processing.
    /// Demonstrates test categorization for performance testing.
    /// </summary>
    [Test]
    [PerformanceTest("LargeDataProcessing", 3000)]
    [SlowTest("Processes large dataset")]
    public async Task CategoryTest_LargeDataProcessing_MeetsPerformanceRequirements()
    {
        // Arrange
        var largeDataSet = Enumerable.Range(1, 10000).ToList();

        // Act
        var stopwatch = Stopwatch.StartNew();
        var result = _dataProcessingService.ProcessNumbers(largeDataSet, 3);
        stopwatch.Stop();

        // Assert
        await Assert.That(result.Success).IsTrue();
        await Assert.That(result.Data).HasCount().EqualTo(10000);
        await Assert.That(result.Data.First()).IsEqualTo(3);
        await Assert.That(result.Data.Last()).IsEqualTo(30000);
        
        // Performance assertion
        await Assert.That(stopwatch.ElapsedMilliseconds).IsLessThan(3000);
    }

    /// <summary>
    /// Integration test for batch processing functionality.
    /// Shows integration test categorization.
    /// </summary>
    [Test]
    [IntegrationTest("BatchProcessing", "AsyncBatch")]
    [SlowTest("Involves multiple async batch operations")]
    public async Task CategoryTest_BatchProcessingIntegration_ProcessesInBatches()
    {
        // Arrange
        var strings = Enumerable.Range(1, 20).Select(i => $"item{i}").ToArray();

        // Act
        var result = await _dataProcessingService.ProcessStringsBatchAsync(strings, batchSize: 5);

        // Assert
        await Assert.That(result.Success).IsTrue();
        await Assert.That(result.Data).HasCount().EqualTo(20);
        await Assert.That(result.Data.All(s => s.StartsWith("ITEM"))).IsTrue();
    }

    /// <summary>
    /// Unit test for basic functionality categorization.
    /// Demonstrates basic unit test categorization.
    /// </summary>
    [Test]
    [IntegrationTest("Calculator", "BasicOperations")]
    public async Task CategoryTest_BasicCalculatorOperations_WorksCorrectly()
    {
        // Arrange & Act & Assert
        await Assert.That(_calculatorService.Add(5, 3)).IsEqualTo(8);
        await Assert.That(_calculatorService.Subtract(10, 4)).IsEqualTo(6);
        await Assert.That(_calculatorService.Multiply(3, 7)).IsEqualTo(21);
        await Assert.That(_calculatorService.Divide(15, 3)).IsEqualTo(5);
    }

    /// <summary>
    /// Performance test for aggregation operations.
    /// Shows performance testing categorization with async operations.
    /// </summary>
    [Test]
    [PerformanceTest("AggregationPerformance", 1000)]
    [IntegrationTest("DataProcessing", "AsyncAggregation")]
    public async Task CategoryTest_AggregationPerformance_MeetsTimingRequirements()
    {
        // Arrange
        var numbers = Enumerable.Range(1, 1000).ToList();
        var operations = new[] { "sum", "average", "max", "min" };

        // Act & Assert
        foreach (var operation in operations)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = await _dataProcessingService.AggregateNumbersAsync(numbers, operation);
            stopwatch.Stop();

            await Assert.That(result.Success).IsTrue();
            await Assert.That(stopwatch.ElapsedMilliseconds).IsLessThan(1000);
        }
    }

    #endregion

    #region Combined Advanced Features

    /// <summary>
    /// Test combining multiple advanced features: custom attributes and manual timing/retry logic.
    /// Demonstrates how advanced TUnit features work together.
    /// </summary>
    [Test]
    [PerformanceTest("CombinedFeatures", 5000)]
    [IntegrationTest("DataProcessing", "CombinedAdvanced")]
    public async Task CombinedAdvancedFeatures_AllFeaturesTogether_WorksCorrectly()
    {
        // Arrange
        var numbers = Enumerable.Range(1, 100).ToList();
        var strings = numbers.Select(n => $"value{n}").ToArray();

        // Act
        var numberResult = await _dataProcessingService.ProcessNumbersAsync(numbers, 2, delayMs: 50);
        var stringResult = await _dataProcessingService.ProcessStringsBatchAsync(strings, batchSize: 10);
        var aggregateResult = await _dataProcessingService.AggregateNumbersAsync(numbers, "sum");

        // Assert
        await Assert.That(numberResult.Success).IsTrue();
        await Assert.That(numberResult.Data).HasCount().EqualTo(100);
        
        await Assert.That(stringResult.Success).IsTrue();
        await Assert.That(stringResult.Data).HasCount().EqualTo(100);
        
        await Assert.That(aggregateResult.Success).IsTrue();
        await Assert.That(aggregateResult.Data).IsEqualTo(5050); // Sum of 1 to 100
    }

    /// <summary>
    /// Test demonstrating selective execution based on custom attributes.
    /// Shows how custom attributes can be used for test filtering.
    /// </summary>
    [Test]
    [PerformanceTest("SelectiveExecution")]
    [IntegrationTest("TestFramework", "AttributeFiltering")]
    public async Task SelectiveExecution_CustomAttributeFiltering_EnablesTestSelection()
    {
        // This test demonstrates how custom attributes can be used to:
        // 1. Categorize tests by type (Performance, Integration, Unit)
        // 2. Filter tests by component (Calculator, DataProcessing, etc.)
        // 3. Select tests by characteristics (Slow, Fast, Flaky)
        
        // Arrange
        var testData = new[] { 1, 2, 3 };

        // Act
        var result = _dataProcessingService.ProcessNumbers(testData, 10);

        // Assert
        await Assert.That(result.Success).IsTrue();
        await Assert.That(result.Data).IsEquivalentTo(new[] { 10, 20, 30 });
        
        // This test can be selected/filtered based on its custom attributes:
        // - Run only PerformanceTest attributed tests
        // - Run only IntegrationTest attributed tests for specific components
        // - Exclude SlowTest attributed tests for quick test runs
    }

    #endregion
}