param(
  [Parameter(Mandatory=$true)][string]$DatabaseName,
  [Parameter(Mandatory=$true)][string]$DatabaseUsername,
  [Parameter(Mandatory=$true)][securestring]$DatabasePassword
)
$DbPassword = ConvertFrom-SecureString $DatabasePassword -AsPlainText

$SQLTables = Get-ChildItem "../src/Contador.Web/Database/dbo/Tables" -Filter "*.sql" -Recurse
$SQLProcedures = Get-ChildItem "../src/Contador.Web/Database/dbo/StoredProcedures" -Filter "*.sql" -Recurse
Write-Host "Creating or replacing database..."
mysql -u $DatabaseUsername --password=$DbPassword -e "CREATE or REPLACE DATABASE $DatabaseName"
foreach ($file in $SQLTables) {
  Write-Host "Updating database with file: $($file.Name)..."
  mysql -u $DatabaseUsername --password=$DbPassword $DatabaseName -e "source $file"
}

foreach ($file in $SQLProcedures) {
  Write-host "Updating database with file $($file.Name)..."
  mysql -u $DatabaseUsername --password=$DbPassword $DatabaseName -e "source $file"
}

