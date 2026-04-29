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
    [TestCaseSource(nameof(CenariosDeCompra))]
    [Retry(2)]
    [Category("E2E")]
    [Category("Compra")]
    public async Task RealizarCompra(string cenario, string nome, string email, string senha, int[] produtos)
    {
        var login = new LoginPage(Page);
        var signup = new SignupPage(Page);
        var products = new ProductsPage(Page);
        var cart = new CartPage(Page);
        var checkout = new CheckoutPage(Page);

        TestContext.WriteLine($"=== INICIO DO CENARIO: {cenario} ===");
        TestContext.WriteLine($"Usuario: {nome} / {email}");

        await login.Acessar();
        await login.CriarUsuario(nome, email);
        await signup.PreencherFormulario(senha);

        await Page.Locator("text=Logged in as").WaitForAsync();

        await products.AcessarProdutos();

        foreach (var produto in produtos)
        {
            await products.AdicionarProduto(produto);
        }

        await products.IrParaCarrinho();
        await cart.ValidarQuantidadeNoCarrinho(produtos.Length);
        await cart.IrParaCheckout();

        await checkout.PreencherPagamento();
        await checkout.ValidarCompra();

        TestContext.WriteLine("=== CENARIO FINALIZADO COM SUCESSO ===");
    }

    public static IEnumerable<TestCaseData> CenariosDeCompra()
    {
        yield return CriarCenario(1, "Compra_Produto_1", new[] { 1 });
        yield return CriarCenario(2, "Compra_Produto_2", new[] { 2 });
        yield return CriarCenario(3, "Compra_Produto_3", new[] { 3 });
        yield return CriarCenario(4, "Compra_Produtos_1_2", new[] { 1, 2 });
        yield return CriarCenario(5, "Compra_Produtos_2_3", new[] { 2, 3 });
        yield return CriarCenario(6, "Compra_Produtos_3_4", new[] { 3, 4 });
        yield return CriarCenario(7, "Compra_Produtos_4_5", new[] { 4, 5 });
        yield return CriarCenario(8, "Compra_Produtos_1_3_5", new[] { 1, 3, 5 });
        yield return CriarCenario(9, "Compra_Produtos_2_4_6", new[] { 2, 4, 6 });
        yield return CriarCenario(10, "Compra_Produtos_1_2_3_4", new[] { 1, 2, 3, 4 });
    }

    private static TestCaseData CriarCenario(int indice, string nomeCenario, int[] produtos)
    {
        return new TestCaseData(
            nomeCenario,
            FakerFactory.Nome(indice),
            FakerFactory.GerarEmail(indice),
            "123456",
            produtos)
            .SetName($"Cenario_{indice:00}_{nomeCenario}");
    }
}
