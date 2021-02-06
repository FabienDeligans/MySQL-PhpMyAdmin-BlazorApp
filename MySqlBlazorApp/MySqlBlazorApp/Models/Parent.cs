using System.ComponentModel.DataAnnotations;

namespace MySqlBlazorApp.Models
{
    public class Parent : IEntity
    {
        
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse { get; set; }
        public string Cp { get; set; }
        public string Ville { get; set; }
        public string Mail { get; set; }

        [Phone]
        public string Tel { get; set; }

        public Famille Famille { get; set; }

        public int FamilleId { get; set; }
    }
}
