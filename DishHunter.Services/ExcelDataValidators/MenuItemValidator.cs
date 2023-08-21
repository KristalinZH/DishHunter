namespace DishHunter.Services.Data.ExcelDataValidators
{
    using Models.MenuItem;
    using static Common.EntityValidationConstants.MenuItem;
    public static class MenuItemValidator
    {
        public static bool IsMenuItemValid(MenuItemExcelTransferModel menuItem)
        {
            if (menuItem.Name.Length < NameMinLenght || menuItem.Name.Length > NameMaxLenght)
                return false;
            if (menuItem.Price < decimal.Parse(PriceMinValue) || menuItem.Price > decimal.Parse(PriceMaxValue))
                return false;
            if (menuItem.Description.Length < DescriptionMinLenght || menuItem.Description.Length > DescriptionMaxLenght)
                return false;
            if (menuItem.FoodCategory.Length < FoodCategoryMinLenght || menuItem.FoodCategory.Length > FoodCategoryMaxLenght)
                return false;
            if (menuItem.ImageUrl.Length > UrlMaxLenght)
                return false;

            return true;
        }
    }
}
