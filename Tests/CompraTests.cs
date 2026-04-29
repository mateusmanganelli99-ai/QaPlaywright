using NUnit.Framework;
using QaPlaywright.Core;
using QaPlaywright.Pages;
using QaPlaywright.Utils;

namespace QaPlaywright.Tests;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
[Parallelizable(ParallelScope.Children)]
public class CompraTests : BaseTest
{
    [Test]
    [TestCaseSource(nameof(DadosSmoke))]
    [Retry(2)]
    [Category("Smoke")]
    public async Task RealizarCompraSmoke(string nome, string email, string senha)
    {
        await ExecutarCompra(nome, email, senha);
    }

    [Test]
    [TestCaseSource(nameof(DadosRegression))]
    [Retry(2)]
    [Category("Regression")]
    public async Task RealizarCompraRegression(string nome, string email, string senha)
    {
        await ExecutarCompra(nome, email, senha);
    }

    private async Task ExecutarCompra(string nome, string email, string senha)
    {
        var login = new LoginPage(Page);
        var signup = new SignupPage(Page);
        var products = new ProductsPage(Page);
        var cart = new CartPage(Page);
        var checkout = new CheckoutPage(Page);

        TestContext.WriteLine($"=== INICIO DO TESTE: {nome} / {email} ===");

        await login.Acessar();
        await login.CriarUsuario(nome, email);
        await signup.PreencherFormulario(senha);

        await Page.Locator("text=Logged in as").WaitForAsync();

        await products.AcessarProdutos();
        await products.AdicionarProduto(1);
        await products.AdicionarProduto(2);

        await products.IrParaCarrinho();
        await cart.ValidarQuantidadeNoCarrinho(2);
        await cart.IrParaCheckout();

        await checkout.PreencherPagamento();
        await checkout.ValidarCompra();

        TestContext.WriteLine("=== TESTE FINALIZADO COM SUCESSO ===");
    }

    public static IEnumerable<TestCaseData> DadosSmoke()
    {
        return GerarDadosDeCadastro(3, "Smoke");
    }

    public static IEnumerable<TestCaseData> DadosRegression()
    {
        return GerarDadosDeCadastro(12, "Regression");
    }

    private static IEnumerable<TestCaseData> GerarDadosDeCadastro(int quantidade, string categoria)
    {
        for (var i = 1; i <= quantidade; i++)
        {
            yield return new TestCaseData(
                FakerFactory.Nome(i),
                FakerFactory.GerarEmail(i),
                "123456")
                .SetName($"RealizarCompra_{categoria}_{i}");
        }
    }
}
