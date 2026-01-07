# Ng.Server

REST API 서버 프로젝트

## 프로젝트 개요

- **역할**: 백엔드 API 서버, 비즈니스 로직 API 제공
- **프레임워크**: .NET 9.0, ASP.NET Core Web API
- **SDK**: Microsoft.NET.Sdk.Web
- **의존성**: Solution.ServiceDefaults, Microsoft.AspNetCore.OpenApi

## 주요 기능 (예정)

### API 엔드포인트
- 선수 관리 API
- 클럽 관리 API
- 스카우터 관리 API
- 매칭 관리 API
- 인증/인가 API

### 미들웨어 (예정)
- 인증 미들웨어
- 에러 핸들링 미들웨어
- 로깅 미들웨어
- CORS 설정

### OpenAPI/Swagger
- API 문서 자동 생성
- Swagger UI 제공

## 프로젝트 구조

```
Ng.Server/
├── Controllers/
│   └── WeatherForecastController.cs
├── Program.cs
├── WeatherForecast.cs
└── appsettings.json
```

## API 설계 규칙

- RESTful API 원칙 준수
- 버전 관리: `/api/v1/...`
- 응답 형식: JSON
- HTTP 상태 코드 적절히 사용

## 컨트롤러 작성 규칙

```csharp
[ApiController]
[Route("api/v1/[controller]")]
public class PlayersController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerDto>>> GetAll()
    {
        // 구현
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerDto>> GetById(int id)
    {
        // 구현
    }
}
```

## 코딩 규칙

- 루트 CLAUDE.md의 코딩 컨벤션 준수
- 컨트롤러는 얇게 유지 (로직은 Application 레이어에 위임)
- DTO를 통한 데이터 전송
- 유효성 검증은 FluentValidation 권장

## 실행

```bash
dotnet run --project Source/03.Web/Ng.Server/Ng.Server.csproj
```

## 빌드

```bash
dotnet build Source/03.Web/Ng.Server/Ng.Server.csproj
```
