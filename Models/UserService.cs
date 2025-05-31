using System.Configuration;
using System.Data.SqlClient;
using TrabalhoFinal3.Models;
using System.Security.Cryptography;
using System.Text;

public class UserService
{
    private string connectionString = ConfigurationManager.ConnectionStrings["SoftSkillsConnection"].ConnectionString;

    public bool ValidateUser(string email, string password)
    {
        string connStr = ConfigurationManager
                         .ConnectionStrings["SoftSkillsConnection"]
                         .ConnectionString;

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

    private string ComputeSha256Hash(string rawData)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2")); // hexadecimal lowercase
            }
            return builder.ToString();
        }
    }
}
