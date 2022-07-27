
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Controllers
{
    [Area("Admin")]
    [Authorize(Roles= SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);
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
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

           // var categoryFoundDb = _db.Categories.Find(id);
            var categoryFoundDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.id == id);
            if (categoryFoundDbFirst == null)
            {
                return NotFound();
            }

            return View(categoryFoundDbFirst);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfuly!";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }
        //Get
        public IActionResult Delete(int? id)
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
        //Post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            // var obj= _db.Categories.Find(id);
            var obj = _unitOfWork.Category.GetFirstOrDefault( u => u.id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfuly!";
            return RedirectToAction("index");

        }
    }
}
