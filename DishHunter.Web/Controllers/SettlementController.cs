namespace DishHunter.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Interfaces;
    using Services.Data.Models.Settlement;
    using ViewModels.Settlement;
    using static Common.RolesConstants;
    using static Common.NotificationMessagesConstants;
    [Authorize(Roles = AdminRoleName)]
    public class SettlementController : BaseController
    {
        private readonly ISettlementService settlementService;
        public SettlementController(ISettlementService _settlementService)
        {
            settlementService = _settlementService;
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            try
            {
                SettlementFormViewModel settlement = new SettlementFormViewModel();
                return View(settlement);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add(SettlementFormViewModel model)
        {
            try
            {
                int? settlementId = await settlementService
                    .SettlementExistsByNameAndRegionAsync(model.SettlementName, model.Region);         
                if (settlementId.HasValue)
                {
                    TempData[ErrorMessage] = "Това селище вече съществува!";
                    return RedirectToAction("All", "Settlement");
                }
                await settlementService.CreateSettlementAsync(new SettlementPostTransferModel()
                {
                    SettlementName=model.SettlementName,
                    Region=model.Region
                });
                return RedirectToAction("All", "Settlement");
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                bool isSettlementExisting = await settlementService.ExistsByIdAsync(id);
                if (!isSettlementExisting)
                {
                    TempData[ErrorMessage] = "Търсеното от вас селище не съществува!";
                    return RedirectToAction("All", "Settlement");
                }
                var tm = await settlementService.GetSettlementForEditByIdAsync(id);
                return View(new SettlementFormViewModel()
                {
                    SettlementName = tm.SettlementName,
                    Region = tm.Region
                }); 
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, SettlementFormViewModel model)
        {
            try
            {
                bool isSettlementExisting = await settlementService.ExistsByIdAsync(id);
                if (!isSettlementExisting)
                {
                    TempData[ErrorMessage] = "Търсеното от вас селище не съществува!";
                    return RedirectToAction("All", "Settlement");
                }
                int? settlementId = await settlementService
                   .SettlementExistsByNameAndRegionAsync(model.SettlementName, model.Region);
                if (settlementId.HasValue)
                {
                    TempData[ErrorMessage] = "Това селище вече съществува!";
                    return RedirectToAction("All", "Settlement");
                }
                await settlementService.EditSettlementByIdAsync(id, new SettlementPostTransferModel()
                {
                    SettlementName = model.SettlementName,
                    Region=model.Region
                });
                return RedirectToAction("All", "Settlement");
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpGet]
        public async Task<IActionResult> All()
        {
            try
            {
                var categories = (await settlementService.AllSettlementsAsync())
                    .Select(s => new SettlementSelectViewModel()
                    {
                        Id = s.Id,
                        SettlementName = s.SettlementName,
                        Region=s.Region
                    });
                return View(categories);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                bool isSettlementExisting = await settlementService.ExistsByIdAsync(id);
                if (!isSettlementExisting)
                {
                    TempData[ErrorMessage] = "Търсеното от вас селище не съществува!";
                    return RedirectToAction("All", "Settlement");
                }
                await settlementService.DeleteSettlementByIdAsync(id);
                return RedirectToAction("All", "Settlement");
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
    }
}
