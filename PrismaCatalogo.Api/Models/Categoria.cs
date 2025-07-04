﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Api.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        //[Display(Name = "Id cagoria pai")]
        public int? IdPai { get; set; }
        public string Nome { get; set; }

        //[Display(Name = "Categoria pai")]
        public Categoria? CategoriaPai { get; set; }

        public bool FgTelaInicial { get; set; }

        //[Display(Name = "Categorias filhas")]
        public ICollection<Categoria>? CategoriasFilhas { get; set; }

        public ICollection<Produto>? Produtos { get; set; }
    }
}
