using System.ComponentModel.DataAnnotations;

namespace TestTask.Models
{
    public class Folder
    {
        [Required]
        public Guid id { get; set; }
        [Required]
        public string name { get; set; }
        public Guid? parentId { get; set; }
    }
}
