param(
  [Parameter(Mandatory=$true)][string]$DatabaseName,
  [Parameter(Mandatory=$true)][string]$DatabaseUsername,
  [Parameter(Mandatory=$true)][string]$DatabasePassword
)
$SQLTables = Get-ChildItem Tables -Filter "*.sql" -Recurse
$SQLProcedures = Get-ChildItem StoredProcedures -Filter "*.sql" -Recurse
Write-Host "Creating or replacing database..."
mysql -u $DatabaseUsername --password=$DatabasePassword -e "CREATE or REPLACE DATABASE $DatabaseName"
foreach ($file in $SQLTables) {
  Write-Host "Updating database with file: $($file.Name)..."
  mysql -u $DatabaseUsername --password=$DatabasePassword $DatabaseName -e "source $file"
}

foreach ($file in $SQLProcedures) {
  Write-host "Updating database with file $($file.Name)..."
  mysql -u $DatabaseUsername --password=$DatabasePassword $DatabaseName -e "source $file"
}

