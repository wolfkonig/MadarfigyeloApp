namespace Terepnaplo.Models
{
    public class Latogatas
    {
        public int Id { get; set; }

        public int OduId { get; set; }

        public Odu? Odu { get; set; }

        public DateTime Datum {  get; set; }
        
        public Tevekenyseg Tevekenyseg { get; set; }
        
        public Allapot Allapot { get; set; }

        public string? Faj {  get; set; }
        
        public int TojasSzam { get; set; }
        
        public int FiokaSzam { get; set; }
        
        public string? FiokakKora {  get; set; }

        public string? Megjegyzesek { get; set; }
    }
}
