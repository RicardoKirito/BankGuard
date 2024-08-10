using BankGuard.Core.Application.Dtos.Account;
using BankGuard.Core.Application.Helpers;
using BankGuard.Core.Application.Interfaces.Services;
using BankGuard.Core.Application.ViewModels.Beneficiary;
using BankGuard.Core.Application.ViewModels.Product;
using BankGuard.Core.Application.ViewModels.Transaction;
using BankGuard.Middleware;
using BankGuard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BankGuard.Controllers
{
    [Authorize(Roles ="Basic")]
    public class BasicController : Controller
    {
       private readonly IProductService _productService;
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly ITransactionService _transactionServices;
        public BasicController(IProductService productService, IBeneficiaryService beneficiaryService, ITransactionService transactionService )
        {
            _productService = productService;
            _beneficiaryService = beneficiaryService;
            _transactionServices = transactionService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {

                var user = HttpContext.Session.Get<AuthenticationResponse>("user");
                List<ProductViewModel> products = await _productService.GetAllByUserid(user.Id);
                List<string> accounts = await _productService.GetLisAccountNumber(user.Id);
                List<TransactionViewModel> transactions =  _transactionServices.GetByAccountAsync(accounts).Result.OrderByDescending(d=> d.Id).ToList();
                ViewBag.Transactions = transactions;
                ViewBag.Greeting = "Good Morning";
                ViewBag.Greeting = (DateTime.Now.Hour >= 12)? "Good Afternoon": ViewBag.Greeting;
                ViewBag.Greeting = (DateTime.Now.Hour >= 18) ? "Good Evening" : ViewBag.Greeting;
                return View(products);

            }
            catch(Exception ex)
            {
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

        }
        public async Task<IActionResult> Beneficiaries()
        {
            var userid = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            List<BeneficiaryViewModel> beneficiaries = _beneficiaryService.GetAllWithUser().Result.Where(d=> d.Userid==userid).ToList();
            return View(beneficiaries);
        }
        public async Task<IActionResult> AddBeneficiary()
        {
            return View(new SaveBeneficiaryViewModel());
        }
            [HttpPost]
        public async Task<IActionResult> AddBeneficiary(SaveBeneficiaryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);

            }

            model.Userid = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
           // List<BeneficiaryViewModel> beneficiaries = _beneficiaryService.GetAllWithUser().Result.Where(b => b.Userid == model.Userid).ToList();
            var product = _productService.GetById(model.Accountnumber).Result;
            if(product == null)
            {
                ViewBag.Error = "This Account Doesn't exist";
                return View(model);
            }
           await _beneficiaryService.Add(model);
            TempData["modal"] = "Added";
            // beneficiaries = _beneficiaryService.GetAllWithUser().Result.Where(b => b.Userid == model.Userid).ToList();
            //ViewBag.Message = $"{beneficiary.Name} {beneficiary.LastName} added with the following account \"{beneficiary.Accountnumber}\"";
            return RedirectToRoute(new {controller="Basic", action="Beneficiaries"});
        }
        public async Task<IActionResult> DeleteBeneficiary(int id)
        {
            await _beneficiaryService.Delete(id);
            return RedirectToRoute(new { controller = "Basic", action = "Beneficiaries" });
        }
    }
}