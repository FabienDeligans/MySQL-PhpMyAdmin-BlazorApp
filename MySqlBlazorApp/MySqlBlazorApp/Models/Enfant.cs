using System;

namespace MySqlBlazorApp.Models
{
    public class Enfant : IEntity
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime BirthDate { get; set; }
        public Famille Famille { get; set; }
        public int FamilleId { get; set; }

        public int Age()
        {
            var now = DateTime.Today;
            var age = now.Year - BirthDate.Year;
            if (BirthDate > now.AddYears(-age))
            {
                age--;
            }
            return age;
        }

    }
}
