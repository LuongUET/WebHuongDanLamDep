using System.Diagnostics.CodeAnalysis;

namespace BeautyGuide.Models
{
    public class CustomerInterfaceViewModel
    {
        public List<ListContent> ContentDetailList { get; set; }
        public List<ListCategory> CategoryListDetail { get; set; }
    }
    public class ListContent {
        public string NameCategoryGuide { get; set; }
        public int Id_Guide { get; set; }

        public int CategoryIdGuide { get; set; }
        public string Description { get; set; }
        public string NameVideo { get; set; }
        public string Name {  get; set; }   
         
    }
    public class ListCategory
    {
        public string NameCategory { get; set; }
        public int CategoryId { get; set; }
    }
    }

