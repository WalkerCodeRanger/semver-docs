# A Semantic Version Library for .Net

This library implements the `SemVersion` class, which
complies with v2.0.0 of the spec from [semver.org](http://semver.org).

## Installation

With the NuGet console:

```powershell
Install-Package semver
```

## Parsing

```csharp
var version = SemVersion.Parse("1.1.0-rc.1+e471d15", SemVersionStyles.Strict);
```

## Constructing

```csharp
var v1 = new SemVersion(1, 0);
var vNextRc = SemVersion.ParsedFrom(1, 1, 0, "rc.1");
```

## Comparing

```csharp
if (version.ComparePrecedenceTo(vNextRc) == 0)
    Console.WriteLine($"{version} has the same precedence as {vNextRc}");

if (version.CompareSortOrderTo(vNextRc) > 0)
    Console.WriteLine($"{version} sorts after {vNextRc}");
```

Outputs:

```text
1.1.0-rc.1+e471d15 has the same precedence as 1.1.0-rc.1
1.1.0-rc.1+e471d15 sorts after 1.1.0-rc.1
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
