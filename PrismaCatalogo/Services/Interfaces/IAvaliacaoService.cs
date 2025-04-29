using PrismaCatalogo.Web.Models;

namespace PrismaCatalogo.Web.Services.Interfaces
{
    public interface IAvaliacaoService //: IService<AvaliacaoViewModel>
    {
        Task<IEnumerable<AvaliacaoViewModel>> FindByProduto(int id);
        Task<AvaliacaoViewModel> FindByProdutoUsuario(int produtoId, int usuarioId);
        Task<AvaliacaoViewModel> Create(AvaliacaoViewModel avaliacaoViewModel);
        Task<AvaliacaoViewModel> Update(int id, AvaliacaoViewModel avaliacaoViewModel);
        Task<AvaliacaoViewModel> Delete(int id);
    }
}
