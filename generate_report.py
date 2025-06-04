content = r"""
\documentclass[12pt,a4paper]{article}
\usepackage[utf8]{inputenc}
\usepackage[portuguese]{babel}
\usepackage{geometry}
\usepackage{listings}
\usepackage{courier}

\lstset{%
    basicstyle=\footnotesize\ttfamily,
    breaklines=true
}

\title{Relat\'orio T\'ecnico da Base de Dados}
\author{}
\date{}

\begin{document}
\maketitle

Este documento descreve de forma resumida a estrutura da base de dados utilizada no projecto WebForms disponibilizado. As se\c{c}\~oes seguintes apresentam alguns excertos de c\'odigo SQL presentes na aplica\c{c}\~ao, bem como exemplos de artefactos t\'ipicos de uma base de dados relacional, nomeadamente fun\c{c}\~oes, procedimentos armazenados e triggers.

\section{Exemplos de SELECT}
Na camada de acesso a dados \'e poss\'ivel observar v\'arias consultas de sele\c{c}\~ao. A t\'itulo ilustrativo apresenta-se um excerto do m\'etodo ``GetUser'' que obt\'em um utilizador e respectivo papel:
\begin{lstlisting}[language=SQL]
SELECT U.USER_ID, U.USER_EMAIL, U.USER_PASSWORD, U.ROLE_ID,
       U.STATUS_ID, U.USER_FIRST_NAME, U.USER_LAST_NAME,
       U.USER_TITLE, U.USER_BIOGRAPHY, U.USER_PROFILE_PICTURE_URL,
       U.USER_PHONE_NUMBER, U.USER_ADDRESS, U.USER_CITY,
       U.USER_COUNTRY, U.USER_LANGUAGE, U.USER_TIMEZONE,
       U.USER_NOTIFICATION_PREFERENCES, R.ROLE_NAME, S.STATUS_NAME
FROM sc24_197.[USER] U
LEFT JOIN sc24_197.[USERROLE] R ON U.ROLE_ID = R.ROLE_ID
LEFT JOIN sc24_197.[USER_STATUS] S ON U.STATUS_ID = S.STATUS_ID
WHERE U.USER_EMAIL = @Email;
\end{lstlisting}

Outra consulta relevante surge na listagem de cursos com pagina\c{c}\~ao:
\begin{lstlisting}[language=SQL]
SELECT C.COURSE_ID, C.COURSE_NAME,
       (SELECT USER_FIRST_NAME + ' ' + USER_LAST_NAME
        FROM sc24_197.[USER]
        WHERE USER_ID = C.TRAINER_USER_ID) AS Trainer,
       C.COURSE_START_DATE, C.COURSE_END_DATE, C.COURSE_SLOTS,
       (SELECT COUNT(*) FROM sc24_197.ENROLLMENT E WHERE E.COURSE_ID=C.COURSE_ID) AS Inscritos,
       T.TOPIC_NAME, A.AREA_NAME, CT.CATEGORY_NAME, ST.COURSE_STATUS_NAME
FROM sc24_197.COURSE C
LEFT JOIN sc24_197.COURSE_TOPIC T ON C.TOPIC_ID = T.TOPIC_ID
LEFT JOIN sc24_197.COURSE_AREA A ON A.AREA_ID = T.AREA_ID
LEFT JOIN sc24_197.COURSE_CATEGORY CT ON CT.CATEGORY_ID = A.CATEGORY_ID
INNER JOIN sc24_197.COURSE_STATUS ST ON ST.COURSE_STATUS_ID = C.COURSE_STATUS_ID
WHERE {filtro}
ORDER BY C.COURSE_START_DATE DESC
OFFSET (@skip) ROWS FETCH NEXT (@take) ROWS ONLY;
\end{lstlisting}

\section{Fun\c{c}\~ao de Utilidade}
A plataforma poder\'a incluir fun\c{c}\~oes definidas pelo utilizador. Um exemplo comum seria calcular a idade de um utilizador a partir da data de nascimento:
\begin{lstlisting}[language=SQL]
CREATE FUNCTION dbo.fn_CalcularIdade(@DataNascimento DATE) RETURNS INT
AS
BEGIN
    RETURN DATEDIFF(YEAR, @DataNascimento, GETDATE()) -
           CASE
               WHEN MONTH(@DataNascimento) > MONTH(GETDATE()) OR
                    (MONTH(@DataNascimento) = MONTH(GETDATE()) AND
                     DAY(@DataNascimento) > DAY(GETDATE()))
               THEN 1 ELSE 0
           END;
END;
\end{lstlisting}

\section{Procedimento Armazenado}
O projecto fornece o procedimento ``sp_ListUsersCursor'' que demonstra o uso de cursores para gerar uma listagem de utilizadores e respectivos pap\'eis:
\begin{lstlisting}[language=SQL]
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
        INSERT INTO #Users(
            USER_ID, USER_FIRST_NAME, USER_LAST_NAME,
            USER_EMAIL, ROLE_ID, ROLE_NAME)
        VALUES(@UserId, @FirstName, @LastName, @Email, @RoleId, @RoleName);

        FETCH NEXT FROM user_cursor INTO
            @UserId, @FirstName, @LastName, @Email, @RoleId, @RoleName;
    END

    CLOSE user_cursor;
    DEALLOCATE user_cursor;

    SELECT * FROM #Users;
END;
\end{lstlisting}

\section{Triggers}
Para al\'em de procedimentos, \'e \'{u}til definir triggers que garantam integridade e automatizem tarefas. Exemplos poss\'iveis incluem:
\begin{lstlisting}[language=SQL]
CREATE TRIGGER trg_UpdateCourseSlots ON sc24_197.ENROLLMENT
AFTER INSERT
AS
BEGIN
    UPDATE C
    SET C.COURSE_SLOTS = C.COURSE_SLOTS - 1
    FROM sc24_197.COURSE C
    JOIN inserted I ON C.COURSE_ID = I.COURSE_ID;
END;
\end{lstlisting}

\begin{lstlisting}[language=SQL]
CREATE TRIGGER trg_LogCourseUpdates ON sc24_197.COURSE
AFTER UPDATE
AS
BEGIN
    INSERT INTO sc24_197.COURSE_LOG(COURSE_ID, UPDATED_AT, OLD_STATUS, NEW_STATUS)
    SELECT I.COURSE_ID, GETDATE(), D.COURSE_STATUS_ID, I.COURSE_STATUS_ID
    FROM inserted I
    JOIN deleted D ON I.COURSE_ID = D.COURSE_ID;
END;
\end{lstlisting}

Os exemplos apresentados ilustram a interac\c{c}\~ao t\'ipica com uma base de dados SQL Server, recorrendo a instru\c{c}\~oes SELECT parametrizadas, procedimentos armazenados para opera\c{c}\~oes mais complexas, fun\c{c}\~oes reutiliz\'aveis e triggers que respondem automaticamente a altera\c{c}\~oes nos dados. Estas t\'ecnicas contribuem para a robustez e manuten\c{c}\~ao da consist\^encia da informa\c{c}\~ao no sistema.

\end{document}
"""

with open("RelatorioBaseDeDados.tex", "w", encoding="utf-8") as f:
    f.write(content)
