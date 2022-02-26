# A Semantic Version Library for .Net

This library implements the SemVersion class, which complies with v2.0.0 of the spec from [semver.org](http://semver.org).

* [`semver` NuGet Package](https://www.nuget.org/packages/semver)
* [GitHub Project](https://github.com/maxhauser/semver)
* [Source Code for Doc Website](https://github.com/WalkerCodeRanger/semver-docs)

## Installation

With the NuGet console:

```powershell
Install-Package semver
```

## Parsing

```csharp
var version = SemVersion.Parse("1.1.0-rc.1+nightly.2345", SemVersionStyles.Strict);
```

## Constructing

```csharp
var v1 = new SemVersion(1, 0);
```

## Comparing

```csharp
if (version < v1)
    Console.WriteLine($"Initial development version {version}!");
```

## Manipulating

```csharp
Console.WriteLine($"Current: {version}");
if (version.IsPrerelease)
{
    Console.WriteLine($"Prerelease: {version.Prerelease}");
    Console.WriteLine($"Next release version is: {version.WithoutPrereleaseOrMetadata()}");
}
```

Outputs:

```text
Current: 1.1.0-rc.1+nightly.2345
Prerelease: rc.1
Next release version is: 1.1.0
```
