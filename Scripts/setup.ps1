$DbUserName = ""
$DbPasswd = ""
$DbName = ""

$SetupDb = Read-Host "Do you want to setup Contador.Server db? (Y/n)"

if ($SetupDb -eq "Y") {
	$DbName = Read-Host "Database name"
	$DbCredentials = Get-Credential -Message "Database server credentials"
	$DbPasswd = ConvertFrom-SecureString $DbCredentials.Password -AsPlainText
	$DbUserName = $DbCredentials.UserName
	./CreateWebDb.ps1 -DatabaseName $DbName -DatabaseUserName $DbUserName -DatabasePwd $DbPasswd;
}

$SetupSecrets = Read-Host "Do you want to setup dotnet secrets for db connection string? (Y/n)"
if ($SetupSecrets -eq "Y") {
	
	$projectPath = Get-ChildItem -Recurse -Path ../ -Filter Contador.Web.Server.csproj;
	
	dotnet user-secrets clear --project $projectPath
	dotnet user-secrets init --project $projectPath
	
	if ($SetupDb -eq "Y") {
		dotnet user-secrets set "DbCredentials" "user=$DbUserName;password=$DbPasswd;database=$DbName" -p "$($projectPath.FullName)";
	}

	else {
		$DbName = Read-Host "Database name"
		$DbCredentials = Get-Credential -Message "Database server credentials"
		$DbPasswd = ConvertFrom-SecureString $DbCredentials.Password -AsPlainText
		$DbUserName = $DbCredentials.UserName
		
		[void](dotnet user-secrets set "DbCredentials" "user=$DbUserName;password=$DbPasswd;database=$DbName" -p "$($projectPath.FullName)")
	}
}