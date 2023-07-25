namespace DishHunter.Data.Models.Account
{
    using Microsoft.AspNetCore.Identity;
	using System.ComponentModel.DataAnnotations;

	public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            Id = new Guid();
        }
        [Required]
        public string FirstName { get; set; } = null!;

		[Required]
		public string LastName { get; set; } = null!;
	}
}
