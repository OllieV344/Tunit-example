# Design Document

## Overview

This design outlines a .NET 9 solution that demonstrates TUnit testing framework capabilities through a practical example application. The solution will consist of a main console application with business logic and a comprehensive test project showcasing TUnit's features including basic assertions, parameterized tests, async testing, lifecycle management, and advanced features.

## Architecture

The solution follows a standard .NET solution structure with clear separation between application code and tests:

```
DotNetTUnitExample/
├── src/
│   └── DotNetTUnitExample/
│       ├── DotNetTUnitExample.csproj
│       ├── Program.cs
│       ├── Services/
│       │   ├── CalculatorService.cs
│       │   ├── UserService.cs
│       │   └── DataProcessingService.cs
│       └── Models/
│           ├── User.cs
│           └── ProcessingResult.cs
├── tests/
│   └── DotNetTUnitExample.Tests/
│       ├── DotNetTUnitExample.Tests.csproj
│       ├── BasicTests.cs
│       ├── ParameterizedTests.cs
│       ├── AsyncTests.cs
│       ├── LifecycleTests.cs
│       ├── AdvancedFeatureTests.cs
│       └── TestData/
│           └── TestDataSources.cs
├── DotNetTUnitExample.sln
└── README.md
```

## Components and Interfaces

### Main Application Components

**CalculatorService**
- Provides basic mathematical operations (add, subtract, multiply, divide)
- Includes edge case handling (division by zero)
- Demonstrates simple business logic for testing

**UserService**
- Manages user operations (create, validate, update)
- Includes async operations for database simulation
- Demonstrates async testing scenarios

**DataProcessingService**
- Processes collections of data
- Includes both sync and async processing methods
- Demonstrates parameterized testing with various data sets

**Models**
- `User`: Simple user model with validation
- `ProcessingResult`: Result object for data processing operations

### Test Components

**BasicTests**
- Demonstrates fundamental TUnit syntax
- Shows basic assertions and test structure
- Covers simple pass/fail scenarios

**ParameterizedTests**
- Shows `[Arguments]` attribute usage
- Demonstrates `[MethodDataSource]` for complex parameter scenarios
- Covers various data types and edge cases

**AsyncTests**
- Tests async methods with proper await patterns
- Demonstrates async test lifecycle
- Covers async exception handling

**LifecycleTests**
- Shows `[Before(Test)]` and `[After(Test)]` attributes
- Demonstrates `[Before(Class)]` and `[After(Class)]` for class-level setup
- Covers resource management scenarios

**AdvancedFeatureTests**
- Demonstrates parallel test execution
- Shows custom attributes and test categorization
- Covers advanced TUnit features like test timeouts and retries

## Data Models

### User Model
```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public bool IsValid() => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Email);
}
```

### ProcessingResult Model
```csharp
public class ProcessingResult<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public string ErrorMessage { get; set; }
    public TimeSpan ProcessingTime { get; set; }
}
```

## Error Handling

### Application Error Handling
- Custom exceptions for business logic violations
- Proper async exception propagation
- Validation error handling with meaningful messages

### Test Error Handling
- TUnit assertion failures with descriptive messages
- Async test exception verification
- Timeout handling for long-running operations

## Testing Strategy

### Test Organization
- Separate test classes for different feature categories
- Logical grouping of related test methods
- Clear naming conventions for test methods

### TUnit Features Demonstrated

**Basic Testing**
- `[Test]` attribute for test methods
- `Assert.That()` for assertions
- `Assert.Throws()` for exception testing

**Parameterized Testing**
- `[Arguments(value1, value2)]` for inline parameters
- `[MethodDataSource(nameof(MethodName))]` for complex data
- `[ClassDataSource<DataClass>]` for reusable test data

**Async Testing**
- `async Task` test methods
- Proper awaiting of async operations
- `Assert.ThrowsAsync()` for async exception testing

**Lifecycle Management**
- `[Before(Test)]` for test setup
- `[After(Test)]` for test cleanup
- `[Before(Class)]` and `[After(Class)]` for class-level lifecycle

**Advanced Features**
- `[Parallel]` attribute for concurrent execution
- `[Timeout(milliseconds)]` for test timeouts
- `[Retry(count)]` for flaky test handling
- Custom attributes for test categorization

### Test Data Strategy
- Static test data for simple scenarios
- Method-based data sources for complex scenarios
- Realistic test data that demonstrates practical usage
- Edge cases and boundary conditions

### Assertion Strategy
- Fluent assertion syntax using TUnit's `Assert.That()`
- Descriptive assertion messages
- Multiple assertion types (equality, collections, exceptions)
- Custom assertion helpers where beneficial

## Implementation Approach

### Phase 1: Project Structure
- Create .NET 9 solution and projects
- Set up TUnit package references
- Establish basic project structure

### Phase 2: Core Application
- Implement basic services with testable methods
- Create simple models with validation
- Add both sync and async operations

### Phase 3: Basic Tests
- Implement fundamental TUnit test examples
- Show basic assertion patterns
- Demonstrate test structure and organization

### Phase 4: Advanced Test Features
- Add parameterized test examples
- Implement async test scenarios
- Show lifecycle management

### Phase 5: Advanced TUnit Features
- Demonstrate parallel execution
- Add custom attributes and categorization
- Show advanced testing patterns

### Phase 6: Documentation
- Create comprehensive README
- Add inline code comments
- Provide usage instructions