using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Web.Models
{
    public class ProdutoDetalheViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }   
        public string? Observacao { get; set; }  
        public bool Ativo { get; set; }
        //public FotoViewModel? FotoCapa { get; set; }

        public string Preço { get; set; }
        public string Estoque { get; set; } 

        public int? CorId { get; set; }
        public int? TamanhoId { get; set; }

        //public List<CorViewModel> Cores { get; set }
        public List<TamanhoViewModel> Tamanhos { get; set; }
        //[Display(Name = "Produtos filhos")]
        //public IEnumerable<ProdutoFilhoViewModel>? ProdutosFilhos { get; set; }

        public List<FotoViewModel>? Fotos { get; set; }


        public ProdutoDetalheViewModel(ProdutoViewModel produtoView)
        {
            Id = produtoView.Id;
            Nome = produtoView.Nome;
            Descricao = produtoView.Descricao;
            Observacao = produtoView.Observacao;
            Ativo = produtoView.Ativo;
            Fotos = produtoView.Fotos;
            Preço = produtoView.ProdutosFilhos != null && produtoView.ProdutosFilhos.Where(p => p.Preco != null).Select(p => p.Preco).Order().FirstOrDefault() != null ? "R$" + produtoView.ProdutosFilhos.Where(p => p.Preco != null).Select(p => p.Preco).Order().FirstOrDefault() : "Não informado";
            Estoque = produtoView.ProdutosFilhos != null ? produtoView.ProdutosFilhos.Where(p => p.QuantEstoque != null).Sum(p => p.QuantEstoque).ToString() : "Não informado";

            Tamanhos = produtoView.ProdutosFilhos != null ? produtoView.ProdutosFilhos.Select(p => p.Tamanho).Distinct().ToList() : new List<TamanhoViewModel>();

            if (Fotos != null && Fotos.Count() > 0)
                Fotos.OrderBy(f => f.Id).FirstOrDefault().FgPrincipal = true;
        }

        public ProdutoDetalheViewModel(ProdutoViewModel produtoView, int? corId)
        {
            var produtosFilhos = produtoView.ProdutosFilhos.Where(p => p.Cor.Id == corId).ToList();

            Id = produtoView.Id;
            Nome = produtoView.Nome;
            Descricao = produtoView.Descricao;
            Observacao = produtoView.Observacao;
            Ativo = produtoView.Ativo;
            Fotos = produtoView.Fotos;
            CorId = corId;
            Preço = produtosFilhos != null && produtosFilhos.Where(p => p.Preco != null).Select(p => p.Preco).Order().FirstOrDefault() != null ? "R$" + produtosFilhos.Where(p => p.Preco != null).Select(p => p.Preco).Order().FirstOrDefault() : "Não informado";
            Estoque = produtosFilhos != null ? produtosFilhos.Where(p => p.QuantEstoque != null).Sum(p => p.QuantEstoque).ToString() : "Não informado";
            Tamanhos = produtosFilhos != null ? produtosFilhos.Select(p => p.Tamanho).Distinct().ToList() : new List<TamanhoViewModel>();


            if (Fotos != null && Fotos.Count() > 0)
                Fotos.OrderBy(f => f.Id).FirstOrDefault().FgPrincipal = true;
        }

        public ProdutoDetalheViewModel(ProdutoViewModel produtoView, int? corId, int? tamanhoId)
        {

            var produtoFilho = produtoView.ProdutosFilhos.Where(p => p.CorId == corId && p.TamanhoId == tamanhoId).FirstOrDefault();

            Id = produtoView.Id;
            Nome = produtoView.Nome;
            Descricao = produtoView.Descricao;
            Observacao = produtoView.Observacao;
            Ativo = produtoView.Ativo;
            Fotos = produtoView.Fotos;
            Preço = produtoFilho != null && produtoFilho.Preco != null ? "R$" + produtoFilho.Preco : "Não informado";
            Estoque = produtoView.ProdutosFilhos != null && produtoFilho.QuantEstoque != null ? produtoFilho.QuantEstoque.ToString() : "Não informado";
            CorId = corId;
            TamanhoId = tamanhoId;
            Tamanhos = produtoView.ProdutosFilhos != null ? produtoView.ProdutosFilhos.Where(p => p.CorId == corId).Select(p => p.Tamanho).Distinct().ToList() : new List<TamanhoViewModel>();

            if (Fotos != null && Fotos.Count() > 0)
                Fotos.OrderBy(f => f.Id).FirstOrDefault().FgPrincipal = true;
        }
    }
}
