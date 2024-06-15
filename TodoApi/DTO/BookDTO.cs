namespace TodoApi.DTO
{
    public class BookDTO
    {
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int Category { get; set; }
    }

    public class UpdateBookRequestDTO
    {
        public int BookID { get; set; }
        public decimal Price { get; set; }
    }
}
