#addin nuget:?package=Cake.DocFx&version=1.0.0
#tool "nuget:?package=docfx.console&version=2.59.0" 

var target = Argument("target", "build");
var configuration = Argument("configuration", "Release");
FilePath docfxPath = Context.Tools.Resolve("docfx.exe");

// TASKS

Task("clean-metadata")
    .Does(() =>
{
    CleanDirectories("./metadata");
});

Task("metadata/v2.0.x")
    .Does(() =>
{
    Environment.SetEnvironmentVariable("DOCFX_SOURCE_BRANCH_NAME", "v2.0.6");
    DocFxMetadata("v2.0.6", "net452");
    DocFxMetadata("v2.0.6", "netstandard1.1");
    // StartProcess(docfxPath, "metadata src/v2.0.6.json --output metadata/v2.0.6/net452 --property TargetFramework=net452");
    // StartProcess(docfxPath, "metadata src/v2.0.6.json --output metadata/v2.0.6/netstandard1.1 --property TargetFramework=netstandard1.1");
    // Environment.SetEnvironmentVariable("DOCFX_SOURCE_BRANCH_NAME", "v2.0.6");
    // DocFxMetadata(new DocFxMetadataSettings()
    // {
    //     Projects = GetFiles("src/v2.0.x.json")
    // });
});

Task("metadata/release-2.1")
    .Does(() =>
{
    Environment.SetEnvironmentVariable("DOCFX_SOURCE_BRANCH_NAME", "release-2.1");
    DocFxMetadata("release-2.1", "net452");
    DocFxMetadata("release-2.1", "netstandard1.1");
    DocFxMetadata("release-2.1", "netstandard2.0");
    // DocFxMetadata(new DocFxMetadataSettings()
    // {
    //     Projects = GetFiles("src/dev.json")
    // });
});

Task("metadata")
    .IsDependentOn("clean-metadata")
    .IsDependentOn("metadata/v2.0.x")
    .IsDependentOn("metadata/release-2.1");

Task("build")
    .Does(() =>
{
    // Even force build doesn't properly clean up old files
    CleanDirectories("./docs");
    //Environment.SetEnvironmentVariable("DOCFX_SOURCE_BRANCH_NAME", "master");
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

// FUNCTIONS

public void DocFxMetadata(string version, string framework)
{
    StartProcess(docfxPath, $"metadata build/{version}.json --output metadata/{version}/{framework} --property TargetFramework={framework}");
}