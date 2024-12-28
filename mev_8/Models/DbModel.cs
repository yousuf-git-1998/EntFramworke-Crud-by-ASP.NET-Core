using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace mev_8.Models
{
    public enum ProjectType {Homemaking=1,ElectricalWaring,WallPainting  }
    public class Project
    {
        public int ProjectId { get; set; }
        [Required, StringLength(40)]
        public string ProjectName { get; set; } = default!;
        [Required, EnumDataType(typeof(ProjectType))]
        public ProjectType ProjectType { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }
        [Required, Column(TypeName = "money")]
        public decimal? ProjectCost { get; set; }
        [Required, StringLength(40)]
        public string Picture { get; set; } = default!;
        public bool IsCompleted { get; set; }
        public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
    }
    public class Worker
    {
        public int WorkerId { get; set; }
        [Required, StringLength(40)]
        public string WorkerName { get; set; } = default!;
        [Required, StringLength(40)]
        public string PhoneNumber { get; set; } = default!;
        [Required, ForeignKey("Project")]
        public int ProjectId { get; set; }
        public virtual Project? Project { get; set; } = default!;
    }
   
    public class ProjectDbContext(DbContextOptions<ProjectDbContext> options) : DbContext(options)
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Worker> Workers { get; set; }
       
    }
}
