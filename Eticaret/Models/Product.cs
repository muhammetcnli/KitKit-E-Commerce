using System.ComponentModel.DataAnnotations;

namespace Eticaret.Models
{
    public class Product{

        [Display(Name ="Ürün Id")]
        public int ProductId {get;set;}

        [Display(Name ="Adı")]
        [Required(ErrorMessage ="Ürün adı giriniz")]
        [StringLength(100)]
        public string? Name {get;set;}

        [Display(Name ="Fiyatı")]
        [Required(ErrorMessage ="Ürün fiyatı giriniz")]
        [Range(0, 105000, ErrorMessage ="0 ile 105000 arası fiyat girin")]
        public decimal? Price {get;set;}

        [Display(Name ="Fotografı")]

        public string? Image {get;set;} = string.Empty;

        [Display(Name ="Aktif mi?")]
        public bool IsActive {get;set;}
        [Display(Name ="Kategorisi")]
        [Required]
        public int CategoryId {get;set;}
    }
}