using System;
using System.IO;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public static class IOServices
    {
        #region Static methods

        public static string CombinePath(params string[] paths)
        {
            return Path.Combine(paths);
        }

        public static string GetAbsolutePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return Environment.CurrentDirectory;
            }

            return Path.GetFullPath(path);
        }

        public static string GetRelativePath(string referencePath, string path)
        {
            // folders must end in a slash
            string  directorySeparatorChar  = Path.DirectorySeparatorChar.ToString();
            if (!referencePath.EndsWith(directorySeparatorChar))
            {
                referencePath      += Path.DirectorySeparatorChar;
                // hack to stop getting unusual results
                if (referencePath.StartsWith(directorySeparatorChar))
                {
                    referencePath += "a.tmp";
                }
            }

            var     referenceUri    = new Uri(referencePath);
            var     pathUri         = new Uri(path);
            var     relativeUri     = referenceUri.MakeRelativeUri(pathUri);
            return Uri.UnescapeDataString(relativeUri.ToString().Replace('/', Path.DirectorySeparatorChar));
        }
        
        #endregion

        #region Directory operations

        public static bool IsDirectory(string path)
        {
            var     attr    = File.GetAttributes(path);
            return ((attr & FileAttributes.Directory) == FileAttributes.Directory);
        }

        public static bool CreateDirectory(string path, bool overwrite = false)
        {
            if (DirectoryExists(path))
            {
                if (!overwrite)
                {
                    return false;
                }

                DeleteDirectory(path, true);
            }

            Directory.CreateDirectory(path);
            return true;
        }

        public static void DeleteDirectory(string path, bool recursive = true, bool throwError = false)
        {
            var     directoryInfo   = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                if (throwError)
                {
                    throw new FileNotFoundException();
                }
                return;
            }

            directoryInfo.Delete(recursive);
        }

        public static void ClearDirectory(string path)
        {
            var     directoryInfo   = new DirectoryInfo(path);
            if (directoryInfo.Exists)
            {
                foreach (var file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }
                foreach (var dir in directoryInfo.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
        }

        public static string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        public static void CopyDirectory(string root, string dest, bool recursive, string filePattern = null)
        {
            // create sub folders
            if (recursive)
            {
                foreach (var directory in Directory.GetDirectories(root))
                {
                    string  dirName = Path.GetFileName(directory);
                    if (!Directory.Exists(Path.Combine(dest, dirName)))
                    {
                        Directory.CreateDirectory(Path.Combine(dest, dirName));
                    }
                    CopyDirectory(directory, Path.Combine(dest, dirName), recursive, filePattern);
                }
            }

            // copy files
            var     files   = (filePattern == null) ? Directory.GetFiles(root) : Directory.GetFiles(root, filePattern);
            foreach (var file in files)
            {
                File.Copy(file, Path.Combine(dest, Path.GetFileName(file)));
            }
        }

        public static void MoveDirectory(string source, string destination)
        {
            try
            {
                var     parent  = Directory.GetParent(destination);
                CreateDirectory(parent.FullName);

                Directory.Move(source, destination);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public static bool IsSubDirectory(string parent, string path)
        {
            var parentUri = new Uri(GetUriSafePath(parent));
            var     childUri    = new Uri(GetUriSafePath(Path.GetDirectoryName(path)));
            return (parentUri != childUri) && parentUri.IsBaseOf(childUri);
        }

        private static string GetUriSafePath(string path)
        {
            if(IsDirectory(path))
            {
                return Path.Combine(path, $"{Path.PathSeparator}");
            }

            return path;
        }

        public static string GetUniquePath(string path)
        {
            string  uniquePath      = path;
            int     tryCount        = 0;
            while (Directory.Exists(uniquePath))
            {
                uniquePath          = string.Format("{0}{1}", path, ++tryCount);
            }

            return uniquePath;
        }

        public static string GenerateFileName(string prefix, string extension)
        {
            return $"{prefix}_{DateTime.Now:yyyyMMddHHmmssfff}.{extension}";
        }

        #endregion

        #region File operations

        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public static string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public static void CreateFile(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }

        public static void CreateFile(string path, byte[] contents)
        {
            File.WriteAllBytes(path, contents);
        }

        public static string ReadFile(string path)
        {
            return File.ReadAllText(path);
        }

        public static byte[] ReadFileData(string path)
        {
            return File.ReadAllBytes(path);
        }

        public static void CopyFile(string source, string destination, bool overwrite = true)
        {
            File.Copy(source, destination, overwrite);
        }

        public static void MoveFile(string source, string destination)
        {
            if(File.Exists(source))
            {
                File.Move(source, destination);
            }
        }

        public static void DeleteFile(string path, bool throwError = false)
        {
            if (!FileExists(path))
            {
                if (throwError)
                {
                    throw new FileNotFoundException();
                }
                return;
            }

            File.Delete(path);
        }

        #endregion

        #region Hybrid methods

        public static void DeleteFileOrDirectory(string path, bool throwError = false)
        {
            var     fileInfo    = new FileInfo(path);
            if (fileInfo.Exists)
            {
                if ((fileInfo.Attributes & FileAttributes.Directory) != 0)
                {
                    DeleteDirectory(path, true, throwError);
                }
                else
                {
                    DeleteFile(path, throwError);
                }
            }
        }

        public static string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        public static string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }

        #endregion
    }
}
