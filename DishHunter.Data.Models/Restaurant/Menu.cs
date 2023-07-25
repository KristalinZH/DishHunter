
namespace DishHunter.Data.Models.Restaurant
{
    using System.ComponentModel.DataAnnotations.Schema;
    public class Menu
    {
        public Menu()
        {
            MenuItems = new HashSet<MenuItem>();
        }
        public int Id { get; set; }

        [ForeignKey(nameof(Brand))]
        public Guid BrandId { get; set; }

        public Brand Brand { get; set; } = null!;

		public bool IsActive { get; set; }
		public virtual ICollection<MenuItem> MenuItems { get; set; }
    }
}
