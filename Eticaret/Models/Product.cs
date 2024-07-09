using System.ComponentModel.DataAnnotations;

namespace Eticaret.Models
{
    public class Product{

        [Display(Name ="Ürün Id")]
        public int ProductId {get;set;}

        [Display(Name ="Ürün Id")]
        public string Name {get;set;} = string.Empty;

        [Display(Name ="Ürün Fiyatı")]
        public decimal Price {get;set;}
        public string Image {get;set;} = string.Empty;
        public bool IsActive {get;set;}
        public int CategoryId {get;set;}
    }
}