CREATE TABLE [dbo].[User]
(
    [UserId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [Email] VARCHAR(255) NOT NULL,
    [EmailConfirmed] BIT NOT NULL DEFAULT 0,
    [PasswordHash] VARCHAR(255) NULL,
    
    -- 소셜 로그인 지원
    [AuthProvider] VARCHAR(20) NOT NULL DEFAULT 'Local', 
    [ExternalId] VARCHAR(255) NULL, 

    [FullName] VARCHAR(100) NOT NULL, -- 한글/외국어 고려 VARCHAR
    [NickName] VARCHAR(50) NULL,
    [ProfileImageUrl] VARCHAR(2048) NULL,
    [PhoneNumber] VARCHAR(50) NULL,
    [Birthday] DATE NULL,
    [Gender] TINYINT NULL, -- 1: Male, 2: Female, 0: Unknown 등
    
    [CountryCode] CHAR(2) NULL,
    [LanguageCode] VARCHAR(10) NULL DEFAULT 'ko',
    [TimeZone] VARCHAR(50) NULL DEFAULT 'Korea Standard Time',

    [UserStatus] VARCHAR(20) NOT NULL DEFAULT 'Active', -- Active, Inactive, Blocked, Withdrawn
    [UserRole] VARCHAR(20) NOT NULL DEFAULT 'Player',

    -- 약관 동의 (버전 관리가 필요하다면 별도 테이블 권장)
    [AgreedToTermsAt] DATETIME2 NULL,
    [AgreedToMarketingAt] DATETIME2 NULL,
    [AgreedToPrivacyAt] DATETIME2 NULL,

    -- 감사 및 이력
    [LastLoginAt] DATETIME2 NULL,
    [LastLoginAddress] VARCHAR(45) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [DeletedAt] DATETIME2 NULL, 

    CONSTRAINT [PK_User] PRIMARY KEY ([UserId]),
    CONSTRAINT [UQ_User_Email] UNIQUE ([Email])
)