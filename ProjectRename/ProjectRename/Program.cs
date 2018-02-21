using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectRename
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var replaceableFile = new List<string>() { ".cs", ".csproj", ".sln", ".fsx", ".bat", ".md", ".txt", ".nuspec", ".fs" };

            var avoidFile = new List<string>() {  };


            const string sourceDir = @"D:\x\y";

            var replaceMents = new List<Replacement>()
            {
                new Replacement("a","b")
            };

            var directories = DirSearch(sourceDir);

            foreach (var directory in directories)
            {
                Console.WriteLine($"Starting In DIRECTORY {directory} ... ");
                foreach (string f in Directory.GetFiles(directory))
                {

                    Console.WriteLine($"In DIRECTORY {f} ... ");
                    string destinationFile = null;
                    replaceMents.ForEach(
                        r =>
                        {
                            destinationFile = (destinationFile ?? f).Replace(r.Find, r.Replace);
                        });
                    if (!avoidFile.Any(x => destinationFile.EndsWith(x)))
                    {
                        if (!System.IO.File.Exists(destinationFile))
                        {
                            DirectoryInfo destDir = new DirectoryInfo(Path.GetDirectoryName(destinationFile));
                            destDir.Create();
                        }
                        System.IO.File.Copy(f, destinationFile, true);
                        if (replaceableFile.Any(x => destinationFile.EndsWith(x)))
                        {
                            Console.WriteLine($"Replacing FILE  {destinationFile} ... ");
                            string str = File.ReadAllText(destinationFile);
                            replaceMents.ForEach(
                                r =>
                                {
                                    str = str.Replace(r.Find, r.Replace);
                                });

                            File.WriteAllText(destinationFile, str);
                        }
                    }

                }
            }
        }

        private static List<string> DirSearch(string dir, List<string> existingDir = null)
        {
            existingDir = existingDir ?? new List<string>() { dir };
            try
            {
                foreach (string d in Directory.GetDirectories(dir))
                {
                    existingDir.Add(d);
                    existingDir = DirSearch(d, existingDir);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
            return existingDir;
        }
    }

    internal class Replacement
    {
        public Replacement(string find, string replace)
        {
            this.Find = find;
            this.Replace = replace;
        }

        public string Find { get; set; }

        public string Replace { get; set; }
    }
}
