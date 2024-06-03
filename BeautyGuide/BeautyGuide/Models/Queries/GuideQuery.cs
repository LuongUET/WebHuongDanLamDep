using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using static BeautyGuide.Models.GuideViewModel;

namespace BeautyGuide.Models.Queries
{
    public class GuideQuery
    {
        public int InsertItemGuide(
          int categoryId,
          string video,
          string description,
          string name
       )
        {
            int lastIdInsert = 0; // id cua category vua dc them moi
            string sqlQuery = "INSERT INTO [Guides]([CategoryId],[Video],[Description], [CreateAt], [Name]) VALUES(@categoryId, @video ,@description, @createdAt, @name) SELECT SCOPE_IDENTITY()";
            // SELECT SCOPE_IDENTITY() : lay ra id vua dc them moi

            using (SqlConnection connection = Database.GetSqlConnection())
            {
                try
                {
                    connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);

                cmd.Parameters.AddWithValue("@categoryId", categoryId);
                cmd.Parameters.AddWithValue("@video", video ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@name", name ?? DBNull.Value.ToString());
                lastIdInsert = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    // Ghi log lỗi hoặc xử lý lỗi theo yêu cầu của bạn
                }
                connection.Close();
            }

            // lastIdInsert tra ve lon hon 0 insert thanh cong va nguoc lai
            return lastIdInsert;


        }
        public List<GuideDetail> GetCategory()
        {
            List<GuideDetail> guide = new List<GuideDetail>();
            using (SqlConnection conn = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT * FROM [category] WHERE [DeleteAt] IS NULL";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) // Kiểm tra xem có dữ liệu để đọc hay không
                    {
                        GuideDetail guideDetail = new GuideDetail();
                        guideDetail.NameCategory = reader["name"].ToString();
                        guideDetail.CategoryId = Convert.ToInt32(reader["Id"]); ; 
                        guide.Add(guideDetail);

                    }
                }
                conn.Close();
            }
            return guide;
        }
        public GuideDetail GetDataGuideById(int id = 0)
        {
            GuideDetail guideDetail = new GuideDetail();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT Guides.*, category.id AS category_id, category.name AS category_name FROM Guides INNER JOIN category ON Guides.CategoryId = category.id WHERE Guides.Id = @id AND Guides.DeleteAt IS NULL;";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        guideDetail.Id = Convert.ToInt32(reader["Id"]);
                        guideDetail.CategoryId = Convert.ToInt32(reader["category_id"]);
                        guideDetail.Description = reader["Description"].ToString();
                        guideDetail.NameVideo = reader["Video"].ToString();
                        guideDetail.Name = reader["Name"].ToString();
                        guideDetail.NameCategory = reader["category_name"].ToString();
                    }
                    connection.Close(); // ngat ket noi
                }
            }
            return guideDetail;
        }
        public bool UpdateGuideById(
           int categoryId,
          string video,
          string description,
           int id,
            string name
       )
        {
            bool checkUpdate = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlUpdate = "UPDATE [Guides] SET [CategoryId] = @categoryId,[Video] = @video, [Description] = @description, [UpdateAt] = @updateAt, [Name] = @name WHERE [Id] = @id AND [DeleteAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlUpdate, connection);
                
                cmd.Parameters.AddWithValue("@categoryId", categoryId);
                cmd.Parameters.AddWithValue("@description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@video", video ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@name", name ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@updateAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                connection.Close();
                checkUpdate = true;
            }
            return checkUpdate;
        }
        public List<GuideDetail> GetAllGuides(string? keyword)
        {
            string dataKeyword = "%" + keyword + "%";
            List<GuideDetail> Guide = new List<GuideDetail>();
            Dictionary<int, string> courseNames = new Dictionary<int, string>();

            using (SqlConnection conn = Database.GetSqlConnection())
            {
                string sqlQuery = string.Empty;
                sqlQuery = "SELECT Guides.*, category.id AS category_id, category.name AS category_name FROM Guides INNER JOIN category ON Guides.categoryId = category.id WHERE Guides.Name LIKE @keyword AND Guides.DeleteAt IS NULL";
              
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@keyword", dataKeyword);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        GuideDetail guideDetail = new GuideDetail();
                        guideDetail.Id = Convert.ToInt32(reader["Id"]);
                        guideDetail.CategoryId = Convert.ToInt32(reader["category_id"]);

                        guideDetail.Description = reader["Description"].ToString();
                        guideDetail.NameVideo = reader["Video"].ToString();
                        guideDetail.Name = reader["Name"].ToString();
                        guideDetail.CreatedAt = Convert.ToDateTime(reader["CreateAt"]);
                        if (reader["UpdateAt"] != DBNull.Value)
                        {
                            guideDetail.UpdatedAt = Convert.ToDateTime(reader["UpdateAt"]);
                        }
                       
                       
                        Guide.Add(guideDetail);
                    }
                    conn.Close();
                }

            }
            return Guide;
        }
        public bool DeleteItemGuide(int id = 0)
        {
            bool statusDelete = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "UPDATE [Guides] SET [DeleteAt] = @deletedAt WHERE [Id] = @id";
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
