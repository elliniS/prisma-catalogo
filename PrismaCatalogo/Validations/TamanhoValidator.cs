﻿using FluentValidation;
using Newtonsoft.Json.Linq;
using PrismaCatalogo.Context;
using PrismaCatalogo.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace PrismaCatalogo.Validations
{
    public class TamanhoValidator : AbstractValidator<Tamanho>
    {
        public TamanhoValidator(IEnumerable<Tamanho> tamanhos)
        {
            RuleFor(x => x.Nome)
                .NotNull()
                .WithMessage("Informe algum nome para o tamanho!")
                .IsUnique(tamanhos)
                .WithMessage("Tamanho já existe!");
        }
    }
}