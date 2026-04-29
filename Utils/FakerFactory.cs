namespace QaPlaywright.Utils;

public class FakerFactory
{
    public static string GerarEmail()
    {
        return $"user_{Guid.NewGuid():N}@test.com";
    }

    public static string GerarEmail(int indice)
    {
        return $"user_{indice}_{Guid.NewGuid():N}@test.com";
    }

    public static string Nome()
    {
        return "Mateus QA";
    }

    public static string Nome(int indice)
    {
        return $"Mateus QA {indice}";
    }
}
