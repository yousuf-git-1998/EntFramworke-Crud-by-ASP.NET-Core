using mev_8.Models;
using mev_8.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace mev_8.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ProjectDbContext db;
        private readonly IWebHostEnvironment env;
        public ProjectsController(ProjectDbContext db, IWebHostEnvironment env) { this.db = db; this.env = env; }
        public IActionResult Index()
        {
            //Eager loading
            var data = db.Projects.Include(x => x.Workers).ToList();

            return View(data);
        }
        public IActionResult Aggregates()
        {
            return View(db.Projects.Include(x => x.Workers).ToList());
        }
        public IActionResult Grouping()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Grouping(string groupby)
        {
            if (groupby == "type")
            {
                var data = db.Projects.GroupBy(x => x.ProjectType).ToList().Select(g => new GroupedData<Project> { Key = g.Key.ToString(), Items = g }).ToList();
                return View("GroupingResult", data);
            }
            else if (groupby == "year-month")
            {
                var data = db.Projects.ToList().GroupBy(x => new { x.StartDate.Value.Year, x.StartDate.Value.Month }).Select(g => new GroupedData<Project> { Key = $"{g.Key.Month}/{g.Key.Year}", Items = g }).ToList();
                return View("GroupingResult", data);
            }
            return NoContent();
        }
        public IActionResult Create()
        {


            var model = new ProjectInputModel();
            model.Workers.Add(new Worker { });

            return View(model);

        }
        [HttpPost]
        public IActionResult Create(ProjectInputModel model, string operation = "")
        {
            if (operation == "add")
            {
                model.Workers.Add(new Worker { });
                foreach (var item in ModelState.Values)
                {
                    item.Errors.Clear();
                    item.RawValue = null;
                }
            }
            if (operation.StartsWith("remove"))
            {
                //int i = operation.IndexOf("_");
                //int index = int.Parse(operation.Substring(i+1));
                int index = int.Parse(operation.Substring(operation.IndexOf("_") + 1));
                model.Workers.RemoveAt(index);
                foreach (var item in ModelState.Values)
                {
                    item.Errors.Clear();
                    item.RawValue = null;
                }
            }
            if (operation == "insert")
            {
                if (ModelState.IsValid)
                {
                    var w = new Project
                    {
                        ProjectName = model.ProjectName,
                        ProjectType = model.ProjectType,
                        StartDate = model.StartDate,
                        ProjectCost = model.ProjectCost,
                        IsCompleted = model.IsCompleted,

                    };
                    string ext = Path.GetExtension(model.Picture.FileName);
                    string f = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string filePath = Path.Combine(env.WebRootPath, "Pictures", f);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    model.Picture.CopyTo(fs);
                    fs.Close();
                    w.Picture = f;

                    db.Projects.Add(w);
                    db.SaveChanges();
                    foreach (var wl in model.Workers)
                    {
                        //Debug.WriteLine($"EXECUTE InsertSpec ${wl.SpecName}, ${wl.Value}, ${w.ProjectId}");
                        db.Database.ExecuteSqlInterpolated($"EXECUTE InsertWorker {wl.WorkerName}, {wl.PhoneNumber}, {w.ProjectId}");
                    }
                    return RedirectToAction("Index");
                }

               
            }
            return View(model);
        }

    }
}
