namespace Eticaret.Models
{       
         public class Repository{
        private static readonly List<Product> _product = new();
        private static readonly List<Category> _category = new();

        static Repository(){
            _category.Add(new Category{CategoryId = 1, Name = "Kitap"});
            _category.Add(new Category{CategoryId = 2, Name = "Dergi"});
           
            _product.Add(new Product{ProductId=1, Name= "Huzursuzluğun Kitabı", Price = 85000, IsActive=true,Image="1.webp",CategoryId = 1});
            _product.Add(new Product{ProductId=2, Name= "Goebbels", Price = 95000, IsActive=true,Image="2.webp",CategoryId = 1});
            _product.Add(new Product{ProductId=3, Name= "Savaş Sanatı", Price = 75000, IsActive=true,Image="3.webp",CategoryId = 1});
            _product.Add(new Product{ProductId=4, Name= "Çürümenin Kitabı", Price = 65000, IsActive=true,Image="4.webp",CategoryId = 1});

            _product.Add(new Product{ProductId=5, Name= "Rezonans Kanunu", Price = 85000, IsActive=true,Image="5.webp",CategoryId = 1});
            _product.Add(new Product{ProductId=6, Name= "Mac Book Air", Price = 95000, IsActive=false,Image="6.webp",CategoryId = 1});
            _product.Add(new Product{ProductId=7, Name= "Ekonomist Dergisi", Price = 60, IsActive=true,Image="d1.webp",CategoryId = 2});
            _product.Add(new Product{ProductId=8, Name= "Atlas Dergisi", Price = 105, IsActive=true,Image="d2.webp",CategoryId = 2});
        }

        public static void CreateProduct(Product entity){
            _product.Add(entity);
        }
        public static void EditProduct(Product updatedProduct){
            var entity = _product.FirstOrDefault(p=>p.ProductId == updatedProduct.ProductId);

            if(entity != null){
                if(!string.IsNullOrEmpty(updatedProduct.Name)){
                entity.Name = updatedProduct.Name;
                }
                entity.Price = updatedProduct.Price;
                entity.Image = updatedProduct.Image;
                entity.CategoryId = updatedProduct.CategoryId;
                entity.IsActive = updatedProduct.IsActive;
            }
        }
        public static void EditIsActive(Product updatedProduct){
            var entity = _product.FirstOrDefault(p=>p.ProductId == updatedProduct.ProductId);

            if(entity != null){
                entity.IsActive = updatedProduct.IsActive;
            }
        }

        public static void DeleteProduct(Product entities){
            var entity = _product.FirstOrDefault(p=>p.ProductId == entities.ProductId);

            if(entity != null){
                _product.Remove(entity);
            }
        }

        public static List<Product> Products{get{return _product;}}
        public static List<Category> Categories{get{return _category;}}
    }
}