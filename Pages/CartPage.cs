using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Threading.Tasks;

namespace QaPlaywright.Pages
{
    public class CartPage
    {
        private readonly IPage _page;
        // O seletor "tr" dentro do "tbody" do carrinho pega cada linha de produto
        private readonly string _cartRows = "table#cart_info_table tbody tr";

        public CartPage(IPage page)
        {
            _page = page;
        }
        public async Task ValidarProduto()
        {
            await _page.WaitForSelectorAsync(".cart_description");
            // validação simples (ajuste conforme seu HTML real)
        }

        public async Task ValidarQuantidadeNoCarrinho(int quantidadeEsperada)
        {
            var itens = _page.Locator(_cartRows);
            // Isso garante que o teste falhe se não houver exatamente a quantidade pedida
            await Microsoft.Playwright.Assertions.Expect(itens).ToHaveCountAsync(quantidadeEsperada);
        }

        public async Task IrParaCheckout()
        {
            await _page.ClickAsync(".check_out");
        }
    }
}