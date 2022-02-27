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
var version = SemVersion.Parse("1.1.0-rc.1+nightly.2345");
```

## Comparing

```csharp
if(version >= "1.0")
    Console.WriteLine("released version {0}!", version)
```
