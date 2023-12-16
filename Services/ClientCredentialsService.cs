using Azure.Core;
using cnxdevsoftware.Context;
using cnxdevsoftware.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace cnxdevsoftware.Services
{
    public class ClientCredentialsService
    {
        private readonly IDbContextFactory<CnxDevSoftwareContext> _dbContext;
        public ClientCredentialsService(IDbContextFactory<CnxDevSoftwareContext> dbContext)
        { 
            this._dbContext = dbContext;
        }
        public async Task<(bool isErr, string errMsg, Models.AccessToken? token)> GeTokenFormDb()
        {
            bool Err = false;
            string ErrMsg = string.Empty;

            try
            {
                var context = _dbContext.CreateDbContext();
                var credential = await context.AccessTokens.OrderByDescending(x => x.Id).FirstOrDefaultAsync();

                return (Err, ErrMsg, credential);
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
        public async Task<(bool isSave, string errMsg)> SaveAccessToken(ClientCredentials token)
        {
            bool Err = false;
            string ErrMsg = string.Empty;

            try
            {
                var context = _dbContext.CreateDbContext();


                var Data = new Models.AccessToken
                {
                    AccessToken1 = token.access_token,
                    TokenType = token.token_type,
                    ExpiresIn = token.expires_in,
                    LastUpdate = DateTime.Now,
                };

                await context.AccessTokens.AddAsync(Data);
                await context.SaveChangesAsync();

                return (Err, ErrMsg);
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;

                Err = true;
                ErrMsg = ex.Message;

                return(Err, ErrMsg);

            }

        }
    }
}
