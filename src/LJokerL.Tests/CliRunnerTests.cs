using System.Text;
using LJokerL.App;
using Xunit;

namespace LJokerL.Tests;

public class CliRunnerTests
{
    private static string RealConfigRoot()
    {
        return Path.GetDirectoryName(TestHelpers.FindConfigPath("profiles.json"))!;
    }

    private static string Capture(Func<TextWriter, int> run)
    {
        using var writer = new StringWriter();
        run(writer);
        return writer.ToString();
    }

    [Fact]
    public void Run_ApplyWithoutDryRun_ReturnsNotSupportedWithPendingMessage()
    {
        var options = CliOptions.Parse(new[] { "--apply-profile", "gaming_pro" });

        int exitCode = 0;
        var output = Capture(writer => exitCode = CliRunner.Run(options, writer, RealConfigRoot()));

        Assert.Equal(CliRunner.ExitNotSupported, exitCode);
        Assert.Contains(CliRunner.RealExecutionPendingMessage, output);
    }

    [Fact]
    public void Run_RevertWithoutDryRun_ReturnsNotSupportedWithPendingMessage()
    {
        var options = CliOptions.Parse(new[] { "--revert-profile", "balanced" });

        int exitCode = 0;
        var output = Capture(writer => exitCode = CliRunner.Run(options, writer, RealConfigRoot()));

        Assert.Equal(CliRunner.ExitNotSupported, exitCode);
        Assert.Contains(CliRunner.RealExecutionPendingMessage, output);
    }

    [Fact]
    public void Run_Help_ReturnsOkAndUsageText()
    {
        var options = CliOptions.Parse(new[] { "--help" });

        int exitCode = 0;
        var output = Capture(writer => exitCode = CliRunner.Run(options, writer, RealConfigRoot()));

        Assert.Equal(CliRunner.ExitOk, exitCode);
        Assert.Contains(CliOptions.UsageText, output);
    }

    [Fact]
    public void Run_List_ReturnsOkAndProfileIds()
    {
        var options = CliOptions.Parse(new[] { "--list-profiles" });

        int exitCode = 0;
        var output = Capture(writer => exitCode = CliRunner.Run(options, writer, RealConfigRoot()));

        Assert.Equal(CliRunner.ExitOk, exitCode);
        Assert.Contains("gaming_pro", output);
    }

    [Fact]
    public void Run_ApplyDryRun_ReturnsOkAndPlanSummary()
    {
        var options = CliOptions.Parse(new[] { "--apply-profile", "gaming_pro", "--dry-run" });

        int exitCode = 0;
        var output = Capture(writer => exitCode = CliRunner.Run(options, writer, RealConfigRoot()));

        Assert.Equal(CliRunner.ExitOk, exitCode);
        Assert.Contains("dry-run", output);
        Assert.Contains("acciones", output);
        Assert.DoesNotContain(CliRunner.RealExecutionPendingMessage, output);
    }

    [Fact]
    public void Run_WithParseErrors_ReturnsArgumentsErrorAndHelp()
    {
        var options = CliOptions.Parse(new[] { "--modo-turbo" });

        int exitCode = 0;
        var output = Capture(writer => exitCode = CliRunner.Run(options, writer, RealConfigRoot()));

        Assert.Equal(CliRunner.ExitArgumentsError, exitCode);
        Assert.Contains("--modo-turbo", output);
        Assert.Contains(CliOptions.UsageText, output);
    }

    [Fact]
    public void Run_DryRunWithUnknownProfile_ReturnsArgumentsError()
    {
        var options = CliOptions.Parse(new[] { "--apply-profile", "perfil_inexistente", "--dry-run" });

        int exitCode = 0;
        var output = Capture(writer => exitCode = CliRunner.Run(options, writer, RealConfigRoot()));

        Assert.Equal(CliRunner.ExitArgumentsError, exitCode);
        Assert.Contains("perfil_inexistente", output);
    }
}
