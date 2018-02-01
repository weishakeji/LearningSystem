using System;
using System.Xml;
using System.IO;
using System.Collections;
using System.Text;
using System.Web;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.GZip;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace Song.Extend
{
    public class Compress
    {
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="strFileFolder"></param>
        /// <param name="strZip"></param>
        public static void ZipFile(string strFileFolder, string strZip)
        {
            if (strFileFolder[strFileFolder.Length - 1] != Path.DirectorySeparatorChar)
                strFileFolder += Path.DirectorySeparatorChar;
            ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(strZip));
            s.SetLevel(6); // 0 - store only to 9 - means best compression
            zip(strFileFolder, strZip, s);
            s.Finish();
            s.Close();
        }
        private static void zip(string strFile, string StoreFile, ZipOutputStream s)
        {
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
                strFile += Path.DirectorySeparatorChar;
            Crc32 crc = new Crc32();
            string[] filenames = Directory.GetFileSystemEntries(strFile);
            foreach (string file in filenames)
            {
                //如果本目录内有正在被压缩的文件，跳过
                if (file == StoreFile) continue;
                if (Directory.Exists(file))
                {
                    zip(file, StoreFile, s);

                }
                else
                {
                    //打开压缩文件,开始压缩
                    FileStream fs = System.IO.File.OpenRead(file);
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    string tempfile = file.Substring(StoreFile.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(tempfile);
                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    s.PutNextEntry(entry);
                    s.Write(buffer, 0, buffer.Length);
                }
            }
        }

        

        /// <summary>   
        /// 解压缩ZIP文件到指定文件夹   
        /// </summary>   
        /// <param name="zipfileName">ZIP文件</param>   
        /// <param name="UnZipDir">解压文件夹</param>   
        /// <param name="password">压缩文件密码</param>   
        public static void UnZipFile(string zipfileName, string UnZipDir, string password)
        {

            //zipfileName=@"\Storage Card\PDADataExchange\receive\zip\test.zip";
            //UnZipDir= @"\Storage Card\PDADataExchange\receive\xml\";
            //password="";

            ZipInputStream s = new ZipInputStream(System.IO.File.OpenRead(zipfileName));
            if (password != null && password.Length > 0)
                s.Password = password;
            try
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(UnZipDir);
                    string pathname = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    //生成解压目录    
                    pathname = pathname.Replace(":", "$");//处理压缩时带有盘符的问题   
                    directoryName = directoryName + "\\" + pathname;
                    Directory.CreateDirectory(directoryName);

                    if (fileName != String.Empty)
                    {
                        //解压文件到指定的目录   
                        FileStream streamWriter = System.IO.File.Create(directoryName + "\\" + fileName);

                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                        streamWriter.Close();
                    }
                }
                s.Close();
            }
            catch (Exception eu)
            {
                throw eu;
            }
            finally
            {
                s.Close();
            }

        }

    }
}
