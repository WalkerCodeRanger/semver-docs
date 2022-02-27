# PLACEHOLDER

Placeholder for v2.1.x docs

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
