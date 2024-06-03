using BeautyGuide.Validations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BeautyGuide.Models
{
    public class CategoryViewModel
    {
        public List<CategoryDetail> CategoryDetailList { get; set; }
    }
    public class CategoryDetail
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter name's category, please")]
        public string Name { get; set; }

        [AllowNull]
        public DateTime? CreatedAt { get; set; }

        [AllowNull]
        public DateTime? UpdatedAt { get; set; }

        [AllowNull]
        public DateTime? DeletedAt { get; set; }
    }
}
