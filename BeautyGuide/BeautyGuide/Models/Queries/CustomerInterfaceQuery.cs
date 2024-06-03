using Microsoft.Data.SqlClient;

namespace BeautyGuide.Models.Queries
{
    public class CustomerInterfaceQuery
    {
        public List<ListContent> ContentInterface(string? keyword)
        {
            string dataKeyword = "%" + keyword + "%";
            List<ListContent> listContents = new List<ListContent>();


            using (SqlConnection conn = Database.GetSqlConnection())
            {
                string sqlQuery = string.Empty;
                sqlQuery = "SELECT Guides.*, category.id AS category_id, category.name AS category_name FROM Guides INNER JOIN category ON Guides.categoryId = category.id WHERE Guides.DeleteAt IS NULL";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ListContent listContent = new ListContent();
                        listContent.Id_Guide = Convert.ToInt32(reader["Id"]);
                        listContent.CategoryIdGuide = Convert.ToInt32(reader["category_id"]);

                        listContent.Description = reader["Description"].ToString();
                        listContent.NameVideo = reader["Video"].ToString();
                        listContent.Name = reader["Name"].ToString();
                        listContent.NameCategoryGuide = reader["category_name"].ToString();


                        listContents.Add(listContent);
                    }
                    conn.Close();
                }

            }
            return listContents;
        }
        public List<CategoryDetail> GetCategory()
        {
            List<CategoryDetail> category = new List<CategoryDetail>();
            using (SqlConnection conn = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT * FROM [category] WHERE [DeleteAt] IS NULL";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) // Kiểm tra xem có dữ liệu để đọc hay không
                    {
                        CategoryDetail categoryDetail = new CategoryDetail();
                        categoryDetail.Name = reader["name"].ToString();
                        categoryDetail.Id = Convert.ToInt32(reader["Id"]); ;
                        category.Add(categoryDetail);

                    }
                }
                conn.Close();
            }
            return category;
        }
        public List<ListContent> GetGuideByCategory(int categoryId)
        {
            List<ListContent> listContents = new List<ListContent>();


            using (SqlConnection conn = Database.GetSqlConnection())
            {
                string sqlQuery = string.Empty;
                sqlQuery = "SELECT Guides.*, category.id AS category_id, category.name AS category_name FROM Guides INNER JOIN category ON Guides.categoryId = category.id WHERE Guides.DeleteAt IS NULL AND Guides.categoryId = @categoryId";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@categoryId", categoryId);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ListContent listContent = new ListContent();
                        listContent.Id_Guide = Convert.ToInt32(reader["Id"]);
                        listContent.CategoryIdGuide = Convert.ToInt32(reader["category_id"]);

                        listContent.Description = reader["Description"].ToString();
                        listContent.NameVideo = reader["Video"].ToString();
                        listContent.Name = reader["Name"].ToString();
                        listContent.NameCategoryGuide = reader["category_name"].ToString();


                        listContents.Add(listContent);
                    }
                    conn.Close();
                }

            }
            return listContents;

        }
       
    }
}
