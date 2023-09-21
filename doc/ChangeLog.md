# What's new in Answer File Generator

## Answer File Generator 1.1

- Apply a workaround for a [known issue with the `LogonCount` element](https://learn.microsoft.com/windows-hardware/customize/desktop/unattend/microsoft-windows-shell-setup-autologon-logoncount),
  so that the `-AutoLogonCount` argument is accurate.
- Add the `-Partition` argument, which adds the ability to specify a custom partition layout when
  using the `CleanEfi` or `CleanBios` installation method.
- Add the `-FirstLogonCommand` argument to run commands that are not PowerShell scripts.
- Add the `-DisableServerManager` option to disable the automatic launching of Server Manager on
  Windows Server SKUs.
- The `-JoinDomainUser` argument allows the use of a user from a different domain than the one being
  joined.
- Using the `-SetupScript` argument no longer changes the global execution policy for Windows
  PowerShell.
- The local and online account screens are no longer skipped if you do not specify either the
  `-LocalAccount` or the `-JoinDomain` arguments.
- [Source Link](https://github.com/dotnet/sourcelink) support for the Ookii.AnswerFile library.

## Answer File Generator 1.0 (2023-01-25)

- This is the first release of Answer File Generator
