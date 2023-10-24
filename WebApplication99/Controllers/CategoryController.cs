using Microsoft.AspNetCore.Mvc;
using WebApplication99.Data;
using WebApplication99.Data.Entities;
using WebApplication99.Models.Category;
using WebApplication99.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApplication99.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataEFContext _dataEFContext;
        private readonly IImageWorker _imageWorker;
        private readonly IConfiguration _configuration;
        public CategoryController(DataEFContext dataEFContext, IImageWorker imageWorker, IConfiguration configuration)
        {
            _dataEFContext = dataEFContext;
            _imageWorker = imageWorker;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            var list = _dataEFContext.Categories
                .Select(x => new CategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Image = x.Image
                })
                .ToList();
            return View(list);
        }
        [HttpPost]
        public IActionResult Search(string text)
        {
            var list = _dataEFContext.Categories
                .Where(c => EF.Functions.Like(c.Name, "%" + text + "%"))
                .Select(x => new CategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Image = x.Image
                }).ToList();
            return View("Index", list);
        }
        //Метод використовуєть для відображення сторінки, де ми заповняємо інфомацію
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        //Додати новий елемент
        public IActionResult Add(CategoryAddViewModel model)
        {
            try
            {
                if (model.Image == null)
                {
                    ModelState.AddModelError("Image", "Оберіть фото!");
                }

                if (!ModelState.IsValid)
                {
                    return View();
                }

                string imageName = _imageWorker.ImageSave(model.Image);

                CategoryEntity entity = new CategoryEntity();
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Image = imageName;
                _dataEFContext.Categories.Add(entity);
                _dataEFContext.SaveChanges();
                //вертає статус код 302 - потрібно перейти до списку категорій
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Щось пішло не так " + ex.Message);
                return View();
            }
        }
        //Видалити елемент
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _dataEFContext.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            
            // Видалення фото з диска
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "images", category.Image);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _dataEFContext.Categories.Remove(category);
            _dataEFContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        //Редагувати елемент
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var editProduct = _dataEFContext.Categories.SingleOrDefault(x => x.Id == id);
            if (editProduct == null)
            {
                return NotFound();
            }
            var model = new CategoryEditViewModel
            {
                Id = id,
                Name = editProduct.Name,
                Description = editProduct.Description,
                ImageView = "/images/300_" + editProduct.Image
            };
            return View(model);
        }

        //Метод використовуєть для відображення сторінки, де ми заповняємо інфомацію
        [HttpPost]
        public IActionResult Edit(CategoryEditViewModel model)
        {
            var category = _dataEFContext.Categories.SingleOrDefault(x => x.Id == model.Id);

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.Image != null)
            {
                //видаляю старі фото
                var imageSizes = _configuration.GetValue<string>("ImageSizes");
                var sizes = imageSizes.Split(",");
                foreach (var size in sizes)
                {
                    int width = int.Parse(size);
                    var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
                    System.IO.File.Delete(Path.Combine(dir, size + "_" + category.Image));
                }
                //зберігаємо нове фото
                string imageName = _imageWorker.ImageSave(model.Image);
                category.Image = imageName;
            }
            category.Name = model.Name;
            category.Description = model.Description;
            _dataEFContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
