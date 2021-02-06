using System.Collections.Generic;

namespace MySqlBlazorApp.Models
{
    public class Famille : IEntity
    {
        public int Id { get; set; }
        public string NomDeFamille { get; set; }
        public IEnumerable<Parent> Parents { get; set; }
        public IEnumerable<Enfant> Enfants { get; set; }
        public int Qf { get; set; }

    }

}
