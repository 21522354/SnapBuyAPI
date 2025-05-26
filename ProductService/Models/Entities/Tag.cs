namespace ProductService.Models.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<ProductTag> ProductTags { get; set; }
    }
}
