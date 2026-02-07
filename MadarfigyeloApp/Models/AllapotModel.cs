namespace MadarfigyeloApp.Models
{
    public class AllapotModel(Allapot value)
    {
        public Allapot Value { get; } = value;

        public string Name => Value.GetResxText();
    }
}
