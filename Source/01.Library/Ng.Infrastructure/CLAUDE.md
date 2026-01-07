# Ng.Infrastructure

EF Core 및 외부 서비스 연동을 담당하는 인프라스트럭처 레이어 프로젝트

## 프로젝트 개요

- **역할**: 데이터베이스 연동, 외부 서비스 통합, Repository 구현
- **프레임워크**: .NET 9.0
- **의존성**: Ng.Domain, Ng.Application 참조

## 주요 구성 요소

### Data (예정)
- DbContext 구현
- Entity Configuration
- Migrations

### Repositories (예정)
- Repository 패턴 구현
- Unit of Work 패턴

### External Services (예정)
- 이메일 서비스
- 파일 스토리지 서비스
- 외부 API 클라이언트

### Configurations (예정)
- Entity Framework 설정
- 외부 서비스 설정

## EF Core 규칙

- Fluent API로 엔티티 설정
- Migration은 별도 Database 프로젝트에서 관리
- DbContext는 Scoped 라이프타임 사용

## 코딩 규칙

- 루트 CLAUDE.md의 코딩 컨벤션 준수
- Repository 인터페이스는 Application 레이어에 정의
- 구현체만 Infrastructure에 위치
- 외부 서비스 호출 시 Polly로 Retry 패턴 적용 권장

## 빌드

```bash
dotnet build Source/01.Library/Ng.Infrastructure/Ng.Infrastructure.csproj
```
