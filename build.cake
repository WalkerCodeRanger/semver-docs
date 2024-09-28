#addin nuget:?package=Cake.DocFx&version=1.0.0
#tool nuget:?package=docfx.console&version=2.59.4
#tool nuget:?package=NuGet.CommandLine&version=6.0.0
#tool nuget:?package=vswhere&version=3.1.7

var target = Argument("target", "build");
var configuration = Argument("configuration", "Release");

// TASKS

Task("nuget-restore")
    .Does(() =>
{
    var solutions = GetFiles("src/**/Semver.sln");
    foreach (var solution in solutions)
    {
        Information($"Restoring NuGet for {solution}");
        NuGetRestore(solution, new NuGetRestoreSettings { Verbosity = NuGetVerbosity.Quiet });
    }
});
Task("build-solution")
    .IsDependentOn("nuget-restore")
    .Does(() =>
{
    // Find MSBuild since it is on a non-standard path on my machine
    var vsPath = VSWhereLatest(new VSWhereLatestSettings { Requires = "Microsoft.Component.MSBuild" });
    Information("vsPath: {0}", vsPath);
    var msBuildPath = vsPath.CombineWithFilePath(@"MSBuild\Current\Bin\MSBuild.exe");
    var projects = GetFiles("src/**/Semver.csproj");
    foreach (var project in projects)
    {
        Information($"Building {project}");
        MSBuild(project, settings =>
        {
            settings.WithTarget("Rebuild");
            if (project.FullPath.Contains("v2.3.0"))
                settings.WithProperty("TargetFramework", "net452");
            settings.ToolPath = msBuildPath;
        });
    }
});

Task("metadata")
    // Depend on the build because we use the DLLs so `<include ...` works
    .IsDependentOn("build-solution")
    .Does(() =>
{
    Environment.SetEnvironmentVariable("DOCFX_SOURCE_BRANCH_NAME", "v2.3.0");
    DocFxMetadata();
});

Task("build")
    .IsDependentOn("metadata")
    .Does(() =>
{
    // Even force build doesn't properly clean up old files
    CleanDirectories("./docs");
    Environment.SetEnvironmentVariable("DOCFX_SOURCE_BRANCH_NAME", "master");
    DocFxBuild(new DocFxBuildSettings()
    {
        Force = true,
    });
});

Task("serve")
    .IsDependentOn("build")
    .Does(() =>
{
    DocFxServe("./docs");
});


// EXECUTION

RunTarget(target);