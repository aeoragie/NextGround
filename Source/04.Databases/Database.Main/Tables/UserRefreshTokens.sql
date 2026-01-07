CREATE TABLE [dbo].[UserRefreshTokens]
(
    [TokenId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [Token] VARCHAR(500) NOT NULL,
    [IsRevoked] BIT NOT NULL DEFAULT 0,
    [ExpiresAt] DATETIME2 NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT [PK_UserRefreshTokens] PRIMARY KEY ([TokenId]),
    CONSTRAINT [FK_UserRefreshTokens_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([UserId])
);