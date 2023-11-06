using System.ComponentModel.DataAnnotations;

namespace MyFirstWebApi.Models
{
    public class TypeModel
    {
        [Required]
        public string NameType { get; set; }
    }
}
