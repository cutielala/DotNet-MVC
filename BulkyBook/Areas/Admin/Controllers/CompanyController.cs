
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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            Company company = new();
           
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
                return View(company);
            }
            else
            {
                company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id== id);

                return View(company);
            }
        


        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {
      
                if(obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj);
                    TempData["success"] = "Company created successfuly!";

                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                    TempData["success"] = "Company updated successfuly!";

                }

                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }


        #region API calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Company.GetAll(); 
            return Json(new {data = companyList });   

        }

        //Post
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            // var obj= _db.Categories.Find(id);
            var obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting"});
            }

            
            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Company deleted successfuly!" });
    

        }

        #endregion 
    }
}
