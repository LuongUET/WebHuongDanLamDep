namespace BeautyGuide.Helper
{
    public class UploadFileHelper
    {
        public static async Task<string> UploadFile(IFormFile file)
    {
        string uniqueFileName = string.Empty;
        try
        {
            string pathUploadServer = "wwwroot\\uploads\\images";

            string extension = Path.GetExtension(file.FileName);
            string uniqueStr = Guid.NewGuid().ToString();
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            string fileNameUpload = uniqueStr + "-" + time + extension;

            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), pathUploadServer, fileNameUpload);
            
            // Mở một luồng để lưu file
            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                // Sử dụng CopyToAsync để sao chép dữ liệu từ file vào stream một cách bất đồng bộ
                await file.CopyToAsync(stream);
            }
            
            uniqueFileName = fileNameUpload;
        }
        catch (Exception ex)
        {
            // Trả về thông báo lỗi nếu có lỗi xảy ra trong quá trình lưu file
            uniqueFileName = ex.Message.ToString();
        }
        return uniqueFileName;
    }
    }
}
