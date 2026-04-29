using Microsoft.Playwright;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace QaPlaywright.Core;

public class BaseTest
{
    protected IPlaywright Playwright = null!;
    protected IPage Page = null!;
    protected IBrowser Browser = null!;
    protected IBrowserContext Context = null!;
    private string ArtifactsRoot => Environment.GetEnvironmentVariable("GITHUB_WORKSPACE")
        ?? Directory.GetCurrentDirectory();

    [SetUp]
    public async Task Setup()
    {
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

        var isCI = Environment.GetEnvironmentVariable("CI") == "true";
        var videosDir = Path.Combine(ArtifactsRoot, "videos");

        Directory.CreateDirectory(videosDir);

        Browser = await Playwright.Chromium.LaunchAsync(new()
        {
            Headless = isCI
        });

        Context = await Browser.NewContextAsync(new()
        {
            RecordVideoDir = videosDir,
            RecordVideoSize = new() { Width = 1280, Height = 720 }
        });

        Page = await Context.NewPageAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        var testName = TestContext.CurrentContext.Test.Name;
        var status = TestContext.CurrentContext.Result.Outcome.Status;

        if (status == TestStatus.Failed && Page is not null)
        {
            var screenshotsDir = Path.Combine(ArtifactsRoot, "screenshots");

            Directory.CreateDirectory(screenshotsDir);

            await Page.ScreenshotAsync(new()
            {
                Path = Path.Combine(screenshotsDir, $"{testName}.png"),
                FullPage = true
            });
        }

        if (Context is not null)
        {
            await Context.CloseAsync();
        }

        if (Browser is not null)
        {
            await Browser.CloseAsync();
        }

        Playwright?.Dispose();
    }
}
