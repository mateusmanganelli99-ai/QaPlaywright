using Microsoft.Playwright;
using System.Net.Http;

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
        var form = new MultipartFormDataContent
        {
            { new StringContent(nome), "name" },
            { new StringContent(email), "email" },
            { new StringContent(senha), "password" }
        };

        return await _api.PostAsync("/createAccount", new APIRequestContextOptions
        {

        });
    }
}