﻿namespace DishHunter.Services.Data.Models.Restaurant
{
    public class BrandRestaurantTranferModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string SettlementName { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}