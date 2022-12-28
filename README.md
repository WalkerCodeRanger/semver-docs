# Semver NuGet Package Docs

This repository generates the docs for the [semver NuGet package](https://www.nuget.org/packages/semver/) ([github.com/maxhauser/semver](https://github.com/maxhauser/semver)). The docs are published via GitHub pages to [semver-nuget.org](https://semver-nuget.org).

## Building

The docs are generated using [docfx](https://dotnet.github.io/docfx) via a [cake](https://cakebuild.net/) build script. Cake is set up in the repository directory as a dotnet tool. A basic build can be run from the command line using:

```bat
dotnet cake
```

To serve the docs locally, run:

```bat
dotnet cake --target=serve
```

## Project Structure

The source code of the semver project is imported via a git submodule as the `src/` directory. API metadata is generated into the `api/` directory, but is not committed. The actual documentation site is generated into and served from the `docs/` directory. Because GitHub pages cannot run cake or docfx, the `docs/` is committed and GitHub pages serves it as a completely static site. Use `dotnet cake --target=build` before committing the `docs/` directory to ensure it is clean.
