namespace DishHunter.Services.Data
{
    using System.IO;
    using System.Threading.Tasks;
    using System.Globalization;
	using System.Text;
    using System.Net;
    using OfficeOpenXml;
    using Models.Excel;
    using Models.Menu;
    using Models.MenuItem;
    using Models.Restaurant;
    using Interfaces;
    using static Common.NotificationMessagesConstants;

    public class ExcelService : IExcelService
    {
        private const char csvDelimeter = '~';
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
        public async Task<MenuExtractResult> ExtractMenuDataFromExcel(Stream stream)
        {
            MenuExtractResult result = new MenuExtractResult()
            {
                IsDataExtracted = false,
                Message = string.Empty
            };
            List<MenuExcelTransferModel> menus =
                new List<MenuExcelTransferModel>();
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			using (var package = new ExcelPackage(stream))
            {
				var format = new ExcelOutputTextFormat();
				format.Delimiter = csvDelimeter;
				format.Encoding = Encoding.UTF8;
				var worksheets = package.Workbook.Worksheets;
                foreach (var ws in worksheets)
                {
                    var tables = ws.Tables;
                    foreach (var t in tables)
                    {
                        var rows = (await t.ToTextAsync(format))
                            .Split(Environment.NewLine)
                            .Skip(1);
                        string[] data = rows.First().Split(csvDelimeter, StringSplitOptions.RemoveEmptyEntries);
                        MenuExcelTransferModel menu;
                        try
                        {
                            menu = new MenuExcelTransferModel()
                            {
                                MenuType = WebUtility.HtmlEncode(data[0]),
                                FoodType = WebUtility.HtmlEncode(data[1]),
                                Description = WebUtility.HtmlEncode(data[2])
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
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			using (var package = new ExcelPackage(stream))
            {
                var worksheets = package.Workbook.Worksheets;
				var format = new ExcelOutputTextFormat();
				format.Delimiter = csvDelimeter;
				format.Encoding = Encoding.UTF8;
				foreach (var ws in worksheets)
                {
                    var tables = ws.Tables;
                    foreach (var t in tables)
                    {						
						var rows = (await t.ToTextAsync(format))
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
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			using (var package = new ExcelPackage(stream))
            {
                var worksheets = package.Workbook.Worksheets;
				var format = new ExcelOutputTextFormat();
				format.Delimiter = csvDelimeter;
				format.Encoding = Encoding.UTF8;
				foreach (var ws in worksheets)
                {
                    var tables = ws.Tables;
                    foreach (var t in tables)
                    {
                        var rows = (await t.ToTextAsync(format))
                            .Split(Environment.NewLine)
                            .Skip(1);
                        foreach (var row in rows)
                        {
                            string[] data = row.Split(csvDelimeter);
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
                                    Name = WebUtility.HtmlEncode(data[0]),
                                    Region = WebUtility.HtmlEncode(data[1]),
                                    SettlementName = WebUtility.HtmlEncode(data[2]),
                                    Address = WebUtility.HtmlEncode(data[3]),
                                    PhoneNumber = WebUtility.HtmlEncode(data[4]),
                                    CategoryName = WebUtility.HtmlEncode(data[5]),
                                    ImageUrl = WebUtility.HtmlEncode(data[6])
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
            => await Task.Run(() =>
            {
                MenuItemExtractResult result = new MenuItemExtractResult()
                {
                    IsDataExtracted = false,
                    Message = string.Empty
                };
                List<MenuItemExcelTransferModel> menuItems =
                                new List<MenuItemExcelTransferModel>();
                foreach (var row in csvRows)
                {
                    string[] data = row.Split(csvDelimeter);
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
                            FoodCategory = WebUtility.HtmlEncode(data[0]),
                            Name = WebUtility.HtmlEncode(data[1]),
                            Price = Convert.ToDecimal(WebUtility.HtmlEncode(data[2])),
                            Description = WebUtility.HtmlEncode(data[3]),
                            ImageUrl = WebUtility.HtmlEncode(data[4])
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
