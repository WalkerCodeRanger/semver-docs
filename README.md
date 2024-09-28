# Semver NuGet Package Docs

This repository generates the docs for the [semver NuGetpackage](https://www.nuget.org/packages/semver/)
([github.com/WalkerCodeRanger/semver](https://github.com/WalkerCodeRanger/semver)). The docs are
published via GitHub pages to [semver-nuget.org](https://semver-nuget.org).

## Building

The docs are generated using [docfx](https://dotnet.github.io/docfx) via a [cake](https://cakebuild.net/)
build script. Cake is set up in the repository directory as a dotnet tool. Use `dotnet tool restore`
to download it before running the first time. A basic build can be run from the command line using:

```bat
dotnet cake
```

To serve the docs locally, run:

```bat
dotnet cake --target=serve
```

## Project Structure

The source code of the semver project is imported via a git submodules a the `src/v?.?.?/`
directory. Each version that docs are generated for is imported as a separate submodule. API
metadata is generated into a `v?.?.x/` directory, but is not committed. Docs are generated for all
versions with the same major and minor version because patch versions should not change the public
API. However, the latest patch version should be used to generate docs because there may be updated
doc comments. DocFx's support of generating docs for multiple versions is limited and incomplete.
When configured to generate for multiple versions, it seems to treat them as fully independent. To
work around this, the build generates a separate site for each version into the `build` directory.
That creates docs for each version that have the proper relationship to the table of contents at the
root level. In addition, it builds a "merged" version to properly generate the root level pages in a
way that references all versions. The build then assembles the final site from those. Any version
specific files are taken from the corresponding version build. All files outside the version
specific directories are taken from the "merged" version. The actual documentation site is generated
into and served from the `docs/` directory. Because GitHub pages cannot run cake or docfx, the
`docs/` directory is committed and GitHub pages serves it as a completely static site. Use `dotnet
cake` before committing the `docs/` directory to ensure it matches the build/project state.
