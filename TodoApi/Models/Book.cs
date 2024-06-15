namespace TodoApi.Models;

public class Book
{
    public int book_id { get; set; }
    public string title { get; set; }
    public string author { get; set; }
    public string description { get; set; }
    public double price { get; set; }
    public int category { get; set; }
}
