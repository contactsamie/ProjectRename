using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRename
{
    class Program
    {
        static void Main(string[] args)
        {
            var replaceableFile=new List<string>() {".cs",".csproj",".sln",".fsx",".bat",".md",".txt",".nuspec"};

            var directories = DirSearch(sourceDir);
           
            foreach (var directory in directories)
            {
                foreach (string f in Directory.GetFiles(directory))
                {
                    var destinationFile = f.Replace(find, replace );
                    destinationFile = destinationFile.Replace(secondFind, replace);

                    if (!System.IO.File.Exists(destinationFile))
                    {
                        DirectoryInfo destDir = new DirectoryInfo(Path.GetDirectoryName(destinationFile));
                        destDir.Create();
                    }
                 
                    //  new FileInfo(f).CopyTo(destinationFile);
                    System.IO.File.Copy(f, destinationFile,true);


                    if (replaceableFile.Any(x => destinationFile.EndsWith(x)))
                    {
                        string str = File.ReadAllText(destinationFile);
                        str = str.Replace(find,replace);
                        str = str.Replace(secondFind, replace);
                        File.WriteAllText(destinationFile, str);
                    }
                   
                }
            }
        }

        private static List<string> DirSearch(string dir,List<string> existingDir=null )
        {
            existingDir = existingDir ?? new List<string>() {dir};
            try
            {
                foreach (string d in Directory.GetDirectories(dir))
                {
                    existingDir.Add(d);
                    existingDir= DirSearch(d, existingDir);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
            return existingDir;
        }
    }
}
