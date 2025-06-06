CREATE PROCEDURE sc24_197.sp_DeleteUser
    @Email NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @uid INT;
    SELECT @uid = USER_ID FROM sc24_197.[USER] WHERE USER_EMAIL = @Email;

    IF @uid IS NULL RETURN;

    DELETE FROM sc24_197.NOTIFICATION WHERE USER_ID = @uid;
    DELETE FROM sc24_197.USER_VERIFY_TOKEN WHERE USER_ID = @uid;
    DELETE FROM sc24_197.USER_SESSION WHERE USER_ID = @uid;
    DELETE FROM sc24_197.ENROLLMENT WHERE TRAINEE_USER_ID = @uid;
    UPDATE sc24_197.COURSE
        SET TRAINER_USER_ID = NULL
        WHERE TRAINER_USER_ID = @uid;
    DELETE FROM sc24_197.[USER] WHERE USER_ID = @uid;
END
GO
