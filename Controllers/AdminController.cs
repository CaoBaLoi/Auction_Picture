using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Auction_Picture.Models;
namespace Auction_Picture.Controllers;
public class AdminController : Controller{
    private readonly ILogger<AdminController> _logger;
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _hostingEnvironment;
    public AdminController(ILogger<AdminController> logger, AppDbContext dbContext, IWebHostEnvironment hostingEnvironment)
    {
        _logger = logger;
        _db = dbContext;
        _hostingEnvironment = hostingEnvironment;
    }
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult AccountManagement(){
        List<Account> accounts = _db.Account.ToList();
        return View(accounts);
    }
    public IActionResult Delete(int id){
        var account = _db.Account.FirstOrDefault(a => a.ID == id);
        if(account == null){
            return NotFound();
        }
        try
        {
            var user = _db.User.FirstOrDefault(u => u.ID == id);
            if(user!=null){
                _db.User.Remove(user);
                _db.SaveChanges();
            }
            _db.Account.Remove(account);
            _db.SaveChanges();  
            return RedirectToAction("AccountManagement");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.InnerException, "Error deleting account");
            return StatusCode(500, "An error occurred while deleting the account.");
        }
    }
    public IActionResult ProductManagement(){
        List<Product> products = _db.Product.ToList();
        ViewData["Products"] = products;
        return View();
    }
    [HttpPost]
    public IActionResult AddProduct(Product product, IFormFile imageFile){
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Image");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }

                product.Image_Url = "/Image/" + uniqueFileName; // Lưu đường dẫn ảnh vào Image_Url
            }

            // Thêm sản phẩm vào cơ sở dữ liệu
            _db.Product.Add(product);
            _db.SaveChanges();

            // Chuyển hướng đến trang khác hoặc thông báo thành công
            return RedirectToAction("ProductManagement");
        }
        return RedirectToAction("ProductManagement");
    }
    public IActionResult DelProduct(int id){
        var product = _db.Product.FirstOrDefault(a => a.Id_Product == id);
        //Console.WriteLine(product.Id_Product);
        if (product == null)
        {
            // Nếu không tìm thấy sản phẩm, trả về một phản hồi không hợp lệ hoặc phản hồi lỗi
            return NotFound(); // Hoặc BadRequest() tùy vào logic của bạn
        }
        // Trích xuất tên tệp hình ảnh từ URL
        var imageName = Path.GetFileName(product.Image_Url);

        // Đường dẫn tới thư mục Image
        var wwwRootPath = _hostingEnvironment.WebRootPath;

        // Tạo đường dẫn tuyệt đối đến tệp hình ảnh
        if (wwwRootPath != null && imageName != null)
        {
            // Tạo đường dẫn tuyệt đối đến thư mục Image
            var imagePath = Path.Combine(wwwRootPath, "Image", imageName);

            // Kiểm tra tồn tại của tệp hình ảnh trước khi xóa
            if (System.IO.File.Exists(imagePath))
            {
                // Xóa tệp hình ảnh
                System.IO.File.Delete(imagePath);
            }
        }
        // Xóa sản phẩm khỏi cơ sở dữ liệu
        _db.Product.Remove(product);
        _db.SaveChanges();

        // Chuyển hướng đến một trang hoặc hành động khác sau khi xóa thành công
        return RedirectToAction("ProductManagement"); // Ví dụ: chuyển hướng đến trang danh sách sản phẩm
    }
}