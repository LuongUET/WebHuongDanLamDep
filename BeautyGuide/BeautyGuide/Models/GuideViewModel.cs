using BeautyGuide.Validations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BeautyGuide.Models
{
    public class GuideViewModel
    {
        public List<GuideDetail> GuideDetailList { get; set; }
    }
        public class GuideDetail
        {
            public int Id { get; set; }

            public int CategoryId { get; set; }
            public string Name { get; set; }
        public string Description { get; set; }

            [AllowNull]
            public string? NameVideo { get; set; }


        [AllowNull]
        [AllowExtensionFile(new string[] { ".mp4", ".avi", ".mov", ".wmv" })]
            [AllowMaxSizeFile(90 * 1024 * 1024)]
            public IFormFile? VideoFile { get; set; }


            [AllowNull]
            public DateTime? CreatedAt { get; set; }

            [AllowNull]
            public DateTime? UpdatedAt { get; set; }

            [AllowNull]
            public DateTime? DeletedAt { get; set; }

            [AllowNull]
            public string? NameCategory { get; set; }
        
    }
}
