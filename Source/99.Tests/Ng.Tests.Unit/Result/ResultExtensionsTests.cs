using FluentAssertions;
using Ng.Shared.Result;

namespace Ng.Tests.Unit.Result;

public class ResultExtensionsTests
{
    #region Test Models

    private record TestEntity(int Id, string Name);
    private record TestDto(string DisplayName);

    #endregion

    #region ToHttpResponse Tests

    [Fact]
    public void ToHttpResponse_WhenSuccess_ShouldReturn200()
    {
        // Arrange
        var entity = new TestEntity(1, "Test");
        var result = Result<TestEntity>.Success(entity);

        // Act
        var response = result.ToHttpResponse();

        // Assert
        response.StatusCode.Should().Be(200);
        response.IsSuccess.Should().BeTrue();
        response.Value.Should().Be(entity);
    }

    [Fact]
    public void ToHttpResponse_WhenNotFound_ShouldReturn404()
    {
        // Arrange
        var result = Result<TestEntity>.Error(ErrorCode.NotFound);

        // Act
        var response = result.ToHttpResponse();

        // Assert
        response.StatusCode.Should().Be(404);
        response.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void ToHttpResponse_NonGeneric_WhenSuccess_ShouldReturn200()
    {
        // Arrange
        var result = Ng.Shared.Result.Result.Success();

        // Act
        var response = result.ToHttpResponse();

        // Assert
        response.StatusCode.Should().Be(200);
        response.IsSuccess.Should().BeTrue();
    }

    #endregion

    #region ToLogInfo Tests

    [Fact]
    public void ToLogInfo_WhenError_ShouldReturnCorrectLevel()
    {
        // Arrange
        var result = Result<TestEntity>.Error(ErrorCode.DatabaseError);

        // Act
        var logInfo = result.ToLogInfo("TestOperation");

        // Assert
        logInfo.Level.Should().Be("Error");
        logInfo.Category.Should().Be("system_error");
        logInfo.OperationName.Should().Be("TestOperation");
        logInfo.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void ToLogInfo_WhenSuccess_ShouldReturnInformationLevel()
    {
        // Arrange
        var entity = new TestEntity(1, "Test");
        var result = Result<TestEntity>.Success(entity);

        // Act
        var logInfo = result.ToLogInfo("TestOperation");

        // Assert
        logInfo.Level.Should().Be("Information");
        logInfo.Category.Should().Be("success");
        logInfo.IsSuccess.Should().BeTrue();
    }

    #endregion

    #region ToMetricInfo Tests

    [Fact]
    public void ToMetricInfo_ShouldReturnCorrectMetricData()
    {
        // Arrange
        var entity = new TestEntity(1, "Test");
        var result = Result<TestEntity>.Success(entity);
        var duration = TimeSpan.FromMilliseconds(100);

        // Act
        var metricInfo = result.ToMetricInfo("TestOperation", duration);

        // Assert
        metricInfo.OperationName.Should().Be("TestOperation");
        metricInfo.Category.Should().Be("success");
        metricInfo.IsSuccess.Should().BeTrue();
        metricInfo.Duration.Should().Be(duration);
    }

    #endregion

    #region OnSuccessAsync / OnErrorAsync Tests

    [Fact]
    public async Task OnSuccessAsync_WhenSuccess_ShouldExecuteAction()
    {
        // Arrange
        var entity = new TestEntity(1, "Test");
        var result = Result<TestEntity>.Success(entity);
        var actionCalled = false;

        // Act
        await result.OnSuccessAsync(async e =>
        {
            await Task.Delay(1);
            actionCalled = true;
        });

        // Assert
        actionCalled.Should().BeTrue();
    }

    [Fact]
    public async Task OnSuccessAsync_WhenError_ShouldNotExecuteAction()
    {
        // Arrange
        var result = Result<TestEntity>.Error(ErrorCode.NotFound);
        var actionCalled = false;

        // Act
        await result.OnSuccessAsync(async e =>
        {
            await Task.Delay(1);
            actionCalled = true;
        });

        // Assert
        actionCalled.Should().BeFalse();
    }

    [Fact]
    public async Task OnErrorAsync_WhenError_ShouldExecuteAction()
    {
        // Arrange
        var result = Result<TestEntity>.Error(ErrorCode.NotFound);
        var actionCalled = false;

        // Act
        await result.OnErrorAsync(async info =>
        {
            await Task.Delay(1);
            actionCalled = true;
        });

        // Assert
        actionCalled.Should().BeTrue();
    }

    #endregion

    #region IsRetryable / IsUserFriendly Tests

    [Fact]
    public void IsRetryable_WhenNetworkTimeout_ShouldReturnTrue()
    {
        // Arrange
        var result = Result<TestEntity>.Error(ErrorCode.NetworkTimeout);

        // Act
        var isRetryable = result.IsRetryable();

        // Assert
        isRetryable.Should().BeTrue();
    }

    [Fact]
    public void IsRetryable_WhenInvalidInput_ShouldReturnFalse()
    {
        // Arrange
        var result = Result<TestEntity>.Error(ErrorCode.InvalidInput);

        // Act
        var isRetryable = result.IsRetryable();

        // Assert
        isRetryable.Should().BeFalse();
    }

    [Fact]
    public void IsUserFriendly_WhenUserError_ShouldReturnTrue()
    {
        // Arrange
        var result = Result<TestEntity>.Error(ErrorCode.InvalidInput);

        // Act
        var isUserFriendly = result.IsUserFriendly();

        // Assert
        isUserFriendly.Should().BeTrue();
    }

    #endregion

    #region MapWhenValue Tests

    [Fact]
    public void MapWhenValue_WhenSuccessWithValue_ShouldTransform()
    {
        // Arrange
        var entity = new TestEntity(1, "Test");
        var result = Result<TestEntity>.Success(entity);

        // Act
        var mapped = result.MapWhenValue(e => new TestDto(e.Name.ToUpper()));

        // Assert
        mapped.IsSuccess.Should().BeTrue();
        mapped.Value!.DisplayName.Should().Be("TEST");
    }

    [Fact]
    public void MapWhenValue_WhenError_ShouldPropagateError()
    {
        // Arrange
        var result = Result<TestEntity>.Error(ErrorCode.NotFound);

        // Act
        var mapped = result.MapWhenValue(e => new TestDto(e.Name));

        // Assert
        mapped.IsError.Should().BeTrue();
    }

    #endregion

    #region CombineAll / CombineAny Tests

    [Fact]
    public void CombineAll_AllSuccess_ShouldReturnSuccess()
    {
        // Arrange
        var result1 = Result<int>.Success(1);
        var result2 = Result<int>.Success(2);
        var result3 = Result<int>.Success(3);

        // Act
        var combined = ResultExtensions.CombineAll(result1, result2, result3);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }

    [Fact]
    public void CombineAll_OneError_ShouldReturnError()
    {
        // Arrange
        var result1 = Result<int>.Success(1);
        var result2 = Result<int>.Error(ErrorCode.NotFound);
        var result3 = Result<int>.Success(3);

        // Act
        var combined = ResultExtensions.CombineAll(result1, result2, result3);

        // Assert
        combined.IsError.Should().BeTrue();
    }

    [Fact]
    public void CombineAny_AllErrors_ShouldReturnFirstError()
    {
        // Arrange
        var result1 = Result<int>.Error(ErrorCode.NotFound);
        var result2 = Result<int>.Error(ErrorCode.InvalidInput);

        // Act
        var combined = ResultExtensions.CombineAny(result1, result2);

        // Assert
        combined.IsError.Should().BeTrue();
    }

    [Fact]
    public void CombineAny_OneSuccess_ShouldReturnSuccess()
    {
        // Arrange
        var result1 = Result<int>.Error(ErrorCode.NotFound);
        var result2 = Result<int>.Success(2);

        // Act
        var combined = ResultExtensions.CombineAny(result1, result2);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(2);
    }

    #endregion

    #region ResultAsync Tests

    [Fact]
    public async Task TryAsync_WhenNoException_ShouldReturnSuccess()
    {
        // Act
        var result = await ResultAsync.TryAsync(async () =>
        {
            await Task.Delay(1);
            return new TestEntity(1, "Test");
        });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(1);
    }

    [Fact]
    public async Task TryAsync_WhenException_ShouldReturnError()
    {
        // Act
        var result = await ResultAsync.TryAsync<TestEntity>(async () =>
        {
            await Task.Delay(1);
            throw new InvalidOperationException("Test error");
        });

        // Assert
        result.IsError.Should().BeTrue();
        result.Message.Should().Contain("Test error");
    }

    #endregion
}
