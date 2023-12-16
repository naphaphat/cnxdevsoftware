using cnxdevsoftware.Context;
using cnxdevsoftware.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cnxdevsoftware.Controllers
{
    public class ItemController : Controller
    {
        private readonly ItemService _itemService;

        public ItemController(ItemService itemService)
        { 
            this._itemService = itemService;
        }

        public async Task<IActionResult> Index()
        {
            var (isErr, errMsg, items) =  await _itemService.GetItemAll();
          
            if (!isErr)
            {
                return View(items);
            }
            else
            {
                string ErrMag = $"error : {errMsg}";
                return View(ErrMag);
            }
        }

    }
}
