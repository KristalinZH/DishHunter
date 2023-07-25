namespace DishHunter.Data.Models.Account
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using Restaurant;

	public class RestaurantOwner
	{
        public RestaurantOwner()
        {
			OwnedBrands = new HashSet<Brand>();
		}
        [Key]
        public Guid Id { get; set; }
		[ForeignKey(nameof(ApplicationUser))]
        public Guid ApplicationUserId { get; set; }
		public virtual ApplicationUser ApplicationUser { get; set; } = null!;
        public IEnumerable<Brand> OwnedBrands { get; set; }
    }
}
