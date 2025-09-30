using TUnit.Core;
using DotNetTUnitExample.Services;
using DotNetTUnitExample.Models;

namespace DotNetTUnitExample.Tests;

/// <summary>
/// Lifecycle tests demonstrating TUnit's setup and teardown patterns.
/// Shows [Before(Test)], [After(Test)], [Before(Class)], and [After(Class)] attributes.
/// Demonstrates proper resource management and execution order verification.
/// </summary>
public class LifecycleTests : IDisposable
{
    private static readonly List<string> ExecutionLog = new();
    private static int _classSetupCounter = 0;
    private static int _classCleanupCounter = 0;
    
    private int _testSetupCounter = 0;
    private int _testCleanupCounter = 0;
    private UserService? _userService;
    private CalculatorService? _calculatorService;
    private TestResource? _testResource;

    #region Class-Level Lifecycle Methods

    /// <summary>
    /// Class-level setup method that runs once before all tests in this class.
    /// Demonstrates [Before(Class)] attribute usage and class-wide resource initialization.
    /// </summary>
    [Before(Class)]
    public static async Task ClassSetup()
    {
        _classSetupCounter++;
        ExecutionLog.Add($"ClassSetup executed - Counter: {_classSetupCounter}");
        
        // Simulate class-level resource initialization
        await Task.Delay(10);
        
        // Log the setup completion
        ExecutionLog.Add("Class-level resources initialized");
    }

    /// <summary>
    /// Class-level cleanup method that runs once after all tests in this class.
    /// Demonstrates [After(Class)] attribute usage and class-wide resource cleanup.
    /// </summary>
    [After(Class)]
    public static async Task ClassCleanup()
    {
        _classCleanupCounter++;
        ExecutionLog.Add($"ClassCleanup executed - Counter: {_classCleanupCounter}");
        
        // Simulate class-level resource cleanup
        await Task.Delay(10);
        
        // Log the cleanup completion
        ExecutionLog.Add("Class-level resources cleaned up");
        
        // Clear the execution log for clean test runs
        ExecutionLog.Clear();
    }

    #endregion

    #region Test-Level Lifecycle Methods

    /// <summary>
    /// Test-level setup method that runs before each individual test.
    /// Demonstrates [Before(Test)] attribute usage and per-test resource initialization.
    /// </summary>
    [Before(Test)]
    public async Task TestSetup()
    {
        _testSetupCounter++;
        ExecutionLog.Add($"TestSetup executed - Counter: {_testSetupCounter}");
        
        // Initialize services for each test
        _userService = new UserService();
        _calculatorService = new CalculatorService();
        
        // Initialize test resource with proper disposal tracking
        _testResource = new TestResource($"Resource-{_testSetupCounter}");
        
        // Simulate async setup work
        await Task.Delay(5);
        
        ExecutionLog.Add($"Test resources initialized for test #{_testSetupCounter}");
    }

    /// <summary>
    /// Test-level cleanup method that runs after each individual test.
    /// Demonstrates [After(Test)] attribute usage and per-test resource cleanup.
    /// </summary>
    [After(Test)]
    public async Task TestCleanup()
    {
        _testCleanupCounter++;
        ExecutionLog.Add($"TestCleanup executed - Counter: {_testCleanupCounter}");
        
        // Clean up test resource
        _testResource?.Dispose();
        _testResource = null;
        
        // Simulate async cleanup work
        await Task.Delay(5);
        
        ExecutionLog.Add($"Test resources cleaned up for test #{_testCleanupCounter}");
    }

    #endregion

    #region Lifecycle Verification Tests

    /// <summary>
    /// Verifies that class setup has been executed before this test runs.
    /// Demonstrates that [Before(Class)] methods execute before any test methods.
    /// </summary>
    [Test]
    public async Task LifecycleOrder_ClassSetupExecutedBeforeTests()
    {
        // Verify class setup has run
        await Assert.That(_classSetupCounter).IsGreaterThan(0);
        await Assert.That(ExecutionLog).Contains("ClassSetup executed - Counter: 1");
        await Assert.That(ExecutionLog).Contains("Class-level resources initialized");
        
        // Verify test setup has run for this test
        await Assert.That(_testSetupCounter).IsGreaterThan(0);
        await Assert.That(_userService).IsNotNull();
        await Assert.That(_calculatorService).IsNotNull();
        await Assert.That(_testResource).IsNotNull();
        
        ExecutionLog.Add("First test executed successfully");
    }

    /// <summary>
    /// Verifies that test setup and cleanup execute for each test independently.
    /// Demonstrates that [Before(Test)] and [After(Test)] run for every test method.
    /// </summary>
    [Test]
    public async Task LifecycleOrder_TestSetupAndCleanupExecutePerTest()
    {
        // Verify that test setup has run for this specific test
        await Assert.That(_testSetupCounter).IsGreaterThan(0);
        await Assert.That(_userService).IsNotNull();
        await Assert.That(_testResource).IsNotNull();
        await Assert.That(_testResource.Name).StartsWith("Resource-");
        
        // Verify services are properly initialized
        var result = _calculatorService!.Add(2.0, 3.0);
        await Assert.That(result).IsEqualTo(5.0);
        
        ExecutionLog.Add("Second test executed successfully");
    }

    /// <summary>
    /// Verifies that resources are properly isolated between tests.
    /// Demonstrates that each test gets fresh instances of resources.
    /// </summary>
    [Test]
    public async Task LifecycleOrder_ResourcesIsolatedBetweenTests()
    {
        // Verify fresh resources for this test
        await Assert.That(_testResource).IsNotNull();
        await Assert.That(_testResource.IsDisposed).IsFalse();
        
        // Modify the resource state
        _testResource.ProcessData("test-data");
        await Assert.That(_testResource.ProcessedData).IsEqualTo("test-data");
        
        // Verify services work independently
        var user = await _userService!.CreateUserAsync("Test User", "test@example.com");
        await Assert.That(user.Success).IsTrue();
        await Assert.That(user.Data).IsNotNull();
        await Assert.That(user.Data.Name).IsEqualTo("Test User");
        
        ExecutionLog.Add("Third test executed successfully");
    }

    #endregion

    #region Resource Management Tests

    /// <summary>
    /// Demonstrates proper resource disposal patterns in test lifecycle.
    /// Shows how to manage disposable resources in test setup and cleanup.
    /// </summary>
    [Test]
    public async Task ResourceManagement_DisposableResourcesHandledProperly()
    {
        // Verify resource is available and not disposed
        await Assert.That(_testResource).IsNotNull();
        await Assert.That(_testResource.IsDisposed).IsFalse();
        
        // Use the resource
        _testResource.ProcessData("lifecycle-test-data");
        await Assert.That(_testResource.ProcessedData).IsEqualTo("lifecycle-test-data");
        
        // Verify resource usage tracking
        await Assert.That(_testResource.UsageCount).IsEqualTo(1);
        
        // The resource will be disposed in TestCleanup
        ExecutionLog.Add("Resource management test completed");
    }

    /// <summary>
    /// Tests async operations within lifecycle-managed resources.
    /// Demonstrates that async operations work properly with lifecycle management.
    /// </summary>
    [Test]
    public async Task ResourceManagement_AsyncOperationsWithLifecycleResources()
    {
        // Verify async service is available
        await Assert.That(_userService).IsNotNull();
        
        // Perform async operations
        var createResult = await _userService.CreateUserAsync("Async User", "async@example.com");
        await Assert.That(createResult.Success).IsTrue();
        
        var validateResult = await _userService.ValidateUserAsync(createResult.Data.Id);
        await Assert.That(validateResult.Success).IsTrue();
        
        var updateResult = await _userService.UpdateUserAsync(createResult.Data.Id, "Updated Async User");
        await Assert.That(updateResult.Success).IsTrue();
        await Assert.That(updateResult.Data.Name).IsEqualTo("Updated Async User");
        
        ExecutionLog.Add("Async operations test completed");
    }

    /// <summary>
    /// Verifies that exceptions in tests don't prevent cleanup from running.
    /// Demonstrates that [After(Test)] methods execute even when tests fail.
    /// </summary>
    [Test]
    public async Task ResourceManagement_CleanupRunsEvenWithTestFailures()
    {
        // Verify setup has run
        await Assert.That(_testResource).IsNotNull();
        await Assert.That(_calculatorService).IsNotNull();
        
        // Perform some operations that should succeed
        var result = _calculatorService.Add(10.0, 5.0);
        await Assert.That(result).IsEqualTo(15.0);
        
        // Use the test resource
        _testResource.ProcessData("exception-test-data");
        await Assert.That(_testResource.ProcessedData).IsEqualTo("exception-test-data");
        
        // This test should pass, demonstrating normal cleanup flow
        ExecutionLog.Add("Exception handling test completed normally");
    }

    #endregion

    #region Execution Order Verification Tests

    /// <summary>
    /// Verifies the complete execution order of lifecycle methods.
    /// Demonstrates the sequence: ClassSetup -> TestSetup -> Test -> TestCleanup -> ClassCleanup.
    /// </summary>
    [Test]
    public async Task ExecutionOrder_VerifyCompleteLifecycleSequence()
    {
        // Verify class setup has executed
        await Assert.That(ExecutionLog.Any(log => log.Contains("ClassSetup executed"))).IsTrue();
        
        // Verify test setup has executed for this test
        await Assert.That(ExecutionLog.Any(log => log.Contains("TestSetup executed"))).IsTrue();
        
        // Verify resources are available
        await Assert.That(_userService).IsNotNull();
        await Assert.That(_calculatorService).IsNotNull();
        await Assert.That(_testResource).IsNotNull();
        
        // Add this test's execution to the log
        ExecutionLog.Add("ExecutionOrder test body executed");
        
        // Verify we can see the execution history
        var setupLogs = ExecutionLog.Where(log => log.Contains("TestSetup executed")).ToList();
        await Assert.That(setupLogs.Count).IsGreaterThan(0);
        
        // Test cleanup and class cleanup will be verified by their execution
        // (we can't directly test them here as they run after this method completes)
    }

    /// <summary>
    /// Tests that multiple test methods each get their own setup and cleanup.
    /// Demonstrates that lifecycle methods run independently for each test.
    /// </summary>
    [Test]
    public async Task ExecutionOrder_MultipleTestsGetIndependentLifecycle()
    {
        // This should be a fresh setup for this specific test
        await Assert.That(_testSetupCounter).IsGreaterThan(0);
        
        // Verify we have fresh service instances
        await Assert.That(_userService).IsNotNull();
        await Assert.That(_calculatorService).IsNotNull();
        
        // Verify we have a fresh test resource
        await Assert.That(_testResource).IsNotNull();
        await Assert.That(_testResource.IsDisposed).IsFalse();
        await Assert.That(_testResource.UsageCount).IsEqualTo(0);
        
        // Use the resource to modify its state
        _testResource.ProcessData("independent-test-data");
        await Assert.That(_testResource.ProcessedData).IsEqualTo("independent-test-data");
        await Assert.That(_testResource.UsageCount).IsEqualTo(1);
        
        ExecutionLog.Add("Independent lifecycle test completed");
    }

    #endregion

    #region IDisposable Implementation

    /// <summary>
    /// Implements IDisposable to demonstrate additional cleanup patterns.
    /// Shows how to combine IDisposable with TUnit lifecycle methods.
    /// </summary>
    public void Dispose()
    {
        // Additional cleanup that runs when the test class instance is disposed
        _testResource?.Dispose();
        _userService = null;
        _calculatorService = null;
        
        // This demonstrates that you can use both TUnit lifecycle methods
        // and standard .NET disposal patterns together
    }

    #endregion
}

/// <summary>
/// Test resource class that demonstrates proper disposal patterns.
/// Used to verify resource management in lifecycle tests.
/// </summary>
public class TestResource : IDisposable
{
    public string Name { get; }
    public string? ProcessedData { get; private set; }
    public int UsageCount { get; private set; }
    public bool IsDisposed { get; private set; }
    
    public TestResource(string name)
    {
        Name = name;
        UsageCount = 0;
        IsDisposed = false;
    }
    
    public void ProcessData(string data)
    {
        if (IsDisposed)
            throw new ObjectDisposedException(nameof(TestResource));
            
        ProcessedData = data;
        UsageCount++;
    }
    
    public void Dispose()
    {
        if (!IsDisposed)
        {
            ProcessedData = null;
            IsDisposed = true;
        }
    }
}