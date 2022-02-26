#addin nuget:?package=Cake.DocFx&version=1.0.0
#tool "nuget:?package=docfx.console&version=2.59.0" 

var target = Argument("target", "build");
var configuration = Argument("configuration", "Release");

// TASKS

Task("metadata")
    .Does(() =>
{
    Environment.SetEnvironmentVariable("DOCFX_SOURCE_BRANCH_NAME", "v2.0.6");
    DocFxMetadata();
});

Task("build")
    .IsDependentOn("Metadata")
    .Does(() =>
{
    DocFxBuild();
});

Task("rebuild")
    .IsDependentOn("Metadata")
    .Does(() =>
{
    DocFxBuild(new DocFxBuildSettings()
    {
        Force = true,
    });
});

Task("serve")
    .IsDependentOn("Build")
    .Does(() =>
{
    DocFxServe("./docs");
});


// EXECUTION

RunTarget(target);