using System.ComponentModel.DataAnnotations;

namespace Eticaret.Models
{
    public class Product{

        [Display(Name ="Ürün Id")]
        public int ProductId {get;set;}

        [Display(Name ="Ürün Adı")]
        [Required]
        public string Name {get;set;} = string.Empty;

        [Display(Name ="Ürün Fiyatı")]
        public decimal Price {get;set;}

        [Display(Name ="Ürün Fotografı")]
        public string Image {get;set;} = string.Empty;

        [Display(Name ="Aktif mi?")]
        public bool IsActive {get;set;}
        public int CategoryId {get;set;}
    }
}