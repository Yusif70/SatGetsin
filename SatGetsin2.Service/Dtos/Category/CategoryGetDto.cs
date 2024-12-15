namespace SatGetsin2.Service.Dtos.Category
{
    public class CategoryGetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public List<ChildCategoryDto> ChildCategories { get; set; }
    }
    public class ChildCategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
