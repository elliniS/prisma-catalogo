namespace PrismaCatalogo.Api.Models
{
    public class ProdutoFoto
    {
        public int Id { get; set; }
        public string? FotoBytes { get; set; }
        public string? Caminho { get; set; }
        public string? Nome { get; set; }

        public int ProdutoId { get; set; }
        public Produto? Produto { get; set; }


    }
}
