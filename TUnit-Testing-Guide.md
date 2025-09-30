# TUnit Testing Framework Guide

This guide explains all the TUnit attributes, patterns, and testing features demonstrated in the .NET 9 TUnit Example project.

## Table of Contents

1. [Basic Test Attributes](#basic-test-attributes)
2. [Parameterized Testing](#parameterized-testing)
3. [Lifecycle Management](#lifecycle-management)
4. [Async Testing](#async-testing)
5. [Advanced Features](#advanced-features)
6. [Assertion Patterns](#assertion-patterns)
7. [Test Data Sources](#test-data-sources)
8. [Custom Attributes](#custom-attributes)
9. [Best Practices](#best-practices)

---

## Basic Test Attributes

### `[Test]`
The fundamental attribute that marks a method as a test case.

```csharp
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
```

**Key Points:**
- All test methods must be `async Task` and use `await` with assertions
- Follow the Arrange-Act-Assert (AAA) pattern
- Use descriptive method names that explain the scenario and expected outcome

---

## Parameterized Testing

### `[Arguments]` - Inline Test Data
Provides test data directly in the attribute for simple parameterized tests.

```csharp
[Test]
[Arguments(1.0, 2.0, 3.0)]
[Arguments(0.0, 5.0, 5.0)]
[Arguments(-3.0, 7.0, 4.0)]
[Arguments(-5.0, -2.0, -7.0)]
public async Task Add_WithInlineArguments_ReturnsExpectedResult(double a, double b, double expected)
{
    // Act
    double result = _calculatorService.Add(a, b);

    // Assert
    await Assert.That(result).IsEqualTo(expected);
}
```

**Use Cases:**
- Simple test data with primitive types
- Small number of test cases
- Quick parameterization without external data sources

### `[MethodDataSource]` - External Test Data
References a static method that provides test data for complex scenarios.

```csharp
[Test]
[MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.AdditionTestData))]
public async Task Add_WithMethodDataSource_ReturnsExpectedResult(double a, double b, double expected)
{
    // Act
    double result = _calculatorService.Add(a, b);

    // Assert
    await Assert.That(result).IsEqualTo(expected);
}
```

**Data Source Method Example:**
```csharp
public static IEnumerable<(double a, double b, double expected)> AdditionTestData()
{
    yield return (1.0, 2.0, 3.0);
    yield return (0.0, 5.0, 5.0);
    yield return (-3.0, 7.0, 4.0);
    // ... more test cases
}
```

**Use Cases:**
- Complex test data with multiple parameters
- Large datasets
- Reusable test data across multiple test methods
- Dynamic test data generation

---

## Lifecycle Management

### `[Before(Class)]` - Class Setup
Runs once before all tests in the class. Used for expensive setup operations.

```csharp
[Before(Class)]
public static async Task ClassSetup()
{
    // Initialize class-level resources
    // Database connections, external services, etc.
    await InitializeSharedResourcesAsync();
}
```

### `[After(Class)]` - Class Cleanup
Runs once after all tests in the class. Used for cleanup of class-level resources.

```csharp
[After(Class)]
public static async Task ClassCleanup()
{
    // Clean up class-level resources
    await CleanupSharedResourcesAsync();
}
```

### `[Before(Test)]` - Test Setup
Runs before each individual test method. Used for per-test initialization.

```csharp
[Before(Test)]
public async Task TestSetup()
{
    // Initialize fresh resources for each test
    _userService = new UserService();
    _testResource = new TestResource();
    await _testResource.InitializeAsync();
}
```

### `[After(Test)]` - Test Cleanup
Runs after each individual test method. Used for per-test cleanup.

```csharp
[After(Test)]
public async Task TestCleanup()
{
    // Clean up test-specific resources
    _testResource?.Dispose();
    await _userService.ClearDataAsync();
}
```

**Execution Order:**
1. `[Before(Class)]` - Once before all tests
2. `[Before(Test)]` - Before each test
3. Test Method - The actual test
4. `[After(Test)]` - After each test
5. `[After(Class)]` - Once after all tests

---

## Async Testing

### Async Test Methods
All TUnit test methods must be `async Task` and use `await` with assertions.

```csharp
[Test]
public async Task CreateUserAsync_WithValidData_ReturnsSuccessResult()
{
    // Arrange
    string name = "John Doe";
    string email = "john.doe@example.com";

    // Act
    var result = await _userService.CreateUserAsync(name, email);

    // Assert
    await Assert.That(result.Success).IsTrue();
    await Assert.That(result.Data).IsNotNull();
    await Assert.That(result.Data.Name).IsEqualTo(name);
}
```

### Async Exception Testing
Test that async methods throw expected exceptions.

```csharp
[Test]
public async Task CreateUserAsync_WithNullName_ThrowsArgumentException()
{
    // Arrange
    string? name = null;
    string email = "test@example.com";

    // Act & Assert
    await Assert.ThrowsAsync<ArgumentException>(() => 
        _userService.CreateUserAsync(name!, email));
}
```

### Concurrent Async Testing
Test multiple async operations running concurrently.

```csharp
[Test]
public async Task MultipleAsyncOperations_RunConcurrently()
{
    // Arrange
    var tasks = new[]
    {
        _userService.CreateUserAsync("User A", "usera@example.com"),
        _userService.CreateUserAsync("User B", "userb@example.com"),
        _userService.CreateUserAsync("User C", "userc@example.com")
    };
    
    // Act
    var results = await Task.WhenAll(tasks);
    
    // Assert
    await Assert.That(results.All(r => r.Success)).IsTrue();
}
```

---

## Advanced Features

### Performance Testing with Manual Timing
TUnit doesn't have built-in timeout attributes, but you can implement manual timing verification.

```csharp
[Test]
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
    await Assert.That(stopwatch.ElapsedMilliseconds).IsLessThan(2000);
}
```

### Manual Retry Logic
Implement retry patterns for testing flaky operations.

```csharp
[Test]
public async Task RetryTest_FlakyOperation_RetriesOnFailure()
{
    // Arrange
    var data = new[] { 1, 2, 3, 4, 5 };
    const int maxRetries = 3;
    
    // Act - Implement manual retry logic
    ProcessingResult<List<int>> result = null!;
    for (int attempt = 0; attempt < maxRetries; attempt++)
    {
        result = _dataProcessingService.ProcessWithPotentialFailure(data, shouldFail: false);
        
        if (result.Success)
            break;
            
        if (attempt < maxRetries - 1)
            await Task.Delay(10);
    }

    // Assert
    await Assert.That(result.Success).IsTrue();
}
```

### Parallel Test Execution
TUnit supports parallel test execution by default. Tests in different classes run concurrently.

```csharp
[Test]
public async Task ParallelProcessing_IndependentOperations_ExecutesConcurrently()
{
    // This test can run in parallel with other tests
    var result = _dataProcessingService.ProcessNumbers(numbers, multiplier);
    await Assert.That(result.Success).IsTrue();
}
```

---

## Assertion Patterns

### Basic Assertions
```csharp
// Equality
await Assert.That(result).IsEqualTo(expected);
await Assert.That(result).IsNotEqualTo(unexpected);

// Comparison
await Assert.That(value).IsGreaterThan(minimum);
await Assert.That(value).IsLessThan(maximum);
await Assert.That(value).IsGreaterThanOrEqualTo(minimum);
await Assert.That(value).IsLessThanOrEqualTo(maximum);

// Boolean
await Assert.That(condition).IsTrue();
await Assert.That(condition).IsFalse();

// Null checks
await Assert.That(object).IsNull();
await Assert.That(object).IsNotNull();

// Type checks
await Assert.That(object).IsTypeOf<ExpectedType>();
```

### Collection Assertions
```csharp
// Count
await Assert.That(collection).HasCount().EqualTo(expectedCount);

// Contains
await Assert.That(collection).Contains(expectedItem);

// Equivalence (order doesn't matter)
await Assert.That(actualArray).IsEquivalentTo(expectedArray);

// All items match condition
await Assert.That(collection.All(predicate)).IsTrue();
```

### String Assertions
```csharp
await Assert.That(text).StartsWith("prefix");
await Assert.That(text).Contains("substring");
await Assert.That(text).IsNotEmpty();
```

### Exception Assertions
```csharp
// Async exceptions
await Assert.ThrowsAsync<ExceptionType>(() => asyncMethod());

// Sync exceptions wrapped in Task
await Assert.ThrowsAsync<ExceptionType>(() => Task.FromResult(syncMethod()));
```

---

## Test Data Sources

### Simple Data Sources
For basic parameterized tests with primitive types.

```csharp
public static IEnumerable<(double a, double b, double expected)> AdditionTestData()
{
    yield return (1.0, 2.0, 3.0);
    yield return (0.0, 5.0, 5.0);
    yield return (-3.0, 7.0, 4.0);
}
```

### Complex Data Sources
For tests with complex objects or multiple parameters.

```csharp
public static IEnumerable<(int[] numbers, int multiplier, int[] expected)> NumberProcessingTestData()
{
    yield return (new[] { 1, 2, 3 }, 2, new[] { 2, 4, 6 });
    yield return (new[] { 5, 10, 15 }, 3, new[] { 15, 30, 45 });
    yield return (new int[] { }, 10, new int[] { });
}
```

### Edge Case Data Sources
For boundary value and edge case testing.

```csharp
public static IEnumerable<(int value, string category)> BoundaryValueTestData()
{
    yield return (int.MinValue, "Minimum integer");
    yield return (int.MaxValue, "Maximum integer");
    yield return (0, "Zero");
    yield return (-1, "Negative one");
    yield return (1, "Positive one");
}
```

### Null and Empty Data Sources
For testing null and empty scenarios.

```csharp
public static IEnumerable<(int[]? collection, string description)> CollectionStateTestData()
{
    yield return (null, "Null collection");
    yield return (new int[] { }, "Empty collection");
    yield return (new[] { 42 }, "Single item collection");
}
```

---

## Custom Attributes

### Creating Custom Test Attributes
You can create custom attributes for test categorization and metadata.

```csharp
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
```

### Using Custom Attributes
```csharp
[Test]
[PerformanceTest("ParallelProcessing", 500)]
[IntegrationTest("DataProcessing", "AsyncBatch")]
public async Task ComplexTest_WithCustomAttributes_WorksCorrectly()
{
    // Test implementation
}
```

**Benefits:**
- Test categorization and filtering
- Metadata for test reporting
- Custom test execution logic
- Documentation and organization

---

## Best Practices

### Test Organization
```csharp
public class CalculatorTests
{
    #region Addition Tests
    // Group related tests together
    #endregion

    #region Subtraction Tests
    // More related tests
    #endregion
}
```

### Naming Conventions
- **Method Names**: `MethodName_Scenario_ExpectedResult`
- **Test Classes**: `ClassNameTests` or `ClassNameTest`
- **Test Data**: Descriptive parameter names and clear expected values

### Resource Management
```csharp
public class ResourceTests : IDisposable
{
    private TestResource? _resource;

    [Before(Test)]
    public async Task Setup()
    {
        _resource = new TestResource();
        await _resource.InitializeAsync();
    }

    [After(Test)]
    public async Task Cleanup()
    {
        _resource?.Dispose();
    }

    public void Dispose()
    {
        _resource?.Dispose();
    }
}
```

### Async Best Practices
- Always use `async Task` for test methods
- Always `await` assertions
- Use `Task.WhenAll()` for concurrent operations
- Implement proper timeout verification manually

### Assertion Best Practices
- Use descriptive assertion messages when needed
- Test one concept per test method
- Use specific assertions (e.g., `IsEqualTo()` vs `IsTrue()`)
- Verify both positive and negative cases

### Data-Driven Testing
- Use `[Arguments]` for simple, few test cases
- Use `[MethodDataSource]` for complex or many test cases
- Create reusable data sources for common scenarios
- Include edge cases and boundary values

### Error Testing
- Test both expected exceptions and error result patterns
- Verify error messages contain expected information
- Test error recovery scenarios
- Use appropriate exception types in assertions

---

## Summary

TUnit provides a modern, async-first testing framework for .NET with:

- **Simple Syntax**: Clean, readable test methods with async support
- **Flexible Parameterization**: Multiple ways to provide test data
- **Lifecycle Management**: Comprehensive setup/teardown patterns
- **Advanced Features**: Performance testing, retry logic, parallel execution
- **Extensibility**: Custom attributes and test organization

The framework emphasizes:
- Async-first design with `await` assertions
- Clear test organization and naming
- Comprehensive parameterized testing
- Proper resource management
- Performance and timing verification

This makes TUnit an excellent choice for modern .NET applications requiring robust, maintainable test suites.