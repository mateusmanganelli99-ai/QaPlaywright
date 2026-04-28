using NUnit.Framework;
using QaPlaywright.Core;
using QaPlaywright.Pages;
using QaPlaywright.Utils;

namespace QaPlaywright.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class CompraTests : BaseTest
{
    [Test]
    [Retry(2)] // 🔥 anti-flaky
    [Category("Smoke")]
    public async Task RealizarCompra()
    {
        var login = new LoginPage(Page);
        var signup = new SignupPage(Page);
        var products = new ProductsPage(Page);
        var cart = new CartPage(Page);
        var checkout = new CheckoutPage(Page);

        var email = FakerFactory.GerarEmail();
        var senha = "123456";

        TestContext.WriteLine("=== INÍCIO DO TESTE ===");

        // 1. CRIAR USUÁRIO
        await login.Acessar();
        await login.CriarUsuario("Mateus QA", email);
        await signup.PreencherFormulario(senha);

        // 2. VALIDAR LOGIN
        await Page.Locator("text=Logged in as").WaitForAsync();

        // 3. PRODUTOS
        await products.AcessarProdutos();
        await products.AdicionarProduto(1);
        await products.AdicionarProduto(2);

        // 4. CARRINHO
        await products.IrParaCarrinho();
        await cart.ValidarQuantidadeNoCarrinho(2);
        await cart.IrParaCheckout();

        // 5. PAGAMENTO
        await checkout.PreencherPagamento();
        await checkout.ValidarCompra();

        TestContext.WriteLine("=== TESTE FINALIZADO COM SUCESSO ===");
    }
}