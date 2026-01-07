# Ng.Application

유즈케이스와 애플리케이션 로직을 담당하는 레이어 프로젝트

## 프로젝트 개요

- **역할**: 유즈케이스 구현, 매칭 로직, 애플리케이션 서비스
- **프레임워크**: .NET 9.0
- **의존성**: Ng.Domain 참조

## 주요 구성 요소

### Use Cases (예정)
- 선수 매칭 유즈케이스
- 스카우트 검색 유즈케이스
- 프로필 관리 유즈케이스

### Application Services (예정)
- 매칭 서비스
- 알림 서비스
- 검색 서비스

### DTOs (예정)
- Command/Query 객체
- Response 객체

### Interfaces (예정)
- 외부 서비스 인터페이스
- Repository 인터페이스 (Domain에서 상속)

## CQRS 패턴

- Command: 상태 변경 작업
- Query: 조회 작업
- Handler: 각 Command/Query 처리

## 코딩 규칙

- 루트 CLAUDE.md의 코딩 컨벤션 준수
- 유즈케이스별 단일 책임 원칙 적용
- MediatR 패턴 활용 권장
- 비즈니스 로직은 Domain 레이어에 위임

## 빌드

```bash
dotnet build Source/01.Library/Ng.Application/Ng.Application.csproj
```
