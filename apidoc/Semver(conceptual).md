---
uid: Semver
conceptual: *content
---
### Range Syntax

A version range is composed of one or more comparisons (e.g. `>=1.2.6`). These comparisons are
joined into comparison sets with whitespace (e.g. `>=1.2.6 <2.0.0`). A comparison set forms an
intersection of the comparisons in the set. That is, for a version to be included in the range, it
must satisfy all the comparisons in that set. Comparison sets can be joined together using the `||`
operator (e.g. `>=1.2.6 <2.0.0 || 2.0.0-beta`). The `||` forms a union of the comparison sets. That
is, for a version to be included in the range, it must satisfy *one* of the comparison sets.

The simple joining of basic and advanced comparisons is enough to create any sort of version range.
For example:

* `4.32.9`
* `3.*`
* `>=1.3.5 <3.1.0`
* `^1.2.6 || 2.0.0-rc`

There are two things to note about ranges. First, that a range can contain version gaps if it is
composed of multiple comparison sets. Second, that by default not all prerelease versions are
included in a range. Only prerelease versions whose major, minor, and patch versions are equal to a
comparison version that is itself prerelease are includes (see [Prerelease
Versions](#prerelease-versions)).

#### Versions in Ranges

By default the syntax of all versions appear in a range expression must strictly conform to the
semantic versioning spec and not have a metadata section. This can be relaxed using the
[SemVersionRangeOptions](xref:Semver.SemVersionRangeOptions).

#### Comparison Sets

Comparison sets combine one or more comparisons consisting of basic operators, advanced operators,
and wildcards into a single [UnbrokenSemVersionRange](xref:Semver.UnbrokenSemVersionRange).
Individual comparisons in a comparison set may be optional surrounded by one or more spaces on
either side. A space is not needed to separate two comparisons (e.g. `>1.0.0<2.0.0`).

Since all comparisons in a comparison set must be met, they can always be simplified down to an
unbroken range with a minimum and maximum version each of which may or may not be included in the
range.

Comparison sets can be combined using the `||` operator. The `||` operator may be optionally
separated from the comparison sets by one or more spaces on either side. The `||` operator unions
the comparison sets it joins together.

#### Prerelease Versions

By default, not all prerelease versions are included in a version range. This can be overridden with
the [IncludeAllPrerelease](xref:Semver.SemVersionRangeOptions.IncludeAllPrerelease) option.
Alternatively, including the comparison `*-*` in a comparison set will make that comparison set
include all prerelease versions. This does not affect the other comparison sets allowing some
comparison sets to include all prerelease versions while others do not.

Without the [IncludeAllPrerelease](xref:Semver.SemVersionRangeOptions.IncludeAllPrerelease) option,
only prerelease versions contained in the range and matching the major, minor, and patch version of
some comparison in the range that is a prerelease version will be included. For example, the range
`>=1.2.0-alpha <2.0.0` includes the prerelease versions `1.2.0-alpha` and `1.2.0-beta` but not the
versions `1.6.0-rc` or `1.23.1-alpha`.

The special wildcard comparison `*-*` matches all versions including prerelease versions. Thus, when
it is combined with another comparison (e.g. `*-* >=1.5.0`), it satisfies the requirement that the
prerelease version matches some comparison for all prerelease versions.

#### Basic Operators

There are five basic operators. Each prefixes a version number to form a comparison to that version.
The basic operators are:

* `<` Less than
* `<=` Less than or equal to
* `>` Greater than
* `>=` Greater than or equal to
* `=` Equal

If no operator is specified, then equality is assumed, so this operator is optional, but may be
included.

#### Advanced Operators

There are two advanced operators that provide both a lower and upper bound with a single comparison.

##### Compatible Operator (Caret)

The caret (`^`) is the compatible operator. It matches all backwards compatible versions to the
given version according to the semantic version specification. Thus it matches all versions greater
than or equal to the given version and less than the next major version. Since many authors treat a
`0.x.y` version as if the `x` were the major "breaking-change" indicator, the caret operator behaves
differently when used with initial development versions (i.e. `0.x.y`). The exact rule is that it
allows changes that do not modify the left-most non-zero element of the major, minor, or patch
version. In other words, this allows patch and minor updates for versions `1.0.0-0` and above, patch
updates for versions `0.x.y-* >=0.1.0`, and only prerelease updates for versions `0.0.x-*`.

The caret operator is always equivalent to a `>=` combined with a `<`. The examples below illustrate
each of the possible cases.

* `^1.2.3` := `>=1.2.3 <2.0.0-0`
* `^0.2.3` := `>=0.2.3 <0.3.0-0`
* `^0.0.3` := `>=0.0.3 <0.0.4-0` (i.e. `=0.0.3`)
* `^1.2.3-beta.2` := `>=1.2.3-beta.2 <2.0.0-0` Note that prereleases in the `1.2.3` version will be
  allowed, if they are greater than or equal to `beta.2`. So, `1.2.3-beta.4` would be allowed, but
  `1.2.4-beta.2` would not, because it is a prerelease with a different major, minor, or patch
  version.
* `^0.0.3-beta` := `>=0.0.3-beta <0.0.4-0` Note that prereleases in the `0.0.3` version only will be
  allowed, if they are greater than or equal to `beta`. So, `0.0.3-pr.2` would be allowed.

##### Approximately Equivalent Operator (Tilde)

The tilde (`~`) is the approximately equivalent operator. This allows only bug fix releases for the
given version. That is, it matches only versions with a patch level difference.

The tilde operator is always equivalent to a `>=` combined with a `<`. The examples below illustrate
each of the possible cases.

* `~1.2.3` := `>=1.2.3 <1.3.0-0`
* `~0.2.3` := `>=0.2.3 <0.3.0-0`
* `~1.2.3-beta.2` := `>=1.2.3-beta.2 <1.3.0-0` Note that prereleases in the `1.2.3` version will be
  allowed, if they are greater than or equal to `beta.2`. So, `1.2.3-beta.4` would be allowed, but
  `1.2.4-beta.2` would not, because it is a prerelease of a different major, minor, or patch
  version.

#### Wildcards

In addition to the basic and advanced operators, comparisons can be formed using wildcards. A
wildcard comparison cannot be combined with an operator. The wildcard character "`*`" may be used in
place of the major, minor, or patch version as well as the final prerelease identifier.

A wildcard in the major, minor, or patch version matches any version number in that position. For
example, `2.*.*` matches all release versions with a major version of `2`. If a version number is
replaced with a wildcard, then all later version numbers must also be wildcards. Thus `2.*.6` is an
invalid wildcard version. Additionally, no prerelease identifiers can be specified after a wildcard
version number. A wildcard may be optionally used to match all prerelease versions (e.g. `1.2.*-*`).
In a wildcard version with multiple wildcards, the subsequent wildcards can be omitted. For example,
`6.*.*` can be shortened to `6.*` and `*.*.*` can be shortened to just `*`.

A wildcard may be used in place of the final prerelease identifier (e.g. `1.2.3-alpha.*`,
`1.2.3-*`). Such a prerelease wildcard will match all prerelease versions with the same prerelease
identifiers as appear before the prerelease wildcard. It does not match prerelease versions with no
identifier in that place. For example, `1.2.3-alpha.*` does not match `1.2.3-alpha`, but it does
match `1.2.3-alpha.0` and `1.2.3-alpha.something`.

There are two wildcard versions of special note. The first "`*`" matches all release versions. The
second "`*-*`" matches all versions both release and prerelease. The latter can be used in a
comparison set to include all prerelease versions in that comparison set.

#### Differences from npm Syntax

Unfortunately, there is no official standard for the syntax of semantic versions ranges. However,
the syntax defined by the npm package seems to have become somewhat of a defacto standard. Many
other tools and libraries implement a similar syntax. However, they often have subtle or major
differences in the syntax or interpretation. The standard syntax of this library is also inspired by
the npm syntax but has major deviations for clarity and increased flexibility. The differences
between npm syntax and the syntax used by this library are listed below with a brief statement of
the reasoning behind the difference.

* Omitted minor and patch versions are not treated as wildcards.
  * Treating them as wildcards is surprising and unintuitive. People are used to omitted version
    numbers being zero. For example, the version `2.1` is read as `2.1.0` not as `2.1.*`.
* Asterisk '`*`' is the only wildcard character, '`X`' and '`x`' are not supported.
  * Both '`X`' and '`x`' are valid characters inside prerelease identifiers and so couldn't be used
    for a wildcard in a prerelease identifiers. Also, '`*`' is more clearly and universally
    recognized as a wildcard.
* A wildcard is allowed as the last prerelease identifier.
  * This adds a lot of flexibility and expressiveness to ranges. It also provides a syntax
    equivalent to the allow all prerelease option (e.g. include `*-*` in a comparison set). Finally,
    it is a natural and expected extension of the wildcard version number syntax.
* Wildcards can't be combined with operators.
  * It is difficult and confusing to reason about the interaction of the wildcard with the operator.
    At the same time most wildcard operator combinations have straightforward equivalent expressions
    that do not involve a wildcard. So removing this does not limit the expressiveness of ranges.
* Hyphen ranges aren't supported.
  * Hyphen is a valid character in semantic versions. As a result, whitespace must be included on
    either side of the hyphen. If that whitespace is omitted, it easily leads to a valid range
    expression different than intended. The requirement for spaces only applies to the hyphen and
    not any of the other operators so it breaks expectations. Finally hyphen ranges aren't that much
    shorter or clearer than a simple greater than or equal to combined with a less than (e.g.
    "`1.1.0 - 2.0.0`" is equivalent to "`>=1.1.0 <2.2.0`").
* Only ASCII space characters are allowed within a range.

### Parsing npm Range Syntax

The [ParseNpm](xref:Semver.SemVersionRange.Parse(System.String,Semver.SemVersionRangeOptions,System.Int32))
and related methods enable the parsing of [npm range
syntax](https://github.com/npm/node-semver#ranges) into a version range. The [npm range
syntax](https://github.com/npm/node-semver#ranges) is defined and implemented by the npm project.

#### Deviations from npm Range Syntax

There are a few minor differences between the syntax supported by npm and by
[ParseNpm](xref:Semver.SemVersionRange.Parse(System.String,Semver.SemVersionRangeOptions,System.Int32))
and related methods methods. In all cases these are arguably bugs or at least inconsistencies in how
npm parses ranges. An issue has been filed with the npm project for each and is included for
reference in the list of differences below.

* Build metadata can be combined with partial versions. (npm issue
  [#509](https://github.com/npm/node-semver/issues/509))
* Prerelease versions cannot follow wildcards. (npm issue
  [#510](https://github.com/npm/node-semver/issues/510) and
  [#509](https://github.com/npm/node-semver/issues/509))
* A minor or patch version cannot follow a wildcard. (npm issue
  [#511](https://github.com/npm/node-semver/issues/511))
* The tilde wildcard range (e.g. "`~1.2.*`") with the include all prerelease versions option
  includes prerelease versions with the same major and minor version. (npm issue
  [#512](https://github.com/npm/node-semver/issues/512))
