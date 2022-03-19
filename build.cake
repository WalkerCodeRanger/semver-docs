#addin nuget:?package=Cake.DocFx&version=1.0.0
#tool nuget:?package=docfx.console&version=2.59.0
#tool nuget:?package=NuGet.CommandLine&version=6.0.0
#tool nuget:?package=vswhere&version=3.0.1

var target = Argument("target", "build");
var configuration = Argument("configuration", "Release");

// TASKS

Task("nuget-restore")
    .Does(() =>
{
    var solution = GetFiles("src/**/Semver.sln").Single();
    NuGetRestore(solution);
});
Task("build-solution")
    .IsDependentOn("nuget-restore")
    .Does(() =>
{
    // Find MSBuild since it is on a non-standard path on my machine
    var vsPath = VSWhereLatest(new VSWhereLatestSettings { Requires = "Microsoft.Component.MSBuild"});
    Information("vsPath: {0}", vsPath);
    var msBuildPath = vsPath.CombineWithFilePath(@"MSBuild\Current\Bin\MSBuild.exe");
    var solution = GetFiles("src/**/Semver.csproj").Single();
    MSBuild(solution, settings =>
    {
        settings.WithTarget("Rebuild").WithProperty("TargetFramework", "net452");
        settings.ToolPath = msBuildPath;
    });
});

Task("metadata")
    // Depend on the build because we use the DLLs so `<include ...` works
    .IsDependentOn("build-solution")
    .Does(() =>
{
    Environment.SetEnvironmentVariable("DOCFX_SOURCE_BRANCH_NAME", "v2.2.0");
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