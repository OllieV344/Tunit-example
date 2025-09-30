# .NET 9 TUnit Example Project

A comprehensive example project demonstrating the key features and capabilities of TUnit, a modern testing framework for .NET. This project showcases practical examples of unit testing, parameterized testing, async testing, test lifecycle management, and advanced TUnit features through a simple console application with extensive test coverage.

## Table of Contents

- [Overview](#overview)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [TUnit Features Demonstrated](#tunit-features-demonstrated)
- [Running the Application](#running-the-application)
- [Running Tests](#running-tests)
- [Code Examples](#code-examples)
- [Extending the Examples](#extending-the-examples)
- [TUnit vs Other Testing Frameworks](#tunit-vs-other-testing-frameworks)
- [Contributing](#contributing)

## Overview

TUnit is a modern, fast, and feature-rich testing framework for .NET that provides excellent performance and developer experience. This project demonstrates TUnit's capabilities through:

- **Basic Unit Tests**: Simple test methods with fundamental assertions
- **Parameterized Tests**: Data-driven testing with inline arguments and method data sources
- **Async Testing**: Comprehensive async/await testing patterns
- **Test Lifecycle**: Setup and teardown methods for test preparation and cleanup
- **Advanced Features**: Parallel execution, custom attributes, timeouts, and retries

The example application includes services for mathematical calculations, user management, and data processing, providing realistic scenarios for testing various TUnit features.

## Project Structure

```
DotNetTUnitExample/
├── src/
│   └── DotNetTUnitExample/              # Main console application
│       ├── Models/                      # Data models
│       │   ├── User.cs                  # User entity with validation
│       │   └── ProcessingResult.cs      # Generic result wrapper
│       ├── Services/                    # Business logic services
│       │   ├── CalculatorService.cs     # Mathematical operations
│       │   ├── UserService.cs           # User management (async)
│       │   └── DataProcessingService.cs # Data processing operations
│       ├── Program.cs                   # Application entry point
│       └── DotNetTUnitExample.csproj    # Project file
├── tests/
│   └── DotNetTUnitExample.Tests/        # TUnit test project
│       ├── BasicTests.cs                # Fundamental TUnit features
│       ├── ParameterizedTests.cs        # Data-driven testing
│       ├── AsyncTests.cs                # Async testing patterns
│       ├── LifecycleTests.cs            # Setup/teardown examples
│       ├── AdvancedFeatureTests.cs      # Advanced TUnit capabilities
│       ├── TestData/                    # Test data sources
│       │   └── TestDataSources.cs       # Method data sources
│       └── DotNetTUnitExample.Tests.csproj # Test project file
├── DotNetTUnitExample.sln               # Solution file
└── README.md                            # This documentation
```

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- IDE with .NET support (Visual Studio, VS Code, JetBrains Rider, etc.)
- Basic understanding of C# and unit testing concepts

## Getting Started

### 1. Clone or Download the Project

```bash
git clone <repository-url>
cd DotNetTUnitExample
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Build the Solution

```bash
dotnet build
```

### 4. Run Tests

```bash
dotnet test
```

## TUnit Features Demonstrated

### 1. Basic Unit Testing (`BasicTests.cs`)

**Key Features:**
- `[Test]` attribute for test methods
- `Assert.That()` fluent assertion syntax
- Exception testing with `Assert.ThrowsAsync()`
- Various assertion patterns (equality, comparison, boolean, null checks)

**Example:**
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

### 2. Parameterized Testing (`ParameterizedTests.cs`)

**Key Features:**
- `[Arguments]` attribute for inline test parameters
- `[MethodDataSource]` for complex test data
- Testing with various data types (numbers, strings, arrays, objects)
- Data-driven exception testing

**Example:**
```csharp
[Test]
[Arguments(1.0, 2.0, 3.0)]
[Arguments(0.0, 5.0, 5.0)]
[Arguments(-3.0, 7.0, 4.0)]
public async Task Add_WithInlineArguments_ReturnsExpectedResult(double a, double b, double expected)
{
    // Act
    double result = _calculatorService.Add(a, b);

    // Assert
    await Assert.That(result).IsEqualTo(expected);
}
```

### 3. Async Testing (`AsyncTests.cs`)

**Key Features:**
- `async Task` test methods
- Proper async/await patterns in tests
- Async exception handling with `Assert.ThrowsAsync()`
- Performance and timing verification
- Concurrent operation testing

**Example:**
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

### 4. Test Lifecycle Management (`LifecycleTests.cs`)

**Key Features:**
- `[Before(Test)]` and `[After(Test)]` for test-level setup/cleanup
- `[Before(Class)]` and `[After(Class)]` for class-level setup/cleanup
- Resource management and test isolation
- Setup execution order verification

**Example:**
```csharp
[Before(Test)]
public async Task SetupBeforeEachTest()
{
    _testData.Clear();
    _setupExecutionOrder.Add("Before Test");
    await Task.CompletedTask;
}

[After(Test)]
public async Task CleanupAfterEachTest()
{
    _setupExecutionOrder.Add("After Test");
    await Task.CompletedTask;
}
```

### 5. Advanced Features (`AdvancedFeatureTests.cs`)

**Key Features:**
- `[Parallel]` attribute for concurrent test execution
- `[Timeout]` for test execution time limits
- `[Retry]` for handling flaky tests
- Custom test attributes and categorization
- Test filtering and selective execution

**Example:**
```csharp
[Test]
[Parallel]
[Timeout(5000)]
[Retry(3)]
public async Task ParallelTest_WithTimeout_ExecutesConcurrently()
{
    // Test implementation with parallel execution
}
```

## Running the Application

### Build and Run the Console Application

```bash
# Navigate to the main project directory
cd src/DotNetTUnitExample

# Run the application
dotnet run
```

The console application demonstrates the services being used and provides examples of the functionality being tested.

### Build Configuration

```bash
# Debug build (default)
dotnet build

# Release build
dotnet build --configuration Release
```

## Running Tests

### Run All Tests

```bash
# Run all tests in the solution
dotnet test

# Run tests with detailed output
dotnet test --verbosity normal

# Run tests with logger for better formatting
dotnet test --logger "console;verbosity=detailed"
```

### Run Specific Test Classes

```bash
# Run only basic tests
dotnet test --filter "FullyQualifiedName~BasicTests"

# Run only parameterized tests
dotnet test --filter "FullyQualifiedName~ParameterizedTests"

# Run only async tests
dotnet test --filter "FullyQualifiedName~AsyncTests"
```

### Run Tests with Coverage

```bash
# Install coverage tools (if not already installed)
dotnet tool install --global dotnet-reportgenerator-globaltool

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Generate coverage report
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage" -reporttypes:Html
```

### Performance Testing

```bash
# Run tests with performance profiling
dotnet test --logger trx --results-directory TestResults

# Run specific performance-related tests
dotnet test --filter "FullyQualifiedName~Timing"
```

## Detailed Feature Examples

### BasicTests.cs - Fundamental TUnit Features

This file demonstrates the core TUnit testing capabilities:

**Key TUnit Features Shown:**
- `[Test]` attribute for marking test methods
- `await Assert.That(actual).IsEqualTo(expected)` fluent assertion syntax
- `await Assert.ThrowsAsync<TException>()` for exception testing
- Various assertion types: `IsGreaterThan()`, `IsLessThan()`, `IsNotNull()`, `IsTrue()`, `IsFalse()`
- Exception handling patterns with `ArgumentException` and `DivideByZeroException`

**Example Pattern:**
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

### ParameterizedTests.cs - Data-Driven Testing

This file showcases TUnit's powerful parameterized testing capabilities:

**Key TUnit Features Shown:**
- `[Arguments(value1, value2, value3)]` for inline test parameters
- `[MethodDataSource(typeof(Class), nameof(Method))]` for complex test data
- Testing with multiple data types (numbers, strings, arrays, objects)
- Parameterized exception testing
- Complex object testing with `ProcessingResult<T>`

**Example Patterns:**
```csharp
// Inline arguments
[Test]
[Arguments(1.0, 2.0, 3.0)]
[Arguments(0.0, 5.0, 5.0)]
public async Task Add_WithInlineArguments_ReturnsExpectedResult(double a, double b, double expected)

// Method data source
[Test]
[MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.AdditionTestData))]
public async Task Add_WithMethodDataSource_ReturnsExpectedResult(double a, double b, double expected)
```

### AsyncTests.cs - Asynchronous Testing Patterns

This file demonstrates comprehensive async testing with TUnit:

**Key TUnit Features Shown:**
- `async Task` test methods with proper async/await patterns
- `await Assert.ThrowsAsync<TException>()` for async exception testing
- Testing async operations with timing verification
- Concurrent operation testing with `Task.WhenAll()`
- Complex async workflow testing with multiple operations

**Example Pattern:**
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

### LifecycleTests.cs - Setup and Teardown Patterns

This file shows TUnit's lifecycle management capabilities:

**Key TUnit Features Shown:**
- `[Before(Class)]` and `[After(Class)]` for class-level setup/cleanup
- `[Before(Test)]` and `[After(Test)]` for test-level setup/cleanup
- Resource management with `IDisposable` integration
- Execution order verification
- Test isolation patterns

**Example Pattern:**
```csharp
[Before(Test)]
public async Task TestSetup()
{
    _userService = new UserService();
    _testResource = new TestResource($"Resource-{_testSetupCounter}");
    await Task.CompletedTask;
}

[After(Test)]
public async Task TestCleanup()
{
    _testResource?.Dispose();
    await Task.CompletedTask;
}
```

### AdvancedFeatureTests.cs - Advanced TUnit Capabilities

This file demonstrates sophisticated TUnit features:

**Key TUnit Features Shown:**
- Custom test attributes for categorization (`[PerformanceTest]`, `[IntegrationTest]`)
- Manual timing verification with `Stopwatch` for performance testing
- Manual retry logic implementation for flaky test scenarios
- Test categorization and selective execution patterns
- Combining multiple advanced features in single tests

**Example Patterns:**
```csharp
// Custom attributes
[Test]
[PerformanceTest("ParallelProcessing", 500)]
[IntegrationTest("DataProcessing")]
public async Task AdvancedTest_WithCustomAttributes_WorksCorrectly()

// Manual timing verification
var stopwatch = Stopwatch.StartNew();
var result = await _service.ProcessAsync(data);
stopwatch.Stop();
await Assert.That(stopwatch.ElapsedMilliseconds).IsLessThan(1000);
```

### TestDataSources.cs - Test Data Management

This file shows how to organize and provide test data:

**Key TUnit Features Shown:**
- Static methods returning `IEnumerable<(T1, T2, T3)>` for test data
- Complex object test data with `ProcessingResult<T>`
- Edge case and boundary value test data
- Null and empty collection test data
- Reusable test data across multiple test classes

## Code Examples

### Creating a Basic Test

```csharp
[Test]
public async Task YourMethod_WithSpecificInput_ReturnsExpectedOutput()
{
    // Arrange - Set up test data and dependencies
    var service = new YourService();
    var input = "test input";
    var expected = "expected output";

    // Act - Execute the method being tested
    var result = service.YourMethod(input);

    // Assert - Verify the results
    await Assert.That(result).IsEqualTo(expected);
}
```

### Creating a Parameterized Test

```csharp
[Test]
[Arguments("input1", "output1")]
[Arguments("input2", "output2")]
[Arguments("input3", "output3")]
public async Task YourMethod_WithVariousInputs_ReturnsCorrectOutputs(string input, string expected)
{
    // Arrange
    var service = new YourService();

    // Act
    var result = service.YourMethod(input);

    // Assert
    await Assert.That(result).IsEqualTo(expected);
}
```

### Creating an Async Test

```csharp
[Test]
public async Task YourAsyncMethod_WithValidInput_CompletesSuccessfully()
{
    // Arrange
    var service = new YourAsyncService();
    var input = "test input";

    // Act
    var result = await service.YourAsyncMethod(input);

    // Assert
    await Assert.That(result.Success).IsTrue();
    await Assert.That(result.Data).IsNotNull();
}
```

### Testing Exceptions

```csharp
[Test]
public async Task YourMethod_WithInvalidInput_ThrowsException()
{
    // Arrange
    var service = new YourService();
    var invalidInput = null;

    // Act & Assert
    await Assert.ThrowsAsync<ArgumentNullException>(() => 
        Task.FromResult(service.YourMethod(invalidInput)));
}
```

## Extending the Examples

### Adding New Test Classes

1. **Create a new test class** in the `tests/DotNetTUnitExample.Tests/` directory:

```csharp
using TUnit.Core;

namespace DotNetTUnitExample.Tests;

public class YourNewTests
{
    [Test]
    public async Task YourTest_Description_ExpectedBehavior()
    {
        // Your test implementation
        await Assert.That(true).IsTrue();
    }
}
```

2. **Follow naming conventions**:
   - Test class: `[FeatureName]Tests`
   - Test method: `[MethodName]_[Scenario]_[ExpectedResult]`

### Adding New Services to Test

1. **Create a new service** in `src/DotNetTUnitExample/Services/`:

```csharp
namespace DotNetTUnitExample.Services;

public class YourNewService
{
    public string ProcessData(string input)
    {
        // Your business logic here
        return input.ToUpper();
    }
    
    public async Task<ProcessingResult<T>> ProcessAsync<T>(T data)
    {
        // Async business logic
        await Task.Delay(100);
        return ProcessingResult<T>.CreateSuccess(data, TimeSpan.FromMilliseconds(100));
    }
}
```

2. **Create corresponding tests**:

```csharp
public class YourNewServiceTests
{
    private readonly YourNewService _service;
    
    public YourNewServiceTests()
    {
        _service = new YourNewService();
    }
    
    [Test]
    public async Task ProcessData_WithValidInput_ReturnsExpectedOutput()
    {
        // Arrange
        var input = "test";
        var expected = "TEST";
        
        // Act
        var result = _service.ProcessData(input);
        
        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }
}

### Adding Method Data Sources

1. **Create data source methods** in `TestDataSources.cs`:

```csharp
public static IEnumerable<(string input, string expected)> YourTestData()
{
    yield return ("input1", "expected1");
    yield return ("input2", "expected2");
    // Add more test cases
}
```

2. **Use in parameterized tests**:

```csharp
[Test]
[MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.YourTestData))]
public async Task YourTest_WithMethodDataSource_WorksCorrectly(string input, string expected)
{
    // Test implementation
}
```

### Adding Custom Attributes

```csharp
// Create custom attribute
public class CategoryAttribute : Attribute
{
    public string Category { get; }
    
    public CategoryAttribute(string category)
    {
        Category = category;
    }
}

// Use in tests
[Test]
[Category("Integration")]
public async Task YourIntegrationTest_Scenario_ExpectedResult()
{
    // Test implementation
}
```

### Adding Setup and Teardown

```csharp
public class YourTestsWithLifecycle
{
    [Before(Class)]
    public static async Task ClassSetup()
    {
        // One-time setup for all tests in the class
        await Task.CompletedTask;
    }

    [Before(Test)]
    public async Task TestSetup()
    {
        // Setup before each test
        await Task.CompletedTask;
    }

    [After(Test)]
    public async Task TestCleanup()
    {
        // Cleanup after each test
        await Task.CompletedTask;
    }

    [After(Class)]
    public static async Task ClassCleanup()
    {
        // One-time cleanup after all tests in the class
        await Task.CompletedTask;
    }
}
```

## TUnit vs Other Testing Frameworks

### TUnit Advantages

- **Performance**: Faster test execution compared to traditional frameworks
- **Modern Syntax**: Clean, fluent assertion syntax with async support
- **Built-in Parallelization**: Easy parallel test execution
- **Rich Assertions**: Comprehensive assertion library
- **Flexible Data Sources**: Multiple ways to provide test data

### Migration from Other Frameworks

**From NUnit:**
- Replace `[Test]` → `[Test]` (same attribute name)
- Replace `Assert.AreEqual()` → `await Assert.That(actual).IsEqualTo(expected)`
- Replace `[TestCase]` → `[Arguments]`
- Replace `[TestCaseSource]` → `[MethodDataSource]`

**From xUnit:**
- Replace `[Fact]` → `[Test]`
- Replace `[Theory]` + `[InlineData]` → `[Test]` + `[Arguments]`
- Replace `Assert.Equal()` → `await Assert.That(actual).IsEqualTo(expected)`

**From MSTest:**
- Replace `[TestMethod]` → `[Test]`
- Replace `[DataRow]` → `[Arguments]`
- Replace `Assert.AreEqual()` → `await Assert.That(actual).IsEqualTo(expected)`

## Contributing

### Adding New Examples

1. **Identify a TUnit feature** not currently demonstrated
2. **Create or extend a service** in the main application to provide testable functionality
3. **Add comprehensive tests** demonstrating the feature
4. **Update documentation** to include the new examples
5. **Add code comments** explaining the TUnit features being used

### TUnit-Specific Best Practices

#### Assertion Patterns
- Always use `await Assert.That(actual).IsEqualTo(expected)` instead of traditional assertion methods
- Leverage TUnit's fluent assertion API for better readability
- Use specific assertion methods like `IsGreaterThan()`, `Contains()`, `IsNotNull()` for clearer intent

#### Async Testing
- All test methods should be `async Task` even for synchronous operations being tested
- Use `await Assert.ThrowsAsync<TException>()` for async exception testing
- Use `Task.FromResult()` when wrapping synchronous operations for exception testing

#### Parameterized Testing
- Prefer `[Arguments]` for simple inline data
- Use `[MethodDataSource]` for complex test data or when data needs to be computed
- Create dedicated `TestDataSources` classes for reusable test data

#### Lifecycle Management
- Use `[Before(Test)]` and `[After(Test)]` for test-level setup/cleanup
- Use `[Before(Class)]` and `[After(Class)]` for expensive setup that can be shared
- Implement `IDisposable` when managing resources that need explicit cleanup

#### Performance and Timing
- Use `Stopwatch` for manual timing verification in performance tests
- Create custom attributes like `[PerformanceTest]` for categorizing timing-sensitive tests
- Test both success scenarios and timeout/performance edge cases

### Code Style Guidelines

- Use descriptive test method names following the pattern: `MethodName_Scenario_ExpectedResult`
- Include XML documentation comments for all public methods
- Group related tests using regions (`#region` / `#endregion`)
- Use async/await consistently in test methods
- Provide meaningful assertion messages where helpful

### Testing Guidelines

- Each test should focus on a single behavior or scenario
- Use the Arrange-Act-Assert pattern consistently
- Include both positive and negative test cases
- Test edge cases and boundary conditions
- Ensure tests are independent and can run in any order

## Troubleshooting

### Common Issues and Solutions

#### Test Discovery Issues
**Problem:** Tests not appearing in test explorer or `dotnet test` not finding tests.
**Solution:** 
- Ensure test methods are marked with `[Test]` attribute
- Verify test classes are public
- Check that TUnit package reference is correct in `.csproj`
- Rebuild the solution: `dotnet clean && dotnet build`

#### Assertion Failures
**Problem:** Assertions failing with unclear error messages.
**Solution:**
- Use specific assertion methods: `IsEqualTo()`, `IsGreaterThan()`, etc.
- Add descriptive messages to assertions when needed
- Use `IsEquivalentTo()` for collection comparisons instead of `IsEqualTo()`

#### Async Test Issues
**Problem:** Async tests hanging or not completing.
**Solution:**
- Ensure all test methods are `async Task`
- Use `await` with all async operations
- Use `await Assert.ThrowsAsync<T>()` for async exception testing
- Avoid `Task.Result` or `.Wait()` in test code

#### Parameterized Test Problems
**Problem:** Method data source not providing data or causing compilation errors.
**Solution:**
- Ensure data source methods are `static` and return `IEnumerable<T>`
- Use correct tuple syntax: `(param1, param2, param3)`
- Verify method names match exactly in `[MethodDataSource]` attribute
- Check that data source class is accessible from test class

#### Performance Test Failures
**Problem:** Performance tests failing inconsistently.
**Solution:**
- Use reasonable timeout values accounting for CI/CD environment overhead
- Implement manual timing verification with `Stopwatch`
- Consider using relative performance comparisons rather than absolute timings
- Run performance tests multiple times to establish baseline

#### Lifecycle Method Issues
**Problem:** Setup/teardown methods not executing or executing in wrong order.
**Solution:**
- Ensure lifecycle methods are properly attributed with `[Before(Test)]`, `[After(Test)]`, etc.
- Make class-level lifecycle methods `static`
- Verify method signatures match expected patterns
- Check for exceptions in lifecycle methods that might prevent execution

### Debugging Tips

1. **Use Detailed Logging:** Add `--verbosity detailed` to `dotnet test` command
2. **Run Single Tests:** Use `--filter` to isolate problematic tests
3. **Check Test Output:** Use `--logger console` for better test output formatting
4. **Verify Dependencies:** Ensure all required packages are installed and up-to-date
5. **Clean and Rebuild:** Use `dotnet clean && dotnet restore && dotnet build` to resolve build issues

### Performance Optimization

1. **Parallel Execution:** TUnit supports parallel test execution by default
2. **Resource Management:** Properly dispose of resources in lifecycle methods
3. **Test Data:** Use efficient test data generation methods
4. **Async Operations:** Prefer async operations for I/O bound test scenarios
5. **Test Isolation:** Ensure tests don't share mutable state

---

## Additional Resources

- [TUnit Documentation](https://github.com/thomhurst/TUnit) - Official TUnit documentation
- [.NET Testing Best Practices](https://docs.microsoft.com/en-us/dotnet/core/testing/) - Microsoft's testing guidelines
- [C# Async/Await Best Practices](https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming) - Async programming guidance

This project serves as a comprehensive reference for implementing TUnit tests in .NET applications. The examples progress from basic concepts to advanced features, providing a solid foundation for adopting TUnit in your own projects.