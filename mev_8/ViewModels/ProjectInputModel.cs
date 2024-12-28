using mev_8.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace mev_8.ViewModels
{
    public class ProjectInputModel
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
        [Required]
        public IFormFile Picture { get; set; } = default!;
        public bool IsCompleted { get; set; }
        public List<Worker> Workers { get; set; } = new List<Worker>();
    }
}
