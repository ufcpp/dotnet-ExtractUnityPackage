using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;

namespace ExtractUnityPackage
{
    [Command(Description = "A global tool for extracting .unitypackage", Name = "dotnet ExtractUnityPackage")]
    public class Program
    {
        [Argument(0, Description = "source .unitypackage path")]
        [Required]
        public string Package { get; }

        [Argument(1, Description = "destination folder path")]
        [Required]
        public string Destination { get; }

        [Option("-o|--overwrite", Description = "overwrites if file exists")]
        public bool Overwrite { get; }

        static void Main(string[] args)
        {
            CommandLineApplication.Execute<Program>(args);
        }

        public void OnExecute()
        {
            Extractor.ImportUnityPackage(Package, Destination, Overwrite);
        }
    }
}
