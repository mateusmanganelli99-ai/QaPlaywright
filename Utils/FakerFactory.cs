namespace QaPlaywright.Utils;

/// <summary>
/// Classe responsável por gerar dados dinâmicos para testes.
///
/// RESPONSABILIDADES:
/// - Evitar dados duplicados
/// - Gerar massa de teste dinâmica
/// - Facilitar execução repetida dos testes
///
/// OBS:
/// Muito usada em testes automatizados para evitar
/// erro de "email já cadastrado", por exemplo.
/// </summary>
public class FakerFactory
{
    /// <summary>
    /// Gera um email único baseado no timestamp atual
    ///
    /// Exemplo:
    /// user638497234234@test.com
    /// </summary>
    public static string GerarEmail()
    {
        return $"user{DateTime.Now.Ticks}@test.com";
    }

    /// <summary>
    /// Retorna um nome padrão para testes
    ///
    /// OBS:
    /// Pode evoluir para nome dinâmico
    /// </summary>
    public static string Nome()
    {
        return "Mateus QA";
    }
}