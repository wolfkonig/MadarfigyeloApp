using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MadarfigyeloApp.Models
{
    public class Odu
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Odú azonosító")]
        public string OduAzonosito { get; set; }

        [ForeignKey("Id")]
        public int OdutelepId { get; set; }
        public Odutelep? Odutelep { get; set; }

        [DisplayName("Odú típus")]
        public string OduTipus {  get; set; }
        
        [DisplayName("Bejárati nyílás átmérője (mm)")]
        public int BejaratiNyilasMm {  get; set; }
        
        [DisplayName("GPS hosszúság")]
        [Precision(9, 6)]
        public Decimal GpsLatitude { get; set; }
        
        [DisplayName("GPS szélesség")]
        [Precision(9, 6)]
        public Decimal GpsLongitude { get; set; }
        
        [DisplayName("Élőhelykód")]
        public string? Elohelykod {  get; set; }
        
        [DisplayName("Mire van helyezve")]
        public string? MireVanHelyezve { get; set; }
        
        [DisplayName("Felhelyezés módja")]
        public string? FelhelyezesModja { get; set; }
        
        [DisplayName("Odú tájolása")]
        public string? OduTajolasa { get; set; }
        [DisplayName("Odút tartó növényfaj")]
        public string? OdutTartoNovenyfaj { get; set; }
        [DisplayName("Magasság (m)")]
        public string? MagassagMeter {  get; set; }
        public ICollection<Latogatas>? Latogatas { get; set; }
     }
}
