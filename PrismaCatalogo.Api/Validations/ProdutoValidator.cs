﻿using FluentValidation;
using PrismaCatalogo.Api.Context;
using PrismaCatalogo.Api.DTOs;
using PrismaCatalogo.Api.Extensions;
using PrismaCatalogo.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace PrismaCatalogo.Api.Validations
{
    public class ProdutoValidator : AbstractValidator<Produto>
    {
        public ProdutoValidator(IEnumerable<Produto> produto)
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Informe algum nome para o produto!")
                .IsUnique(produto)
                .WithMessage("Outro produto já esta usando este nome!");
        }
    }
}
