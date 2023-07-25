
namespace DishHunter.Data.Models.Restaurant
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class MenuItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public string FoodCategory { get; set; } = null!;
        [ForeignKey(nameof(Menu))]
        public int MenuId { get; set; }
        public virtual Menu Menu { get; set; } = null!;
    }
}
