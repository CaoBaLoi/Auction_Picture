using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auction_Picture.Models;
public class Product{
    [Key, Column(Order = 1)]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id_Product { get; set; }

    public string? Product_Name {get;set;}

    public string? Image_Url {get;set;}

    public string? Info {get;set;}

    public double Starting_Price {get;set;}

}