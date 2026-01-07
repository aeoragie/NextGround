CREATE TABLE [dbo].[UserFamilies]
(
    [ParentUid] UNIQUEIDENTIFIER NOT NULL,
    [PlayerUid] UNIQUEIDENTIFIER NOT NULL,
    [Relationship] NVARCHAR(20) NULL, -- 'Father', 'Mother', 'Guardian' 등
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    -- 부모-자녀 쌍은 유일해야 하므로 복합 PK 설정
    CONSTRAINT [PK_UserFamilies] PRIMARY KEY ([ParentUid], [PlayerUid]),
    
    -- Users 테이블과의 관계 설정
    CONSTRAINT [FK_UserFamilies_Parent] FOREIGN KEY ([ParentUid]) REFERENCES [dbo].[Users]([UserId]),
    CONSTRAINT [FK_UserFamilies_Child] FOREIGN KEY ([PlayerUid]) REFERENCES [dbo].[Users]([UserId])
)