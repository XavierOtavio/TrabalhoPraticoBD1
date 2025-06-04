# Relatório Técnico

Este repositório contém uma aplicação WebForms suportada por uma base de dados SQL Server. O presente documento resume a estrutura da base de dados e apresenta exemplos de consultas e objetos.

## Consultas de Exemplo

```sql
SELECT COUNT(*)
FROM sc24_197.[USER]
WHERE USER_EMAIL = @Email;
```

```sql
SELECT U.USER_ID, U.USER_EMAIL, U.USER_PASSWORD,
       U.ROLE_ID, U.STATUS_ID, U.USER_FIRST_NAME,
       U.USER_LAST_NAME, R.ROLE_NAME, S.STATUS_NAME
FROM sc24_197.[USER] U
LEFT JOIN sc24_197.[USERROLE] R ON U.ROLE_ID = R.ROLE_ID
LEFT JOIN sc24_197.[USER_STATUS] S ON U.STATUS_ID = S.STATUS_ID
WHERE U.USER_EMAIL = @Email;
```

## Função de Utilidade

```sql
CREATE FUNCTION dbo.fn_CalcularIdade(@DataNascimento DATE) RETURNS INT
AS
BEGIN
    RETURN DATEDIFF(YEAR, @DataNascimento, GETDATE()) -
           CASE WHEN MONTH(@DataNascimento)  > MONTH(GETDATE())
                 OR (MONTH(@DataNascimento) = MONTH(GETDATE())
                 AND DAY(@DataNascimento) > DAY(GETDATE()))
                THEN 1 ELSE 0
           END;
END;
```

## Procedimento Armazenado

```sql
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
        SELECT u.USER_ID, u.USER_FIRST_NAME, u.USER_LAST_NAME,
               u.USER_EMAIL, u.ROLE_ID, r.ROLE_NAME
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
    FETCH NEXT FROM user_cursor INTO
        @UserId, @FirstName, @LastName, @Email, @RoleId, @RoleName;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO #Users(USER_ID, USER_FIRST_NAME, USER_LAST_NAME,
                           USER_EMAIL, ROLE_ID, ROLE_NAME)
        VALUES(@UserId, @FirstName, @LastName, @Email, @RoleId, @RoleName);
        FETCH NEXT FROM user_cursor INTO
            @UserId, @FirstName, @LastName, @Email, @RoleId, @RoleName;
    END
    CLOSE user_cursor;
    DEALLOCATE user_cursor;
    SELECT * FROM #Users;
END;
```

## Triggers

```sql
CREATE TRIGGER trg_UpdateCourseSlots ON sc24_197.ENROLLMENT
AFTER INSERT
AS
BEGIN
    UPDATE C
    SET C.COURSE_SLOTS = C.COURSE_SLOTS - 1
    FROM sc24_197.COURSE C
    JOIN inserted I ON C.COURSE_ID = I.COURSE_ID;
END;
```

```sql
CREATE TRIGGER trg_LogCourseUpdates ON sc24_197.COURSE
AFTER UPDATE
AS
BEGIN
    INSERT INTO sc24_197.COURSE_LOG(COURSE_ID, UPDATED_AT, OLD_STATUS, NEW_STATUS)
    SELECT I.COURSE_ID, GETDATE(), D.COURSE_STATUS_ID, I.COURSE_STATUS_ID
    FROM inserted I
    JOIN deleted D ON I.COURSE_ID = D.COURSE_ID;
END;
```

Estas consultas e objetos ilustram as boas práticas de interação com uma base de dados relacional, mantendo a consistência e a rastreabilidade das alterações na aplicação.
