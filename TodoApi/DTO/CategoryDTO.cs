namespace TodoApi.DTO
{
    public class CategoryDTO
    {
        public required string CategoryName { get; set; }
    }

    public class UpdateCategoryRequestDTO
    {
        public int CategoryID { get; set; }
        public required string CategoryName { get; set; }
    }

    public class GetBooksCountByCategoryResponseDTO
    {
        public string CategoryName { get; set; }
        public int BookCount { get; set; }
    }
}
