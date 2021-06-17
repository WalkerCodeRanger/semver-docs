#addin nuget:?package=Cake.DocFx&version=1.0.0
#tool "nuget:?package=docfx.console&version=2.58.0" 

var target = Argument("target", "Doc");
var configuration = Argument("configuration", "Release");

// TASKS

Task("Doc")
    .Does(() =>
{
    DocFxMetadata();
    DocFxBuild();
});

Task("Serve")
    .IsDependentOn("Doc")
    .Does(() =>
{
    DocFxServe("./_site");
});

Task("Clean")
    .WithCriteria(c => HasArgument("rebuild"))
    .Does(() =>
{
    CleanDirectory($"./src/Example/bin/{configuration}");
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreBuild("./src/Example.sln", new DotNetCoreBuildSettings
    {
        Configuration = configuration,
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCoreTest("./src/Example.sln", new DotNetCoreTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
    });
});

// EXECUTION

RunTarget(target);