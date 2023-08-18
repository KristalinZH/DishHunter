namespace DishHunter.Services.Data.ExcelDataValidators
{
    using Models.Restaurant;
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.Restaurant;
    public static class RestaurantValidator
    {
        public static bool IsRestaurantDataValid(RestaurantExcelTransferModel restaurant)
        {
            if (restaurant.Name.Length < NameMinLenght || restaurant.Name.Length > NameMaxLenght)
                return false;
            if (restaurant.Address.Length < AddressMinLenght || restaurant.Address.Length > AddressMaxLenght)
                return false;
            if (restaurant.ImageUrl.Length>UrlMaxLenght)
                return false;
            PhoneAttribute phoneAttribute = new PhoneAttribute();
            if (!phoneAttribute.IsValid(restaurant.PhoneNumber)
                || restaurant.PhoneNumber.Length < PhoneMinLenght
                || restaurant.PhoneNumber.Length > PhoneMaxLenght)
                return false;
            return true;
        }
    }
}
