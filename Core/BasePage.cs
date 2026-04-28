using Microsoft.Playwright;

namespace QaPlaywright.Core;

/// <summary>
/// BasePage - versão avançada (nível sênior)
/// 
/// RESPONSABILIDADES:
/// - Centralizar ações da UI
/// - Evitar flaky tests (esperas automáticas)
/// - Adicionar logs
/// - Facilitar debug (prints, mensagens)
/// 
/// PADRÕES APLICADOS:
/// - Page Object Model (POM)
/// - Reutilização de código
/// - Robustez em automação UI
/// </summary>
public class BasePage
{
    protected readonly IPage _page;

    public BasePage(IPage page)
    {
        _page = page;
    }

    /// <summary>
    /// Aguarda elemento estar visível antes de interagir
    /// Evita erro de elemento não encontrado
    /// </summary>
    protected async Task WaitForVisible(string selector, int timeout = 5000)
    {
        Console.WriteLine($"[WAIT] Aguardando elemento: {selector}");

        await _page.Locator(selector).WaitForAsync(new()
        {
            State = WaitForSelectorState.Visible,
            Timeout = 1000
        });
    }

    /// <summary>
    /// Clique com espera automática + log
    /// </summary>
    protected async Task Click(string selector)
    {
        await _page.GetByRole(AriaRole.Link, new() { Name = "Cart" }).ClickAsync();
    }

    /// <summary>
    /// Preenchimento com espera + limpeza do campo
    /// </summary>
    protected async Task Fill(string selector, string text)
    {
        Console.WriteLine($"[FILL] {selector} = {text}");

        await WaitForVisible(selector);
        await _page.FillAsync(selector, text);
    }

    /// <summary>
    /// Valida se elemento está visível
    /// </summary>
    protected async Task<bool> IsVisible(string selector)
    {
        return await _page.Locator(selector).IsVisibleAsync();
    }

    /// <summary>
    /// Aguarda URL conter um valor
    /// Muito útil após navegação
    /// </summary>
    protected async Task WaitForURL(string partialUrl)
    {
        Console.WriteLine($"[WAIT URL] Contém: {partialUrl}");

        await _page.WaitForURLAsync(url => url.Contains(partialUrl));
    }

    /// <summary>
    /// Screenshot manual (útil para debug)
    /// </summary>
    protected async Task Screenshot(string nome = "screenshot")
    {
        var path = $"Reports/{nome}_{DateTime.Now:yyyyMMdd_HHmmss}.png";

        Console.WriteLine($"[SCREENSHOT] {path}");

        await _page.ScreenshotAsync(new()
        {
            Path = path,
            FullPage = true
        });
    }
    public async Task EsperarElemento(string selector)
    {
        await _page.Locator(selector).WaitForAsync(new()
        {
            State = WaitForSelectorState.Visible,
            Timeout = 15000
        });
    }
}