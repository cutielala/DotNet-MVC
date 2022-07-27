
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BulkyBook.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;   
        public ProductController(IUnitOfWork unitOfWork , IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;   
        }

        public IActionResult Index()
        {
            //IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
            return View();
        }
        //Get
        public IActionResult Create()
        {
            return View();
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name","The DisplayOrder cannot exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfuly!";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
            
        }

        //Get
        public IActionResult Upsert(int? id)
        {
            ProductVM productvM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(
                    i => new SelectListItem{
                    Text = i.Name,
                    Value = i.id.ToString()
                }),
                CovertTypeList = _unitOfWork.CoverType.GetAll().Select(
                    i => new SelectListItem{
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
         //   IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(
         //       u => new SelectListItem
         //       { 
         //           Text = u.Name,
         //           Value = u.id.ToString()
         //       }
         //   );
         //   IEnumerable<SelectListItem> CovertTypeList = _unitOfWork.CoverType.GetAll().Select(
         //    u => new SelectListItem
         //    {
         //        Text = u.Name,
         //        Value = u.Id.ToString()
         //    }
         //);

            if (id == null || id == 0)
            {
                //create product
                return View(productvM);
            }
            else
            {
                productvM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id== id);

                //update product
            }
        

            return View(productvM);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString();    
                    var uploads = Path.Combine(wwwRootPath, @"images\products"); 
                    var extension = Path.GetExtension(file.FileName);

                    if(obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);

                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }
                if(obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }
               
                _unitOfWork.Save();
                TempData["success"] = "Product created successfuly!";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }
        //Get
        public IActionResult Deletexx(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //var categoryFoundDb = _db.Categories.Find(id);
            var categoryFoundFirst= _unitOfWork.Category.GetFirstOrDefault(u => u.id == id);
            if (categoryFoundFirst == null)
            {
                return NotFound();
            }

            return View(categoryFoundFirst);
        }


        #region API calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType"); 
            return Json(new {data = productList });   

        }

        //Post
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            // var obj= _db.Categories.Find(id);
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting"});
            }

           
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Product deleted successfuly!" });
    

        }

        #endregion 
    }
}
