using System.ComponentModel.DataAnnotations;

namespace DbDomain.Models
{
    public class Synonym
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:50)]
        public string Term { get; set; }
        [Required]
        public string Synonyms { get; set; }   
    }
}
