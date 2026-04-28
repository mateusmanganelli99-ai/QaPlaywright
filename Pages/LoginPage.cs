using QaPlaywright.Core;
using Microsoft.Playwright;

namespace QaPlaywright.Pages;

/// <summary>
/// Page Object da tela de Login / Signup.
/// 
/// RESPONSABILIDADES:
/// - Acessar a página de login
/// - Criar novo usuário (fluxo inicial)
/// - Realizar login com usuário existente
/// 
/// PADRÃO:
/// Page Object Model (POM)
/// 
/// IMPORTANTE:
/// Essa classe NÃO contém validações de teste,
/// apenas ações da interface.
/// </summary>
public class LoginPage : BasePage
{
    /// <summary>
    /// Construtor recebe a instância da página (Playwright)
    /// vinda do BaseTest
    /// </summary>
    public LoginPage(IPage page) : base(page) { }

    /// <summary>
    /// Acessa a URL de login do sistema
    /// </summary>

    public async Task Acessar()
    {
        // Navega sem esperar o carregamento total (que demora por causa dos anúncios)
        await _page.GotoAsync("https://automationexercise.com/login", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.Commit // Assim que o servidor responder, ele segue
        });

        // Espera o campo especificamente, ignorando o status da página
        var loginInput = _page.Locator("input[data-qa='login-email']");
        await loginInput.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = 15000 // Se não aparecer em 15s, algo está errado
        });
    }

    /// <summary>
    /// Preenche os campos de criação de usuário
    /// e envia o formulário inicial (Signup)
    /// 
    /// @param nome Nome do usuário
    /// @param email Email único do usuário
    /// </summary>
    public async Task CriarUsuario(string nome, string email)
    {
        await _page.FillAsync("input[data-qa='signup-name']", nome);
        await _page.FillAsync("input[data-qa='signup-email']", email);

        // Em vez de apenas clicar, esperamos a navegação para a próxima tela
        await Task.WhenAll(
            _page.WaitForURLAsync("**/signup"), // Espera a URL mudar para a do formulário
            _page.ClickAsync("button[data-qa='signup-button']")
        );
    }

    /// <summary>
    /// Realiza login com usuário existente
    /// 
    /// @param email Email do usuário
    /// @param senha Senha do usuário
    /// </summary>
    public async Task Login(string email, string senha)
    {
        await _page.WaitForSelectorAsync("input[data-qa='login-email']");

        await _page.FillAsync("input[data-qa='login-email']", email);
        await _page.FillAsync("input[data-qa='login-password']", senha);

        await _page.ClickAsync("button[data-qa='login-button']");
    }

    public async Task<bool> EstaNaTelaDeLogin()
    {
        return await IsVisible("input[data-qa='login-email']");
    }
    public async Task IrParaLogin()
    {
        await _page.GotoAsync("https://automationexercise.com/login");
        await _page.WaitForURLAsync("**/login");
    }

    public async Task IrParaProdutos()
    {
        await _page.ClickAsync("a[href='/products']");
    }
    public async Task IrParaCarrinho()
    {
        await _page.Locator("a[href='/view_cart']")
            .Filter(new() { HasText = "Cart" })
            .ClickAsync();
    }
}

