using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class PostBuildServerCopy
{
    [PostProcessBuildAttribute(1)]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        Debug.Log($"Project build successfully! Target: {target} | Path: {pathToBuiltProject}");
        var projectServerPath = Application.dataPath;
        projectServerPath = projectServerPath.Substring(0, projectServerPath.Length - "Assets".Length); // this gives: ProjectPath/ (in which exists Assets folder)
        projectServerPath += "Server"; // ProjectPath/Server
        var buildServerPath = pathToBuiltProject.Substring(0, pathToBuiltProject.LastIndexOf('/') + 1); // this gives: BuildPath/ (in which exists project .exe)
        buildServerPath += "Server";
        Debug.Log(projectServerPath);
        Debug.Log(buildServerPath);
        CopyFilesRecursively(new DirectoryInfo(projectServerPath), new DirectoryInfo(buildServerPath));
    }

    private static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
    {
        foreach (DirectoryInfo dir in source.GetDirectories())
            CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
        foreach (FileInfo file in source.GetFiles())
            file.CopyTo(Path.Combine(target.FullName, file.Name), true);
    }
}
