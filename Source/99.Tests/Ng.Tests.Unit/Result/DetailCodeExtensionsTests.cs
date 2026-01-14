using FluentAssertions;
using Ng.Shared.Result;

namespace Ng.Tests.Unit.Result;

public class DetailCodeExtensionsTests
{
    #region GetMessage Tests

    [Fact]
    public void GetMessage_ShouldReturnDefaultMessage()
    {
        // Arrange
        var code = ErrorCode.NotFound;

        // Act
        var message = code.GetMessage();

        // Assert
        message.Should().Be("The requested resource could not be found.");
    }

    #endregion

    #region As / TryAs Tests

    [Fact]
    public void As_WhenCorrectType_ShouldReturnCastedCode()
    {
        // Arrange
        DetailCode code = ErrorCode.NotFound;

        // Act
        var errorCode = code.As<ErrorCode>();

        // Assert
        errorCode.Should().Be(ErrorCode.NotFound);
    }

    [Fact]
    public void As_WhenWrongType_ShouldReturnNull()
    {
        // Arrange
        DetailCode code = ErrorCode.NotFound;

        // Act
        var successCode = code.As<SuccessCode>();

        // Assert
        successCode.Should().BeNull();
    }

    [Fact]
    public void TryAs_WhenCorrectType_ShouldReturnTrue()
    {
        // Arrange
        DetailCode code = ErrorCode.NotFound;

        // Act
        var result = code.TryAs<ErrorCode>(out var errorCode);

        // Assert
        result.Should().BeTrue();
        errorCode.Should().Be(ErrorCode.NotFound);
    }

    [Fact]
    public void TryAs_WhenWrongType_ShouldReturnFalse()
    {
        // Arrange
        DetailCode code = ErrorCode.NotFound;

        // Act
        var result = code.TryAs<SuccessCode>(out var successCode);

        // Assert
        result.Should().BeFalse();
        successCode.Should().BeNull();
    }

    #endregion

    #region IsInRange Tests

    [Fact]
    public void IsInRange_WhenInRange_ShouldReturnTrue()
    {
        // Arrange
        var code = ErrorCode.NotFound;

        // Act
        var result = code.IsInRange(1200, 1299);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsInRange_WhenOutOfRange_ShouldReturnFalse()
    {
        // Arrange
        var code = ErrorCode.NotFound;

        // Act
        var result = code.IsInRange(1000, 1099);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region Error Type Tests

    [Fact]
    public void IsUserError_ShouldIdentifyCorrectly()
    {
        // Assert
        ErrorCode.InvalidInput.IsUserError().Should().BeTrue();
        ErrorCode.NotFound.IsUserError().Should().BeTrue();
        ErrorCode.BusinessRuleViolation.IsUserError().Should().BeFalse();
    }

    [Fact]
    public void IsSystemError_ShouldIdentifyCorrectly()
    {
        // Assert
        ErrorCode.DatabaseError.IsSystemError().Should().BeTrue();
        ErrorCode.NetworkError.IsSystemError().Should().BeTrue();
        ErrorCode.InvalidInput.IsSystemError().Should().BeFalse();
    }

    [Fact]
    public void IsBusinessError_ShouldIdentifyCorrectly()
    {
        // Assert - Business Logic Error (2000-2199: Business + Sports)
        ErrorCode.BusinessRuleViolation.IsBusinessError().Should().BeTrue();
        ErrorCode.MatchAlreadyStarted.IsBusinessError().Should().BeTrue(); // Sports도 포함
        ErrorCode.InvalidInput.IsBusinessError().Should().BeFalse();
        ErrorCode.DatabaseError.IsBusinessError().Should().BeFalse();
    }

    #endregion

    #region ToHttpStatusCode Tests

    [Fact]
    public void ToHttpStatusCode_NotFound_ShouldReturn404()
    {
        // Act
        var statusCode = ErrorCode.NotFound.ToHttpStatusCode();

        // Assert
        statusCode.Should().Be(404);
    }

    [Fact]
    public void ToHttpStatusCode_Unauthorized_ShouldReturn401()
    {
        // Act
        var statusCode = ErrorCode.Unauthorized.ToHttpStatusCode();

        // Assert
        statusCode.Should().Be(401);
    }

    [Fact]
    public void ToHttpStatusCode_Forbidden_ShouldReturn403()
    {
        // Act
        var statusCode = ErrorCode.Forbidden.ToHttpStatusCode();

        // Assert
        statusCode.Should().Be(403);
    }

    [Fact]
    public void ToHttpStatusCode_BadRequest_ShouldReturn400()
    {
        // Act
        var statusCode = ErrorCode.BadRequest.ToHttpStatusCode();

        // Assert
        statusCode.Should().Be(400);
    }

    [Fact]
    public void ToHttpStatusCode_TooManyRequests_ShouldReturn429()
    {
        // Act
        var statusCode = ErrorCode.TooManyRequests.ToHttpStatusCode();

        // Assert
        statusCode.Should().Be(429);
    }

    [Fact]
    public void ToHttpStatusCode_ServiceUnavailable_ShouldReturn503()
    {
        // Act
        var statusCode = ErrorCode.ServiceUnavailable.ToHttpStatusCode();

        // Assert
        statusCode.Should().Be(503);
    }

    [Fact]
    public void ToHttpStatusCode_UserError_ShouldReturn400()
    {
        // Act
        var statusCode = ErrorCode.InvalidInput.ToHttpStatusCode();

        // Assert
        statusCode.Should().Be(400);
    }

    [Fact]
    public void ToHttpStatusCode_BusinessError_ShouldReturn422()
    {
        // Act
        var statusCode = ErrorCode.BusinessRuleViolation.ToHttpStatusCode();

        // Assert
        statusCode.Should().Be(422);
    }

    [Fact]
    public void ToHttpStatusCode_SystemError_ShouldReturn500()
    {
        // Act
        var statusCode = ErrorCode.DatabaseError.ToHttpStatusCode();

        // Assert
        statusCode.Should().Be(500);
    }

    [Fact]
    public void ToHttpStatusCode_Success_ShouldReturn200()
    {
        // Act
        var statusCode = SuccessCode.Ok.ToHttpStatusCode();

        // Assert
        statusCode.Should().Be(200);
    }

    [Fact]
    public void ToHttpStatusCode_Warning_ShouldReturn200()
    {
        // Act
        var statusCode = WarningCode.PartialSuccess.ToHttpStatusCode();

        // Assert
        statusCode.Should().Be(200);
    }

    #endregion

    #region GetLogLevel Tests

    [Fact]
    public void GetLogLevel_CriticalError_ShouldReturnFatal()
    {
        // Act
        var level = ErrorCode.InternalError.GetLogLevel();

        // Assert
        level.Should().Be("Fatal");
    }

    [Fact]
    public void GetLogLevel_SystemError_ShouldReturnError()
    {
        // Act
        var level = ErrorCode.NetworkError.GetLogLevel();

        // Assert
        level.Should().Be("Error");
    }

    [Fact]
    public void GetLogLevel_BusinessError_ShouldReturnWarning()
    {
        // Act
        var level = ErrorCode.BusinessRuleViolation.GetLogLevel();

        // Assert
        level.Should().Be("Warning");
    }

    [Fact]
    public void GetLogLevel_UserError_ShouldReturnInformation()
    {
        // Act
        var level = ErrorCode.InvalidInput.GetLogLevel();

        // Assert
        level.Should().Be("Information");
    }

    [Fact]
    public void GetLogLevel_Warning_ShouldReturnWarning()
    {
        // Act
        var level = WarningCode.PartialSuccess.GetLogLevel();

        // Assert
        level.Should().Be("Warning");
    }

    [Fact]
    public void GetLogLevel_Success_ShouldReturnInformation()
    {
        // Act
        var level = SuccessCode.Ok.GetLogLevel();

        // Assert
        level.Should().Be("Information");
    }

    #endregion

    #region IsRetryable Tests

    [Fact]
    public void IsRetryable_RetryableError_ShouldReturnTrue()
    {
        // Assert
        ErrorCode.NetworkTimeout.IsRetryable().Should().BeTrue();
        ErrorCode.ServiceUnavailable.IsRetryable().Should().BeTrue();
    }

    [Fact]
    public void IsRetryable_NonRetryableError_ShouldReturnFalse()
    {
        // Assert
        ErrorCode.InvalidInput.IsRetryable().Should().BeFalse();
        ErrorCode.NotFound.IsRetryable().Should().BeFalse();
    }

    [Fact]
    public void IsRetryable_NonErrorCode_ShouldReturnFalse()
    {
        // Assert
        SuccessCode.Ok.IsRetryable().Should().BeFalse();
        WarningCode.PartialSuccess.IsRetryable().Should().BeFalse();
    }

    #endregion

    #region IsUserFriendly Tests

    [Fact]
    public void IsUserFriendly_UserError_ShouldReturnTrue()
    {
        // Assert
        ErrorCode.InvalidInput.IsUserFriendly().Should().BeTrue();
    }

    [Fact]
    public void IsUserFriendly_BusinessError_ShouldReturnTrue()
    {
        // Assert
        ErrorCode.BusinessRuleViolation.IsUserFriendly().Should().BeTrue();
    }

    [Fact]
    public void IsUserFriendly_SystemError_ShouldReturnFalse()
    {
        // Assert
        ErrorCode.DatabaseError.IsUserFriendly().Should().BeFalse();
    }

    [Fact]
    public void IsUserFriendly_SuccessCode_ShouldReturnTrue()
    {
        // Assert
        SuccessCode.Ok.IsUserFriendly().Should().BeTrue();
    }

    #endregion

    #region GetMetricCategory Tests

    [Fact]
    public void GetMetricCategory_UserError_ShouldReturnClientError()
    {
        // Act
        var category = ErrorCode.InvalidInput.GetMetricCategory();

        // Assert
        category.Should().Be("client_error");
    }

    [Fact]
    public void GetMetricCategory_BusinessError_ShouldReturnBusinessError()
    {
        // Act
        var category = ErrorCode.BusinessRuleViolation.GetMetricCategory();

        // Assert
        category.Should().Be("business_error");
    }

    [Fact]
    public void GetMetricCategory_SystemError_ShouldReturnSystemError()
    {
        // Act
        var category = ErrorCode.DatabaseError.GetMetricCategory();

        // Assert
        category.Should().Be("system_error");
    }

    [Fact]
    public void GetMetricCategory_Success_ShouldReturnSuccess()
    {
        // Act
        var category = SuccessCode.Ok.GetMetricCategory();

        // Assert
        category.Should().Be("success");
    }

    #endregion

    #region RequiresNotification Tests

    [Fact]
    public void RequiresNotification_CriticalError_ShouldReturnTrue()
    {
        // Assert
        ErrorCode.DatabaseError.RequiresNotification().Should().BeTrue();
        ErrorCode.InternalError.RequiresNotification().Should().BeTrue();
    }

    [Fact]
    public void RequiresNotification_NonCriticalError_ShouldReturnFalse()
    {
        // Assert
        ErrorCode.InvalidInput.RequiresNotification().Should().BeFalse();
    }

    [Fact]
    public void RequiresNotification_SystemWarning_ShouldReturnTrue()
    {
        // Assert
        WarningCode.ServiceDegradation.RequiresNotification().Should().BeTrue();
    }

    #endregion

    #region GetPriority Tests

    [Fact]
    public void GetPriority_CriticalError_ShouldReturn5()
    {
        // Act
        var priority = ErrorCode.InternalError.GetPriority();

        // Assert
        priority.Should().Be(5);
    }

    [Fact]
    public void GetPriority_SystemError_ShouldReturn4()
    {
        // Act
        var priority = ErrorCode.NetworkError.GetPriority();

        // Assert
        priority.Should().Be(4);
    }

    [Fact]
    public void GetPriority_BusinessError_ShouldReturn3()
    {
        // Act
        var priority = ErrorCode.BusinessRuleViolation.GetPriority();

        // Assert
        priority.Should().Be(3);
    }

    [Fact]
    public void GetPriority_UserError_ShouldReturn2()
    {
        // Act
        var priority = ErrorCode.InvalidInput.GetPriority();

        // Assert
        priority.Should().Be(2);
    }

    [Fact]
    public void GetPriority_Success_ShouldReturn1()
    {
        // Act
        var priority = SuccessCode.Ok.GetPriority();

        // Assert
        priority.Should().Be(1);
    }

    #endregion

    #region GetUserFriendlyMessage Tests

    [Fact]
    public void GetUserFriendlyMessage_SystemError_ShouldReturnGenericMessage()
    {
        // Act
        var message = ErrorCode.DatabaseError.GetUserFriendlyMessage();

        // Assert
        message.Should().Contain("temporary system issue");
    }

    [Fact]
    public void GetUserFriendlyMessage_UserError_ShouldReturnDefaultMessage()
    {
        // Act
        var message = ErrorCode.InvalidInput.GetUserFriendlyMessage();

        // Assert
        message.Should().Be(ErrorCode.InvalidInput.DefaultMessage);
    }

    [Fact]
    public void GetUserFriendlyMessage_WithCustomMessage_ShouldReturnCustomMessage()
    {
        // Arrange
        const string customMessage = "Custom error message";

        // Act
        var message = ErrorCode.InvalidInput.GetUserFriendlyMessage(customMessage);

        // Assert
        message.Should().Be(customMessage);
    }

    #endregion

    #region GetResolutionSuggestion Tests

    [Fact]
    public void GetResolutionSuggestion_InvalidInput_ShouldReturnSuggestion()
    {
        // Act
        var suggestion = ErrorCode.InvalidInput.GetResolutionSuggestion();

        // Assert
        suggestion.Should().NotBeNullOrEmpty();
        suggestion.Should().Contain("check your input");
    }

    [Fact]
    public void GetResolutionSuggestion_Unauthorized_ShouldReturnSuggestion()
    {
        // Act
        var suggestion = ErrorCode.Unauthorized.GetResolutionSuggestion();

        // Assert
        suggestion.Should().Contain("log in");
    }

    [Fact]
    public void GetResolutionSuggestion_UnknownError_ShouldReturnNull()
    {
        // Act
        var suggestion = ErrorCode.UnknownError.GetResolutionSuggestion();

        // Assert
        suggestion.Should().BeNull();
    }

    #endregion
}
