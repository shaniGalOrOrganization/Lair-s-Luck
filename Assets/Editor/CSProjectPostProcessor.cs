using UnityEditor;
using System.IO;

public class CSProjectPostProcessor : AssetPostprocessor
{
    private static void OnGeneratedCSProjectFiles()
    {
        string projectPath = Path.Combine(Directory.GetCurrentDirectory(), "Assembly-CSharp.csproj");
        if (File.Exists(projectPath))
        {
            string content = File.ReadAllText(projectPath);
            if (!content.Contains("<RestoreProjectStyle>PackageReference</RestoreProjectStyle>"))
            {
                // Add RestoreProjectStyle to the first PropertyGroup
                content = content.Replace(
                    "<PropertyGroup>",
                    "<PropertyGroup>\n  <RestoreProjectStyle>PackageReference</RestoreProjectStyle>"
                );
                File.WriteAllText(projectPath, content);
                UnityEngine.Debug.Log("Modified Assembly-CSharp.csproj to include RestoreProjectStyle.");
            }
        }
    }
}

