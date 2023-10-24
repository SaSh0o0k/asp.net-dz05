using Microsoft.EntityFrameworkCore;
using WebApplication99.Data.Entities;
using WebApplication99.Interfaces;

namespace WebApplication99.Data
{
    public static class SeederDB
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var service = scope.ServiceProvider;
                //Отримую посилання на наш контекст
                var context = service.GetRequiredService<DataEFContext>();
                var imageWorker = service.GetRequiredService<IImageWorker>();
                context.Database.Migrate();
                //Якщо категорій в БД немає, то ми їх створимо по default
                if (!context.Categories.Any())
                {
                    var c1 = new CategoryEntity
                    {
                        Name = "Одяг",
                        Description = "Для усіх людей на планеті",
                        Image = imageWorker.ImageSave("https://kasta.ua/imgw/loc/0x0/s3/9/75/29/10986537/32202394/32202394_original.jpeg")
                    };

                    var c2 = new CategoryEntity
                    {
                        Name = "Взуття",
                        Description = "Для дівчат",
                        Image = imageWorker.ImageSave("https://kasta.ua/image/345/s3/supplier_provided_link/feed/9b4/cde/5be/40f/a3f/c20/2c5/ab4/e46.jpeg")
                    };
                    context.Categories.Add(c1);
                    context.Categories.Add(c2);
                    context.SaveChanges();
                }
                //if (!context.Products.Any())
                //{
                //    var category1 = context.Categories.FirstOrDefault(c => c.Name == "Одяг");
                //    var category2 = context.Categories.FirstOrDefault(c => c.Name == "Взуття");

                //    var product1 = new ProductEntity
                //    {
                //        Name = "Пальто",
                //        Description = "Зручне пальто для холодних днів",
                //        CategoryId = category1.Id
                //    };
                //    product1.Photos.Add(new ProductPhotoEntity { FileName = imageWorker.ImageSave("https://laluna.com.ua/image/cache/catalog/easyphoto/1701202309_jdkjVbgctXk-720x1080.jpg"), DisplayOrder = 1 });
                //    context.Products.Add(product1);

                //    var product2 = new ProductEntity
                //    {
                //        Name = "Черевики",
                //        Description = "Стильні черевики для жінок",
                //        CategoryId = category2.Id
                //    };
                //    product2.Photos.Add(new ProductPhotoEntity { FileName = imageWorker.ImageSave("https://static.insalescdn.com/r/T4IgNzxW0YA/rs:fit:640:640:1/plain/images/products/1/4682/364245578/DSC_9891.jpg"), DisplayOrder = 1 });
                //    context.Products.Add(product2);

                //    context.SaveChanges();
                //}
            }
        }
    }
}
