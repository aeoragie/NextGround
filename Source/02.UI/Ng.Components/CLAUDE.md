# Ng.Components

Blazor UI 컴포넌트를 담당하는 Razor Class Library 프로젝트

## 프로젝트 개요

- **역할**: 재사용 가능한 Blazor 컴포넌트 라이브러리 (디자인 원천)
- **프레임워크**: .NET 9.0
- **SDK**: Microsoft.NET.Sdk.Razor
- **의존성**: Microsoft.AspNetCore.Components.Web

## 주요 구성 요소

### Components (예정)
- 공통 UI 컴포넌트
- 폼 컴포넌트
- 레이아웃 컴포넌트
- 네비게이션 컴포넌트

### Styles (예정)
- CSS 스타일시트
- CSS 변수 정의
- 테마 설정

### JavaScript Interop
- ExampleJsInterop.cs - JS 상호작용 예시

### Static Assets (예정)
- 이미지
- 폰트
- JavaScript 파일

## Blazor 컴포넌트 규칙

- 파일명: PascalCase (예: `PlayerCard.razor`)
- 컴포넌트 구조: 마크업 → @code 블록 순서
- 매개변수: `[Parameter]` 특성 사용
- 이벤트: `EventCallback<T>` 사용
- CSS 격리: `ComponentName.razor.css` 사용

## 컴포넌트 작성 예시

```razor
@* PlayerCard.razor *@
<div class="player-card">
    <h3>@Name</h3>
    <p>@Position</p>
    <button @onclick="OnSelect">선택</button>
</div>

@code {
    [Parameter]
    public string Name { get; set; } = string.Empty;

    [Parameter]
    public string Position { get; set; } = string.Empty;

    [Parameter]
    public EventCallback OnSelect { get; set; }
}
```

## 코딩 규칙

- 루트 CLAUDE.md의 코딩 컨벤션 준수
- 컴포넌트는 단일 책임 원칙 적용
- 복잡한 로직은 서비스로 분리
- 접근성(a11y) 고려한 마크업 작성

## 빌드

```bash
dotnet build Source/02.UI/Ng.Components/Ng.Components.csproj
```
