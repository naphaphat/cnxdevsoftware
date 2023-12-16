using cnxdevsoftware.Context;
using cnxdevsoftware.Models;
using Microsoft.EntityFrameworkCore;

namespace cnxdevsoftware.Services
{
    public class ItemService
    {
        private readonly IDbContextFactory<CnxDevSoftwareContext> _dbContext;
        public ItemService(IDbContextFactory<CnxDevSoftwareContext> dbContext)
        { 
            this._dbContext = dbContext;
        }
        public async Task<(bool isErr, string errMsg, List<ItemMaster>? items)> GetItemAll()
        {
            bool Err = false;
            string ErrMsg = string.Empty;

            try
            {
                var context = _dbContext.CreateDbContext();
                var selectItem = await context.ItemMasters.OrderByDescending(x=> x.ItemPrice).ToListAsync();

                return (Err, ErrMsg, selectItem);
            }
            catch (Exception ex)
            {
                while(ex.InnerException != null)
                    ex = ex.InnerException;

                Err = true;
                ErrMsg = ex.Message;

                return (Err, ErrMsg, null);

            }

        }
    }
}
