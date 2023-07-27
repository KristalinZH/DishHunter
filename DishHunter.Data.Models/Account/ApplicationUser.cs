namespace DishHunter.Data.Models.Account
{
    using Microsoft.AspNetCore.Identity;
	using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.ApplicationUser;
	public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            Id = new Guid();
        }
        [Required]
        [MaxLength(FirstNameMaxLenght)]
        public string FirstName { get; set; } = null!;
		[Required]
		[MaxLength(LastNameMaxLenght)]
		public string LastName { get; set; } = null!;
	}
}
