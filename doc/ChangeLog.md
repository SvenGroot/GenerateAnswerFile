# What's new in Answer File Generator

## Answer File Generator 2.2 (TBD)

- Add an option to specify a password for the built-in local Administrator account. This account is
  disabled if no password is provided.

## Answer File Generator 2.1 (2025-03-09)

- You can now [generate computer names containing a random number](../README.md#specifying-a-computer-name),
  by using the `#` character in the name you provide.
- Improved error messages for certain kinds of invalid JSON input.

## Answer File Generator 2.0 (2024-10-07)

- The [`-DomainAccount`][] argument now allows you to specify users from different domains than the
  one you're joining.
- The [`-LocalAccount`][] and [`-DomainAccount`][] arguments now allow you to customize which groups
  the account is added to.
- You can join a domain using provisioning with the new [`-JoinDomainProvisioningFile`][] argument,
  and do it during the offlineServicing pass with the new [`-JoinDomainOffline`][] argument.
- The `-CmdKeyUser` and `-CmdKeyPassword` arguments have been removed; this was a bad security
  practice that I don't wish to promote. You can still get identical behavior using the
  [`-FirstLogonCommand`][] argument if desired.
- You can specify options using a [custom JSON file format](Json.md), as an alternative to using
  command line arguments.
- If no output file name is provided, the answer file is now written to the console.
- The `-SetupScript` argument has been renamed to [`-FirstLogonScript`][], for consistency with the
  [`-FirstLogonCommand`][] argument. A `-SetupScript` alias is provided for compatibility.
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
[`-JoinDomainOffline`]: CommandLine.md#-joindomainoffline
[`-JoinDomainProvisioningFile`]: CommandLine.md#-joindomainprovisioningfile
[`-JoinDomainUser`]: CommandLine.md#-joindomainuser
[`-LocalAccount`]: CommandLine.md#-localaccount
[`-Partition`]: CommandLine.md#-partition
[`-FirstLogonScript`]: CommandLine.md#-firstlogonscript
[`-SetupScript`]: CommandLine.md#-firstlogonscript
