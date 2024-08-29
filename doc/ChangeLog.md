# What's new in Answer File Generator

## Answer File Generator 2.0 (TBD)

- The [`-DomainAccount`][] argument allows you to specify users from different domains than the one
  you're joining.
- The [`-LocalAccount`][] and [`-DomainAccount`][] allow you to customize which groups the account
  is added to.
- You can specify options using a [custom JSON file format](Json.md), as an alternative to using
  command line arguments.
- If no output file argument is provided, the answer file is now written to the console.
- The Answer File Generator is now available in standalone single-file versions, that do not require
  you to install the .Net Runtime.
- There are some breaking changes to the [Ookii.AnswerFile library](Library.md#breaking-changes).

## Answer File Generator 1.1 (2023-10-10)

- Apply a workaround for a [known issue with the `LogonCount` element](https://learn.microsoft.com/windows-hardware/customize/desktop/unattend/microsoft-windows-shell-setup-autologon-logoncount),
  so that the [`-AutoLogonCount`][] argument is accurate.
- Add the [`-Partition`][] argument, which adds the ability to specify a custom partition layout
  when using the `CleanEfi` or `CleanBios` installation method.
- Add the [`-FirstLogonCommand`][] argument to run commands that are not PowerShell scripts.
- Add the [`-DisableServerManager`][] option to disable the automatic launching of Server Manager on
  Windows Server SKUs.
- The [`-JoinDomainUser`][] argument allows the use of a user from a different domain than the one
  being joined.
- Using the [`-SetupScript`][] argument no longer changes the global execution policy for Windows
  PowerShell.
- The local and online account screens are no longer skipped if you do not specify either the
  [`-LocalAccount`][] or the [`-JoinDomain`][] arguments.
- [Source Link](https://github.com/dotnet/sourcelink) support for the
  [Ookii.AnswerFile library](Library.md).

## Answer File Generator 1.0 (2023-01-25)

- This is the first release of Answer File Generator

[`-AutoLogonCount`]: CommandLine.md#-autologoncount
[`-DisableServerManager`]: CommandLine.md#-disableservermanager
[`-DomainAccount`]: CommandLine.md#-domainaccount
[`-FirstLogonCommand`]: CommandLine.md#-firstlogoncommand
[`-JoinDomain`]: CommandLine.md#-joindomain
[`-JoinDomainUser`]: CommandLine.md#-joindomainuser
[`-LocalAccount`]: CommandLine.md#-localaccount
[`-Partition`]: CommandLine.md#-partition
[`-SetupScript`]: CommandLine.md#-setupscript
