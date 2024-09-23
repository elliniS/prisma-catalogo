using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Models
{
    public class Tamanho
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
