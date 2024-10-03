# Using JSON to provide options

Because the Answer File Generator has many options you can customize, using
[command line arguments](CommandLine.md) to specify them all can become unwieldy.

As an alternative, you can use a JSON input file to provide the options that will be used to
generate the answer file. Answer File Generator JSON files are more structured, providing an easy
to read and write format for specifying the options. The same options can be specified using both
command line arguments and JSON.

To specify a JSON file, you must pipe the JSON document into the standard input of the Answer File
Generator.

```cmd
GenerateAnswerFile.exe unattend.xml < unattend.json
```

Or, using PowerShell:

```pwsh
Get-Content unattend.json | ./GenerateAnswerFile.exe unattend.xml
```

Other command line arguments, beside the output file, are not available when a JSON file is
provided.

An simple example JSON file that performs a clean installation on a UEFI system could look like
this:

```json
{
    "$schema": "https://www.ookii.org/Link/AnswerFileJsonSchema-2.0",
    "InstallOptions": {
        "$type": "CleanEfi"
    },
    "LocalAccounts": [
        {
            "UserName": "localuser",
            "Password": "password"
        }
    ],
    "Language": "en-US",
    "ProductKey": "ABCDE-12345-ABCDE-12345-ABCDE",
    "ProcessorArchitecture": "amd64",
    "TimeZone": "Pacific Standard Time"
}
```

The above file would generate an answer file that installs Windows using the default UEFI partition
layout, activates it using the specified product key, and creates a local user with the specified
password.

> [!WARNING]
> Passwords in the JSON files are always in plain text, so do not store an Answer File Generator
> JSON file in an unsecure location.

A [JSON schema](json/schema.json) is provided that provides documentation, as well as validation and
auto-completion (when using an editor such as Visual Studio Code).

A few other JSON samples are provided:

- [cleanefi.json](json/cleanefi.json): a more complex clean UEFI installation example, which
  uses a [custom partition layout](../README.md#custom-partition-layout), and installs
  [optional features](../README.md#optional-features). It does not create a user account, so the
  user will be asked to create one during OOBE.
- [cleanbios.json](json/cleanbios.json): generates an answer file to install Windows on a 32 bit
  BIOS system. It does not specify a product key, but instead uses the WIM image index to indicate
  which image to install. This is suitable for e.g. volume licensed editions of Windows.
- [existingpartition.json](json/existingpartition.json): installs Windows to an existing partition,
  in this case the fifth partition on the fourth disk (partitions are one-based, and disks are
  zero-based).
- [manual.json](json/manual.json): generates an answer file without installation target information,
  so the user will be prompted to specify a disk and partition to install to.
- [preinstalled.json](json/preinstalled.json): this file does not specify install options, so it's
  suitable for an already deployed image such as those created using sysprep or DISM tools. It also
  demonstrates a number of other options, including joining a domain, adding domain users to local
  groups, automatic log-on, and first-log-on commands and scripts.
