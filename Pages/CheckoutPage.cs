using Microsoft.Playwright;

namespace QaPlaywright.Pages;

public class CheckoutPage
{
    private readonly IPage _page;

    public CheckoutPage(IPage page)
    {
        _page = page;
    }

    public async Task PreencherPagamento()
    {
        // 🔥 garante que chegou na tela
        await _page.WaitForURLAsync("**/payment", new() { Timeout = 15000 });

        // 🔥 espera o campo certo
        await _page.Locator("input[data-qa='name-on-card']")
            .WaitForAsync(new() { Timeout = 15000 });

        await _page.FillAsync("input[data-qa='name-on-card']", "Mateus QA");
        await _page.FillAsync("input[data-qa='card-number']", "4111111111111111");
        await _page.FillAsync("input[data-qa='cvc']", "123");
        await _page.FillAsync("input[data-qa='expiry-month']", "12");
        await _page.FillAsync("input[data-qa='expiry-year']", "2030");

        await _page.ClickAsync("button[data-qa='pay-button']");
    }
    public async Task ValidarCompra()
    {
        await _page.WaitForSelectorAsync("text=Order Placed!");
    }
}