using AbbyWeb.Data;
using AbbyWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {
        
        private readonly ApplicationDbContext _db;
        public Category Category { get; set; }

        public EditModel(ApplicationDbContext db)
        {
            _db = db;

        }
        public void OnGet(int id)
        {
            Category = _db.Category.Find(id);
            
        }

        public async Task<IActionResult> OnPost()
        {
            if(Category.Name == Category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Category.Name", "The DsiplayOrder cannot exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                _db.Category.Update(Category);
                await _db.SaveChangesAsync();
                return RedirectToPage("Index");

            }
            return Page();
            
        }

      

    }
}
