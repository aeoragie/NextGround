# Ng.Domain

핵심 비즈니스 로직을 담당하는 도메인 레이어 프로젝트

## 프로젝트 개요

- **역할**: 도메인 모델, 엔티티, 비즈니스 규칙 정의
- **프레임워크**: .NET 9.0
- **의존성**: 없음 (Clean Architecture 원칙에 따라 외부 의존성 최소화)

## 주요 구성 요소

### Entities (예정)
- 선수(Player) 엔티티
- 클럽(Club) 엔티티
- 스카우터(Scout) 엔티티
- 학부모(Parent) 엔티티

### Value Objects (예정)
- 도메인 값 객체들

### Domain Events (예정)
- 도메인 이벤트 정의

### Interfaces (예정)
- Repository 인터페이스
- Domain Service 인터페이스

## 코딩 규칙

- 루트 CLAUDE.md의 코딩 컨벤션 준수
- 엔티티는 반드시 private setter 사용
- 비즈니스 규칙은 엔티티 내부에 캡슐화
- 외부 프레임워크 의존성 금지

## 빌드

```bash
dotnet build Source/01.Library/Ng.Domain/Ng.Domain.csproj
```
