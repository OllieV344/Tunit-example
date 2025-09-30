namespace DotNetTUnitExample.Models;

/// <summary>
/// Represents the result of a processing operation with generic data type support.
/// </summary>
/// <typeparam name="T">The type of data contained in the result.</typeparam>
public class ProcessingResult<T>
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the data returned by the operation.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Gets or sets the error message if the operation failed.
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the time taken to process the operation.
    /// </summary>
    public TimeSpan ProcessingTime { get; set; }

    /// <summary>
    /// Initializes a new instance of the ProcessingResult class.
    /// </summary>
    public ProcessingResult()
    {
    }

    /// <summary>
    /// Initializes a new instance of the ProcessingResult class with success status and data.
    /// </summary>
    /// <param name="success">Whether the operation was successful.</param>
    /// <param name="data">The data returned by the operation.</param>
    /// <param name="processingTime">The time taken to process the operation.</param>
    public ProcessingResult(bool success, T? data, TimeSpan processingTime)
    {
        Success = success;
        Data = data;
        ProcessingTime = processingTime;
    }

    /// <summary>
    /// Initializes a new instance of the ProcessingResult class with error information.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="processingTime">The time taken before the error occurred.</param>
    public ProcessingResult(string errorMessage, TimeSpan processingTime)
    {
        Success = false;
        ErrorMessage = errorMessage ?? string.Empty;
        ProcessingTime = processingTime;
    }

    /// <summary>
    /// Creates a successful result with data.
    /// </summary>
    /// <param name="data">The data to include in the result.</param>
    /// <param name="processingTime">The processing time.</param>
    /// <returns>A successful ProcessingResult.</returns>
    public static ProcessingResult<T> CreateSuccess(T data, TimeSpan processingTime)
    {
        return new ProcessingResult<T>(true, data, processingTime);
    }

    /// <summary>
    /// Creates a failed result with an error message.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="processingTime">The processing time before failure.</param>
    /// <returns>A failed ProcessingResult.</returns>
    public static ProcessingResult<T> CreateFailure(string errorMessage, TimeSpan processingTime)
    {
        return new ProcessingResult<T>(errorMessage, processingTime);
    }

    /// <summary>
    /// Returns a string representation of the processing result.
    /// </summary>
    /// <returns>A string containing the result information.</returns>
    public override string ToString()
    {
        if (Success)
        {
            return $"ProcessingResult {{ Success: {Success}, Data: {Data}, ProcessingTime: {ProcessingTime.TotalMilliseconds}ms }}";
        }
        else
        {
            return $"ProcessingResult {{ Success: {Success}, ErrorMessage: {ErrorMessage}, ProcessingTime: {ProcessingTime.TotalMilliseconds}ms }}";
        }
    }
}