namespace DishHunter.Services.Data.ExcelDataValidators
{
    using Models.Menu;
    using static Common.EntityValidationConstants.Menu;
    public static class MenuValidator
    {
        public static bool IsMenuValid(MenuExcelTransferModel menu)
        {
            if (menu.MenuType.Length < MenuTypeMinLenght || menu.MenuType.Length > MenuTypeMaxLenght)
                return false;
            if (menu.FoodType.Length < FoodTypeMinLenght || menu.FoodType.Length > FoodTypeMaxLenght)
                return false;
            if (menu.Description.Length < DescriptionMinLenght || menu.Description.Length > DescriptionMaxLenght)
                return false;
            return true;
        }
    }
}
