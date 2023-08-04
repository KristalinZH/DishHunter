namespace DishHunter.Data.Models.Account
{
    using Microsoft.AspNetCore.Identity;
	using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static Common.EntityValidationConstants.ApplicationUser;
	public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            Id = new Guid();
        }
        [Required]
        [PersonalData]
        [MaxLength(FirstNameMaxLenght)]
        public string FirstName { get; set; } = null!;
		[Required]
        [PersonalData]
        [MaxLength(LastNameMaxLenght)]
		public string LastName { get; set; } = null!;
        [ForeignKey(nameof(RestaurantOwner))]
        public Guid? RestaurantOwnerId { get; set; }
        public virtual RestaurantOwner? RestaurantOwner { get; set; } 
    }
}
