using QaPlaywright.API;
using QaPlaywright.Pages;
using Microsoft.Playwright;
using QaPlaywright.Core;

namespace QaPlaywright.Flows;

public class HybridFlow
{
    private readonly IPage _page;
    private readonly IAPIRequestContext _api;

    public HybridFlow(IPage page, IAPIRequestContext api)
    {
        _page = page;
        _api = api;
    }

    public async Task Executar()
    {
        var userService = new UserService(_api);
        var login = new LoginPage(_page);
        var products = new ProductsPage(_page);
        var cart = new CartPage(_page);
        var checkout = new CheckoutPage(_page);

        var email = $"user{DateTime.Now.Ticks}@test.com";
        var senha = "123456";

        // =========================
        // 1. CRIA USUÁRIO VIA API
        // =========================
        var response = await userService.CriarUsuario("Mateus QA", email, senha);

        var body = await response.TextAsync();
        Console.WriteLine($"[API RESPONSE] {body}");

        if (!response.Ok)
            throw new Exception("Erro ao criar usuário via API");

        // =========================
        // 2. LOGIN VIA UI
        // =========================
        await login.Acessar();
        await login.Login(email, senha);

        // valida login
        await _page.Locator("text=Logged in as").WaitForAsync();

        // =========================
        // 3. COMPRA
        // =========================
        await products.AcessarProdutos();

        await products.AdicionarProduto(1);
        await products.AdicionarProduto(2);

        await products.IrParaCarrinho();

        await cart.IrParaCheckout();

        await checkout.PreencherPagamento();
        await checkout.ValidarCompra();
    }
}