namespace ProductService.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public Category Parent { get; set; }
        public string Slug { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
}
