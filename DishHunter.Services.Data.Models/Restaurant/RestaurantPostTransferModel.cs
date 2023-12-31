﻿namespace DishHunter.Services.Data.Models.Restaurant
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.Restaurant;
    using static Common.ValidationErrorMessages;
    public class RestaurantPostTransferModel
    {
        [Required]
        [StringLength(NameMaxLenght, MinimumLength = NameMinLenght, ErrorMessage = FieldLenghtMessage)]
        public string Name { get; set; } = null!;
        [Required]
        [StringLength(AddressMaxLenght, MinimumLength = AddressMinLenght, ErrorMessage = FieldLenghtMessage)]
        public string Address { get; set; } = null!;
        [Required]
        [Phone]
        [StringLength(PhoneMaxLenght, MinimumLength = PhoneMinLenght, ErrorMessage = FieldLenghtMessage)]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        [MaxLength(UrlMaxLenght, ErrorMessage = UrlLenghtMessage)]
        public string ImageUrl { get; set; } = null!;
        public string BrandId { get; set; } = null!;
        public int CategoryId { get; set; }
        public int SettlementId { get; set; }
    }
}
