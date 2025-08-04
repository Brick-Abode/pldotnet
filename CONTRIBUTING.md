# Contributing to PL/.NET

PL/.NET is an open source project. Everyone is welcome to contribute.

## Commit Message Guidelines

We follow the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) specification for our commit messages. This standard helps to keep a consistent commit history and enables automatic versioning.

### Format

Each commit message should have the following structure:

```text
<type>(<scope>): <description>

[optional body]

[optional footer(s)]
```

#### Structure Breakdown

- **type**: The type of change, such as `feat`, `fix`, `docs`, `style`, `refactor`, `test`, or `chore`.
- **scope**: Optional. We use the scope to add automation for managing tasks. You can add the Jira or Github identification ticket, if applicable.
- **description**: A short, clear summary of what was changed.
- **body**: Optional. Provides additional details, such as the reasoning behind the change or additional context.
- **footer**: Optional. Used to reference issues or breaking changes, like `BREAKING CHANGE: <description>` or `Closes #123`.

### Commit Types

We use the following commit types:

- **feat**: A new feature
- **fix**: A bug fix
- **docs**: Documentation changes
- **style**: Changes that do not affect the meaning of the code (e.g., formatting)
- **refactor**: Code changes that neither fix a bug nor add a feature
- **perf**: Commits are special refactor commits, that improve performance
- **test**: Adding or updating tests
- **build**: Commits that affect build components like build tool, ci pipeline, dependencies, project version
- **ops**: Commits that affect operational components like infrastructure, deployment, backup, recovery.
- **chore**: Miscellaneous commits e.g. modifying .gitignore.

### Examples

#### Simple Commit Messages

- `feat(PLNET-175): Adds support for setting .NET version dynamically`
- `chore(PLNET-165): Update submodule and commit pointer`

#### Commit with Body

```text
refactor(PLNET-165): reorganize xUnit tests

Separated benchmark and SQL tests into different modules to improve code maintainability.
```

### Additional Notes

1. **Breaking Changes**:
   - If a change is not backward-compatible, include `BREAKING CHANGE:` in the footer.
   - Use a type with ! to draw attention to breaking change. `feat(PLNET-13)!: ...`

### MR Description

We use the following template for writing decriptive Merge Requests.

#### Title

The pull request title contains a meaningful title

- Short and informative: serves as a summary
- Prefixed with corresponding ticket/story ID from Jira or Github

```text
[PLNET-X] Content of the MR
```

#### Description

```text
# Contents

Explanation of your pull request in arbitrary form goes here. Please make sure the description explains the purpose and effect of your pull request and is worded well enough to be understood. Provide as much context and examples as possible.

# Issue

This PR is associated with the following Jira/Github Tasks:

[PLNET-X] Jira/Github ticket name

[PLNET-X]: https://brickabode-internal.atlassian.net/browse/PLNET-X

# Tests

How was this branch tested?

# Notes

(Add any additional information that would be useful to the developer or QA tester)

```
