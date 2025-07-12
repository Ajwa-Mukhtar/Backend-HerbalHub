using Application.DataTransferModels.ResponseModels;
using Microsoft.EntityFrameworkCore;
using HerbalHub.AppDb;
using HerbalHub.DTO.UserVM;
using HerbalHub.Interfaces;
using HerbalHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HerbalHub.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUser _userService;
        private readonly AppDbcontext _db;

        public UserController(IUser userService, AppDbcontext db)
        {
            _userService = userService;
            _db = db;
        }

        [HttpPost("User")]
        [AllowAnonymous]
        public async Task<IActionResult> createuser(CreateUserVm user)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid user data.");

            var result = await _userService.createuser(user);
            return Ok(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser(LoginUserVm loginVm)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid login data.");

            var result = await _userService.LoginUser(loginVm);
            return Ok(result);
        }
        [HttpGet("get-user-by-email")]
        [Authorize]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
                return BadRequest("Email is required.");

            var user = await _db.UsersDetails.FirstOrDefaultAsync(u => u.UserEmail == userEmail);
            if (user == null)
                return NotFound("User not found.");

            return Ok(new
            {
                responseCode = 200,
                message = "User profile fetched successfully.",
                data = new
                {
                    user.UserName,
                    user.UserEmail,
                    user.PhoneNumber,
                    user.Address
                }
            });
        }


        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string userEmail)
        {
            if (!ModelState.IsValid)
                return BadRequest("Email Not Found.");

            var result = await _userService.VerifyEmail(userEmail);
            return Ok(result);
        }

        [HttpGet("getallUser")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("get-User-ByName")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserByName(string UserName)
        {
            var result = await _userService.GetUserByName(UserName);
            return Ok(result);
        }

        [HttpPost("ContactUs")]
        [AllowAnonymous]
        public async Task<IActionResult> ContactUs(ContactFormVM contact)
        {
            var result = await _userService.ContactUs(contact);
            return Ok(result);
        }

        [HttpPost("consultation")]
        [AllowAnonymous]
        public async Task<IActionResult> Consultation(consultationvm con)
        {
            var result = await _userService.Consultation(con);
            return Ok(result);
        }

        // ✅ Updated Checkout Method with Product Saving
        [HttpPost("checkout")]
        [AllowAnonymous]
        public async Task<IActionResult> Checkout(checkoutvm check)
        {
            if (check == null || check.Products == null || !check.Products.Any())
            {
                return BadRequest("Invalid checkout data.");
            }

            // Step 1: Save main checkout info
            var newCheckout = new Checkout
            {
                FullName = check.FullName,
                UserEmail = check.UserEmail,
                PhoneNumber = check.PhoneNumber,
                ShippingAddress = check.ShippingAddress,
                Country = check.Country,
                City = check.City,
                PaymentMethod = check.PaymentMethod
            };

            _db.Checkouts.Add(newCheckout);
            await _db.SaveChangesAsync(); // Needed to get newCheckout.Id

            // Step 2: Save related products
            foreach (var item in check.Products)
            {
                var product = new CheckoutProduct
                {
                    CheckoutId = newCheckout.Id,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                _db.CheckoutProducts.Add(product);
            }

            await _db.SaveChangesAsync();
            var result = await _userService.checkouts(check);
            return Ok(new
            {
                ResponseCode = 200,
                Message = "Order placed successfully"
            });
        }

    }
}

