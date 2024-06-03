using BeautyGuide.Controllers;
using BeautyGuide.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
namespace BeautyGuide.Models.Queries
{
    public class CategoryQuery
    {
        public int InsertItemCategory(
           string nameCategory
        
       )
        {
            int lastIdInsert = 0; // id cua category vua dc them moi
            string sqlQuery = "INSERT INTO [category]([name], [CreateAt ]) VALUES(@nameCategory,  @createdAt) SELECT SCOPE_IDENTITY()";
            // SELECT SCOPE_IDENTITY() : lay ra id vua dc them moi
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@nameCategory", nameCategory ?? DBNull.Value.ToString());
           
                cmd.Parameters.AddWithValue("@createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                lastIdInsert = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            // lastIdInsert tra ve lon hon 0 insert thanh cong va nguoc lai
            return lastIdInsert;
        }
        public List<CategoryDetail> GetAllCategories(string? keyword)
        {
            string dataKeyword = "%" + keyword + "%";
            List<CategoryDetail> category = new List<CategoryDetail>();
            using (SqlConnection conn = Database.GetSqlConnection())
            {
                string sqlQuery = string.Empty;

                sqlQuery = "SELECT * FROM [category] WHERE [name] LIKE @keyword AND [DeleteAt] IS NULL ;";
               
        

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@keyword", dataKeyword ?? DBNull.Value.ToString());
               
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CategoryDetail categoryDetail = new CategoryDetail();
                        categoryDetail.Id = Convert.ToInt32(reader["Id"]);
                        categoryDetail.Name = reader["name"].ToString();
                        categoryDetail.CreatedAt = Convert.ToDateTime(reader["CreateAt"]);
                        category.Add(categoryDetail);
                    }
                    conn.Close();
                }
            }
            return category;
        }
        public CategoryDetail GetDataCategoryById(int id = 0)
        {
            CategoryDetail categoryDetail = new CategoryDetail();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT * FROM [category] WHERE [Id] = @id AND [DeleteAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categoryDetail.Id = Convert.ToInt32(reader["Id"]);
                        categoryDetail.Name = reader["name"].ToString();
                        
                    }
                    connection.Close(); // ngat ket noi
                }
            }
            return categoryDetail;
        }
        public bool UpdateCategoryById(
         string nameCategory,
         int id
     )
        {
            bool checkUpdate = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlUpdate = "UPDATE [category] SET [name] = @name, [UpdateAt] = @updatedAt WHERE [Id] = @id AND [DeleteAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlUpdate, connection);
                cmd.Parameters.AddWithValue("@name", nameCategory ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                connection.Close();
                checkUpdate = true;
            }
            return checkUpdate;
        }
        public bool DeleteItemCategory(int id = 0)
        {
            bool statusDelete = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "UPDATE [category] SET [DeleteAt] = @deletedAt WHERE [Id] = @id";
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
