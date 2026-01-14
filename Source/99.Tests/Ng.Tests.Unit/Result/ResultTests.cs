using FluentAssertions;
using Ng.Shared.Result;

namespace Ng.Tests.Unit.Result;

public class ResultTests
{
    #region Success Factory Methods

    [Fact]
    public void Success_WithNoArguments_ShouldReturnSuccessResult()
    {
        // Act
        var result = Ng.Shared.Result.Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.ResultData.DetailCode.Should().Be(SuccessCode.Ok);
    }

    #endregion

    #region Error Factory Methods

    [Fact]
    public void Error_WithCode_ShouldReturnErrorResult()
    {
        // Act
        var result = Ng.Shared.Result.Result.Error(ErrorCode.NotFound);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsError.Should().BeTrue();
        result.IsFailure.Should().BeTrue();
        result.ResultData.DetailCode.Should().Be(ErrorCode.NotFound);
    }

    [Fact]
    public void Error_WithCodeAndMessage_ShouldReturnErrorWithCustomMessage()
    {
        // Arrange
        const string customMessage = "Resource not found";

        // Act
        var result = Ng.Shared.Result.Result.Error(ErrorCode.NotFound, customMessage);

        // Assert
        result.IsError.Should().BeTrue();
        result.ResultData.DetailCode.Should().Be(ErrorCode.NotFound);
        result.Message.Should().Be(customMessage);
    }

    #endregion

    #region Warning Factory Methods

    [Fact]
    public void Warning_WithCode_ShouldReturnWarningResult()
    {
        // Act
        var result = Ng.Shared.Result.Result.Warning(WarningCode.PartialSuccess);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsWarning.Should().BeTrue();
        result.ResultData.DetailCode.Should().Be(WarningCode.PartialSuccess);
    }

    #endregion

    #region Information Factory Methods

    [Fact]
    public void Information_WithCode_ShouldReturnInformationResult()
    {
        // Act
        var result = Ng.Shared.Result.Result.Information(InformationCode.ProcessStarted);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsInfo.Should().BeTrue();
        result.ResultData.DetailCode.Should().Be(InformationCode.ProcessStarted);
    }

    #endregion

    #region Match Methods

    [Fact]
    public void Match_WhenSuccess_ShouldExecuteOnSuccess()
    {
        // Arrange
        var result = Ng.Shared.Result.Result.Success();

        // Act
        var matched = result.Match(
            onSuccess: () => "Success",
            onFailure: info => $"Failed: {info.Message}");

        // Assert
        matched.Should().Be("Success");
    }

    [Fact]
    public void Match_WhenError_ShouldExecuteOnFailure()
    {
        // Arrange
        var result = Ng.Shared.Result.Result.Error(ErrorCode.NotFound, "Item not found");

        // Act
        var matched = result.Match(
            onSuccess: () => "Success",
            onFailure: info => $"Failed: {info.Message}");

        // Assert
        matched.Should().Be("Failed: Item not found");
    }

    #endregion

    #region OnSuccess / OnError Methods

    [Fact]
    public void OnSuccess_WhenSuccess_ShouldExecuteAction()
    {
        // Arrange
        var result = Ng.Shared.Result.Result.Success();
        var executed = false;

        // Act
        result.OnSuccess(() => executed = true);

        // Assert
        executed.Should().BeTrue();
    }

    [Fact]
    public void OnSuccess_WhenError_ShouldNotExecuteAction()
    {
        // Arrange
        var result = Ng.Shared.Result.Result.Error(ErrorCode.NotFound);
        var executed = false;

        // Act
        result.OnSuccess(() => executed = true);

        // Assert
        executed.Should().BeFalse();
    }

    [Fact]
    public void OnError_WhenError_ShouldExecuteAction()
    {
        // Arrange
        var result = Ng.Shared.Result.Result.Error(ErrorCode.NotFound);
        var executed = false;

        // Act
        result.OnError(info => executed = true);

        // Assert
        executed.Should().BeTrue();
    }

    [Fact]
    public void OnWarning_WhenWarning_ShouldExecuteAction()
    {
        // Arrange
        var result = Ng.Shared.Result.Result.Warning(WarningCode.PartialSuccess);
        var executed = false;

        // Act
        result.OnWarning(info => executed = true);

        // Assert
        executed.Should().BeTrue();
    }

    #endregion

    #region FromDetailCode / FromException

    [Fact]
    public void FromDetailCode_WhenSuccess_ShouldReturnSuccess()
    {
        // Act
        var result = Ng.Shared.Result.Result.FromDetailCode(SuccessCode.Ok);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void FromDetailCode_WhenError_ShouldReturnError()
    {
        // Act
        var result = Ng.Shared.Result.Result.FromDetailCode(ErrorCode.NotFound);

        // Assert
        result.IsError.Should().BeTrue();
        result.ResultData.DetailCode.Should().Be(ErrorCode.NotFound);
    }

    [Fact]
    public void FromException_ShouldReturnErrorWithExceptionDetails()
    {
        // Arrange
        var exception = new InvalidOperationException("Test error");

        // Act
        var result = Ng.Shared.Result.Result.FromException(exception);

        // Assert
        result.IsError.Should().BeTrue();
        result.Message.Should().Contain("Test error");
    }

    #endregion

    #region ToString

    [Fact]
    public void ToString_Success_ShouldReturnFormattedString()
    {
        // Arrange
        var result = Ng.Shared.Result.Result.Success();

        // Act
        var str = result.ToString();

        // Assert
        str.Should().Contain("Success");
    }

    [Fact]
    public void ToString_Failure_ShouldReturnFormattedString()
    {
        // Arrange
        var result = Ng.Shared.Result.Result.Error(ErrorCode.NotFound, "Item not found");

        // Act
        var str = result.ToString();

        // Assert
        str.Should().Contain("Failure");
    }

    #endregion
}
