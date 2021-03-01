param(
    # Database name
    [Parameter(Mandatory=$true)][string]$DatabaseName,
    # Database user name
    [Parameter(Mandatory=$true)][string]$DatabaseUserName,
    # Database password
    [Parameter(Mandatory=$true)][securestring]$DatabasePassword,
    # WSL IP Address
    [Parameter(Mandatory=$true)][string]$WslIpAddress,
    # Determines if git hooks should be set
    [Parameter(Mandatory=$true)][bool]$SetupHooks
)

.\CreateWebDb.ps1 -DatabaseName $DatabaseName -DatabaseUserName $DatabaseUserName -DatabasePassword $DatabasePassword