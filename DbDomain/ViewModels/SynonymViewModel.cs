using DbDomain.Models;
using System.Collections.Generic;

namespace DbDomain.ViewModels
{
    public class SynonymViewModel
    {
        public Synonym Synonym { get; set; }
        public List<string> SynonymsList { get; set; }
        public string Message { get; set; }
        public bool IsValid()
        {
            return (Synonym != null && SynonymsList.Count > 0);            
        }
    }
}
