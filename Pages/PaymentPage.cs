using Microsoft.Playwright;
using QaPlaywright.Core;

namespace QaPlaywright.Pages;

public class PaymentPage : BasePage
{
    public PaymentPage(IPage page) : base(page) { }

    public async Task PagarComCartao()
    {
        await Fill("input[name='name_on_card']", "Mateus QA");
        await Fill("input[name='card_number']", "4111111111111111");
        await Fill("input[name='cvc']", "123");
        await Fill("input[name='expiry_month']", "12");
        await Fill("input[name='expiry_year']", "2030");

        await Click("#submit");

        await WaitForVisible("text=Order Placed!");
    }

    public async Task ValidarCompra()
    {
        await WaitForVisible("text=Congratulations");
    }
}