using Application.DataTransferModels.ResponseModels;
using Dapper;
using HerbalHub.AppDb;
using HerbalHub.CommonMethods;

//using HerbalHub.CommonMethods;
using HerbalHub.DTO.UserVM;
using HerbalHub.Helpers;
using HerbalHub.Interfaces;
using HerbalHub.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HerbalHub.Services
{
    public class UserService : IUser
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbcontext _db;
        private readonly EmailService _emailService;
        private readonly JwtService _jwtService;
        // private readonly StoredProcExecutor _storedProcExecutor;

        public UserService(AppDbcontext db, IConfiguration configuration, JwtService jwtService, EmailService emailService)
        {
            _configuration = configuration;
            _db = db;
            _jwtService = jwtService;
            _emailService = emailService;
            // _storedProcExecutor = storedProcExecutor;
        }

        public async Task<ResponseVM> createuser(CreateUserVm user)
        {
            ResponseVM response = new ResponseVM();

                    var existingUser = await _db.UsersDetails.FirstOrDefaultAsync(u => u.UserEmail == user.UserEmail);
                    if (existingUser != null)
                    {
                        response.ResponseCode = 409;
                        response.ErrorMessage = "User already exists.";
                        return response;
                    }

                    User newUser = new User
                    {
                        UserEmail = user.UserEmail,
                        UserName = user.UserName,
                        Password = user.Password,
                        UserType = user.UserType,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Gender = user.Gender,
                        PhoneNumber = user.PhoneNumber,
                        DateOfBirth = user.DateOfBirth ?? DateTime.Now,
                        Address = user.Address,
                        IsEmailVerified = false,
                        IsBlocked = false
                    };

                    await _db.UsersDetails.AddAsync(newUser);
                    await _db.SaveChangesAsync();

                    // Send verification email
                    string verificationLink = $"https://yourdomain.com/verify-email?userId={newUser.Id}";
                    string subject = "Verify your HerbalHub account";
                    string body = $"<h2>Welcome to HerbalHub!</h2><p>Click below to verify your email:</p><a href='{verificationLink}'>Verify Email</a>";
                    await _emailService.SendEmailAsync(newUser.UserEmail, subject, body);

                    response.ResponseCode = 200;
                    response.ResponseMessage = "User created. Please verify email.";
                    response.Data = newUser;
                   

            return response;
        }

        public async Task<ResponseVM> LoginUser(LoginUserVm loginVm)
        {
            var response = new ResponseVM();

            var user = await _db.UsersDetails.FirstOrDefaultAsync(u =>
                u.UserEmail == loginVm.UserEmail && u.Password == loginVm.Password);

            if (user == null)
            {
                response.ResponseCode = 401;
                response.ErrorMessage = "Invalid credentials.";
                return response;
            }

            // ✅ Use JwtService here
            var token = _jwtService.GenerateJwtToken(user);

            response.ResponseCode = 200;
            response.ResponseMessage = "Login success.";
            response.Data = new { Token = token };

            return response;
        }
        public async Task<ResponseVM> GetAllUsersAsync()
        {
            var response = new ResponseVM();

            try
            {
                dynamic result = CommonMethod.ExecuteStoredProcedure("[GetAllUsers]");

                if (result == null || result.Count == 0)
                {
                    response.ResponseCode = 400;
                    response.ErrorMessage = "User not found.";
                }
                else
                {
                    response.ResponseCode = 200;
                    response.ResponseMessage = "User fetched successfully";
                    response.Data = result;
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.ErrorMessage = $"Internal error: {ex.Message}";
            }

            return response;
        }


        public async Task<ResponseVM> VerifyEmail(string userEmail)
        {
            var response = new ResponseVM();

            var user = await _db.UsersDetails.FirstOrDefaultAsync(u => u.UserEmail == userEmail);

            if (user == null)
            {
                response.ResponseCode = 401;
                response.ErrorMessage = "Not Verified. Try Again.";
                return response;
            }

            user.IsEmailVerified = true; // ✅ set the flag
            await _db.SaveChangesAsync();

            response.ResponseCode = 200;
            response.ResponseMessage = "Email Verified Successfully";
            return response;
        }
        public async Task<ResponseVM> GetUserByName(string UserName)
        {
            var response = new ResponseVM();
            var parameters = new DynamicParameters();
            parameters.Add("@searchName", UserName);

            dynamic result = CommonMethod.ExecuteStoredProcedure(parameters, "[GetUsersByNameSearch]");

            if (result == null || result.Count == 0)
            {
                response.ResponseCode = 400;
                response.ErrorMessage = "User not found.";
            }
            else
            {
                response.ResponseCode = 200;
                response.ResponseMessage = "User fetched successfully";
                response.Data = result;
            }

            return response;
        }
        public async Task<ResponseVM> ContactUs(ContactFormVM contact)
        {
            var response = new ResponseVM();
            ContactForm newContact = new ContactForm
            {
                Name = contact.Name,
                Email = contact.Email,
                Message = contact.Message,

            };
            await _db.Contacts.AddAsync(newContact);
            await _db.SaveChangesAsync();
            response.ResponseCode = 200;
            response.ResponseMessage = "Contact form submitted successfully.";
            return response;

        }
       public async Task<ResponseVM> Consultation(consultationvm con)
        {
            var response = new ResponseVM();
            consultation cons = new consultation
            {
                name = con.Name,
                email = con.Email,
                message = con.Message
            };
            await _db.Consultations.AddAsync(cons);
            await _db.SaveChangesAsync();
            string verificationLink = $"https://yourdomain.com/verify-email?userId={cons.id}";
            string subject = "Your Consultation has Been Listed ";
            string body = $@"
<html>
<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #000000;'>
<h2 style='color: #2e7d32;'>Welcome to Herbal Hub : Consultation</h2>
    <p>Dear {cons.name},</p>
    
    <p>Thank you for choosing Herbal Hub for your wellness consultation!</p>
    
    <p>
        <span style='color: #2e7d32;'>✓</span> <strong>Request Received:</strong> We've got your details<br>
        <span style='color: #2e7d32;'>✓</span> <strong>Next Step:</strong> Our team will contact you within 24-48 hours<br>
        <span style='color: #2e7d32;'>✓</span> <strong>You'll Receive:</strong><br>
        &nbsp;&nbsp;- Personalized appointment time<br>
        &nbsp;&nbsp;- Google Meet link<br>
        &nbsp;&nbsp;- Preparation guidelines
    </p>
    
    <p>While you wait:</p>
    <ul>
        <li>Explore our Social Media Platforms:</li>
        <li>Follow @sardargeedawakhanaglobal on Instagram</li>
        <li>Follow @sardargeedawakhanaglobal on TikTok</li>
    </ul>
    
    <p>Warm regards,<br>
    The Herbal Hub Team</p>
</body>
</html>";
            await _emailService.SendEmailAsync(cons.email, subject, body);
            response.ResponseCode = 200;
            response.ResponseMessage = "User created. Please verify email.";
            response.Data = cons;

            return response;

        }

        // Checkout method
        public async Task<ResponseVM> checkouts(checkoutvm check)
        {
            var response = new ResponseVM();

            try
            {
                // Step 1: Save the main checkout
                var checkout = new Checkout
                {
                    FullName = check.FullName,
                    UserEmail = check.UserEmail,
                    PhoneNumber = check.PhoneNumber,
                    ShippingAddress = check.ShippingAddress,
                    Country = check.Country,
                    City = check.City,
                    PaymentMethod = check.PaymentMethod
                   
                };
                //Product Deils table to send data through email
                string productTable = "<table style='width:100%; border-collapse: collapse;'>"
                    + "<thead><tr>"
                    + "<th style='border: 1px solid #ccc; padding: 8px;'>Product</th>"
                    + "<th style='border: 1px solid #ccc; padding: 8px;'>Quantity</th>"
                    + "<th style='border: 1px solid #ccc; padding: 8px;'>Price</th>"
                    + "</tr></thead><tbody>";

                foreach (var item in check.Products)
                {
                    productTable += "<tr>"
                                  + $"<td style='border: 1px solid #ccc; padding: 8px;'>{item.ProductName}</td>"
                                  + $"<td style='border: 1px solid #ccc; padding: 8px;'>{item.Quantity}</td>"
                                  + $"<td style='border: 1px solid #ccc; padding: 8px;'>Rs. {item.Price}</td>"
                                  + "</tr>";
                }

                productTable += "</tbody></table>";

                await _db.Checkouts.AddAsync(checkout);
                await _db.SaveChangesAsync(); // Must save here to get checkout.Id

                // Step 2: Save related products (if any)
                if (check.Products != null && check.Products.Any())
                {
                    foreach (var item in check.Products)
                    {
                        var product = new CheckoutProduct
                        {
                            CheckoutId = checkout.Id,
                            ProductName = item.ProductName,
                            Quantity = item.Quantity,
                            Price = item.Price
                        };
                        await _db.CheckoutProducts.AddAsync(product);
                    }
                    await _db.SaveChangesAsync();
                }

                // Step 3: Send confirmation email
                string subject = "Your HerbalHub Order is Confirmed!";
                string body = $@"
    <div style='font-family: Arial, sans-serif;'>
        <h2 style='color: #4CAF50;'>Thank you, {checkout.FullName}!</h2>
        <p>Your order has been successfully placed. We’ll begin processing it shortly.</p>

        <h3 style='color: #4CAF50;'>📦 Order Summary:</h3>
        {productTable}

        <p><strong>Shipping Address:</strong> {checkout.ShippingAddress}, {checkout.City}, {checkout.Country}</p>
        <p><strong>Payment Method:</strong> {checkout.PaymentMethod}</p>

        <p style='margin-top: 20px;'>We’ll contact you soon with tracking info!</p>
        <hr />
        <p style='font-size: 12px; color: #888;'>This is an automated email from HerbalHub. Please do not reply.</p>
    </div>";

                await _emailService.SendEmailAsync(checkout.UserEmail, subject, body);

                response.ResponseCode = 200;
                response.ResponseMessage = "Order placed and confirmation email sent.";
                response.Data = checkout;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.ErrorMessage = "Order failed: " + ex.Message;
            }

            return response;
        }



    }
}