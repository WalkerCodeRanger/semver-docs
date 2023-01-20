---
uid: Semver.Ranges
conceptual: *content
---
### Range Syntax

TODO: inspired by npm

TODO: overview and examples, mention prerelease

TODO: gaps

#### Comparison Sets

#### Prerelease Versions

#### Basic Operators

#### Advanced Operators

##### Compatible Operator (Caret)

Bug fix releases

##### Approximately Equivalent Operator (Tilde)

Backwards compatible releases

#### Wildcards

#### Differences from npm Syntax

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

### Parsing npm Syntax

The `ParseNpm` and `TryParseNpm` methods enable the parsing of [npm range
syntax](https://github.com/npm/node-semver#ranges) into a version range. The [npm range
syntax](https://github.com/npm/node-semver#ranges) is defined and implemented by the npm project.

#### Differences from npm Syntax

There are a few minor differences between the syntax supported by npm and by the `ParseNpm` and
`TryParseNpm` methods. In all cases these are arguably bugs or at least inconsistencies in how npm
parses ranges. An issue has been filed with the npm project for each and is included for reference
in the list of differences below.

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
