param($installPath, $toolsPath, $package, $project)

# --- configure names:
$nameForAssemblyInfoFile = "AssemblyInformationalVersion.gen.cs"
$namespaceOfdll = "Nortal.Utilities.AssemblyVersioning"
$nameForDll = $namespaceOfdll + ".dll"
$nameForMSBuildTask = "GenerateExtendedAssemblyInfoTask"
$fullNameForMSBuildTaskFull = $namespaceOfdll + "." + $nameForMSBuildTask
# ---

#set the BuildAction to NONE for versioning tool assembly.
Write-Host "Setting _tools/dll to BuildAction=None .."
$project.ProjectItems.Item("_tools").ProjectItems.Item($nameForDll).Properties.Item("BuildAction").Value = 0
$project.ProjectItems.Item("_tools").ProjectItems.Item($namespaceOfdll + ".targets").Properties.Item("BuildAction").Value = 0
$project.ProjectItems.Item("_tools").ProjectItems.Item($namespaceOfdll + ".props").Properties.Item("BuildAction").Value = 0

$project.Save()

$xml = [XML] (gc $project.FullName)
$nsmgr = New-Object System.Xml.XmlNamespaceManager -ArgumentList $xml.NameTable
$nsmgr.AddNamespace('p',$xml.Project.GetAttribute("xmlns"))


# --- TASK: import our MSBuild project extension
# P.S.: note that nuget 2.5 will have better tooling to work with imports but unfortunately seems not to work with custom package repository and/or package restore.
Write-Host "Including MSBuild extensions to project file.."
$ourTargetImport = $xml.Project.SelectSingleNode("//p:Import[@Project='_tools\" + $namespaceOfdll + ".targets']", $nsmgr)
if (!$ourTargetImport)
{
	$ourTargetImport = $xml.CreateElement("Import", $xml.Project.GetAttribute("xmlns"))
	$ourTargetImport.SetAttribute("Project", "_tools\" + $namespaceOfdll + ".targets")
	$ourTargetImportInsertionPoint = $xml.Project.Import | where {$_.Project -like "*Microsoft.Common.props"} |select -first 1
	if ($ourTargetImportInsertionPoint) 
	{
		$xml.Project.InsertAfter($ourTargetImport, $ourTargetImportInsertionPoint)
	}
	else
	{
		$xml.Project.appendChild($ourTargetImport)
	}
}

$xml.Save($project.FullName)
