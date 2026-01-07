# Ng.Shared

DTO 및 공통 상수를 담당하는 공유 라이브러리 프로젝트

## 프로젝트 개요

- **역할**: 프로젝트 전체에서 공유되는 DTO, 상수, 유틸리티
- **프레임워크**: .NET 9.0
- **의존성**: 없음 (독립적인 공유 라이브러리)

## 주요 구성 요소

### DTOs (예정)
- API Request/Response DTO
- View Models
- Transfer Objects

### Constants (예정)
- 애플리케이션 상수
- 에러 코드
- 메시지 상수

### Enums (예정)
- 스포츠 종류 (SportType)
- 사용자 역할 (UserRole)
- 매칭 상태 (MatchStatus)

### Extensions (예정)
- 문자열 확장 메서드
- 컬렉션 확장 메서드
- 날짜/시간 확장 메서드

### Utilities (예정)
- 공통 유틸리티 클래스
- 헬퍼 메서드

## 코딩 규칙

- 루트 CLAUDE.md의 코딩 컨벤션 준수
- DTO는 불변 객체로 설계 (record 타입 권장)
- 상수는 static readonly 또는 const 사용
- 모든 public 멤버에 XML 주석 권장

## 빌드

```bash
dotnet build Source/01.Library/Ng.Shared/Ng.Shared/Ng.Shared.csproj
```
