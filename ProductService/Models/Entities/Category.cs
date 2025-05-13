namespace ProductService.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }     
        public string Name { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }        
        public int ParentId { get; set; }   
        public Category Parent { get; set; }    
        public DateTime CreatedAt { get; set; } 
        public ICollection<Category> ChildCategories { get; set; }
        public ICollection<Product> Products { get; set; }  
    }
}
