﻿namespace PrismaCatalogo.Api.Models
{
    public class ProdutoFilhoFoto
    {
        public int Id { get; set; }
        public string? FotoBytes { get; set; }
        public string? Caminho { get; set; }
        public string? Nome { get; set; }

        public int ProdutoFilhoId { get; set; }
        public ProdutoFilho? ProdutoFilho { get; set; }
    }
}
