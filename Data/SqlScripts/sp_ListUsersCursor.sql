CREATE PROCEDURE sc24_197.sp_ListUsersCursor
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId INT,
            @FirstName NVARCHAR(100),
            @LastName NVARCHAR(100),
            @Email NVARCHAR(100),
            @RoleId INT,
            @RoleName NVARCHAR(100);

    DECLARE user_cursor CURSOR FOR
        SELECT u.USER_ID,
               u.USER_FIRST_NAME,
               u.USER_LAST_NAME,
               u.USER_EMAIL,
               u.ROLE_ID,
               r.ROLE_NAME
        FROM sc24_197.[USER] u
        INNER JOIN sc24_197.USERROLE r ON u.ROLE_ID = r.ROLE_ID;

    CREATE TABLE #Users(
        USER_ID INT,
        USER_FIRST_NAME NVARCHAR(100),
        USER_LAST_NAME NVARCHAR(100),
        USER_EMAIL NVARCHAR(100),
        ROLE_ID INT,
        ROLE_NAME NVARCHAR(100)
    );

    OPEN user_cursor;
    FETCH NEXT FROM user_cursor INTO @UserId, @FirstName, @LastName, @Email, @RoleId, @RoleName;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO #Users(USER_ID, USER_FIRST_NAME, USER_LAST_NAME, USER_EMAIL, ROLE_ID, ROLE_NAME)
        VALUES(@UserId, @FirstName, @LastName, @Email, @RoleId, @RoleName);
        FETCH NEXT FROM user_cursor INTO @UserId, @FirstName, @LastName, @Email, @RoleId, @RoleName;
    END
    CLOSE user_cursor;
    DEALLOCATE user_cursor;

    SELECT * FROM #Users;
END
GO
