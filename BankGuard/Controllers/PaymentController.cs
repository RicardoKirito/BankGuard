using BankGuard.Core.Application.Dtos.Account;
using BankGuard.Core.Application.Dtos.Transaction;
using BankGuard.Core.Application.Enums;
using BankGuard.Core.Application.Helpers;
using BankGuard.Core.Application.Interfaces.Services;
using BankGuard.Core.Application.ViewModels.Transaction;
using BankGuard.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace BankGuard.Controllers
{
   
     [Authorize(Roles ="Basic")]
    public class PaymentController : Controller
    {
        private readonly IProductService _productService;
        private readonly ITransactionService _transactionService;
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly IUserServices _userServices;
        public PaymentController(IProductService productService, IUserServices userServices, ITransactionService transactionService, IBeneficiaryService beneficiaryService)
        {
            _productService = productService;
            _userServices = userServices;
            _transactionService = transactionService;
            _beneficiaryService = beneficiaryService;
        }
        public IActionResult Express()
        {
            TransactionResponse validation = new TransactionResponse
            {
                HasError = false
            };
            ViewBag.Error = validation;
            var id = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            ViewBag.Account = _productService.GetAllByUserid(id).Result.Where(d=> d.Type== Accounttype.Saving.ToString()).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Express(SaveTransactionViewModel save)
        {
            var id = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            ViewBag.Account = _productService.GetAllByUserid(id).Result.Where(d => d.Type == Accounttype.Saving.ToString()).ToList();

            TransactionResponse response = new TransactionResponse
            {
                HasError = false

            };


            if (!ModelState.IsValid)
            {
                ViewBag.Error = response;
                return View(save);
            }
            
            var validation = await _transactionService.TransactionValitor(save);
            if (validation.HasError)
            {
                ViewBag.Error = validation;
                return View(save);
            }
            save.By = id;
            save.Type = Operations.Payment.ToString();
            if(save.Detail == null)
            {
                save.Detail = $"Express payment to {save.AccountTo}";

            }
            return RedirectToAction("ConfirmPayment", save);
            
        }
        public async Task<IActionResult> CreditCard()
        {
            var id = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            var accounts = _productService.GetAllByUserid(id).Result.Where(d => d.Type == Accounttype.Saving.ToString()).ToList();
            var cards = _productService.GetAllByUserid(id).Result.Where(d => d.Type == Accounttype.CreditCard.ToString()).ToList();
            if (!cards.Any())
            {

            }
            ViewBag.Account = accounts;
            ViewBag.Cards = cards;
            TransactionResponse response = new TransactionResponse
            {
                HasError = false
            };
            ViewBag.Error = response;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreditCard(SaveTransactionViewModel save)
        {
            var id = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            var accounts = _productService.GetAllByUserid(id).Result.Where(d => d.Type == Accounttype.Saving.ToString()).ToList();
            var cards = _productService.GetAllByUserid(id).Result.Where(d => d.Type == Accounttype.CreditCard.ToString()).ToList();
            if (!cards.Any())
            {

            }
            ViewBag.Account = accounts;
            ViewBag.Cards = cards;
            TransactionResponse response = new TransactionResponse
            {
                HasError = false
            };

            if (!ModelState.IsValid)
            {
                ViewBag.Error = response;
                return View(save);
            }
            var validation = await _transactionService.TransactionValitor(save);
            if (validation.HasError)
            {
                ViewBag.Error = validation;
                return View(save);
            }
            save.Type = Operations.Payment.ToString();
            save.By = id;
            if (save.Detail == null)
            {
                save.Detail = $"Payment maid to creditcard";

            }
            return RedirectToAction("ConfirmPayment", save);
        }
        public async Task<IActionResult> Loan()
        {
            var id = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            var products = _productService.GetAllByUserid(id).Result.Where(d => d.Type == Accounttype.Saving.ToString()).ToList();
            var loans = _productService.GetAllByUserid(id).Result.Where(d => d.Type == Accounttype.Loan.ToString()).ToList();

            TransactionResponse response = new TransactionResponse { HasError = false };

            ViewBag.Error = response;
            ViewBag.Accounts = products;
            ViewBag.Loans = loans;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Loan(SaveTransactionViewModel save)
        {
            var id = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            var products = _productService.GetAllByUserid(id).Result.Where(d => d.Type == Accounttype.Saving.ToString()).ToList();
            var loans = _productService.GetAllByUserid(id).Result.Where(d => d.Type == Accounttype.Loan.ToString()).ToList();

            TransactionResponse response = new TransactionResponse { HasError = false };

            ViewBag.Accounts = products;
            ViewBag.Loans = loans;
            if (!ModelState.IsValid)
            {
                ViewBag.Error = response;
                return View(save);
            }
            var validation = await _transactionService.TransactionValitor(save);
            if (validation.HasError)
            {
                ViewBag.Error = validation;
                return View(save);
            }
            save.Type = Operations.Payment.ToString();
            save.By = id;
            if (save.Detail == null)
            {
                save.Detail = $"Payment maid to Loan";

            }
            return RedirectToAction("ConfirmPayment", save);


        }
        public async Task<IActionResult> Beneficiaries()
        {
            var id = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            var products = _productService.GetAllByUserid(id).Result.Where(d => d.Type == Accounttype.Saving.ToString()).ToList();
            var result = await _beneficiaryService.GetAllWithUser();
            var beneficiaries = result.Where(d => d.Userid == id).ToList();

            TransactionResponse response = new TransactionResponse { HasError = false };

            ViewBag.Error = response;
            ViewBag.Accounts = products;
            ViewBag.bnf = beneficiaries;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Beneficiaries(SaveTransactionViewModel save)
        {
            var id = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            var products = _productService.GetAllByUserid(id).Result.Where(d => d.Type == Accounttype.Saving.ToString()).ToList();
            var result = await _beneficiaryService.GetAllWithUser();
            var beneficiaries = result.Where(d => d.Userid == id).ToList();

            TransactionResponse response = new TransactionResponse { HasError = false };

            ViewBag.Accounts = products;
            ViewBag.bnf = beneficiaries;
            if (!ModelState.IsValid)
            {
                ViewBag.Error = response;
                return View(save);
            }
            var validation = await _transactionService.TransactionValitor(save);
            if (validation.HasError)
            {
                ViewBag.Error = validation;
                return View(save);
            }
            save.Type = Operations.Payment.ToString();
            save.By = id;
            if (save.Detail == null)
            {
                save.Detail = $"Payment maid to {save.AccountTo}";

            }
            return RedirectToAction("ConfirmPayment", save);


        }
        public async Task<IActionResult> CashAdvance()
        {
            var id = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            var cards = _productService.GetAllByUserid(id).Result.Where(d => d.Type == Accounttype.CreditCard.ToString()).ToList();

            ViewBag.Cards = cards;

            TransactionResponse response = new TransactionResponse
            {
                HasError = false
            };
            
            ViewBag.Error = response;
            SaveTransactionViewModel model = new SaveTransactionViewModel
            {
                AccountTo = _productService.GetPrimary(id).Result.accountnumber
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CashAdvance(SaveTransactionViewModel save)
        {
            var id = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            var cards = _productService.GetAllByUserid(id).Result.Where(d => d.Type == Accounttype.CreditCard.ToString()).ToList();

            ViewBag.Cards = cards;
            TransactionResponse response = new TransactionResponse
            {
                HasError = false
            };

            if (!ModelState.IsValid)
            {
                ViewBag.Error = response;
                return View(save);
            }
            var validation = await _transactionService.TransactionValitor(save);
            if (validation.HasError)
            {
                ViewBag.Error = validation;
                return View(save);
            }
            save.Type = Operations.CashAdvance.ToString();
            save.By = id;
            save.Date = DateTime.Now.ToShortDateString();
            save.Detail = "Cash Advance";
            await _transactionService.Add(save);
            return RedirectToRoute(new {controller ="Basic", action="Index"});
        }
        public IActionResult Transfer()
        {
            TransactionResponse validation = new TransactionResponse
            {
                HasError = false
            };
            ViewBag.Error = validation;
            var id = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            ViewBag.Account = _productService.GetAllByUserid(id).Result.Where(d => d.Type == Accounttype.Saving.ToString()).ToList();
            ViewBag.Account2 = _productService.GetAllByUserid(id).Result.Where(d => d.Type == Accounttype.Saving.ToString()).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Transfer(SaveTransactionViewModel save)
        {
            var id = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            ViewBag.Account = _productService.GetAllByUserid(id).Result.Where(d => d.Type == Accounttype.Saving.ToString()).ToList();
            ViewBag.Account2 = _productService.GetAllByUserid(id).Result.Where(d => d.Type == Accounttype.Saving.ToString()).ToList();

            TransactionResponse response = new TransactionResponse
            {
                HasError = false

            };


            if (!ModelState.IsValid)
            {
                ViewBag.Error = response;
                return View(save);
            }

            var validation = await _transactionService.TransactionValitor(save);
            if (validation.HasError)
            {
                ViewBag.Error = validation;
                return View(save);
            }
            save.By = id;
            save.Type = Operations.Transfer.ToString();
            save.Date = DateTime.Now.ToShortDateString();
            save.Detail = "Transfer between my accounts";
            await _transactionService.Add(save);
            return RedirectToRoute(new { controller = "Basic", action = "Index" });

        }

        public async Task<IActionResult> ConfirmPayment(SaveTransactionViewModel save)
        {
            var product =await _productService.GetById(save.AccountTo);
            var user = await  _userServices.GetUserByid(product.UserId);
            TempData["Name"] = $"{user.Name} {user.LastName}";
            return View(save);
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmPaymentPost(SaveTransactionViewModel save)
        {
            await _transactionService.Add(save);

            return RedirectToRoute(new{ controller="Basic", action="Index"});
        }
    }
}
