namespace QaPlaywright.Utils;

/// <summary>
/// Classe de configuração global do projeto.
///
/// RESPONSABILIDADES:
/// - Centralizar URLs do sistema
/// - Evitar valores hardcoded no código
/// - Facilitar troca de ambiente (dev, staging, prod)
///
/// OBS:
/// Essa classe deve conter apenas configurações,
/// nunca lógica de negócio.
/// </summary>
public class Config
{
    /// <summary>
    /// URL base da aplicação
    ///
    /// Pode ser utilizada em todas as páginas:
    /// Ex: /login, /signup, etc.
    /// </summary>
    public static string BaseUrl => "https://automationexercise.com";
}