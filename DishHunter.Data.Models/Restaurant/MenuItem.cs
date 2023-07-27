
namespace DishHunter.Data.Models.Restaurant
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static Common.EntityValidationConstants.MenuItem;
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(NameMaxLenght)]
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        [Required]
        [MaxLength(DescriptionMaxLenght)]
        public string Description { get; set; } = null!;
        [Required]
        [MaxLength(FoodCategoryMaxLenght)]
        public string FoodCategory { get; set; } = null!;
		[Required]
        [MaxLength(UrlMaxLenght)]
		public string ImageUrl { get; set; } = null!;
		[ForeignKey(nameof(Menu))]
        public int MenuId { get; set; }
        public virtual Menu Menu { get; set; } = null!;
		public bool IsActive { get; set; }
	}
}
