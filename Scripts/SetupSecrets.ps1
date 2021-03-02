param(
    # Database user name
    [Parameter(Mandatory=$true)]
    [string]
    $DbUserName,
    # Database password string
    [Parameter(Mandatory=$true)]
    [string]
    $DbPasswd
)

$project = Get-ChildItem -Recurse -Path ../ -Filter Contador.Web.Server.csproj;
dotnet user-secrets clear --project "$($project.FullName)"; 
dotnet user-secrets set "DbCredentials" "user=$DbUserName;password=$DbPasswd;" -p "$($project.FullName)";