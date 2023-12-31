﻿namespace DishHunter.Services.Data.Models.Brand
{
    using Menu;
    using Restaurant;

    public class DetailsBrandTransferModel
    {
        public DetailsBrandTransferModel()
        {
            Restaurants = new HashSet<RestaurantListTranferModel>();
            Menus = new HashSet<MenuListTrasnferModel>();
        }
        public string BrandName { get; set; } = null!;
        public string LogoUrl { get; set; } = null!;
        public string WebsiteUrl { get; set; } = null!;
        public string Description { get; set; } = null!;
        public IEnumerable<RestaurantListTranferModel> Restaurants { get; set; }
        public IEnumerable<MenuListTrasnferModel> Menus { get; set; }
    }
}
