using BulkyBook.DataAccess1.Repository.IRepository;
using BulkyBook.Models1;
using BulkyBook.Models1.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;

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
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
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
            ProductVM productVM = new()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };
            if (id == null || id == 0)
            {
                //restituisce una view per la creazione di un nuovo prodotto
                return View(productVM);
            }
            else
            {
                var productInDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
                if (productInDb != null)
                {
                    productVM.Product = productInDb;
                    //restituisce una view per l'aggiornamento del prodotto
                    //questa view riceve un productVM con tutti i campi di Product
                    return View(productVM);
                }
                //il prodotto con l'id inviato non è stato trovato nel database.
                //restituisce una view per creare un nuovo prodotto
                return View(productVM);
            }
        }

		//POST Upsert
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(ProductVM obj, IFormFile? file)

        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if(file!=null){
                    //creiamo un nuovo nome per il file che l'utente ha caricato
                    //facciamo in modo che non possano esistere due file con lo stesso nome
                    string fileName = Guid.NewGuid().ToString();
                    var uploadDir =Path.Combine(wwwRootPath,"images","products");
                    var fileExtension = Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadDir, fileName + fileExtension);
                    var fileUrlString = filePath[wwwRootPath.Length..].Replace(@"\\", @"\");
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Product.ImageUrl = fileUrlString;
                }
                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
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
        #region API CALLS

        [HttpGet]

        public IActionResult GetAll()

        {

            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");

            return Json(new { data = productList });

        }
        #endregion
    }
}
