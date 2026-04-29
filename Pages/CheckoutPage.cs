using Microsoft.Playwright;
using QaPlaywright.Utils;

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
        var nomeCartao = _page.Locator("input[data-qa='name-on-card']");

        if (!await nomeCartao.IsVisibleAsync())
        {
            await IrParaTelaDePagamento();
        }

        await nomeCartao.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 15000 });

        await nomeCartao.FillAsync("Mateus QA");
        await _page.FillAsync("input[data-qa='card-number']", "4111111111111111");
        await _page.FillAsync("input[data-qa='cvc']", "123");
        await _page.FillAsync("input[data-qa='expiry-month']", "12");
        await _page.FillAsync("input[data-qa='expiry-year']", "2030");

        await _page.ClickAsync("button[data-qa='pay-button']");
    }

    private async Task IrParaTelaDePagamento()
    {
        if (!_page.Url.Contains("/checkout"))
        {
            await _page.WaitForURLAsync("**/checkout", new() { Timeout = 15000 });
        }

        var comentario = _page.Locator("textarea[name='message']");
        if (await comentario.IsVisibleAsync())
        {
            await comentario.FillAsync("Pedido criado por teste automatizado.");
        }

        var placeOrder = _page.Locator("a[href='/payment']").First;
        await placeOrder.ScrollIntoViewIfNeededAsync();
        await placeOrder.ClickAsync();

        try
        {
            await _page.Locator("input[data-qa='name-on-card']")
                .WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 7000 });
        }
        catch (TimeoutException)
        {
            await _page.GotoAsync($"{Config.BaseUrl}/payment", new()
            {
                WaitUntil = WaitUntilState.Commit
            });
        }
    }

    public async Task ValidarCompra()
    {
        await _page.WaitForSelectorAsync("text=Order Placed!", new() { Timeout = 15000 });
    }
}
