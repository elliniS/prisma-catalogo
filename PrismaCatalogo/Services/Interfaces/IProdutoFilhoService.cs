﻿using PrismaCatalogo.Web.Models;
using PrismaCatalogo.Web.Services.Interfaces;

namespace PrismaCatalogo.Web.Services.Interfaces
{
    public interface IProdutoFilhoService : IService<ProdutoFilhoViewModel>
    {
        Task<IEnumerable<ProdutoFilhoViewModel>> FindByPruduto(int id);
    }
}