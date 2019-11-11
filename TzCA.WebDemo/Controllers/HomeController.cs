using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TzCA.DataAccess;
using TzCA.Entities.ApplicationOrganization;
using TzCA.Entities.BusinessOrganization;

namespace TzCA.Web.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 注入Demo
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEntityRepository<Person> _iPersonRepository;
        public HomeController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEntityRepository<Person> iPersonRepository)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._iPersonRepository = iPersonRepository;
        }
        public IActionResult Index()
        {
            var persons = _iPersonRepository.GetAll().ToList();
            return View();
        }        

        public IActionResult Error()
        {
            return View();
        }
    }
}
