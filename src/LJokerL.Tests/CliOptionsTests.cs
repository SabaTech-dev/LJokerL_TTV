using LJokerL.App;
using Xunit;

namespace LJokerL.Tests;

public class CliOptionsTests
{
    [Fact]
    public void Parse_WithNoArguments_ReturnsNoneActionWithoutErrors()
    {
        var options = CliOptions.Parse(Array.Empty<string>());

        Assert.Equal(CliAction.None, options.Action);
        Assert.False(options.DryRun);
        Assert.Null(options.ProfileId);
        Assert.Empty(options.Errors);
    }

    [Fact]
    public void Parse_WithApplyProfile_ParsesActionAndProfileId()
    {
        var options = CliOptions.Parse(new[] { "--apply-profile", "gaming_pro" });

        Assert.Equal(CliAction.Apply, options.Action);
        Assert.Equal("gaming_pro", options.ProfileId);
        Assert.False(options.DryRun);
        Assert.Empty(options.Errors);
    }

    [Fact]
    public void Parse_WithRevertProfileAndDryRun_ParsesActionIdAndDryRun()
    {
        var options = CliOptions.Parse(new[] { "--revert-profile", "balanced", "--dry-run" });

        Assert.Equal(CliAction.Revert, options.Action);
        Assert.Equal("balanced", options.ProfileId);
        Assert.True(options.DryRun);
        Assert.Empty(options.Errors);
    }

    [Fact]
    public void Parse_WithListProfiles_ParsesListAction()
    {
        var options = CliOptions.Parse(new[] { "--list-profiles" });

        Assert.Equal(CliAction.List, options.Action);
        Assert.Null(options.ProfileId);
        Assert.Empty(options.Errors);
    }

    [Fact]
    public void Parse_WithHelp_ParsesHelpAction()
    {
        var options = CliOptions.Parse(new[] { "--help" });

        Assert.Equal(CliAction.Help, options.Action);
        Assert.Empty(options.Errors);
    }

    [Fact]
    public void Parse_WithApplyProfileMissingId_RecordsError()
    {
        var options = CliOptions.Parse(new[] { "--apply-profile" });

        Assert.NotEmpty(options.Errors);
        Assert.False(options.DryRun);
    }

    [Fact]
    public void Parse_WithUnknownArgument_RecordsError()
    {
        var options = CliOptions.Parse(new[] { "--modo-turbo" });

        Assert.NotEmpty(options.Errors);
        Assert.Contains("--modo-turbo", options.Errors[0]);
    }
}
