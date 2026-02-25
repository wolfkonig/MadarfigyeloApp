using MadarfigyeloApp.Resources;
using System.Text.Json.Serialization;

namespace MadarfigyeloApp.Models
{
    public class Odu
    {
        public int Id { get; set; }

        public string? OduAzonosito { get; set; }

        public int OdutelepId { get; set; }
        public Odutelep? Odutelep { get; set; }

        public string? OduTipus {  get; set; }
        
        public int BejaratiNyilasMm {  get; set; }

        public Decimal GpsLatitude { get; set; }
        
        public Decimal GpsLongitude { get; set; }

        public string? Elohelykod {  get; set; }
        
        public string? MireVanHelyezve { get; set; }
        
        public string? FelhelyezesModja { get; set; }
        
        public string? OduTajolasa { get; set; }

        public string? OdutTartoNovenyfaj { get; set; }

        public string? MagassagMeter {  get; set; }
        [JsonIgnore]
        public ICollection<Latogatas>? Latogatasok { get; set; }
        [JsonIgnore]
        public static Odu Empty => new() { Id = 0, OduAzonosito = AppRes.NoneSelected };
    }
}
