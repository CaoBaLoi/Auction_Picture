using Auction_Picture.Models;
using Microsoft.AspNetCore.Mvc;

namespace Auction_Picture.Controllers;
public class UserController : Controller{
    private readonly ILogger<UserController> _logger;
    private readonly AppDbContext _db;
    public UserController(ILogger<UserController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _db = dbContext;
    }
    public IActionResult AddInfo(){
        return View();
    }

    [HttpPost]
    public IActionResult AddInfo(User user){
        if(ModelState.IsValid){
            var ID = HttpContext.Session.GetInt32("ID_Account");
            if(ID!=null){
                user.ID = (int)ID;
                _db.User.Add(user);
                _db.SaveChanges();
                return RedirectToAction("Index","Home");
            }
        }
        return View();
    }
}