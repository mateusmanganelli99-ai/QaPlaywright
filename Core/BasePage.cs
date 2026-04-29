using Microsoft.Playwright;

namespace QaPlaywright.Core;

/// <summary>
/// BasePage centraliza acoes comuns de UI para os Page Objects.
/// </summary>
public class BasePage
{
    protected readonly IPage _page;

    public BasePage(IPage page)
    {
        _page = page;
    }

    protected async Task WaitForVisible(string selector, int timeout = 5000)
    {
        Console.WriteLine($"[WAIT] Aguardando elemento: {selector}");

        await _page.Locator(selector).WaitForAsync(new()
        {
            State = WaitForSelectorState.Visible,
            Timeout = timeout
        });
    }

    protected async Task Click(string selector, int timeout = 5000)
    {
        Console.WriteLine($"[CLICK] {selector}");

        await WaitForVisible(selector, timeout);
        await _page.ClickAsync(selector);
    }

    protected async Task Fill(string selector, string text, int timeout = 5000)
    {
        Console.WriteLine($"[FILL] {selector} = {text}");

        await WaitForVisible(selector, timeout);
        await _page.FillAsync(selector, text);
    }

    protected async Task<bool> IsVisible(string selector)
    {
        return await _page.Locator(selector).IsVisibleAsync();
    }

    protected async Task WaitForURL(string partialUrl)
    {
        Console.WriteLine($"[WAIT URL] Contem: {partialUrl}");

        await _page.WaitForURLAsync(url => url.Contains(partialUrl));
    }

    protected async Task Screenshot(string nome = "screenshot")
    {
        Directory.CreateDirectory("Reports");

        var path = $"Reports/{nome}_{DateTime.Now:yyyyMMdd_HHmmss}.png";

        Console.WriteLine($"[SCREENSHOT] {path}");

        await _page.ScreenshotAsync(new()
        {
            Path = path,
            FullPage = true
        });
    }

    public async Task EsperarElemento(string selector, int timeout = 15000)
    {
        await WaitForVisible(selector, timeout);
    }
}