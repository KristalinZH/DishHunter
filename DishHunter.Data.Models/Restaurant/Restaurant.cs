
namespace DishHunter.Data.Models.Restaurant
{
	using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Restaurant
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

		[ForeignKey(nameof(Categoy))]
		public int CategoryId { get; set; }

		public virtual Category Categoy { get; set; } = null!;

		[ForeignKey(nameof(Settlement))]
        public int SettlementId { get; set; }
        public virtual Settlement Settlement { get; set; } = null!;

        [ForeignKey(nameof(Brand))]
        public Guid BrandId { get; set; }

        public virtual Brand Brand { get; set; } = null!;

		public bool IsActive { get; set; }

		[Required]
		public string ImageUrl { get; set; } = null!;
	}
}
