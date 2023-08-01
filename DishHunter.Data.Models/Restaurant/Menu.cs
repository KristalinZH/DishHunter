
namespace DishHunter.Data.Models.Restaurant
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static Common.EntityValidationConstants.Menu;
    public class Menu
    {
        public Menu()
        {
            MenuItems = new HashSet<MenuItem>();
        }
        public int Id { get; set; }
        [Required]
        [MaxLength(DescriptionMaxLenght)]
        public string Description { get; set; } = null!;
        [Required]
        [MaxLength(MenuTypeMaxLenght)]
        public string MenuType { get; set; } = null!;
        [Required]
        [MaxLength(FoodTypeMaxLenght)]
        public string FoodType { get; set; } = null!;
        [ForeignKey(nameof(Brand))]
        public Guid BrandId { get; set; }
        public Brand Brand { get; set; } = null!;
		public bool IsActive { get; set; }
		public virtual IEnumerable<MenuItem> MenuItems { get; set; }
    }
}
