using AutoMapper;
using BankGuard.Core.Application.Dtos.Account;
using BankGuard.Core.Application.Enums;
using BankGuard.Core.Application.Interfaces.Services;
using BankGuard.Core.Application.ViewModels.Product;
using BankGuard.Core.Application.ViewModels.User;
using BankGuard.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BankGuard.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminServices _adminServices;
        private readonly IProductService _productService;
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        public AdminController(IAdminServices adminServices, IMapper mapper, IProductService service, ITransactionService transactionService)
        {
            _adminServices = adminServices;
            _mapper = mapper;
            _productService = service;
            _transactionService = transactionService;
        }

        public IActionResult Home()
        {
            ViewBag.ProductsAmount= _productService.GetAll().Result.Count();

            ViewBag.ClientsAmount = _adminServices.GetUsersAsync().Result.Count();

            ViewBag.Active = _adminServices.GetUsersAsync().Result.Where(u => u.IsVerified == true).Count();

            ViewBag.Inactive = _adminServices.GetUsersAsync().Result.Where(u => u.IsVerified == false).Count();

            ViewBag.TransferAmount = _transactionService.GetAll().Result.Where(t => t.Type == Operations.Transfer.ToString()).Count();

            ViewBag.TransferToday = _transactionService.GetAll().Result
                .Where(t => t.Type == Operations.Transfer.ToString() && t.Date
                .Contains(DateTime.Now.ToString("d"))).Count();

            ViewBag.PaymentAmount = _transactionService.GetAll().Result.Where(p => p.Type == Operations.Payment.ToString()).Count();

            ViewBag.PaymentToday = _transactionService.GetAll().Result
                .Where(p => p.Type == Operations.Payment.ToString() 
                && p.Date.Contains(DateTime.Now.ToString("d"))).Count();

            return View();
        }
        public async Task<IActionResult> UserManager()
        {
            List<UserViewModel> users = await _adminServices.GetUsersAsync();
            return View(users);
        }
        public async Task<IActionResult> Register()
        {

            return View(new SaveUserViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var origin = Request.Headers["origin"];
            RegisterResponse response = await _adminServices.RegisterUserAsync(vm, origin);
            if (response.HasError)
            {
                vm.Error = response.Error;
                vm.HasError = response.HasError;
                return View(vm);
            }
            return RedirectToRoute(new { controller = "Admin", action = "UserManager" });
        }
        public IActionResult Rolecheck(Roles role)
        {
            TempData["Role"] = role.ToString();
            TempData["Role"] = "";
            if (role == Roles.Admin)
            {
                TempData["Basic"] = "visually-hidden";
                
            }
            return RedirectToAction("Register");
        }
        public async Task<IActionResult> UserView(string id)
        {
            ViewBag.Response = new UserUpdateResponse();
            SaveUserViewModel save = await _adminServices.GetUserById(id);

            return View(save);
        }

        [HttpPost]
        public async Task<IActionResult> UserView(SaveUserViewModel save)
        {
            if (!ModelState.IsValid)
            {
                return View(save);
            }
            UserUpdateResponse response = await _adminServices.UpdateAsync(save);
            save.HasError = response.HasError;
            ViewBag.Response = response;
            
            if (!save.HasError)
            {
               return RedirectToRoute(new { controller = "Admin", action = "UserManager" });
            }
            return View(save);
        }
        public async Task<IActionResult> Statuschange(string id,bool status)
        {
            await _adminServices.ChangeSatatus(id, status);
            return RedirectToRoute(new { controller = "Admin", action = "UserManager" });

        }
        public async Task<IActionResult> ProductManager(string id)
        {
            List<ProductViewModel> products = await _productService.GetAllByUserid(id);
            return View(products);
        }
        public async Task<IActionResult> AddProduct(string id, Accounttype type)
        {
            SaveProductViewModel product = new SaveProductViewModel();
            product.UserId = id;
            product.Type = type.ToString();
            return View(product);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddProduct(SaveProductViewModel save)
        {

            if (!ModelState.IsValid)
            {
                return View(save);
            }
            await _productService.Add(save);
            return RedirectToRoute(new { controller = "Admin", action = "ProductManager", id =save.UserId});
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> DeleteProduct(string id, string userid)
        {

            string message = await _productService.DeleteAsync(id);
            List<ProductViewModel> products = await _productService.GetAllByUserid(userid);
            TempData["message"] = message;
            if (message != "")
            {
                return View("ProductManager", products);
            }
            return View("ProductManager", products);
        }

    }
}
