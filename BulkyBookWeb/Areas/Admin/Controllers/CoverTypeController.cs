using BulkyBook.DataAccess1;
using BulkyBook.DataAccess1.Repository.IRepository;
using BulkyBook.Models1;
using Microsoft.AspNetCore.Mvc;
//gang gang bro
namespace BulkyBookWeb.Controllers
{
	[Area("Admin")]
    public class CoverTypeController : Controller
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
		public CoverTypeController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IActionResult Index()
        {
			//recupero dati dal db
			//IEnumerable<Category> objCategoryList = _db.GetAll();
			IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
			return View(objCoverTypeList);
        }
        public IActionResult Create()
        {
            return View();
        }
        //POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
			if (ModelState.IsValid)
            {
				//_db.Add(obj);
				//_db.Save();
				_unitOfWork.CoverType.Add(obj);
				_unitOfWork.Save();
				TempData["success"] = "CoverType created successfully";
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
			var coverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
			if (coverTypeFromDbFirst == null)
			{
				return NotFound();
			}
			return View(coverTypeFromDbFirst);

		}
	
		//POST Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(CoverType obj)
		{
			if (ModelState.IsValid)
			{
				//_db.Update(obj);
				//_db.Save();
				_unitOfWork.CoverType.Update(obj);
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
			var coverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
			if (coverTypeFromDbFirst == null)
			{
				return NotFound();
			}
			return View(coverTypeFromDbFirst);
		}
        //POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id, [Bind("Id")] CoverType coverType)
        {
            if (id != coverType.Id)
            {
                return NotFound();
            }
			//var obj = _db.Categories.Find(id);
			//if (obj == null)
			//{
			//	  return NotFound();
			//}
			// _db.Categories.Remove(obj);
			var coverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
			if (coverTypeFromDbFirst == null)
			{
				return NotFound();
			}
			_unitOfWork.CoverType.Remove(coverTypeFromDbFirst);

            _unitOfWork.Save();
			TempData["success"] = "Category deleted successfully";
			return RedirectToAction(nameof(Index));
        }
    }
}
