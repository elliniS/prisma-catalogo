using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Api.Models
{
    public class Tamanho
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public ICollection<ProdutoFilho>? ProdutosFilhos { get; set; }
    }
}
