using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TzCA.Common.TzRandomData;
using TzCA.DataAccess;
using TzCA.Entities.ApplicationOrganization;
using TzCA.Entities.Attachments;
using TzCA.Entities.SiteManagement;
using TzCA.ViewModels.ApplicationOrganization;
using TzCA.ViewModels.TzDataVM;
using TzCA.ViewModels.TzPagination;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TzCA.Web.Controllers
{
    /// <summary>
    /// 全站数据访问控制器（GET）
    /// </summary>
    public class TzDataController : TzControllerBase
    {
        private readonly IRandomDataHepler _randomDataHepler;
        public TzDataController(
                 UserManager<ApplicationUser> userManager,
            IEntityRepository<BusinessImage> businessImage,
            IEntityRepository<TzSiteLog> tzSiteLog,
                IRandomDataHepler randomDataHepler
            ) : base(userManager, businessImage, tzSiteLog)
        {
            this._randomDataHepler = randomDataHepler;
        }

        /// <summary>
        /// 系统默认头像
        /// </summary>
        /// <returns></returns>
        public List<string> DefaultAvatars()
        {
            return _randomDataHepler.DefaultAvatars();
        }

        /// <summary>
        /// 系统默认语录
        /// </summary>
        /// <returns></returns>
        public List<string> Quotations()
        {
            return _randomDataHepler.GetQuotations();
        }

        /// <summary>
        /// 获取全站用户
        /// </summary>
        /// <returns></returns>
        public async Task<PaginationOut<List<ApplicationUserDto>>> AllUsers(GetAllUsersInput input)
        {
            return await GetAllUsersDto(input);
        }

        /// <summary>
        /// 获取用户数量
        /// </summary>
        /// <returns></returns>
        public int AllUsersCount() => _userManager.Users.Count();
     
    }
}
