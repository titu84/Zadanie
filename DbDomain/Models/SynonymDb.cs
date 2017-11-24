using System.Data.Entity;
namespace DbDomain.Models
{
    public class SynonymDb : DbContext
    {      
        public SynonymDb() : base("name=SynonymsDb")
        {
        }
        public SynonymDb(string conn) : base(conn)
        {
        }
        public virtual DbSet<Synonym> Synonyms { get; set; }
    }
}