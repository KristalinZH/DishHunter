namespace DishHunter.Web.Infrastructrure.Helpers
{
    public class ActionHelper
    {
        public bool IsAllowed { get; set; }
        public string? Message { get; set; }
        public string? ActionName { get; set; }
        public string? ControllerName { get; set; }
    }
}
