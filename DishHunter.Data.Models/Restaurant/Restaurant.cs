
namespace DishHunter.Data.Models.Restaurant
{
	using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static Common.EntityValidationConstants.Restaurant;
    public class Restaurant
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(NameMaxLenght)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(AddressMaxLenght)]
        public string Address { get; set; } = null!;

        [Required]
        [Phone]
        [MaxLength(PhoneMaxLenght)]
        public string PhoneNumber { get; set; } = null!;

		[ForeignKey(nameof(Category))]
		public int CategoryId { get; set; }

		public virtual Category Category { get; set; } = null!;

		[ForeignKey(nameof(Settlement))]
        public int SettlementId { get; set; }
        public virtual Settlement Settlement { get; set; } = null!;

        [ForeignKey(nameof(Brand))]
        public Guid BrandId { get; set; }

        public virtual Brand Brand { get; set; } = null!;

		public bool IsActive { get; set; }

		[Required]
        [MaxLength(UrlMaxLenght)]
		public string ImageUrl { get; set; } = null!;
	}
}
