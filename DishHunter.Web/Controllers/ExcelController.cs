namespace DishHunter.Web.Controllers
{
    public class ExcelController : BaseController
    {
        protected async Task<bool> IsExcelFile(IFormFile file)
        {
            return await Task.Run(() =>
            {
                return file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                || file.ContentType == "application/vnd.ms-excel"
                || Path.GetExtension(file.FileName).Equals(".xls", StringComparison.OrdinalIgnoreCase)
                || Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase);
            });
        }
    }
}
