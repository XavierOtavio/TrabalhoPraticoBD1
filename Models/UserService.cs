
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using TrabalhoFinal3.Models;

public class UserService
{
    private readonly string connStr = ConfigurationManager.ConnectionStrings["SoftSkillsConnection"].ConnectionString;

    public bool ValidateUser(string email, string password)
    {
        const string sql =
            "SELECT COUNT(*) " +
            "FROM  sc24_197.[USER] " +
            "WHERE USER_EMAIL = @Email AND USER_PASSWORD = @Password";

        using (SqlConnection conn = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);

            conn.Open();
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }
    }

    public User GetUser(string email)
    {
        const string sql =
                "SELECT U.USER_ID, U.USER_EMAIL, U.USER_PASSWORD, " +
                "       U.ROLE_ID, U.STATUS_ID, U.USER_FIRST_NAME, U.USER_LAST_NAME, U.USER_TITLE, U.USER_BIOGRAPHY, U.USER_PROFILE_PICTURE_URL, " +
                "       U.USER_PHONE_NUMBER, U.USER_ADDRESS, U.USER_CITY, U.USER_COUNTRY, " +
                "       U.USER_LANGUAGE, U.USER_TIMEZONE, U.USER_NOTIFICATION_PREFERENCES," +
                "       R.ROLE_NAME, S.STATUS_NAME " +
                "FROM   sc24_197.[USER]               U " +
                "LEFT  JOIN sc24_197.[USERROLE]       R ON U.ROLE_ID   = R.ROLE_ID " +
                "LEFT  JOIN sc24_197.[USER_STATUS]    S ON U.STATUS_ID = S.STATUS_ID " +
                "WHERE  U.USER_EMAIL = @Email";

        using (SqlConnection conn = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("@Email", email);

            conn.Open();
            var dr = cmd.ExecuteReader(CommandBehavior.SingleRow);


            if (!dr.Read()) return null;

            return new User
            {
                UserId = dr.GetInt32(0),
                Email = dr.GetString(1),
                Password = dr.GetString(2),
                UserRoleId = dr.GetInt32(3),
                UserStatusId = dr.GetInt32(4),
                FirstName = dr.GetString(5),
                LastName = dr.GetString(6),
                Title = dr.IsDBNull(7) ? null : dr.GetString(7),
                Bio = dr.IsDBNull(8) ? null : dr.GetString(8),
                PhotoPath = dr.IsDBNull(9) ? null : dr.GetString(9),
                Phone = dr.IsDBNull(10) ? null : dr.GetString(10),
                Address = dr.IsDBNull(11) ? null : dr.GetString(11),
                City = dr.IsDBNull(12) ? null : dr.GetString(12),
                Country = dr.IsDBNull(13) ? null : dr.GetString(13),
                Language = dr.IsDBNull(14) ? null : dr.GetString(14),
                TimeZone = dr.IsDBNull(15) ? null : dr.GetString(15),
                NotifyOptions = dr.IsDBNull(16) ? null : dr.GetString(16),
                RoleName = dr.IsDBNull(17) ? null : dr.GetString(17),
                StatusName = dr.IsDBNull(18) ? null : dr.GetString(18)
            };
        }
    }

    public void UpdateUser(User u)
    {
        const string sql =
          "UPDATE sc24_197.[USER] SET " +
          "ROLE_ID = (SELECT ROLE_ID FROM sc24_197.[USERROLE] WHERE ROLE_NAME = @RoleName), " +
          "STATUS_ID = (SELECT STATUS_ID FROM sc24_197.[USER_STATUS] WHERE STATUS_NAME = @StatusName), " +
          " USER_FIRST_NAME                 = @FirstName," +
          " USER_LAST_NAME                  = @LastName, " +
          " USER_TITLE                      = @Title," +
          " USER_BIOGRAPHY                  = @Bio," +
          " USER_PROFILE_PICTURE_URL        = @Photo," +
          " USER_PHONE_NUMBER               = @Phone," +
          " USER_ADDRESS                    = @Address," +
          " USER_CITY                       = @City," +
          " USER_COUNTRY                    = @Country," +
          " USER_LANGUAGE                   = @Lang," +
          " USER_TIMEZONE                   = @Tz," +
          " USER_NOTIFICATION_PREFERENCES   = @Notify," +
          " USER_UPDATED_AT                 = @UpdatedAt" +       
          " WHERE USER_ID                   = @Id";

        using (SqlConnection conn = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {

            cmd.Parameters.AddWithValue("@FirstName", string.IsNullOrEmpty(u.FirstName) ? (object)DBNull.Value : u.FirstName);
            cmd.Parameters.AddWithValue("@LastName", string.IsNullOrEmpty(u.LastName) ? (object)DBNull.Value : u.LastName);
            cmd.Parameters.AddWithValue("@Title", string.IsNullOrEmpty(u.Title) ? (object)DBNull.Value : u.Title);
            cmd.Parameters.AddWithValue("@Bio", string.IsNullOrEmpty(u.Bio) ? (object)DBNull.Value : u.Bio);
            cmd.Parameters.AddWithValue("@Photo", string.IsNullOrEmpty(u.PhotoPath) ? (object)DBNull.Value : u.PhotoPath);
            cmd.Parameters.AddWithValue("@Phone", string.IsNullOrEmpty(u.Phone) ? (object)DBNull.Value : u.Phone);
            cmd.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(u.Address) ? (object)DBNull.Value : u.Address);
            cmd.Parameters.AddWithValue("@City", string.IsNullOrEmpty(u.City) ? (object)DBNull.Value : u.City);
            cmd.Parameters.AddWithValue("@Country", string.IsNullOrEmpty(u.Country) ? (object)DBNull.Value : u.Country);
            cmd.Parameters.AddWithValue("@Lang", string.IsNullOrEmpty(u.Language) ? (object)DBNull.Value : u.Language);
            cmd.Parameters.AddWithValue("@Tz", string.IsNullOrEmpty(u.TimeZone) ? (object)DBNull.Value : u.TimeZone);
            cmd.Parameters.AddWithValue("@Notify", string.IsNullOrEmpty(u.NotifyOptions) ? (object)DBNull.Value : u.NotifyOptions);
            cmd.Parameters.AddWithValue("@RoleName", u.RoleName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@StatusName", u.StatusName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.UtcNow);
            cmd.Parameters.AddWithValue("@Id", u.UserId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
    public void UpdateUserPhoto(string email, string path)
    {
        const string sql = "UPDATE sc24_197.[USER] SET USER_PROFILE_PICTURE_URL = @Path WHERE USER_EMAIL = @Email";
        using (SqlConnection conn = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("@Path", path);
            cmd.Parameters.AddWithValue("@Email", email);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
    public void DeleteAccount(string email)
    {
        const string sql = "DELETE FROM sc24_197.[USER] WHERE USER_EMAIL = @Email";
        using (SqlConnection conn = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(sql, conn)) {
            cmd.Parameters.AddWithValue("@Email", email);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
    public List<SessionInfo> ListSessions(int userId)
    {
        const string sql =
          "SELECT SessionId, Device, IpAddress, LastSeen " +
          "FROM sc24_197.USER_SESSION WHERE UserId = @Id";

        var list = new List<SessionInfo>();

        using (SqlConnection conn = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("@Id", userId);

            conn.Open();
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new SessionInfo
                {
                    SessionId = dr.GetGuid(0),
                    Device = dr.GetString(1),
                    Ip = dr.GetString(2),
                    LastSeen = dr.GetDateTime(3).ToString("yyyy-MM-dd HH:mm")
                });
            }
            return list;
        }
    }


    public class SessionInfo
    {
        public Guid SessionId { get; set; }
        public string Device { get; set; }
        public string Ip { get; set; }
        public string LastSeen { get; set; }
    }

    public List<UserRole> GetAllRoles()
    {
        const string sql = "SELECT ROLE_ID, ROLE_NAME, ROLE_DESCRIPTION " +
                           "FROM sc24_197.USERROLE";

        var list = new List<UserRole>();

        using (SqlConnection conn = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            conn.Open();
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    list.Add(new UserRole
                    {
                        UserRoleId = dr.GetInt32(0),
                        RoleName = dr.GetString(1),
                        RoleDescription = dr.IsDBNull(2) ? null : dr.GetString(2)
                    });
                }
            }
        }
        return list;
    }

    public List<UserStatus> GetAllStatuses()
    {
        const string sql = "SELECT STATUS_ID, STATUS_NAME " +
                           "FROM sc24_197.USER_STATUS";
        var list = new List<UserStatus>();
        using (SqlConnection cn = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(sql, cn))
        {
            cn.Open();
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    list.Add(new UserStatus
                    {
                        UserStatusId = dr.GetInt32(0),
                        StatusName = dr.GetString(1)
                    });
                }
            }
        }
        return list;
    }
    public void GuardarTokenVerificacao(int userId, string token)
    {
        const string sql =
          "INSERT INTO sc24_197.USER_VERIFY_TOKEN (USER_ID, TOKEN_VALUE, TOKEN_EXPIRATION_DATE, TOKEN_CREATED_DATE) " +
          "VALUES (@user, @token, DATEADD(day, 1, @date), @date)";

        using (SqlConnection conn = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("@user", userId);
            cmd.Parameters.AddWithValue("@token", token);
            cmd.Parameters.AddWithValue("@date", DateTime.UtcNow);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
    public bool ValidarToken(int userId, string token)
    {
        const string sql =
          "SELECT COUNT(*) FROM sc24_197.USER_VERIFY_TOKEN " +
          "WHERE USER_ID=@user AND TOKEN_VALUE =@token AND TOKEN_EXPIRATION_DATE>@date";

        using (SqlConnection conn = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("@user", userId);
            cmd.Parameters.AddWithValue("@token", token);
            cmd.Parameters.AddWithValue("@date", DateTime.UtcNow);
            conn.Open();
            int cnt = (int)cmd.ExecuteScalar();

            if (cnt == 0) return false;

            var cmd2 = new SqlCommand(
                "UPDATE sc24_197.[USER] SET STATUS_ID = 1 WHERE USER_ID = @user", conn);
            cmd2.Parameters.AddWithValue("@user", userId);
            cmd2.ExecuteNonQuery();

            var cmd3 = new SqlCommand(
                "UPDATE sc24_197.USER_VERIFY_TOKEN SET TOKEN_USED = 1 WHERE USER_ID = @user", conn);
            cmd3.Parameters.AddWithValue("@user", userId);
            cmd3.ExecuteNonQuery();
            return true;
        }

    }
    


}