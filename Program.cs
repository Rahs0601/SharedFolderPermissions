using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace SharedFolderPermissions
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter the path of the root folder:");
                string rootFolderPath = Console.ReadLine();
                //read or read write
                Console.WriteLine("What permission do you want to grant read or read-write? (r / rw)");
                string permission = Console.ReadLine();
                string toprint = permission == "r" ? "read" : "read/write";
                Console.WriteLine("Enter the folder name you want to modify to " + toprint + ":");
                string target = Console.ReadLine();

                Console.WriteLine("Enter the user-name:");
                string user = Console.ReadLine();

                foreach (string folderPath in Directory.GetDirectories(rootFolderPath, target, SearchOption.AllDirectories))
                {
                    if (Directory.Exists(folderPath))
                    {

                        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
                        DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();

                        string userName = user;
                        NTAccount userAccount = new NTAccount(userName);
                        SecurityIdentifier userIdentifier = (SecurityIdentifier)userAccount.Translate(typeof(SecurityIdentifier));

                        FileSystemAccessRule accessRule = new FileSystemAccessRule(
                            userIdentifier,
                            permission == "r" ? FileSystemRights.Read : FileSystemRights.Read | FileSystemRights.Write,
                            InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                            PropagationFlags.None,
                            AccessControlType.Allow
                        );

                        directorySecurity.AddAccessRule(accessRule);

                        directoryInfo.SetAccessControl(directorySecurity);
                        Console.WriteLine("Permission granted to " + folderPath);


                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}