using Microsoft.AspNetCore.Mvc;

namespace VTP_22_Dashboard.ViewComponents
{
    public class SidebarViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
