# Ng.Web.Portal

선수/학부모용 포털 웹 애플리케이션

## 프로젝트 개요

- **역할**: 선수 및 학부모를 위한 포털 서비스
- **프레임워크**: .NET 9.0, Blazor WebAssembly (Auto 모드)
- **SDK**: Microsoft.NET.Sdk.Web
- **의존성**: Solution.ServiceDefaults, Ng.Web.Portal.Client

## 프로젝트 구성

Blazor Auto 렌더링 모드를 사용하여 서버와 클라이언트 프로젝트로 분리:

- **Ng.Web.Portal** (Server): 서버 사이드 렌더링 및 API 호스팅
- **Ng.Web.Portal.Client** (Client): WebAssembly 클라이언트

## 주요 기능 (예정)

### 선수 기능
- 프로필 등록 및 관리
- 하이라이트 영상 업로드
- 스카우트 제안 확인
- 매칭 요청 응답

### 학부모 기능
- 자녀 선수 프로필 관리
- 스카우트 제안 검토
- 클럽 정보 조회
- 계약 관련 알림

### 공통 기능
- 사용자 인증/인가
- 알림 센터
- 메시지 기능

## 프로젝트 구조

```
Ng.Web.Portal/
├── Ng.Web.Portal/          (Server)
│   ├── Components/
│   │   ├── App.razor
│   │   ├── Routes.razor
│   │   ├── Layout/
│   │   └── Pages/
│   └── Program.cs
└── Ng.Web.Portal.Client/   (Client)
    ├── _Imports.razor
    └── Program.cs
```

## Blazor Auto 모드 특성

- 초기 로딩: 서버 사이드 렌더링 (빠른 초기 로딩)
- 이후 상호작용: WebAssembly (오프라인 지원 가능)
- 점진적 향상 지원

## 코딩 규칙

- 루트 CLAUDE.md의 코딩 컨벤션 준수
- 서버/클라이언트 공유 코드는 별도 공유 프로젝트 활용
- 클라이언트 전용 코드는 Client 프로젝트에 배치
- 서버 전용 코드는 Server 프로젝트에 배치

## 실행

```bash
dotnet run --project Source/03.Web/Ng.Web.Portal/Ng.Web.Portal/Ng.Web.Portal.csproj
```

## 빌드

```bash
dotnet build Source/03.Web/Ng.Web.Portal/Ng.Web.Portal/Ng.Web.Portal.csproj
```
