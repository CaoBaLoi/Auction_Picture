using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Auction_Picture.Models;
using MySqlX.XDevAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Auction_Picture.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _db;
    private readonly IEmailSender _sm;
    public HomeController(ILogger<HomeController> logger, AppDbContext dbContext, IEmailSender sendmail)
    {
        _logger = logger;
        _db = dbContext;
        _sm = sendmail;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Login(){
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string email,string password)
    {
        if (ModelState.IsValid)
        {
            var data = _db.Account.FirstOrDefault(s => s.Email.Equals(email) && s.Password.Equals(password));
            if (data != null)
            {
                HttpContext.Session.SetString("IsLogin","true");
                HttpContext.Session.SetString("Username",data.Username);
                HttpContext.Session.SetInt32("ID_Account",data.ID);
                if(data.Role == 0){
                    HttpContext.Session.SetInt32("Role",0);
                    return RedirectToAction("Index","Admin");
                }
                else{
                    HttpContext.Session.SetInt32("Role",1);
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.error = "Login failed";
                return RedirectToAction("Login");
            }
        }
        return View();
    }
    public IActionResult Logout(){
        HttpContext.Session.Remove("IsLogin");
        return RedirectToAction("Index");
    }
    public IActionResult Signup(){
        return View();
    }
    [HttpPost]
    public IActionResult Signup(Account account)
    {
        if (ModelState.IsValid)
        {
            var check = _db.Account.FirstOrDefault(s => s.Email == account.Email);
            if (check == null)
            {
                int role = Convert.ToInt32(Request.Form["role"]);
                account.Role = role;
                _db.Account.Add(account);
                _db.SaveChanges();
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.error = "Email already exists";
                return View();
            }
        }
        return View(); 
    }
    public IActionResult GetOTP(){
        return View();
    }
    public int GenerateOTP()
    {
        Random random = new Random();
        int otp = random.Next(100000, 1000000); // Số ngẫu nhiên từ 100000 đến 999999
        return otp;
    }
    //Tạo và lưu OTP
    [HttpPost]
    public async Task<IActionResult> GetOTP(string email)
    {
        var user = _db.Account.FirstOrDefault(u => u.Email == email);
        if (user == null)
        {
            ViewBag.error = "Email already exists";
            return View();
        }

        var otp = GenerateOTP(); // Hàm tạo mã OTP ngẫu nhiên
        var otpExpiration = DateTime.Now.AddMinutes(5); // Thời gian hết hạn của OTP (ví dụ: 10 phút sau)

        // Lưu mã OTP vào cơ sở dữ liệu
        user.OTP = otp;
        user.OtpExpiration = otpExpiration;
        _db.SaveChanges();

        // Gửi OTP qua email
        await _sm.SendEmailAsync(email, "OTP for Password Reset", $"Your OTP is: {otp}");
        HttpContext.Session.SetString("Email",user.Email);

        // Trả về trang nhập OTP
        return RedirectToAction("ResetPassword");
    }
    public IActionResult ResetPassword(){
        return View();
    }
    [HttpPost]
    public IActionResult ResetPassword(string otp, string Password)
    {
        var email = HttpContext.Session.GetString("Email");
        var user = _db.Account.FirstOrDefault(u => u.Email == email && u.OTP.ToString() == otp && u.OtpExpiration > DateTime.Now);
        if (user == null)
        {
            ViewBag.error = "Invalid OTP or OTP has expired.";
            return View("ResetPassword");
        }

        // Cập nhật mật khẩu mới cho người dùng
        user.Password = Password; // Giả sử Password là thuộc tính lưu mật khẩu trong Account
        _db.SaveChanges();
        // Chuyển hướng người dùng đến trang thông báo mật khẩu đã được cập nhật thành công
        return RedirectToAction("Login");
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
