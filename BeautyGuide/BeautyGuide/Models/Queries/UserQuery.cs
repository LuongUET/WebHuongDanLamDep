using Microsoft.Data.SqlClient;
using static BeautyGuide.Models.UserViewModel;

namespace BeautyGuide.Models.Queries
{
    public class UserQuery
    {
        public int InsertItemUser(
         int roleId,
         string username,
         string password,
         string email,
         string avatar
      )
        {
            int lastIdInsert = 0; // id cua category vua dc them moi
            string sqlQuery = "INSERT INTO [Users]([RoleId],[Username],[Password],[Email], [Avatar], [CreatedAt]) VALUES(@roleId, @username, @password, @email, @avatar, @createdAt ) SELECT SCOPE_IDENTITY()";
            // SELECT SCOPE_IDENTITY() : lay ra id vua dc them moi

            using (SqlConnection connection = Database.GetSqlConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@roleId", roleId);
                cmd.Parameters.AddWithValue("@username", username ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@password", password ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@email", email ?? DBNull.Value.ToString());
                
                cmd.Parameters.AddWithValue("@avatar", avatar ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                lastIdInsert = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }

            // lastIdInsert tra ve lon hon 0 insert thanh cong va nguoc lai
            return lastIdInsert;
        }

        public List<UserDetail> GetRole()
        {
            List<UserDetail> roles = new List<UserDetail>();
            using (SqlConnection conn = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT * FROM [Roles] WHERE Id <> 1;";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) // Kiểm tra xem có dữ liệu để đọc hay không
                    {
                        UserDetail UserDetail = new UserDetail();
                        UserDetail.NameRole = reader["Name"].ToString();
                        UserDetail.RoleId = Convert.ToInt32(reader["Id"]);
                        roles.Add(UserDetail);

                    }
                }
                conn.Close();
            }
            return roles;
        }
        public List<UserDetail> GetAllUsers(string? keyword, int roleId)
        {
            string dataKeyword = "%" + keyword + "%";
            List<UserDetail> User = new List<UserDetail>();
            Dictionary<int, string> roleName = new Dictionary<int, string>();

            using (SqlConnection conn = Database.GetSqlConnection())
            {
                string sqlQuery = string.Empty;
                sqlQuery = "SELECT * FROM [Users] WHERE [Username] LIKE @keyword AND [DeletedAt] IS NULL AND RoleId = @roleId";
              
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@roleId", roleId);
                cmd.Parameters.AddWithValue("@keyword", dataKeyword ?? DBNull.Value.ToString());


                conn.Open();
                using (SqlCommand cmdUser = new SqlCommand("SELECT Id, Name FROM Roles", conn))
                {
                    using (SqlDataReader readerUser = cmdUser.ExecuteReader())
                    {
                        while (readerUser.Read())
                        {
                            roleName.Add(Convert.ToInt32(readerUser["Id"]), readerUser["Name"].ToString());
                        }
                    }
                }
                conn.Close();


                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserDetail userDetail = new UserDetail();
                        userDetail.Id = Convert.ToInt32(reader["Id"]);
                        userDetail.RoleId = Convert.ToInt32(reader["RoleId"]);
                        userDetail.UserName = reader["Username"].ToString();
                        userDetail.Password = reader["Password"].ToString();
                        userDetail.Email = reader["Email"].ToString();
                      
                        userDetail.Avatar = reader["Avatar"].ToString();
                       
                        if (reader["LastLogin"] != DBNull.Value)
                        {
                            userDetail.LastLogin = Convert.ToDateTime(reader["LastLogin"]);
                            userDetail.LastLogout = Convert.ToDateTime(reader["LastLogout"]);
                        }
                        userDetail.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
                        if (reader["UpdatedAt"] != DBNull.Value)
                        {
                            userDetail.UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]);
                        }
                        if (roleName.ContainsKey(userDetail.RoleId))
                        {
                            userDetail.NameRole = roleName[userDetail.RoleId];
                        }
                        User.Add(userDetail);

                    }
                    conn.Close();
                }

            }
            return User;
        }
        public UserDetail GetViewDetail(int id)
        {

            UserDetail userDetail = new UserDetail();
            Dictionary<int, string> roleName = new Dictionary<int, string>();

            using (SqlConnection conn = Database.GetSqlConnection())
            {
                string sqlQuery = string.Empty;
                sqlQuery = "SELECT * FROM [Users] WHERE [Id] = @id";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using (SqlCommand cmdUser = new SqlCommand("SELECT Id, Name FROM Roles", conn))
                {
                    using (SqlDataReader readerUser = cmdUser.ExecuteReader())
                    {
                        while (readerUser.Read())
                        {
                            roleName.Add(Convert.ToInt32(readerUser["Id"]), readerUser["Name"].ToString());
                        }
                    }
                }
                conn.Close();
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        userDetail.Id = Convert.ToInt32(reader["Id"]);
                        userDetail.RoleId = Convert.ToInt32(reader["RoleId"]);
                        userDetail.UserName = reader["Username"].ToString();
                        userDetail.Email = reader["Email"].ToString();
                        userDetail.Avatar = reader["Avatar"].ToString();
                      
                        if (reader["LastLogin"] != DBNull.Value)
                        {
                            userDetail.LastLogin = Convert.ToDateTime(reader["LastLogin"]);
                            userDetail.LastLogout = Convert.ToDateTime(reader["LastLogout"]);
                        }
                        userDetail.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
                        if (reader["UpdatedAt"] != DBNull.Value)
                        {
                            userDetail.UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]);
                        }
                        if (roleName.ContainsKey(userDetail.RoleId))
                        {
                            userDetail.NameRole = roleName[userDetail.RoleId];
                        }

                    }
                    conn.Close();
                }

            }
            return userDetail;
        }
        public bool UpdateUserById(
          string username,
          string password,
          string email,
         
          string avatar,
          int id)
        {
            bool checkUpdate = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlUpdate = "UPDATE [Users] SET [Username] = @username, [Email] = @email, [Avatar] = @avatar,  [UpdatedAt] = @updatedAt WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlUpdate, connection);
                cmd.Parameters.AddWithValue("@username", username ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@email", email ?? DBNull.Value.ToString());
            
                cmd.Parameters.AddWithValue("@avatar", avatar ?? DBNull.Value.ToString());
              
                cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                connection.Close();
                checkUpdate = true;
            }
            return checkUpdate;
        }
        public bool DeleteItemUser(int id = 0)
        {
            bool statusDelete = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "UPDATE [Users] SET [DeletedAt] = @deletedAt WHERE [Id] = @id";
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                connection.Open();
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@deletedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                statusDelete = true;
                connection.Close();
            }
            // false : ko xoa dc - true : xoa thanh cong
            return statusDelete;
        }
    }
}
