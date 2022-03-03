namespace WebApiAutores.Entidades
{
    public class Libro_Copia
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AutorId { get; set; }
        public Autor Autor { get; set; }
    }
}
