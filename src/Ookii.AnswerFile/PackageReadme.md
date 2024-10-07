# Ookii.AnswerFile

Ookii.AnswerFile is a library for generating answer files for unattended Windows installations.
These files are commonly called unattend.xml or autounattend.xml (depending on the installation
method used).

This library can be used to generate answer files as part of an automated workflow for installing
Windows, or just as a convenient way to generate answer files for personal use, without the need to
install the Windows System Image Manager, or manually edit XML files.

Answer files can customize many aspects of the Windows installation, only some of which are
available through this tool. Customizations supported by the Answer File Generator include:

- The installation method, partition layout, and target disk and partition.
- Enabling optional features during installation.
- Creation of local user accounts.
- Joining a domain, and adding domain accounts to a local security group.
- Configuring automatic log-on.
- The product key, computer name, language/culture, and time zone.
- Display resolution.
- Disabling Windows Defender.
- Enabling remote desktop access.
- Running PowerShell scripts and other commands on first log-on.

For more information, see the [GitHub project page](https://github.com/SvenGroot/GenerateAnswerFile),
where you can also find a command line tool that uses this library.
