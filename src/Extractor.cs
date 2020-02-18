using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System.IO;

namespace ExtractUnityPackage
{
    internal static class Extractor
    {
        public static void ImportUnityPackage(string packagePath, string destinationFolderPath, bool overwrite)
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), Path.GetFileName(packagePath) + ".unitypackage.extract");

            using (var i = new FileStream(packagePath, FileMode.Open, FileAccess.Read))
            using (var gi = new GZipInputStream(i))
            using (var tar = TarArchive.CreateInputTarArchive(gi))
            {
                tar.ExtractContents(tempFolder);
            }

            foreach (var item in Directory.GetDirectories(tempFolder))
            {
                var pathname = File.ReadAllText(Path.Combine(item, "pathname"));
                var path = Path.Combine(destinationFolderPath, pathname);

                var assetPath = Path.Combine(item, "asset");
                if (File.Exists(assetPath))
                {
                    var folder = Path.GetDirectoryName(Path.GetFullPath(path));
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                    if (overwrite || !File.Exists(path)) File.Copy(assetPath, path, true);
                }
                else
                {
                    var folder = path;
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                }
                var assetMetaPath = Path.Combine(item, "asset.meta");
                var metaPath = path + ".meta";
                if (overwrite || !File.Exists(metaPath)) File.Copy(assetMetaPath, metaPath, true);
            }

            Directory.Delete(tempFolder, true);
        }
    }
}
