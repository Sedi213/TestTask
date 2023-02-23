using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TestTask.Models;

namespace TestTask.Controllers
{
    public class HomeController : Controller
    {

        private readonly IFolderDbContext _dbcontext;
        public HomeController(IFolderDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        [HttpGet("/{id?}")]
        public IActionResult Index(Guid id)
        {
            var Vm = new ModelVm();
            Folder selectedFolder;
            if (id == Guid.Empty || id == null)
            {
                 selectedFolder = _dbcontext.folders.FirstOrDefault(o => o.parentId == null);
                if (selectedFolder == null)
                {
                    throw new Exception("DB Empty");
                }
            }
            else
            {
                selectedFolder= _dbcontext.folders.FirstOrDefault(o => o.id == id);

            }
            Vm.Name = selectedFolder.name;
            Vm.Folders = _dbcontext.folders.Where(o => o.parentId == selectedFolder.id).ToList();
            return View(Vm);
        }
        public async Task<IActionResult> CreateFile()
        {
            TableMods.CreateDBFile(_dbcontext.folders.ToList());

            return Redirect("/");
        }
        public async Task<IActionResult> ReadFile()
        {
            TableMods.ReadDBFile(_dbcontext);

            return Redirect("/");
        }

        public async Task<IActionResult> DefaultTable()
        {
            _dbcontext.folders.RemoveRange(_dbcontext.folders);
            _dbcontext.folders.AddRange(TableMods.ReturnDefaultTable());
            await _dbcontext.SaveChangesAsync();

            return Redirect("/");
        }
        
        public async Task<IActionResult> AddSystemDirInDb()
        {
           
            _dbcontext.folders.RemoveRange(_dbcontext.folders);
            _dbcontext.folders.AddRange(TableMods.ReturnListAllSubdirSystem());
            await _dbcontext.SaveChangesAsync();
            return Redirect("/");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}