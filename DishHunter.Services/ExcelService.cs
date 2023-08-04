﻿namespace DishHunter.Services.Data
{
    using System.IO;
    using System.Threading.Tasks;
    using System.Globalization;
    using OfficeOpenXml;
    using Models.Excel;
    using Models.Menu;
    using Models.MenuItem;
    using Models.Restaurant;
    using Interfaces;
    using static Common.NotificationMessagesConstants;

    public class ExcelService : IExcelService
    {
        public async Task<bool> IsExcelFileStructureValidByEntityAllowedColumnsAsync(Stream stream, int columnsPerEntity)
            => await Task.Run(() =>
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        if (package == null)
                            return false;
                        var worksheets = package.Workbook.Worksheets;
                        foreach (var ws in worksheets)
                        {
                            var tables = ws.Tables;
                            foreach (var t in tables)
                            {
                                if (t.Range.Columns != columnsPerEntity)
                                    return false;
                            }
                        }
                        return true;
                    }
                });
        public async Task<MenuExctractResult> ExtractMenuDataFromExcel(Stream stream)
        {
            MenuExctractResult result = new MenuExctractResult()
            {
                IsDataExtracted = false,
                Message = string.Empty
            };
            List<MenuExcelTransferModel> menus =
                new List<MenuExcelTransferModel>();
            using (var package = new ExcelPackage(stream))
            {
                var worksheets = package.Workbook.Worksheets;
                foreach (var ws in worksheets)
                {
                    var tables = ws.Tables;
                    foreach (var t in tables)
                    {
                        var rows = (await t.ToTextAsync())
                            .Split(Environment.NewLine)
                            .Skip(1);
                        string[] data = rows.First().Split(',', StringSplitOptions.RemoveEmptyEntries);
                        MenuExcelTransferModel menu;
                        try
                        {
                            menu = new MenuExcelTransferModel()
                            {
                                MenuType = data[0],
                                FoodType = data[1],
                                Description = data[2]
                            };
                        }
                        catch (Exception)
                        {
                            result.Message = MissingExcelData;
                            result.Menus = null;
                            return result;
                        }
                        MenuItemExtractResult menuItemsResult = await ExtractMenuItemsFromCSVRows(rows.Skip(1).ToArray());
                        if (!menuItemsResult.IsDataExtracted)
                        {
                            result.IsDataExtracted = false;
                            result.Message = menuItemsResult.Message;
                            result.Menus = null;
                            return result;
                        }
                        menu.MenuItems = menuItemsResult.MenuItems!;
                        menus.Add(menu);
                    }
                }
            }
            result.IsDataExtracted = true;
            result.Message = SuccessfulyExtractedExcelData;
            result.Menus = menus;
            return result;
        }

        public async Task<MenuItemExtractResult> ExtractMenuItemsFromExcel(Stream stream)
        {
            MenuItemExtractResult result = new MenuItemExtractResult()
            {
                IsDataExtracted = false,
                Message = string.Empty
            };
            List<MenuItemExcelTransferModel> menuItems = 
                new List<MenuItemExcelTransferModel>();
            using (var package = new ExcelPackage(stream))
            {
                var worksheets = package.Workbook.Worksheets;
                foreach(var ws in worksheets)
                {
                    var tables = ws.Tables;
                    foreach(var t in tables)
                    {
                        var rows = (await t.ToTextAsync())
                            .Split(Environment.NewLine)
                            .Skip(1);
                        MenuItemExtractResult currentTableResult = await ExtractMenuItemsFromCSVRows(rows.ToArray());
                        if (!currentTableResult.IsDataExtracted)
                            return currentTableResult;
                        menuItems.AddRange(currentTableResult.MenuItems!);
                    }
                }
            }
            result.IsDataExtracted = true;
            result.Message = SuccessfulyExtractedExcelData;
            result.MenuItems = menuItems;
            return result;
        }

        public async Task<RestaurantExtractResult> ExtractRestaurantsFromExcel(Stream stream)
        {
            RestaurantExtractResult result = new RestaurantExtractResult()
            {
                IsDataExtracted = false,
                Message = string.Empty
            };
            List<RestaurantExcelTransferModel> restaurants =
                new List<RestaurantExcelTransferModel>();
            using (var package = new ExcelPackage(stream))
            {
                var worksheets = package.Workbook.Worksheets;
                foreach (var ws in worksheets)
                {
                    var tables = ws.Tables;
                    foreach (var t in tables)
                    {
                        var rows = (await t.ToTextAsync())
                            .Split(Environment.NewLine)
                            .Skip(1);
                        foreach (var row in rows)
                        {
                            string[] data = row.Split(',');
                            if (data.Any(el => string.IsNullOrEmpty(el) || string.IsNullOrWhiteSpace(el)))
                            {
                                result.Message = MissingExcelData;
                                result.Restaurants = null;
                                return result;
                            }
                            RestaurantExcelTransferModel restaurant;
                            try
                            {
                                restaurant = new RestaurantExcelTransferModel()
                                {
                                    Name = data[0],
                                    Region = data[1],
                                    SettlementName = data[2],
                                    Address = data[3],
                                    PhoneNumber = data[4],
                                    CategoryName = data[5],
                                    ImageUrl = data[6]
                                };
                            }
                            catch (Exception)
                            {
                                result.Message = WrongExcelData;
                                result.Restaurants = null;
                                return result;
                            }
                            restaurants.Add(restaurant);
                        }
                    }
                }
            }
            result.IsDataExtracted = true;
            result.Message = SuccessfulyExtractedExcelData;
            result.Restaurants = restaurants;
            return result;
        }

        public async Task<MenuItemExtractResult> ExtractMenuItemsFromCSVRows(string[] csvRows)
            => await Task.Run(() => {
                    MenuItemExtractResult result = new MenuItemExtractResult()
                    {
                        IsDataExtracted = false,
                        Message = string.Empty
                    };
                    List<MenuItemExcelTransferModel> menuItems =
                                    new List<MenuItemExcelTransferModel>();
                    foreach (var row in csvRows)
                    {
                        string[] data = row.Split(',');
                        if (data.Any(el => string.IsNullOrEmpty(el) || string.IsNullOrWhiteSpace(el)))
                        {
                            result.Message = MissingExcelData;
                            result.MenuItems = null;
                            return result;
                        }
                        data[2] = data[2].Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                        data[2] = data[2].Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                        try
                        {
                            menuItems.Add(new MenuItemExcelTransferModel()
                            {
                                FoodCategory = data[0],
                                Name = data[1],
                                Price = Convert.ToDecimal(data[2]),
                                Description = data[3],
                                ImageUrl = data[4]
                            });
                        }
                        catch (Exception)
                        {
                            result.Message = WrongExcelData;
                            result.MenuItems = null;
                            return result;
                        }
                    }
                    result.IsDataExtracted = true;
                    result.Message = SuccessfulyExtractedExcelData;
                    result.MenuItems = menuItems;
                    return result;
            });
    }
}
