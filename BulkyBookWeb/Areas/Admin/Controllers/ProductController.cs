using BulkyBook.DataAccess1.Repository.IRepository;
using BulkyBook.Models1;
using Microsoft.AspNetCore.Mvc;
//gang gang bro
namespace BulkyBookWeb.Controllers
{
	[Area("Admin")]
    public class ProductController : Controller
    {
		//private readonly ApplicationDbContext _db;

		//public CategoryController(ApplicationDbContext db)
		//{
		//    _db = db;
		//}
		//private readonly ICategoryRepository _db;
		//public CategoryController(ICategoryRepository db)
		//{
		//	_db = db;
		//}
		private readonly IUnitOfWork _unitOfWork;
		public ProductController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IActionResult Index()
        {
			//recupero dati dal db
			//IEnumerable<Category> objCategoryList = _db.GetAll();
			IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
			return View(objProductList);
        }
        //GET Edit
		public IActionResult Upsert(int id)
		{
            Product product = new();

            if (id == null || id == 0)
            {

                //create product

                return View(product);

            }

            else

            {

                //update product

            }

            return View(product);

        }
	
		//POST Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(Product obj)
		{
            if (ModelState.IsValid)

            {

                _unitOfWork.Product.Update(obj);

                _unitOfWork.Save();

                TempData["success"] = "Product updated successfully";

                return RedirectToAction(nameof(Index));

            }

            return View(obj);
        }
        //GET Delete

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
			//var categoryFromDb = _db.Categories.Find(id);
			//if (categoryFromDb == null)
			//{
			//    return NotFound();
			//}
			//return View(categoryFromDb);
			var productFromDbFirst = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
			if (productFromDbFirst == null)
			{
				return NotFound();
			}
			return View(productFromDbFirst);
		}
        //POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id, [Bind("Id")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
			//var obj = _db.Categories.Find(id);
			//if (obj == null)
			//{
			//	  return NotFound();
			//}
			// _db.Categories.Remove(obj);
			var productFromDbFirst = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
			if (productFromDbFirst == null)
			{
				return NotFound();
			}
			_unitOfWork.Product.Remove(productFromDbFirst);

            _unitOfWork.Save();
			TempData["success"] = "Category deleted successfully";
			return RedirectToAction(nameof(Index));
        }
    }
}
