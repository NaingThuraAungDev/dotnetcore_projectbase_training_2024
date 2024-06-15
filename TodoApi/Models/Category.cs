namespace TodoApi.Models;
[System.ComponentModel.DataAnnotations.Schema.Table("category")]
public class Category
{
    public int category_id { get; set; }
    public string category_name { get; set; }
}