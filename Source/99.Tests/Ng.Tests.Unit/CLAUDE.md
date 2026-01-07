# Ng.Tests.Unit

단위 테스트 프로젝트

## 프로젝트 개요

- **역할**: Domain, Application 레이어의 단위 테스트
- **프레임워크**: .NET 9.0
- **테스트 프레임워크**: xUnit
- **의존성**: Ng.Domain, Ng.Application

## 테스트 라이브러리

- **xUnit**: 테스트 프레임워크
- **Moq**: 목(Mock) 객체 생성
- **FluentAssertions**: 가독성 높은 단언문
- **coverlet.collector**: 코드 커버리지

## 테스트 대상

### Domain 테스트
- 엔티티 비즈니스 로직
- Value Object 동등성
- 도메인 서비스

### Application 테스트
- 유즈케이스 로직
- Command/Query 핸들러
- 애플리케이션 서비스

## 테스트 작성 규칙

### 네이밍 규칙
```
{테스트대상}_{시나리오}_{예상결과}
```

예시:
```csharp
public class PlayerTests
{
    [Fact]
    public void UpdateProfile_WithValidData_ShouldUpdateSuccessfully()
    {
        // Arrange
        // Act
        // Assert
    }
}
```

### AAA 패턴
```csharp
[Fact]
public void CalculateMatchScore_WithHighCompatibility_ShouldReturnHighScore()
{
    // Arrange - 테스트 준비
    var player = new Player("홍길동", "FW");
    var club = new Club("FC Seoul");

    // Act - 실행
    var score = MatchingService.Calculate(player, club);

    // Assert - 검증
    score.Should().BeGreaterThan(80);
}
```

### FluentAssertions 사용
```csharp
// 기본 단언
result.Should().Be(expected);
result.Should().NotBeNull();

// 컬렉션 단언
list.Should().HaveCount(3);
list.Should().Contain(item);

// 예외 단언
action.Should().Throw<ArgumentException>()
    .WithMessage("Invalid argument");
```

## 코딩 규칙

- 루트 CLAUDE.md의 코딩 컨벤션 준수
- 테스트 클래스는 `{대상클래스}Tests` 명명
- 하나의 테스트는 하나의 동작만 검증
- 외부 의존성은 Mock으로 대체

## 테스트 실행

```bash
# 전체 단위 테스트 실행
dotnet test Source/99.Tests/Ng.Tests.Unit/Ng.Tests.Unit.csproj

# 특정 테스트 실행
dotnet test --filter "FullyQualifiedName~PlayerTests"

# 커버리지 포함 실행
dotnet test --collect:"XPlat Code Coverage"
```

## 빌드

```bash
dotnet build Source/99.Tests/Ng.Tests.Unit/Ng.Tests.Unit.csproj
```
