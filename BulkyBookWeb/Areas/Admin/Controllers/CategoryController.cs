using BulkyBook.DataAccess1;
using BulkyBook.DataAccess1.Repository.IRepository;
using BulkyBook.Models1;
using Microsoft.AspNetCore.Mvc;
//gang gang bro
namespace BulkyBookWeb.Controllers
{
	[Area("Admin")]
    public class CategoryController : Controller
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
		public CategoryController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IActionResult Index()
        {
			//recupero dati dal db
			//IEnumerable<Category> objCategoryList = _db.GetAll();
			IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
			return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        //POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
			if (obj.Name == obj.DisplayOrder.ToString())
			{
				ModelState.AddModelError(nameof(obj.Name), $"The name of property {nameof(obj.DisplayOrder)} cannot exactly match the name of property {nameof(obj.Name)}");
			}
			if (obj.Name == obj.DisplayOrder.ToString())
			{
				ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
			}
			if (ModelState.IsValid)
            {
				//_db.Add(obj);
				//_db.Save();
				_unitOfWork.Category.Add(obj);
				_unitOfWork.Save();
				TempData["success"] = "Category created successfully";
				return RedirectToAction("Index");
            }
            return View(obj);
        }
        //GET Edit
		public IActionResult Edit(int id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			//var categoryFromDb = _db.Categories.Find(id);
			//if (categoryFromDb == null)
			//{
			//	return NotFound();
			//}
			//return View(categoryFromDb);
			//var categoryFromDbFirst = _db.GetFirstOrDefault(u => u.Id == id);
			var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
			if (categoryFromDbFirst == null)
			{
				return NotFound();
			}
			return View(categoryFromDbFirst);

		}
	
		//POST Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Category obj)
		{
			if (obj.Name == obj.DisplayOrder.ToString())
			{
				ModelState.AddModelError(nameof(obj.Name), $"The name of property {nameof(obj.DisplayOrder)} cannot exactly match the name of property {nameof(obj.Name)}");
			}
			if (obj.Name == obj.DisplayOrder.ToString())
			{
				ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
			}
			if (ModelState.IsValid)
			{
				//_db.Update(obj);
				//_db.Save();
				_unitOfWork.Category.Update(obj);
				_unitOfWork.Save();
				TempData["success"] = "Category update successfully";
				return RedirectToAction("Index");
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
			var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
			if (categoryFromDbFirst == null)
			{
				return NotFound();
			}
			return View(categoryFromDbFirst);
		}
        //POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id, [Bind("Id")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }
			//var obj = _db.Categories.Find(id);
			//if (obj == null)
			//{
			//	  return NotFound();
			//}
			// _db.Categories.Remove(obj);
			var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
			if (categoryFromDbFirst == null)
			{
				return NotFound();
			}
			_unitOfWork.Category.Remove(categoryFromDbFirst);

            _unitOfWork.Save();
			TempData["success"] = "Category deleted successfully";
			return RedirectToAction(nameof(Index));
        }
    }
}
