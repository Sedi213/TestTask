using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;

namespace TestTask.Models
{
    public static class TableMods
    {
        public static string STRING_PATH { get; set; } = "temp.txt";
        public static void ReadDBFile(IFolderDbContext dbContext)
        {
            List<string> data = new List<string>();
            using (StreamReader sr = File.OpenText(STRING_PATH))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    data.Add(s);
                }
            }

            foreach (var item in data)
            {
                string[] arr= item.Split("/./");
                Folder temp = new Folder
                {
                    id = Guid.Parse(arr[0]),
                    name = arr[1],
                    parentId = arr[2]=="" ?  null:Guid.Parse(arr[2]) 
                };
                dbContext.folders.Add(temp);
            }
            dbContext.SaveChangesAsync();

        }
        public static void CreateDBFile(List<Folder> list)
        {
            string data = "";
            foreach (var item in list)
            {
                data += $"{item.id}/./{item.name}/./{item.parentId}\n";
            }

            string fileName = STRING_PATH;
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (FileStream fs = File.Create(fileName))
            {
                Byte[] title = new UTF8Encoding(true).GetBytes(data);
                fs.Write(title, 0, title.Length);
            }

        }
       
        private static void AddSubDir(List<Folder> list,string path,Guid? parentID)
        {
            string[] dirs = System.IO.Directory.GetDirectories(path);

            Folder item = new Folder
            {
                id = Guid.NewGuid(),
                name = path,
                parentId = parentID
            };
            list.Add(item);
            foreach (var d in dirs)
            {
                AddSubDir(list, d,item.id);
            }
        }
        public static List<Folder> ReturnListAllSubdirSystem()
        {
            string testPath = "W:\\Work\\";
            string[] dirs = System.IO.Directory.GetDirectories(testPath);//Test path
            var list = new List<Folder>();
            Folder root = new Folder
            {
                id = Guid.NewGuid(),
                name = testPath,
                parentId = null
            };
            list.Add(root);

            foreach (string d in dirs)
            {
                AddSubDir(list,d, root.id);
            }

            return list;
        }
        public static List<Folder> ReturnDefaultTable()
        {
            List<Folder> list = new List<Folder>();

            Folder cdi = new Folder
            {
                id = Guid.NewGuid(),
                name = "Creating Digital Images",
                parentId = null
            };
            list.Add(cdi);
            Folder res = new Folder
            {
                id = Guid.NewGuid(),
                name = "Resources",
                parentId = cdi.id
            };
            list.Add(res);
            Folder prisour = new Folder
            {
                id = Guid.NewGuid(),
                name = "Prinary Sources",
                parentId = res.id
            };
            list.Add(prisour);
            Folder sesour = new Folder
            {
                id = Guid.NewGuid(),
                name = "Second Sources",
                parentId = res.id
            };
            list.Add(sesour);
            Folder evid = new Folder
            {
                id = Guid.NewGuid(),
                name = "Evidence",
                parentId = cdi.id
            };
            list.Add(evid);
            Folder GP = new Folder
            {
                id = Guid.NewGuid(),
                name = "Graphic Products",
                parentId = cdi.id
            };
            list.Add(GP);
            Folder Pro = new Folder
            {
                id = Guid.NewGuid(),
                name = "Process",
                parentId = GP.id
            };
            list.Add(Pro);
            Folder FP = new Folder
            {
                id = Guid.NewGuid(),
                name = "Final Product",
                parentId = GP.id
            };
            list.Add(FP);



            return list;
        }
    }
}
