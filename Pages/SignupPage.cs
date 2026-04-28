using QaPlaywright.Core;
using Microsoft.Playwright;

namespace QaPlaywright.Pages;

/// <summary>
/// Page Object responsável pelo formulário completo de cadastro.
///
/// RESPONSABILIDADES:
/// - Preencher dados do usuário
/// - Submeter criação de conta
///
/// OBS:
/// Essa classe executa ações de preenchimento.
/// Validações ficam na AccountPage.
/// </summary>
public class SignupPage : BasePage
{
    /// <summary>
    /// Construtor recebe a página do Playwright
    /// </summary>
    public SignupPage(IPage page) : base(page) { }

    /// <summary>
    /// Preenche o formulário completo de criação de conta
    ///
    /// @param senha Senha do usuário
    /// </summary>
    public async Task PreencherFormulario(string senha)
    {
        var passwordInput = _page.Locator("#password");

        // Espera explícita com scroll para garantir que o elemento está interagível
        await passwordInput.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        await passwordInput.ScrollIntoViewIfNeededAsync();

        await passwordInput.FillAsync(senha);
        await _page.FillAsync("#password", senha);
        await _page.FillAsync("#first_name", "Mateus");
        await _page.FillAsync("#last_name", "QA");
        await _page.FillAsync("#address1", "Rua Teste");
        await _page.FillAsync("#state", "SP");
        await _page.FillAsync("#city", "São Paulo");
        await _page.FillAsync("#zipcode", "00000000");
        await _page.FillAsync("#mobile_number", "11999999999");

        // 🔥 ESSENCIAL
        await _page.ClickAsync("button[data-qa='create-account']");

        // ✔ espera confirmação REAL
        await _page.WaitForSelectorAsync("h2[data-qa='account-created']");

        await _page.ClickAsync("a[data-qa='continue-button']");
    }
    public async Task AguardarTelaCadastro()
    {
        await _page.WaitForURLAsync("**/signup");
        await _page.Locator("#password").WaitForAsync();
    }
}