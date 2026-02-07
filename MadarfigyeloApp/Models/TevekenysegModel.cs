namespace MadarfigyeloApp.Models
{
    public class TevekenysegModel(Tevekenyseg value)
    {
        public Tevekenyseg Value { get; } = value;

        public string Name => Value.GetResxText();
    }
}
