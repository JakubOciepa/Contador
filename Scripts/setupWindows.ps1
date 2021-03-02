param(
    # Database name
    [Parameter(Mandatory = $true)][string]$DatabaseName,
    # Database user name
    [Parameter(Mandatory = $true)][string]$DatabaseUserName,
    # Database password
    [Parameter(Mandatory = $true)][securestring]$DatabasePassword,
    #Dettermines if WSL will be used
    [Parameter(Mandatory = $true)][bool]$SetupWSL,
    # WSL IP Address
    [Parameter(Mandatory = $true)][string]$WslIpAddress,
    # Determines if git hooks should be set
    [Parameter(Mandatory = $true)][bool]$SetupHooks
)

$passwd = $(ConvertFrom-SecureString $DatabasePassword -AsPlainText);

$projectPath = Get-ChildItem -Recurse -Path ../ -Filter Contador.Web.Server.csproj;

dotnet user-secrets clear --project $projectPath
dotnet user-secrets init --project $projectPath

if ($SetupWSL -eq $true) {
    wsl -- pwsh ./CreateWebDb.ps1 -DatabaseName $DatabaseName -DatabaseUserName $DatabaseUserName -DatabasePwd $passwd;
    wsl -- pwsh ./SetupSecrets.ps1 -DbUserName $DatabaseUserName -DbPasswd $passwd;
}
if ($SetupWSL -ne $true) {

    ./CreateWebDb.ps1 -DatabaseName $DatabaseName -DatabaseUserName $DatabaseUserName -DatabasePwd $passwd;
}

./SetupSecrets.ps1 -DbUserName $DatabaseUserName -DbPasswd $passwd;
