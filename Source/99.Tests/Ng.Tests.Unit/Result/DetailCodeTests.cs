using FluentAssertions;
using Ng.Shared.Result;

namespace Ng.Tests.Unit.Result;

public class DetailCodeTests
{
    #region ErrorCode Tests

    [Fact]
    public void ErrorCode_ShouldHaveCorrectCategory()
    {
        // Assert
        ErrorCode.NotFound.Category.Should().Be(ResultCodes.Error);
        ErrorCode.NotFound.IsError.Should().BeTrue();
        ErrorCode.NotFound.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void ErrorCode_ShouldHaveCorrectValue()
    {
        // Assert
        ErrorCode.NotFound.Value.Should().Be(1200);
        ErrorCode.InvalidInput.Value.Should().Be(1000);
        ErrorCode.DatabaseError.Value.Should().Be(3000);
    }

    [Fact]
    public void ErrorCode_GetByValue_ShouldReturnCorrectCode()
    {
        // Act
        var code = ErrorCode.GetByValue(1200);

        // Assert
        code.Should().Be(ErrorCode.NotFound);
    }

    [Fact]
    public void ErrorCode_GetByValue_WhenNotFound_ShouldReturnNull()
    {
        // Act
        var code = ErrorCode.GetByValue(99999);

        // Assert
        code.Should().BeNull();
    }

    [Fact]
    public void ErrorCode_GetAll_ShouldReturnAllCodes()
    {
        // Act
        var codes = ErrorCode.GetAll().ToList();

        // Assert
        codes.Should().NotBeEmpty();
        codes.Should().Contain(ErrorCode.NotFound);
        codes.Should().Contain(ErrorCode.InvalidInput);
    }

    [Fact]
    public void ErrorCode_GetByRange_ShouldReturnCodesInRange()
    {
        // Act
        var clientErrors = ErrorCode.GetByRange(1000, 1099).ToList();

        // Assert
        clientErrors.Should().Contain(ErrorCode.InvalidInput);
        clientErrors.Should().Contain(ErrorCode.InvalidFormat);
        clientErrors.Should().NotContain(ErrorCode.NotFound);
    }

    [Fact]
    public void ErrorCode_IsClientError_ShouldIdentifyCorrectly()
    {
        // Assert - Client Error (1000-1099)
        ErrorCode.InvalidInput.IsClientError.Should().BeTrue();
        ErrorCode.BadRequest.IsClientError.Should().BeTrue();
        ErrorCode.NotFound.IsClientError.Should().BeFalse(); // Resource Error (1200-1299)
        ErrorCode.BusinessRuleViolation.IsClientError.Should().BeFalse();
        ErrorCode.DatabaseError.IsClientError.Should().BeFalse();
    }

    [Fact]
    public void ErrorCode_IsAuthError_ShouldIdentifyCorrectly()
    {
        // Assert - Auth Error (1100-1199)
        ErrorCode.Unauthorized.IsAuthError.Should().BeTrue();
        ErrorCode.Forbidden.IsAuthError.Should().BeTrue();
        ErrorCode.TokenExpired.IsAuthError.Should().BeTrue();
        ErrorCode.InvalidInput.IsAuthError.Should().BeFalse();
        ErrorCode.NotFound.IsAuthError.Should().BeFalse();
    }

    [Fact]
    public void ErrorCode_IsResourceError_ShouldIdentifyCorrectly()
    {
        // Assert - Resource Error (1200-1299)
        ErrorCode.NotFound.IsResourceError.Should().BeTrue();
        ErrorCode.AlreadyExists.IsResourceError.Should().BeTrue();
        ErrorCode.Conflict.IsResourceError.Should().BeTrue();
        ErrorCode.InvalidInput.IsResourceError.Should().BeFalse();
        ErrorCode.Unauthorized.IsResourceError.Should().BeFalse();
    }

    [Fact]
    public void ErrorCode_IsUserError_ShouldIdentifyCorrectly()
    {
        // Assert - User Error (1000-1299: Client + Auth + Resource)
        ErrorCode.InvalidInput.IsUserError.Should().BeTrue();
        ErrorCode.Unauthorized.IsUserError.Should().BeTrue();
        ErrorCode.NotFound.IsUserError.Should().BeTrue();
        ErrorCode.BusinessRuleViolation.IsUserError.Should().BeFalse();
        ErrorCode.DatabaseError.IsUserError.Should().BeFalse();
    }

    [Fact]
    public void ErrorCode_IsBusinessError_ShouldIdentifyCorrectly()
    {
        // Assert - Business Error (2000-2099)
        ErrorCode.BusinessRuleViolation.IsBusinessError.Should().BeTrue();
        ErrorCode.InvalidState.IsBusinessError.Should().BeTrue();
        ErrorCode.MatchAlreadyStarted.IsBusinessError.Should().BeFalse(); // Sports Error
        ErrorCode.InvalidInput.IsBusinessError.Should().BeFalse();
    }

    [Fact]
    public void ErrorCode_IsSportsError_ShouldIdentifyCorrectly()
    {
        // Assert - Sports Error (2100-2199)
        ErrorCode.MatchAlreadyStarted.IsSportsError.Should().BeTrue();
        ErrorCode.PlayerNotEligible.IsSportsError.Should().BeTrue();
        ErrorCode.BusinessRuleViolation.IsSportsError.Should().BeFalse();
        ErrorCode.InvalidInput.IsSportsError.Should().BeFalse();
    }

    [Fact]
    public void ErrorCode_IsBusinessLogicError_ShouldIdentifyCorrectly()
    {
        // Assert - Business Logic Error (2000-2199: Business + Sports)
        ErrorCode.BusinessRuleViolation.IsBusinessLogicError.Should().BeTrue();
        ErrorCode.MatchAlreadyStarted.IsBusinessLogicError.Should().BeTrue();
        ErrorCode.InvalidInput.IsBusinessLogicError.Should().BeFalse();
        ErrorCode.DatabaseError.IsBusinessLogicError.Should().BeFalse();
    }

    [Fact]
    public void ErrorCode_IsSystemError_ShouldIdentifyCorrectly()
    {
        // Assert
        ErrorCode.DatabaseError.IsSystemError.Should().BeTrue();
        ErrorCode.NetworkError.IsSystemError.Should().BeTrue();
        ErrorCode.InvalidInput.IsSystemError.Should().BeFalse();
    }

    [Fact]
    public void ErrorCode_IsRetryable_ShouldIdentifyCorrectly()
    {
        // Assert
        ErrorCode.NetworkTimeout.IsRetryable.Should().BeTrue();
        ErrorCode.DatabaseTimeout.IsRetryable.Should().BeTrue();
        ErrorCode.ServiceUnavailable.IsRetryable.Should().BeTrue();
        ErrorCode.InvalidInput.IsRetryable.Should().BeFalse();
    }

    [Fact]
    public void ErrorCode_IsCritical_ShouldIdentifyCorrectly()
    {
        // Assert
        ErrorCode.DatabaseError.IsCritical.Should().BeTrue();
        ErrorCode.InternalError.IsCritical.Should().BeTrue();
        ErrorCode.InvalidInput.IsCritical.Should().BeFalse();
    }

    #endregion

    #region SuccessCode Tests

    [Fact]
    public void SuccessCode_ShouldHaveCorrectCategory()
    {
        // Assert
        SuccessCode.Ok.Category.Should().Be(ResultCodes.Success);
        SuccessCode.Ok.IsSuccess.Should().BeTrue();
        SuccessCode.Ok.IsError.Should().BeFalse();
    }

    [Fact]
    public void SuccessCode_ShouldHaveCorrectValues()
    {
        // Assert
        SuccessCode.Ok.Value.Should().Be(0);
        SuccessCode.Created.Value.Should().Be(100);
        SuccessCode.Updated.Value.Should().Be(101);
    }

    [Fact]
    public void SuccessCode_GetByValue_ShouldReturnCorrectCode()
    {
        // Act
        var code = SuccessCode.GetByValue(100);

        // Assert
        code.Should().Be(SuccessCode.Created);
    }

    [Fact]
    public void SuccessCode_CategoryProperties_ShouldWorkCorrectly()
    {
        // Assert
        SuccessCode.Ok.IsBasicSuccess.Should().BeTrue();
        SuccessCode.Created.IsCrudOperation.Should().BeTrue();
        SuccessCode.Authenticated.IsAuthOperation.Should().BeTrue();
        SuccessCode.UserRegistered.IsUserOperation.Should().BeTrue();
        SuccessCode.FileUploaded.IsFileOperation.Should().BeTrue();
        SuccessCode.MessageSent.IsCommunicationOperation.Should().BeTrue();
        SuccessCode.MatchCreated.IsSportsOperation.Should().BeTrue();
        SuccessCode.ProcessStarted.IsProcessOperation.Should().BeTrue();
        SuccessCode.SystemStarted.IsSystemOperation.Should().BeTrue();
        SuccessCode.DataImported.IsDataOperation.Should().BeTrue();
    }

    #endregion

    #region WarningCode Tests

    [Fact]
    public void WarningCode_ShouldHaveCorrectCategory()
    {
        // Assert
        WarningCode.PartialSuccess.Category.Should().Be(ResultCodes.Warning);
        WarningCode.PartialSuccess.IsWarning.Should().BeTrue();
    }

    [Fact]
    public void WarningCode_CategoryProperties_ShouldWorkCorrectly()
    {
        // Assert
        WarningCode.DeprecatedFeature.IsGeneralWarning.Should().BeTrue();
        WarningCode.DataQualityIssue.IsDataWarning.Should().BeTrue();
        WarningCode.WeakPassword.IsSecurityWarning.Should().BeTrue();
        WarningCode.SlowResponse.IsPerformanceWarning.Should().BeTrue();
        WarningCode.BusinessRuleBypass.IsBusinessWarning.Should().BeTrue();
        WarningCode.MatchDataIncomplete.IsSportsWarning.Should().BeTrue();
        WarningCode.ServiceDegradation.IsSystemWarning.Should().BeTrue();
        WarningCode.ExternalServiceSlow.IsIntegrationWarning.Should().BeTrue();
        WarningCode.BrowserNotSupported.IsUserExperienceWarning.Should().BeTrue();
    }

    #endregion

    #region InformationCode Tests

    [Fact]
    public void InformationCode_ShouldHaveCorrectCategory()
    {
        // Assert
        InformationCode.Created.Category.Should().Be(ResultCodes.Information);
        InformationCode.Created.IsInformation.Should().BeTrue();
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void DetailCode_SameCode_ShouldBeEqual()
    {
        // Arrange
        var code1 = ErrorCode.NotFound;
        var code2 = ErrorCode.NotFound;

        // Assert
        code1.Should().Be(code2);
        (code1 == code2).Should().BeTrue();
        code1.Equals(code2).Should().BeTrue();
    }

    [Fact]
    public void DetailCode_DifferentCode_ShouldNotBeEqual()
    {
        // Arrange
        var code1 = ErrorCode.NotFound;
        var code2 = ErrorCode.InvalidInput;

        // Assert
        code1.Should().NotBe(code2);
        (code1 != code2).Should().BeTrue();
    }

    [Fact]
    public void DetailCode_GetHashCode_ShouldBeConsistent()
    {
        // Arrange
        var code1 = ErrorCode.NotFound;
        var code2 = ErrorCode.NotFound;

        // Assert
        code1.GetHashCode().Should().Be(code2.GetHashCode());
    }

    [Fact]
    public void DetailCode_ToString_ShouldReturnFormattedString()
    {
        // Act
        var str = ErrorCode.NotFound.ToString();

        // Assert
        str.Should().Contain("Error");
        str.Should().Contain("1200");
        str.Should().Contain("NotFound");
    }

    #endregion
}
