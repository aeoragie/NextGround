# Ng.Tests.Integration

통합 테스트 프로젝트

## 프로젝트 개요

- **역할**: API 서버 및 전체 시스템 통합 테스트
- **프레임워크**: .NET 9.0
- **테스트 프레임워크**: xUnit
- **의존성**: Ng.Server

## 테스트 라이브러리

- **xUnit**: 테스트 프레임워크
- **Moq**: 목(Mock) 객체 생성
- **FluentAssertions**: 가독성 높은 단언문
- **coverlet.collector**: 코드 커버리지

## 테스트 대상

### API 통합 테스트
- REST API 엔드포인트 테스트
- 요청/응답 검증
- HTTP 상태 코드 검증

### 데이터베이스 통합 테스트
- Repository 실제 동작 테스트
- 데이터 영속성 검증
- 트랜잭션 테스트

### End-to-End 테스트
- 전체 워크플로우 테스트
- 사용자 시나리오 테스트

## 테스트 작성 규칙

### WebApplicationFactory 사용
```csharp
public class PlayersApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient mClient;

    public PlayersApiTests(WebApplicationFactory<Program> factory)
    {
        mClient = factory.CreateClient();
    }

    [Fact]
    public async Task GetPlayers_ReturnsSuccessStatusCode()
    {
        // Arrange
        var url = "/api/v1/players";

        // Act
        var response = await mClient.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
```

### 테스트 데이터베이스 설정
```csharp
public class DatabaseFixture : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        // 테스트 DB 초기화
    }

    public async Task DisposeAsync()
    {
        // 테스트 DB 정리
    }
}
```

## 환경 설정

- 테스트용 appsettings.Testing.json 사용
- In-Memory 데이터베이스 또는 테스트 전용 DB 사용
- 외부 서비스는 Mock 또는 테스트 더블 사용

## 코딩 규칙

- 루트 CLAUDE.md의 코딩 컨벤션 준수
- 테스트 간 독립성 보장
- 테스트 데이터 정리 철저
- 느린 테스트는 별도 카테고리로 분류

## 테스트 실행

```bash
# 전체 통합 테스트 실행
dotnet test Source/99.Tests/Ng.Tests.Integration/Ng.Tests.Integration.csproj

# 특정 테스트 실행
dotnet test --filter "FullyQualifiedName~PlayersApiTests"

# 커버리지 포함 실행
dotnet test --collect:"XPlat Code Coverage"
```

## 빌드

```bash
dotnet build Source/99.Tests/Ng.Tests.Integration/Ng.Tests.Integration.csproj
```
