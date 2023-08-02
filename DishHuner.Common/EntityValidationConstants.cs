namespace DishHunter.Common
{
	public static class EntityValidationConstants
	{
		public static class ApplicationUser
		{
			public const int FirstNameMaxLenght = 30;
			public const int FirstNameMinLenght = 2;
			public const int LastNameMaxLenght = 30;
			public const int LastNameMinLenght = 2;
			public const int UserNameMaxLenght = 30;
			public const int UserNameMinLenght = 2;
			public const int PasswordMaxLenght = 60;
		}
		public static class Brand
		{
			public const int BrandNameMaxLenght = 50;
			public const int BrandNameMinLenght = 5;
			public const int UrlMaxLenght = 2048;
			public const int DescriptionMaxLenght = 1500;
			public const int DescriptionMinLenght = 20;
		}
		public static class Category
		{
			public const int CategoryNameMaxLenght = 30;
			public const int CategoryNameMinLenght = 5;
		}
		public static class Menu
		{
            public const int DescriptionMaxLenght = 1500;
            public const int DescriptionMinLenght = 20;
            public const int MenuTypeMaxLenght = 20;
            public const int MenuTypeMinLenght = 3;
            public const int FoodTypeMaxLenght = 50;
            public const int FoodTypeMinLenght = 3;
        }
		public static class MenuItem
		{
			public const int NameMaxLenght = 30;
			public const int NameMinLenght = 3;
			public const int DescriptionMaxLenght = 1500;
			public const int DescriptionMinLenght = 20;
			public const int FoodCategoryMaxLenght = 20;
			public const int FoodCategoryMinLenght = 4;
			public const int UrlMaxLenght = 2048;
			public const string PriceMaxValue = "100000";
			public const string PriceMinValue = "0";
		}
		public static class Restaurant
		{
			public const int NameMaxLenght = 30;
			public const int NameMinLenght = 3;
			public const int AddressMaxLenght = 50;
			public const int AddressMinLenght = 7;
			public const int PhoneMaxLenght = 11;
			public const int PhoneMinLenght = 3;
			public const int UrlMaxLenght = 2048;
		}
		public static class Settlement
		{
			public const int SettlementNameMaxLenght = 30;
			public const int SettlementNameMinLenght = 4;
			public const int RegionMaxLenght = 30;
			public const int RegionMinLenght = 4;
		}
	}
}