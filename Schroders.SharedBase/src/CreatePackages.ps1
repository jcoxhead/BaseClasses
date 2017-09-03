param(
	[string]$outputDirectory = "C:\packages"
)

Function CreatePackages($outputDirectory)
{
	## Create Directory if does not exist
	New-Item -ItemType Directory -Force -Path ($outputDirectory)

	Get-ChildItem -Directory |  ForEach-Object {
		Write-Host $_.FullName
		cd $_.FullName
		dotnet pack --version-suffix=alpha -o ($outputDirectory)
		cd ..
	}
}


CreatePackages $outputDirectory