using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MadarfigyeloApp.Models
{
    public class Latogatas
    {
        public int Id { get; set; }

        public int OduId { get; set; }

        public Odu? Odu { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Dátum")]
        public DateTime Datum {  get; set; }
        
        [EnumDataType(typeof(Tevekenyseg))]
        [DisplayName("Tevékenység")]
        public Tevekenyseg Tevekenyseg { get; set; }
        
        [EnumDataType(typeof(Allapot))]
        [DisplayName("Állapot")]
        public Allapot Allapot { get; set; }
        
        [DisplayName("Faj vagy genus")]
        public string Faj {  get; set; }
        
        [DisplayName("Tojás szám")]
        public int TojasSzam { get; set; }
        
        [DisplayName("Fióka szám")]
        public int FiokaSzam { get; set; }
        
        [DisplayName("Fiókák kora")]
        public string? FiokakKora {  get; set; }
        
        [DisplayName("Megjegyzések")]
        public string? Megjegyzesek { get; set; }
    }
}
