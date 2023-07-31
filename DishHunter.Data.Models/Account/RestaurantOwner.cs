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
        public bool IsActive { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        public Guid? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public IEnumerable<Brand> OwnedBrands { get; set; }
    }
}
