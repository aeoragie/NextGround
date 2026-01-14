using FluentAssertions;
using Ng.Shared.Result;

namespace Ng.Tests.Unit.Result;

public class ResultTTests
{
    #region Test Models

    private record TestEntity(int Id, string Name);

    #endregion

    #region Success Factory Methods

    [Fact]
    public void Success_WithValue_ShouldReturnSuccessResultWithValue()
    {
        // Arrange
        var entity = new TestEntity(1, "Test");

        // Act
        var result = Result<TestEntity>.Success(entity);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(entity);
        result.ResultData.DetailCode.Should().Be(SuccessCode.Ok);
    }

    #endregion

    #region Error Factory Methods

    [Fact]
    public void Error_WithCode_ShouldReturnErrorWithNullValue()
    {
        // Act
        var result = Result<TestEntity>.Error(ErrorCode.NotFound);

        // Assert
        result.IsError.Should().BeTrue();
        result.IsFailure.Should().BeTrue();
        result.Value.Should().BeNull();
        result.ResultData.DetailCode.Should().Be(ErrorCode.NotFound);
    }

    [Fact]
    public void Error_WithCodeAndMessage_ShouldReturnErrorWithCustomMessage()
    {
        // Arrange
        const string customMessage = "Entity not found";

        // Act
        var result = Result<TestEntity>.Error(ErrorCode.NotFound, customMessage);

        // Assert
        result.IsError.Should().BeTrue();
        result.Message.Should().Be(customMessage);
    }

    #endregion

    #region Warning Factory Methods

    [Fact]
    public void Warning_WithValueAndCode_ShouldReturnWarningWithValue()
    {
        // Arrange
        var entity = new TestEntity(1, "Test");

        // Act
        var result = Result<TestEntity>.Warning(entity, WarningCode.DataIncomplete);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsWarning.Should().BeTrue();
        result.Value.Should().Be(entity);
        result.ResultData.DetailCode.Should().Be(WarningCode.DataIncomplete);
    }

    #endregion

    #region Information Factory Methods

    [Fact]
    public void Information_WithValueAndCode_ShouldReturnInformationWithValue()
    {
        // Arrange
        var entity = new TestEntity(1, "Test");

        // Act
        var result = Result<TestEntity>.Information(entity, InformationCode.Retrieved);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsInformation.Should().BeTrue();
        result.Value.Should().Be(entity);
    }

    #endregion

    #region Implicit Operators

    [Fact]
    public void ImplicitOperator_FromValue_ShouldCreateSuccessResult()
    {
        // Arrange
        var entity = new TestEntity(1, "Test");

        // Act
        Result<TestEntity> result = entity;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(entity);
    }

    #endregion

    #region GetValueOrDefault / GetValueOrThrow

    [Fact]
    public void GetValueOrDefault_WhenSuccess_ShouldReturnValue()
    {
        // Arrange
        var entity = new TestEntity(1, "Test");
        var result = Result<TestEntity>.Success(entity);
        var defaultEntity = new TestEntity(0, "Default");

        // Act
        var value = result.GetValueOrDefault(defaultEntity);

        // Assert
        value.Should().Be(entity);
    }

    [Fact]
    public void GetValueOrDefault_WhenError_ShouldReturnDefault()
    {
        // Arrange
        var result = Result<TestEntity>.Error(ErrorCode.NotFound);
        var defaultEntity = new TestEntity(0, "Default");

        // Act
        var value = result.GetValueOrDefault(defaultEntity);

        // Assert
        value.Should().Be(defaultEntity);
    }

    [Fact]
    public void GetValueOrThrow_WhenSuccess_ShouldReturnValue()
    {
        // Arrange
        var entity = new TestEntity(1, "Test");
        var result = Result<TestEntity>.Success(entity);

        // Act
        var value = result.GetValueOrThrow();

        // Assert
        value.Should().Be(entity);
    }

    [Fact]
    public void GetValueOrThrow_WhenError_ShouldThrowException()
    {
        // Arrange
        var result = Result<TestEntity>.Error(ErrorCode.NotFound);

        // Act & Assert
        var act = () => result.GetValueOrThrow();
        act.Should().Throw<InvalidOperationException>();
    }

    #endregion

    #region Match Methods

    [Fact]
    public void Match_WhenSuccess_ShouldExecuteOnSuccess()
    {
        // Arrange
        var entity = new TestEntity(1, "Test");
        var result = Result<TestEntity>.Success(entity);

        // Act
        var matched = result.Match(
            onSuccess: e => $"Found: {e.Name}",
            onFailure: info => $"Error: {info.Message}");

        // Assert
        matched.Should().Be("Found: Test");
    }

    [Fact]
    public void Match_WhenError_ShouldExecuteOnFailure()
    {
        // Arrange
        var result = Result<TestEntity>.Error(ErrorCode.NotFound, "Entity not found");

        // Act
        var matched = result.Match(
            onSuccess: e => $"Found: {e.Name}",
            onFailure: info => $"Error: {info.Message}");

        // Assert
        matched.Should().Be("Error: Entity not found");
    }

    #endregion

    #region Map / Bind Methods

    [Fact]
    public void Map_WhenSuccess_ShouldTransformValue()
    {
        // Arrange
        var entity = new TestEntity(1, "Test");
        var result = Result<TestEntity>.Success(entity);

        // Act
        var mapped = result.Map(e => e.Name.ToUpper());

        // Assert
        mapped.IsSuccess.Should().BeTrue();
        mapped.Value.Should().Be("TEST");
    }

    [Fact]
    public void Map_WhenError_ShouldPropagateError()
    {
        // Arrange
        var result = Result<TestEntity>.Error(ErrorCode.NotFound);

        // Act
        var mapped = result.Map(e => e.Name.ToUpper());

        // Assert
        mapped.IsError.Should().BeTrue();
        mapped.ResultData.DetailCode.Should().Be(ErrorCode.NotFound);
    }

    [Fact]
    public void Bind_WhenSuccess_ShouldChainResults()
    {
        // Arrange
        var entity = new TestEntity(1, "Test");
        var result = Result<TestEntity>.Success(entity);

        // Act
        var bound = result.Bind(e => Result<string>.Success(e.Name.ToUpper()));

        // Assert
        bound.IsSuccess.Should().BeTrue();
        bound.Value.Should().Be("TEST");
    }

    [Fact]
    public void Bind_WhenFirstError_ShouldNotExecuteBinder()
    {
        // Arrange
        var result = Result<TestEntity>.Error(ErrorCode.NotFound);
        var binderCalled = false;

        // Act
        var bound = result.Bind(e =>
        {
            binderCalled = true;
            return Result<string>.Success(e.Name);
        });

        // Assert
        bound.IsError.Should().BeTrue();
        binderCalled.Should().BeFalse();
    }

    #endregion

    #region OnSuccess / OnError Methods

    [Fact]
    public void OnSuccess_WhenSuccess_ShouldExecuteAction()
    {
        // Arrange
        var entity = new TestEntity(1, "Test");
        var result = Result<TestEntity>.Success(entity);
        var executed = false;

        // Act
        result.OnSuccess(e => executed = true);

        // Assert
        executed.Should().BeTrue();
    }

    [Fact]
    public void OnSuccess_WhenError_ShouldNotExecuteAction()
    {
        // Arrange
        var result = Result<TestEntity>.Error(ErrorCode.NotFound);
        var executed = false;

        // Act
        result.OnSuccess(e => executed = true);

        // Assert
        executed.Should().BeFalse();
    }

    [Fact]
    public void OnError_WhenError_ShouldExecuteAction()
    {
        // Arrange
        var result = Result<TestEntity>.Error(ErrorCode.NotFound);
        var executed = false;

        // Act
        result.OnError(info => executed = true);

        // Assert
        executed.Should().BeTrue();
    }

    #endregion

    #region FromDetailCode / FromException

    [Fact]
    public void FromDetailCode_WhenSuccess_ShouldReturnSuccessWithValue()
    {
        // Arrange
        var entity = new TestEntity(1, "Test");

        // Act
        var result = Result<TestEntity>.FromDetailCode(SuccessCode.Ok, entity);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(entity);
    }

    [Fact]
    public void FromDetailCode_WhenError_ShouldReturnError()
    {
        // Act
        var result = Result<TestEntity>.FromDetailCode(ErrorCode.NotFound, null);

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
        var result = Result<TestEntity>.FromException(exception);

        // Assert
        result.IsError.Should().BeTrue();
        result.Message.Should().Contain("Test error");
    }

    #endregion

    #region ToString

    [Fact]
    public void ToString_WhenSuccess_ShouldIncludeValue()
    {
        // Arrange
        var entity = new TestEntity(1, "Test");
        var result = Result<TestEntity>.Success(entity);

        // Act
        var str = result.ToString();

        // Assert
        str.Should().Contain("Success");
        str.Should().Contain("Test");
    }

    [Fact]
    public void ToString_WhenError_ShouldContainFailure()
    {
        // Arrange
        var result = Result<TestEntity>.Error(ErrorCode.NotFound, "Entity not found");

        // Act
        var str = result.ToString();

        // Assert
        str.Should().Contain("Failure");
    }

    #endregion
}
