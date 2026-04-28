using Microsoft.Playwright;

namespace QaPlaywright.Pages;

using QaPlaywright.Core;

public class ProductsPage : BasePage
{
    public ProductsPage(IPage page) : base(page) { }

    public async Task AdicionarProduto(int productId)
    {
        // O .First garante que se houver dois botões (desktop e mobile), ele clica no primeiro disponível
        await _page.Locator($".add-to-cart[data-product-id='{productId}']").First.ClickAsync();

        // Espera o modal de sucesso aparecer e clica em continuar
        var btnContinue = _page.Locator("text=Continue Shopping");
        await btnContinue.WaitForAsync();
        await btnContinue.ClickAsync();
    }

    public async Task AcessarProdutos()
    {
        await _page.GotoAsync("https://automationexercise.com/products");
    }

    public async Task IrParaCarrinho()
    {
        // Opção A: Clicar no link do menu superior (mais estável)
        var cartLink = _page.Locator(".shop-menu a[href='/view_cart']");
        await cartLink.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        await cartLink.ClickAsync();

        // OU Opção B: Navegação direta (estratégia de Senior para evitar anúncios)
        // await _page.GotoAsync("https://automationexercise.com/view_cart");
    }
    public async Task EsperarProdutosCarregarem()
    {
        await _page.WaitForURLAsync(url => url.Contains("products"));
        await _page.Locator(".features_items").WaitForAsync();
    }
}
