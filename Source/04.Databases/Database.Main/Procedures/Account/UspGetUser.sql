CREATE PROCEDURE [dbo].[UspGetUser]
    @UserId UNIQUEIDENTIFIER = NULL,
    @Email VARCHAR(255) = NULL
AS
BEGIN

    SET NOCOUNT ON;
    
    BEGIN TRY

        EXEC [nested].[NspGetUser] @UserId, @Email;
        RETURN 0;
            
    END TRY
    BEGIN CATCH

        EXEC [nested].[NspException];
        RETURN -1;

    END CATCH

END