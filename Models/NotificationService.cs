using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace TrabalhoFinal3.Models
{
    public class NotificationService
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["SoftSkillsConnection"].ConnectionString;

        public List<Notification> GetNotifications(int userId)
        {
            var list = new List<Notification>();
            const string sql = @"SELECT NOTIFICATION_ID, NOTIFICATION_TYPE_ID, USER_ID, NOTIFICATION_MESSAGE, NOTIFICATION_SENT_DATE, NOTIFICATION_READ, NOTIFICATION_READ_DATE FROM sc24_197.NOTIFICATION WHERE USER_ID = @u ORDER BY NOTIFICATION_SENT_DATE DESC";
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@u", userId);
                conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new Notification
                        {
                            Id = dr.GetInt32(0),
                            TypeId = dr.GetInt32(1),
                            UserId = dr.GetInt32(2),
                            Message = dr.IsDBNull(3) ? null : dr.GetString(3),
                            SentDate = dr.IsDBNull(4) ? (DateTime?)null : dr.GetDateTime(4),
                            Read = !dr.IsDBNull(5) && dr.GetBoolean(5),
                            ReadDate = dr.IsDBNull(6) ? (DateTime?)null : dr.GetDateTime(6)
                        });
                    }
                }
            }
            return list;
        }

        public int CountUnread(int userId)
        {
            const string sql = "SELECT COUNT(*) FROM sc24_197.NOTIFICATION WHERE USER_ID = @u AND (NOTIFICATION_READ IS NULL OR NOTIFICATION_READ = 0)";
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@u", userId);
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public void MarkAsRead(int notificationId)
        {
            const string sql = "UPDATE sc24_197.NOTIFICATION SET NOTIFICATION_READ = 1 WHERE NOTIFICATION_ID = @id";
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", notificationId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
