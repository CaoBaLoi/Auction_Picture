using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auction_Picture.Models;
public class User{
    [Key, Column(Order = 1)]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id_User { get; set; }

    public required string Name {get;set;}

    public required string Sdt {get;set;}

    public required string Address {get;set;}

    public int ID {get;set;}
    public Account? Account{get;set;}
}