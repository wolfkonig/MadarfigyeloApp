using System.Text.Json.Serialization;

namespace MadarfigyeloApp.Models
{
    public class Odutelep
    {
        public int Id { get; set; }
        public string? Azonosito { get; set; }
        public string? Telepules { get; set; }

        public string? TeruletNev { get; set; }

        public string? UtmNegyzetKod { get; set; }

        public string? KezeloSzervezetNev { get; set; }

        public string? FelelosSzemelyNev { get; set; }

        public string? FelelosSzemelyCim { get; set; }

        public string? FelelosSzemelyTelefonszam { get; set; }

        public string? FelelosSzemelyEmail { get; set; }

        public string? Megjegyzes { get; set; }
        [JsonIgnore]
        public ICollection<Odu>? Oduk {  get; set; }
    }
}
