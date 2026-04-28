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
        // 1. Clicar no botão 'Place Order' para ir à tela de pagamento real
        var placeOrderBtn = _page.Locator("a[href='/payment']");
        await placeOrderBtn.ScrollIntoViewIfNeededAsync();
        await placeOrderBtn.ClickAsync();

        // 2. Agora sim, preencher os dados do cartão
        await _page.FillAsync("input[data-qa='name-on-card']", "Mateus QA");
        await _page.FillAsync("input[data-qa='card-number']", "4000000000000000");
        await _page.FillAsync("input[data-qa='cvc']", "311");
        await _page.FillAsync("input[data-qa='expiry-month']", "12");
        await _page.FillAsync("input[data-qa='expiry-year']", "2030");

        // 3. Clicar em Pay and Confirm Order
        await _page.ClickAsync("button[data-qa='pay-button']");
    }

    public async Task ValidarCompra()
    {
        await _page.WaitForSelectorAsync("text=Order Placed!");
    }
}