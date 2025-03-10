namespace GenerateAnswerFile;

enum ArgumentCategory
{
    [ResourceDescription(nameof(Properties.Resources.CategoryInstall))]
    Install,
    [ResourceDescription(nameof(Properties.Resources.CategoryUserAccounts))]
    UserAccounts,
    [ResourceDescription(nameof(Properties.Resources.CategoryDomain))]
    Domain,
    [ResourceDescription(nameof(Properties.Resources.CategoryOther))]
    Other,
}
