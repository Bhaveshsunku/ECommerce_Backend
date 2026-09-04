param(
	[string]$Name = "InitialCreate",
	[string]$Project = "ECommerce.DataAccess",
	[string]$StartupProject = "ECommerce.API"
)

Write-Host "Running: dotnet ef migrations add $Name --project $Project --startup-project $StartupProject"

dotnet ef migrations add $Name --project $Project --startup-project $StartupProject
