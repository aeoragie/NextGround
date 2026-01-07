# Ng.Web.Admin

스카우터/클럽용 관리자 대시보드 웹 애플리케이션

## 프로젝트 개요

- **역할**: 스카우터 및 클럽 관리자를 위한 대시보드
- **프레임워크**: .NET 9.0, Blazor Server
- **SDK**: Microsoft.NET.Sdk.Web
- **의존성**: Solution.ServiceDefaults

## 주요 기능 (예정)

### 스카우터 기능
- 선수 검색 및 필터링
- 선수 프로필 상세 조회
- 관심 선수 목록 관리
- 매칭 요청 관리

### 클럽 관리 기능
- 클럽 정보 관리
- 팀 구성 관리
- 스카우터 배정 관리
- 리크루팅 현황 대시보드

### 공통 기능
- 사용자 인증/인가
- 알림 관리
- 설정 관리

## 프로젝트 구조

```
Ng.Web.Admin/
├── Components/
│   ├── App.razor
│   ├── Routes.razor
│   ├── _Imports.razor
│   ├── Layout/
│   │   └── MainLayout.razor
│   └── Pages/
│       ├── Home.razor
│       └── Error.razor
├── Program.cs
└── wwwroot/
```

## Blazor Server 특성

- 서버 사이드 렌더링
- SignalR 연결 기반
- 실시간 데이터 업데이트 용이
- 보안에 민감한 관리자 기능에 적합

## 코딩 규칙

- 루트 CLAUDE.md의 코딩 컨벤션 준수
- 페이지 컴포넌트는 Pages 폴더에 배치
- 레이아웃 컴포넌트는 Layout 폴더에 배치
- 공통 컴포넌트는 Ng.Components 프로젝트 사용

## 실행

```bash
dotnet run --project Source/03.Web/Ng.Web.Admin/Ng.Web.Admin.csproj
```

## 빌드

```bash
dotnet build Source/03.Web/Ng.Web.Admin/Ng.Web.Admin.csproj
```
