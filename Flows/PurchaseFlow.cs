using Microsoft.Playwright;
using QaPlaywright.Pages;
using QaPlaywright.Utils;

namespace QaPlaywright.Flows;

public class PurchaseFlow
{
    private readonly IPage _page;

    public PurchaseFlow(IPage page)
    {
        _page = page;
    }

    public async Task ExecutarCompraCompleta()
    {
        var login = new LoginPage(_page);
        var signup = new SignupPage(_page);
        var products = new ProductsPage(_page);
        var cart = new CartPage(_page);
        var checkout = new CheckoutPage(_page);

        var email = FakerFactory.GerarEmail();
        var senha = "123456";

        await login.Acessar();
        await login.CriarUsuario("Mateus QA", email);
        await signup.PreencherFormulario(senha);

        await login.Login(email, senha);

        await products.AdicionarProduto(1);
        await products.AdicionarProduto(2);

        await products.IrParaCarrinho();

        await cart.ValidarProduto();
        await cart.IrParaCheckout();

        await checkout.PreencherPagamento();
        await checkout.ValidarCompra();
    }
}