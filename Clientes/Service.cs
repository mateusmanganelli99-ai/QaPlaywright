using Microsoft.Playwright;

namespace QaPlaywright.API;

public class UserService
{
    private readonly IAPIRequestContext _api;

    public UserService(IAPIRequestContext api)
    {
        _api = api;
    }

    public async Task<IAPIResponse> CriarUsuario(string nome, string email, string senha)
    {
        var form = _api.CreateFormData();
        form.Set("name", nome);
        form.Set("email", email);
        form.Set("password", senha);

        return await _api.PostAsync("/createAccount", new APIRequestContextOptions
        {
            Form = form
        });
    }
}