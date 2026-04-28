using Microsoft.Playwright;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace QaPlaywright.Core;

public class BaseTest
{
    protected IPage Page;
    protected IBrowser Browser;
    protected IBrowserContext Context;

    [SetUp]
    public async Task Setup()
    {
        var playwright = await Playwright.CreateAsync();

        bool isCI = Environment.GetEnvironmentVariable("CI") == "true";

        Browser = await playwright.Chromium.LaunchAsync(new()
        {
            Headless = isCI
        });

        Context = await Browser.NewContextAsync(new()
        {
            RecordVideoDir = "videos/",
            RecordVideoSize = new() { Width = 1280, Height = 720 }
        });

        Page = await Context.NewPageAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        var testName = TestContext.CurrentContext.Test.Name;
        var status = TestContext.CurrentContext.Result.Outcome.Status;

        // 📸 Screenshot em erro
        if (status == TestStatus.Failed)
        {
            Directory.CreateDirectory("screenshots");

            await Page.ScreenshotAsync(new()
            {
                Path = $"screenshots/{testName}.png",
                FullPage = true
            });
        }

        await Context.CloseAsync(); // 👈 necessário para salvar vídeo
        await Browser.CloseAsync();
    }
}