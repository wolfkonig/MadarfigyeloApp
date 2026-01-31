using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MadarfigyeloApp.Models
{
    public class Odutelep
    {
        public int Id { get; set; }
        [DisplayName("Azonosító")]
        public string Azonosito { get; set; }
        [DisplayName("Település(ek)")]
        public string Telepules { get; set; }
        [DisplayName("Terület(ek) neve")]
        public string TeruletNev { get; set; }
        [DisplayName("UTM négyzet(ek) kódja")]
        public string UtmNegyzetKod { get; set; }
        [DisplayName("Szervezet neve")]
        public string? KezeloSzervezetNev { get; set; }
        [DisplayName("Felelős személy neve")]
        public string? FelelosSzemelyNev { get; set; }
        [DisplayName("Lakcíme")]
        public string? FelelosSzemelyCim { get; set; }
        [DisplayName("Telefonszáma")]
        public string? FelelosSzemelyTelefonszam { get; set; }
        [DisplayName("E-mail címe")]
        public string? FelelosSzemelyEmail { get; set; }
        [DisplayName("Megjegyzés")]
        public string? Megjegyzes { get; set; }
        [JsonIgnore]
        public ICollection<Odu>? Oduk {  get; set; }
    }
}
