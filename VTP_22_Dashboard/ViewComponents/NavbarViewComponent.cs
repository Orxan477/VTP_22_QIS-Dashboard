﻿using Microsoft.AspNetCore.Mvc;

namespace VTP_22_Dashboard.ViewComponents
{
    public class NavbarViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
