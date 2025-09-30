using TUnit.Core;
using DotNetTUnitExample.Services;
using DotNetTUnitExample.Models;

namespace DotNetTUnitExample.Tests;

/// <summary>
/// Async TUnit tests demonstrating asynchronous testing patterns.
/// Shows async Task test methods, proper await syntax, async exception handling,
/// and timing verification for async operations.
/// </summary>
public class AsyncTests
{
    private readonly UserService _userService;

    public AsyncTests()
    {
        _userService = new UserService();
    }

    /// <summary>
    /// Cleans up test data before each test to ensure test isolation.
    /// Demonstrates test setup patterns in async testing.
    /// </summary>
    [Before(Test)]
    public async Task SetupAsync()
    {
        _userService.ClearAllUsers();
        await Task.CompletedTask; // Demonstrate async setup method
    }

    #region Async Operation Success Tests

    /// <summary>
    /// Tests successful user creation with async operations.
    /// Demonstrates basic async test method with proper await syntax.
    /// </summary>
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
        await Assert.That(result.Data.Email).IsEqualTo(email);
        await Assert.That(result.Data.Id).IsGreaterThan(0);
        await Assert.That(result.ErrorMessage).IsEqualTo(string.Empty);
    }

    /// <summary>
    /// Tests successful user validation with async operations.
    /// Demonstrates async testing with setup and verification.
    /// </summary>
    [Test]
    public async Task ValidateUserAsync_WithExistingUser_ReturnsValidResult()
    {
        // Arrange
        var createResult = await _userService.CreateUserAsync("Jane Smith", "jane.smith@example.com");
        await Assert.That(createResult.Success).IsTrue();
        int userId = createResult.Data.Id;

        // Act
        var validateResult = await _userService.ValidateUserAsync(userId);

        // Assert
        await Assert.That(validateResult.Success).IsTrue();
        await Assert.That(validateResult.Data).IsTrue();
        await Assert.That(validateResult.ErrorMessage).IsEqualTo(string.Empty);
    }

    /// <summary>
    /// Tests successful user update with async operations.
    /// Demonstrates async testing with multiple operations and state changes.
    /// </summary>
    [Test]
    public async Task UpdateUserAsync_WithValidData_ReturnsUpdatedUser()
    {
        // Arrange
        var createResult = await _userService.CreateUserAsync("Original Name", "original@example.com");
        await Assert.That(createResult.Success).IsTrue();
        int userId = createResult.Data.Id;
        
        string newName = "Updated Name";
        string newEmail = "updated@example.com";

        // Act
        var updateResult = await _userService.UpdateUserAsync(userId, newName, newEmail);

        // Assert
        await Assert.That(updateResult.Success).IsTrue();
        await Assert.That(updateResult.Data).IsNotNull();
        await Assert.That(updateResult.Data.Name).IsEqualTo(newName);
        await Assert.That(updateResult.Data.Email).IsEqualTo(newEmail);
        await Assert.That(updateResult.Data.Id).IsEqualTo(userId);
        await Assert.That(updateResult.ErrorMessage).IsEqualTo(string.Empty);
    }

    /// <summary>
    /// Tests successful user retrieval with async operations.
    /// Demonstrates async testing with data verification.
    /// </summary>
    [Test]
    public async Task GetUserAsync_WithExistingUser_ReturnsCorrectUser()
    {
        // Arrange
        string expectedName = "Test User";
        string expectedEmail = "test.user@example.com";
        var createResult = await _userService.CreateUserAsync(expectedName, expectedEmail);
        await Assert.That(createResult.Success).IsTrue();
        int userId = createResult.Data.Id;

        // Act
        var getUserResult = await _userService.GetUserAsync(userId);

        // Assert
        await Assert.That(getUserResult.Success).IsTrue();
        await Assert.That(getUserResult.Data).IsNotNull();
        await Assert.That(getUserResult.Data.Id).IsEqualTo(userId);
        await Assert.That(getUserResult.Data.Name).IsEqualTo(expectedName);
        await Assert.That(getUserResult.Data.Email).IsEqualTo(expectedEmail);
        await Assert.That(getUserResult.ErrorMessage).IsEqualTo(string.Empty);
    }

    /// <summary>
    /// Tests successful retrieval of all users with async operations.
    /// Demonstrates async testing with collection operations.
    /// </summary>
    [Test]
    public async Task GetAllUsersAsync_WithMultipleUsers_ReturnsAllUsers()
    {
        // Arrange
        await _userService.CreateUserAsync("User One", "user1@example.com");
        await _userService.CreateUserAsync("User Two", "user2@example.com");
        await _userService.CreateUserAsync("User Three", "user3@example.com");

        // Act
        var getAllResult = await _userService.GetAllUsersAsync();

        // Assert
        await Assert.That(getAllResult.Success).IsTrue();
        await Assert.That(getAllResult.Data).IsNotNull();
        await Assert.That(getAllResult.Data.Count).IsEqualTo(3);
        await Assert.That(getAllResult.ErrorMessage).IsEqualTo(string.Empty);
        
        // Verify all users are present
        var users = getAllResult.Data;
        await Assert.That(users.Any(u => u.Name == "User One")).IsTrue();
        await Assert.That(users.Any(u => u.Name == "User Two")).IsTrue();
        await Assert.That(users.Any(u => u.Name == "User Three")).IsTrue();
    }

    #endregion

    #region Async Error Handling Tests

    /// <summary>
    /// Tests that CreateUserAsync returns failure result for null name.
    /// Demonstrates async error handling with ProcessingResult pattern.
    /// </summary>
    [Test]
    public async Task CreateUserAsync_WithNullName_ReturnsFailureResult()
    {
        // Arrange
        string? name = null;
        string email = "test@example.com";

        // Act
        var result = await _userService.CreateUserAsync(name!, email);

        // Assert
        await Assert.That(result.Success).IsFalse();
        await Assert.That(result.Data).IsNull();
        await Assert.That(result.ErrorMessage).Contains("Name cannot be null or empty");
        await Assert.That(result.ProcessingTime).IsGreaterThan(TimeSpan.Zero);
    }

    /// <summary>
    /// Tests that CreateUserAsync returns failure result for empty email.
    /// Demonstrates async error handling with parameter validation.
    /// </summary>
    [Test]
    public async Task CreateUserAsync_WithEmptyEmail_ReturnsFailureResult()
    {
        // Arrange
        string name = "Test User";
        string email = "";

        // Act
        var result = await _userService.CreateUserAsync(name, email);

        // Assert
        await Assert.That(result.Success).IsFalse();
        await Assert.That(result.Data).IsNull();
        await Assert.That(result.ErrorMessage).Contains("Email cannot be null or empty");
        await Assert.That(result.ProcessingTime).IsGreaterThan(TimeSpan.Zero);
    }

    /// <summary>
    /// Tests that CreateUserAsync returns failure result for duplicate email.
    /// Demonstrates async error handling with business logic validation.
    /// </summary>
    [Test]
    public async Task CreateUserAsync_WithDuplicateEmail_ReturnsFailureResult()
    {
        // Arrange
        string email = "duplicate@example.com";
        var firstResult = await _userService.CreateUserAsync("First User", email);
        await Assert.That(firstResult.Success).IsTrue();

        // Act
        var secondResult = await _userService.CreateUserAsync("Second User", email);

        // Assert
        await Assert.That(secondResult.Success).IsFalse();
        await Assert.That(secondResult.Data).IsNull();
        await Assert.That(secondResult.ErrorMessage).Contains($"User with email '{email}' already exists");
        await Assert.That(secondResult.ProcessingTime).IsGreaterThan(TimeSpan.Zero);
    }

    /// <summary>
    /// Tests that ValidateUserAsync returns failure result for invalid user ID.
    /// Demonstrates async error handling with parameter validation.
    /// </summary>
    [Test]
    public async Task ValidateUserAsync_WithInvalidUserId_ReturnsFailureResult()
    {
        // Arrange
        int invalidUserId = -1;

        // Act
        var result = await _userService.ValidateUserAsync(invalidUserId);

        // Assert
        await Assert.That(result.Success).IsFalse();
        await Assert.That(result.Data).IsFalse();
        await Assert.That(result.ErrorMessage).Contains("User ID must be greater than 0");
        await Assert.That(result.ProcessingTime).IsGreaterThan(TimeSpan.Zero);
    }

    /// <summary>
    /// Tests that ValidateUserAsync returns failure result for non-existent user.
    /// Demonstrates async error handling with data lookup scenarios.
    /// </summary>
    [Test]
    public async Task ValidateUserAsync_WithNonExistentUser_ReturnsFailureResult()
    {
        // Arrange
        int nonExistentUserId = 999;

        // Act
        var result = await _userService.ValidateUserAsync(nonExistentUserId);

        // Assert
        await Assert.That(result.Success).IsFalse();
        await Assert.That(result.Data).IsFalse();
        await Assert.That(result.ErrorMessage).Contains($"User with ID {nonExistentUserId} not found");
        await Assert.That(result.ProcessingTime).IsGreaterThan(TimeSpan.Zero);
    }

    /// <summary>
    /// Tests that UpdateUserAsync returns failure result for non-existent user.
    /// Demonstrates async error handling with update operations.
    /// </summary>
    [Test]
    public async Task UpdateUserAsync_WithNonExistentUser_ReturnsFailureResult()
    {
        // Arrange
        int nonExistentUserId = 999;
        string newName = "New Name";

        // Act
        var result = await _userService.UpdateUserAsync(nonExistentUserId, newName);

        // Assert
        await Assert.That(result.Success).IsFalse();
        await Assert.That(result.Data).IsNull();
        await Assert.That(result.ErrorMessage).Contains($"User with ID {nonExistentUserId} not found");
        await Assert.That(result.ProcessingTime).IsGreaterThan(TimeSpan.Zero);
    }

    /// <summary>
    /// Tests that UpdateUserAsync returns failure result for duplicate email.
    /// Demonstrates async error handling with business rule validation.
    /// </summary>
    [Test]
    public async Task UpdateUserAsync_WithDuplicateEmail_ReturnsFailureResult()
    {
        // Arrange
        var user1 = await _userService.CreateUserAsync("User One", "user1@example.com");
        var user2 = await _userService.CreateUserAsync("User Two", "user2@example.com");
        
        await Assert.That(user1.Success).IsTrue();
        await Assert.That(user2.Success).IsTrue();

        // Act
        var result = await _userService.UpdateUserAsync(user2.Data!.Id, email: "user1@example.com");

        // Assert
        await Assert.That(result.Success).IsFalse();
        await Assert.That(result.Data).IsNull();
        await Assert.That(result.ErrorMessage).Contains("Email 'user1@example.com' is already in use by another user");
        await Assert.That(result.ProcessingTime).IsGreaterThan(TimeSpan.Zero);
    }

    /// <summary>
    /// Tests that GetUserAsync returns failure result for non-existent user.
    /// Demonstrates async error handling with retrieval operations.
    /// </summary>
    [Test]
    public async Task GetUserAsync_WithNonExistentUser_ReturnsFailureResult()
    {
        // Arrange
        int nonExistentUserId = 999;

        // Act
        var result = await _userService.GetUserAsync(nonExistentUserId);

        // Assert
        await Assert.That(result.Success).IsFalse();
        await Assert.That(result.Data).IsNull();
        await Assert.That(result.ErrorMessage).Contains($"User with ID {nonExistentUserId} not found");
        await Assert.That(result.ProcessingTime).IsGreaterThan(TimeSpan.Zero);
    }

    /// <summary>
    /// Tests async exception handling when the service method itself throws.
    /// Demonstrates testing actual exceptions vs ProcessingResult error handling.
    /// </summary>
    [Test]
    public async Task AsyncExceptionHandling_DemonstrationOfActualExceptions()
    {
        // This test demonstrates how to test for actual exceptions if they were thrown
        // In this case, we'll test a scenario that would cause an exception in the service
        
        // For demonstration, we'll test that Assert.ThrowsAsync works with actual exceptions
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            // This would throw if we passed null to a method that doesn't handle it gracefully
            await Task.Run(() => throw new ArgumentNullException("testParam", "This is a test exception"));
        });
    }

    #endregion

    #region Async Timing and Performance Tests

    /// <summary>
    /// Tests that async operations complete within expected time ranges.
    /// Demonstrates async timing verification and performance testing.
    /// </summary>
    [Test]
    public async Task CreateUserAsync_CompletesWithinExpectedTime()
    {
        // Arrange
        string name = "Performance Test User";
        string email = "performance@example.com";
        var startTime = DateTime.UtcNow;

        // Act
        var result = await _userService.CreateUserAsync(name, email);
        var endTime = DateTime.UtcNow;
        var actualDuration = endTime - startTime;

        // Assert
        await Assert.That(result.Success).IsTrue();
        await Assert.That(result.ProcessingTime).IsGreaterThan(TimeSpan.FromMilliseconds(100)); // Should take at least 100ms due to delays
        await Assert.That(result.ProcessingTime).IsLessThan(TimeSpan.FromMilliseconds(500));    // Should complete within 500ms
        await Assert.That(actualDuration).IsLessThan(TimeSpan.FromSeconds(1));                  // Overall operation should be fast
    }

    /// <summary>
    /// Tests that multiple async operations can run concurrently.
    /// Demonstrates concurrent async testing and timing verification.
    /// </summary>
    [Test]
    public async Task MultipleAsyncOperations_RunConcurrently()
    {
        // Arrange
        var startTime = DateTime.UtcNow;
        
        // Act - Run multiple operations concurrently
        var tasks = new[]
        {
            _userService.CreateUserAsync("User A", "usera@example.com"),
            _userService.CreateUserAsync("User B", "userb@example.com"),
            _userService.CreateUserAsync("User C", "userc@example.com")
        };
        
        var results = await Task.WhenAll(tasks);
        var endTime = DateTime.UtcNow;
        var totalDuration = endTime - startTime;

        // Assert
        await Assert.That(results.Length).IsEqualTo(3);
        await Assert.That(results.All(r => r.Success)).IsTrue();
        
        // Concurrent execution should be faster than sequential
        // Each operation takes ~150ms, so concurrent should be much less than 450ms
        await Assert.That(totalDuration).IsLessThan(TimeSpan.FromMilliseconds(400));
        
        // Verify all users were created with unique IDs
        var userIds = results.Select(r => r.Data.Id).ToList();
        await Assert.That(userIds.Distinct().Count()).IsEqualTo(3);
    }

    /// <summary>
    /// Tests async operation timing consistency across multiple calls.
    /// Demonstrates performance consistency testing with async operations.
    /// </summary>
    [Test]
    public async Task ValidateUserAsync_HasConsistentTiming()
    {
        // Arrange
        var createResult = await _userService.CreateUserAsync("Timing Test User", "timing@example.com");
        await Assert.That(createResult.Success).IsTrue();
        int userId = createResult.Data.Id;
        
        var timings = new List<TimeSpan>();

        // Act - Perform multiple validation operations
        for (int i = 0; i < 5; i++)
        {
            var result = await _userService.ValidateUserAsync(userId);
            await Assert.That(result.Success).IsTrue();
            timings.Add(result.ProcessingTime);
        }

        // Assert - Verify timing consistency
        var averageTiming = TimeSpan.FromTicks((long)timings.Select(t => t.Ticks).Average());
        var maxTiming = timings.Max();
        var minTiming = timings.Min();
        
        await Assert.That(averageTiming).IsGreaterThan(TimeSpan.FromMilliseconds(80));  // Should take at least 80ms
        await Assert.That(averageTiming).IsLessThan(TimeSpan.FromMilliseconds(150));   // Should average less than 150ms
        await Assert.That(maxTiming - minTiming).IsLessThan(TimeSpan.FromMilliseconds(50)); // Variance should be small
    }

    /// <summary>
    /// Tests that GetAllUsersAsync timing scales appropriately with data size.
    /// Demonstrates async performance testing with varying data loads.
    /// </summary>
    [Test]
    public async Task GetAllUsersAsync_TimingScalesWithDataSize()
    {
        // Arrange - Create different numbers of users and measure timing
        _userService.ClearAllUsers();
        
        // Test with 1 user
        await _userService.CreateUserAsync("User 1", "user1@example.com");
        var result1 = await _userService.GetAllUsersAsync();
        await Assert.That(result1.Success).IsTrue();
        var timing1 = result1.ProcessingTime;
        
        // Test with 3 users
        await _userService.CreateUserAsync("User 2", "user2@example.com");
        await _userService.CreateUserAsync("User 3", "user3@example.com");
        var result3 = await _userService.GetAllUsersAsync();
        await Assert.That(result3.Success).IsTrue();
        var timing3 = result3.ProcessingTime;

        // Assert - Timing should be consistent regardless of data size (since it's simulated)
        await Assert.That(result1.Data.Count).IsEqualTo(1);
        await Assert.That(result3.Data.Count).IsEqualTo(3);
        await Assert.That(timing1).IsGreaterThan(TimeSpan.FromMilliseconds(100)); // Base delay
        await Assert.That(timing3).IsGreaterThan(TimeSpan.FromMilliseconds(100)); // Base delay
        
        // Both should complete within reasonable time
        await Assert.That(timing1).IsLessThan(TimeSpan.FromMilliseconds(300));
        await Assert.That(timing3).IsLessThan(TimeSpan.FromMilliseconds(300));
    }

    #endregion

    #region Complex Async Workflow Tests

    /// <summary>
    /// Tests a complete async workflow with multiple operations.
    /// Demonstrates complex async testing scenarios with multiple steps.
    /// </summary>
    [Test]
    public async Task CompleteUserWorkflow_WithAsyncOperations_WorksCorrectly()
    {
        // Arrange
        string originalName = "Workflow User";
        string originalEmail = "workflow@example.com";
        string updatedName = "Updated Workflow User";
        string updatedEmail = "updated.workflow@example.com";

        // Act & Assert - Step 1: Create user
        var createResult = await _userService.CreateUserAsync(originalName, originalEmail);
        await Assert.That(createResult.Success).IsTrue();
        await Assert.That(createResult.Data.Name).IsEqualTo(originalName);
        await Assert.That(createResult.Data.Email).IsEqualTo(originalEmail);
        
        int userId = createResult.Data.Id;

        // Act & Assert - Step 2: Validate user
        var validateResult = await _userService.ValidateUserAsync(userId);
        await Assert.That(validateResult.Success).IsTrue();
        await Assert.That(validateResult.Data).IsTrue();

        // Act & Assert - Step 3: Get user
        var getUserResult = await _userService.GetUserAsync(userId);
        await Assert.That(getUserResult.Success).IsTrue();
        await Assert.That(getUserResult.Data.Name).IsEqualTo(originalName);
        await Assert.That(getUserResult.Data.Email).IsEqualTo(originalEmail);

        // Act & Assert - Step 4: Update user
        var updateResult = await _userService.UpdateUserAsync(userId, updatedName, updatedEmail);
        await Assert.That(updateResult.Success).IsTrue();
        await Assert.That(updateResult.Data.Name).IsEqualTo(updatedName);
        await Assert.That(updateResult.Data.Email).IsEqualTo(updatedEmail);

        // Act & Assert - Step 5: Validate updated user
        var validateUpdatedResult = await _userService.ValidateUserAsync(userId);
        await Assert.That(validateUpdatedResult.Success).IsTrue();
        await Assert.That(validateUpdatedResult.Data).IsTrue();

        // Act & Assert - Step 6: Verify changes persisted
        var getFinalResult = await _userService.GetUserAsync(userId);
        await Assert.That(getFinalResult.Success).IsTrue();
        await Assert.That(getFinalResult.Data.Name).IsEqualTo(updatedName);
        await Assert.That(getFinalResult.Data.Email).IsEqualTo(updatedEmail);
    }

    /// <summary>
    /// Tests async error recovery and handling in complex scenarios.
    /// Demonstrates async exception handling in multi-step workflows.
    /// </summary>
    [Test]
    public async Task AsyncErrorRecovery_HandlesExceptionsGracefully()
    {
        // Arrange
        string validEmail = "recovery@example.com";
        string duplicateEmail = "duplicate@example.com";
        
        // Create initial user
        var initialUser = await _userService.CreateUserAsync("Initial User", duplicateEmail);
        await Assert.That(initialUser.Success).IsTrue();

        // Act & Assert - Attempt to create duplicate user (should fail)
        var duplicateResult = await _userService.CreateUserAsync("Duplicate User", duplicateEmail);
        await Assert.That(duplicateResult.Success).IsFalse();
        await Assert.That(duplicateResult.ErrorMessage).Contains("already exists");

        // Act & Assert - Create user with valid email (should succeed)
        var validUser = await _userService.CreateUserAsync("Valid User", validEmail);
        await Assert.That(validUser.Success).IsTrue();
        await Assert.That(validUser.Data.Email).IsEqualTo(validEmail);

        // Act & Assert - Attempt to update to duplicate email (should fail)
        var updateToDuplicateResult = await _userService.UpdateUserAsync(validUser.Data.Id, email: duplicateEmail);
        await Assert.That(updateToDuplicateResult.Success).IsFalse();
        await Assert.That(updateToDuplicateResult.ErrorMessage).Contains("already in use");

        // Act & Assert - Verify original user data is unchanged
        var getUserResult = await _userService.GetUserAsync(validUser.Data.Id);
        await Assert.That(getUserResult.Success).IsTrue();
        await Assert.That(getUserResult.Data.Email).IsEqualTo(validEmail); // Should still be original email
    }

    #endregion
}