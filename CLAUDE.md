# NextGround 프로젝트

축구 선수 스카우팅 및 매칭 플랫폼

## 프로젝트 구조

```
Source/
├── 01.Library/                    (핵심 라이브러리)
│   ├── Ng.Domain/                 핵심 비즈니스 로직, 엔티티
│   ├── Ng.Application/            유즈케이스, 매칭 로직
│   ├── Ng.Infrastructure/         EF Core, DB 연동
│   └── Ng.Shared/                 DTO, 공통 상수
│
├── 02.UI/                         (Blazor UI)
│   └── Ng.Components/             Razor Class Library (디자인 원천)
│
├── 03.Web/                        (웹 애플리케이션)
│   ├── Ng.Web.Admin/              스카우터/클럽용 대시보드 (Blazor Server)
│   ├── Ng.Web.Portal/             선수/학부모용 포털 (Blazor Auto)
│   └── Ng.Server/                 API Server
│
├── 05.Tools/                      (개발 도구)
│   └── Generator.Database/        데이터베이스 생성기
│
├── 99.Tests/                      (테스트)
│   ├── Ng.Tests.Unit/             단위 테스트
│   └── Ng.Tests.Integration/      통합 테스트
│
├── Solution.AppHost/              .NET Aspire 호스트
└── Solution.ServiceDefaults/      서비스 기본 설정
```

## 기술 스택

- **.NET 9.0**
- **Blazor** (Server / WebAssembly Auto)
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **.NET Aspire** (오케스트레이션)
- **xUnit, Moq, FluentAssertions** (테스트)

## 아키텍처

Clean Architecture 기반:
- **Domain**: 비즈니스 로직 (의존성 없음)
- **Application**: 유즈케이스 (Domain 참조)
- **Infrastructure**: 외부 연동 (Domain, Application 참조)
- **Presentation**: Web UI, API (모든 레이어 참조)

---

# 코딩 컨벤션

## C# 코딩 스타일

### 네이밍 규칙

- **클래스, 메서드, 속성**: PascalCase (예: `CustomerService`, `GetCustomerById`)
- **변수, 필드**: camelCase (예: `customerId`, `orderItems`)
- **상수**: PascalCase (예: `DefaultPageSize`, `CacheKeyPrefix`)
- **private 멤버 변수**: `m` 접두사 + PascalCase (예: `mConnectionString`, `mIsInitialized`)
- **readonly 필드**: PascalCase (예: `Repository`, `Logger`)
- **static 멤버 변수**: PascalCase (예: `Repository`, `Logger`)

### 포매팅 규칙

- **들여쓰기**: 4개 공백
- **중괄호**: Java, JavaScript 스타일
- **using 문**: 파일 상단에 배치, System using 문 먼저
- **LINQ 체이닝**: 메서드 체이닝 시 각 메서드는 새 줄에 작성, 들여쓰기는 첫 번째 메서드와 동일한 레벨 유지

### 예외 처리 스타일

- **try-catch 블록**: catch는 새로운 줄에 작성 (JavaScript, Java 스타일)

```csharp
// 올바른 스타일
try 
{
    // 코드 실행
} 
catch (Exception ex) 
{
    // 예외 처리
}

// 잘못된 스타일 - 한 줄에 작성하지 않음
try {
    // 코드 실행
} catch (Exception ex) {
    // 예외 처리
}
```

### Debug.Assert 사용 규칙

- **조건 검증**: 모든 중요한 조건에서 Debug.Assert() 사용
- **예외 상황**: 예상하지 못한 상황에서 Debug.Assert(false, "설명") 사용
- **null 체크**: 중요한 객체의 null 체크 후 Assert 추가

```csharp
// 조건 검증 예시
public void ProcessData(string data) 
{
    Debug.Assert(!string.IsNullOrEmpty(data), "Data cannot be null or empty");
  
    if (string.IsNullOrEmpty(data)) 
    {
        throw new ArgumentNullException(nameof(data));
    }
  
    // 처리 로직...
}

// 예외 상황 예시
PropertyInfo? prop = type.GetProperty(name);
if (prop == null) 
{
    Debug.Assert(false, $"Property '{name}' not found in type '{type.Name}'");
    return null;
}
```

### 코드 예시

- **주석**: 한글로 작성, 메서드 및 클래스에 대한 설명 포함 되도록이면 간결하도록 함수 클래스 변수 명을 보고 알 수있으면 주석 필요 없음
- **예외**: `try-catch` 블록 사용, 예외 메시지에 상세 정보 포함 영어로 작성
- **로그**: `ILogger` NLog 기본 사용, 예외 발생 시 로그 기록 로그 메세지는 영어로 작성

### LINQ 체이닝 스타일

```csharp
// 올바른 스타일 - 각 메서드를 새 줄에, 동일한 들여쓰기 레벨
public IReadOnlyList<HighlightContent> GetContentsBySport(SportType sportType) 
{
    return Contents.Where(c => c.SportType == sportType && c.IsActive)
        .OrderByDescending(c => c.Priority)
        .ThenByDescending(c => c.PublishedDate)
        .ToList()
        .AsReadOnly();
}

// 잘못된 스타일 - 계속 들여쓰기가 증가
public IReadOnlyList<HighlightContent> GetRecentContents(int count = 4) 
{
    return Contents.Where(c => c.IsActive)
                  .OrderByDescending(c => c.Priority)  // X - 잘못된 들여쓰기
                  .ThenByDescending(c => c.PublishedDate)
                  .Take(count)
                  .ToList()
                  .AsReadOnly();
}
```

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace YourNamespace.Services 
{
    public class CustomerService 
    {
        private readonly ICustomerRepository Repository;
        private readonly ILogger<CustomerService> Logger;
        private readonly int MaxRetryCount;
      
        private const int DefaultPageSize = 20;
        private const string CacheKeyPrefix = "CUSTOMER_";
        private const double TaxRate = 0.08;
      
        private string mConnectionString;
        private bool mIsInitialized;

        public CustomerService(ICustomerRepository repository, ILogger<CustomerService> logger, IConfiguration configuration) 
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            MaxRetryCount = configuration.GetValue<int>("MaxRetryCount");
            mConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Customer> GetCustomerByIdAsync(int customerId) 
        {
            if (customerId <= 0) 
            {
                throw new ArgumentException("Customer ID must be greater than zero", nameof(customerId));
            }

            string cacheKey = $"{CacheKeyPrefix}{customerId}";
          
            try 
            {
                var customer = await Repository.GetByIdAsync(customerId);
                if (customer == null) 
                {
                    Debug.Assert(false, $"Customer not found: {customerId}");
                    throw new CustomerNotFoundException($"Customer with ID {customerId} was not found");
                }

                Debug.Assert(customer.Id == customerId, "Retrieved customer ID does not match requested ID");
                return customer;
            } 
            catch (DataAccessException ex) 
            {
                Logger.LogError(ex, "Failed to retrieve customer with ID: {CustomerId}", customerId);
                throw new ServiceException($"Error retrieving customer {customerId}", ex);
            }
        }
    }
}
```

## Blazor 컴포넌트

- **파일명**: PascalCase로 명명 (예: `LandingBenefitsCard.razor`)
- **컴포넌트 구조**: 마크업, 코드블록 순서로 배치
- **매개변수**: `[Parameter]` 특성과 함께 public 속성으로 정의
- **이벤트 콜백**: `EventCallback` 또는 `EventCallback<T>` 사용

## 데이터베이스

- **테이블명**: PascalCase 복수형 (예: `BenefitSections`)
- **컬럼명**: PascalCase 단수형
- **프로시저**: `Usp` 접두사 사용 (User Stored Procedure)

## 빌드 & 테스트

- **빌드**: `dotnet build`
- **실행**: `dotnet run`
- **테스트**: `dotnet test`
- **복원**: `dotnet restore`

## 중요 규칙

- **민감정보 로깅 금지**: 패스워드, API 키 등 민감한 정보 로그 기록 금지
- **보안 모범사례 준수**: SQL Injection, XSS 방지 등
- **기존 코드 패턴 따르기**: 프로젝트 내 일관성 유지
- **null 체크 필수**: ArgumentNullException 및 Debug.Assert 사용
- **예외 처리 철저히**: try-catch 블록과 Debug.Assert 조합 사용
- **Debug.Assert 필수**: 조건 검증, 예외 상황, 데이터 무결성 검증에 사용

## 코딩 품질 규칙

- **모든 public 메서드**: 매개변수 유효성 검증 + Debug.Assert
- **모든 예외 케이스**: Debug.Assert(false, "상황 설명") 추가
- **모든 중요한 조건**: Debug.Assert로 조건 확인
- **try-catch 스타일**: catch는 반드시 새 줄에 작성
- **리플렉션 사용 시**: 각 단계마다 null 체크 + Debug.Assert

## Claude 작업 규칙

- **빌드 및 에러 수정**: 코드 작업 후 빌드 및 에러 수정은 사용자가 직접 수행 (Claude가 하지 않음)
