using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
using System.Linq;

public class SwiftPostProcess
{
    private static PBXProject pbxProject;
    private static string pbxGUID;

    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string physicalPath)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            pbxProject = new PBXProject();

            string pbxjPath = PBXProject.GetPBXProjectPath(physicalPath);
            pbxProject.ReadFromString(File.ReadAllText(pbxjPath));
            pbxGUID = pbxProject.TargetGuidByName(PBXProject.GetUnityTargetName());

            var fileName1 = "Record.storyboard";
            var fileName2 = "IconReturn.png";
            var fileName3 = "IconRecord.png";
            var unityPath1 = "Plugins/iOS/SwiftPlugin/Source/";
            CopyFileToBuild(fileName1, unityPath1, physicalPath);
            CopyFileToBuild(fileName2, unityPath1, physicalPath);
            CopyFileToBuild(fileName3, unityPath1, physicalPath);

            pbxProject.SetBuildProperty(pbxGUID, "ENABLE_BITCODE", "NO");
            //swift要使用ojbc的class時用到
            pbxProject.SetBuildProperty(pbxGUID, "SWIFT_OBJC_BRIDGING_HEADER", "Libraries/Plugins/iOS/SwiftPlugin/Source/SwiftPlugin-Bridging-Header.h");
            //ojbc要使用swift class時用到
            pbxProject.SetBuildProperty(pbxGUID, "SWIFT_OBJC_INTERFACE_HEADER_NAME", "SwiftPlugin-Swift.h");
            //上面兩個設定似乎要讓unity認得swift檔案，
            //因為在原生objc的專案，在.m檔案中import porject name-swift 會認得swift的方式, 在unity輸出的xcode專案會無效

            pbxProject.AddBuildProperty(pbxGUID, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks $(PROJECT_DIR)/lib/$(CONFIGURATION) $(inherited)");
            //proj.AddBuildProperty(targetGuid, "FRAMERWORK_SEARCH_PATHS", "$(inherited) $(PROJECT_DIR) $(PROJECT_DIR)/Frameworks");
            //proj.AddBuildProperty(targetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
            //proj.AddBuildProperty(targetGuid, "DYLIB_INSTALL_NAME_BASE", "@rpath");
            //proj.AddBuildProperty(targetGuid, "LD_DYLIB_INSTALL_NAME", "@executable_path/../Frameworks/$(EXECUTABLE_PATH)");
            pbxProject.AddBuildProperty(pbxGUID, "DEFINES_MODULE", "YES");
            pbxProject.AddBuildProperty(pbxGUID, "SWIFT_VERSION", "4.0");

            //pbxProject.AddFrameworkToProject(pbxGUID, "CoreMedia.framework", weak: false);
            //iOS資料夾裡的 *.framework會自動linked並required

            //xib檔unity會自動匯入，storyboard檔不會

            pbxProject.WriteToFile(pbxjPath);
        }
    }

    private static void CopyFileToBuild(string fileName, string unityPath, string physicalPath)
    {
        CopyRawFileToXCode(fileName, unityPath, physicalPath);
        string fileGUID = pbxProject.AddFile(physicalPath + "/Libraries/" + unityPath + fileName, "Libraries/" + unityPath + fileName, PBXSourceTree.Source);
        pbxProject.AddFileToBuild(pbxGUID, fileGUID);
    }

    private static void CopyRawFileToXCode(string fileName, string unityPath, string physicalPath)
    {
        var srcPath = Path.Combine("Assets/" + unityPath, fileName);
        var desPath = Path.Combine(physicalPath + "/Libraries/" + unityPath, fileName);
        File.Copy(srcPath, desPath, true);
    }

    public static void CopyDirectory(string srcPath, string dstPath, string[] excludeExtensions, bool overwrite = true)
    {
        if (!Directory.Exists(dstPath))
            Directory.CreateDirectory(dstPath);

        foreach (var file in Directory.GetFiles(srcPath, "*.*", SearchOption.TopDirectoryOnly).Where(path => excludeExtensions == null || !excludeExtensions.Contains(Path.GetExtension(path))))
        {
            File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)), overwrite);
        }

        foreach (var dir in Directory.GetDirectories(srcPath))
            CopyDirectory(dir, Path.Combine(dstPath, Path.GetFileName(dir)), excludeExtensions, overwrite);
    }

}
