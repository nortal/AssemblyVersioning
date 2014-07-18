param($installPath, $toolsPath, $package, $project)

# --- configure names:
$nameForAssemblyInfoFile = "AssemblyInformationalVersion.gen.cs"
$namespaceOfdll = "Nortal.Utilities.AssemblyVersioning"
$nameForDll = $namespaceOfdll + ".dll"
$nameForMSBuildTask = "GenerateExtendedAssemblyInfoTask"
$fullNameForMSBuildTaskFull = $namespaceOfdll + "." + $nameForMSBuildTask
# ---

$project.Save() # in case project had unsaved changes, we don't want to lose them while digging around in xml level.

# --- FIX v0.9 uninstaller bug which did not clean up old import clause.
Write-Host "Checking for need of v0.9 uninstaller fix (import not cleaned up).."
$xml = [XML] (gc $project.FullName)
$nsmgr = New-Object System.Xml.XmlNamespaceManager -ArgumentList $xml.NameTable
$nsmgr.AddNamespace('p',$xml.Project.GetAttribute("xmlns"))

$ourTargetImport = $xml.Project.SelectSingleNode("//p:Import[@Project='_tools\" + $namespaceOfdll + ".targets']", $nsmgr)
if ($ourTargetImport)
{
	Write-Host "Fixing v0.9 uninstaller bug"
	$xml.Project.RemoveChild($ourTargetImport);
}
$xml.Save($project.FullName)
# --- ENDFIX v0.9 uninstaller bug which did not clean up old import clause.


