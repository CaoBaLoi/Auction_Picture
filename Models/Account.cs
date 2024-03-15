using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;

namespace Auction_Picture.Models;
public class Account{
    [Key, Column(Order = 1)]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public required string Username { get; set; }
    [Required]
    //[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
    public required string Email { get; set; }

    [Required]
    //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]

    public required string Password { get; set; }
    [NotMapped]
    [Required]
    [System.ComponentModel.DataAnnotations.Compare("Password")]
    public required string ConfirmPassword { get; set; }
    public required int Role{get;set;}
    public int? OTP{get;set;}
    public DateTime? OtpExpiration{get;set;}

    public User? User {get;set;}
}