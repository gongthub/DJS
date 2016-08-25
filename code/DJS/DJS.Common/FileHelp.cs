using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DJS.Common
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public class FileHelp
    {
        private const string PATH_SPLIT_CHAR = "\\";

        /// <summary>
        /// 数据保存路径
        /// </summary>
        private static string DATAPATH = DJS.Common.FileHelp.GetFullPath(ConfigHelp.DataPathPath);
        /// <summary>
        /// 数据保存文件名称
        /// </summary>
        private static string DATANAME = ConfigHelp.DataNamePath;


        #region 单例模式创建对象
        //单例模式创建对象
        private static FileHelp _fileHelp = null;
        // Creates an syn object.
        private static readonly object SynObject = new object();
        FileHelp()
        {
        }

        public static FileHelp fileHelp
        {
            get
            {
                // Double-Checked Locking
                if (null == _fileHelp)
                {
                    lock (SynObject)
                    {
                        if (null == _fileHelp)
                        {
                            _fileHelp = new FileHelp();
                        }
                    }
                }
                return _fileHelp;
            }
        }
        #endregion

        #region 根据文件相对路径获取绝对路径 +static string GetFullPath(string path)
        /// <summary>
        /// 根据文件相对路径获取绝对路径
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <returns></returns>
        public static string GetFullPath(string path)
        {
            return System.IO.Path.GetFullPath(path);
        }
        #endregion

        #region 返回文件是否存在
        /// <summary>
        /// 返回文件是否存在
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否存在</returns>
        public static bool FileExists(string filename)
        {
            return System.IO.File.Exists(filename);
        }
        #endregion

        #region 判断文件名是否为浏览器可以直接显示的图片文件名
        /// <summary>
        /// 判断文件名是否为浏览器可以直接显示的图片文件名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否可以直接显示</returns>
        public static bool IsImgFilename(string filename)
        {
            filename = filename.Trim();
            if (filename.EndsWith(".") || filename.IndexOf(".") == -1)
            {
                return false;
            }
            string extname = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
            return (extname == "jpg" || extname == "jpeg" || extname == "png" || extname == "bmp" || extname == "gif");
        }
        #endregion

        #region 复制指定目录的所有文件
        /// <summary>
        /// 复制指定目录的所有文件
        /// </summary>
        /// <param name="sourceDir">原始目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="overWrite">如果为true,覆盖同名文件,否则不覆盖</param>
        /// <param name="copySubDir">如果为true,包含目录,否则不包含</param>
        public static void CopyFiles(string sourceDir, string targetDir, bool overWrite, bool copySubDir)
        {
            //复制当前目录文件
            foreach (string sourceFileName in Directory.GetFiles(sourceDir))
            {
                string targetFileName = Path.Combine(targetDir, sourceFileName.Substring(sourceFileName.LastIndexOf(PATH_SPLIT_CHAR) + 1));

                if (File.Exists(targetFileName))
                {
                    if (overWrite == true)
                    {
                        File.SetAttributes(targetFileName, FileAttributes.Normal);
                        File.Copy(sourceFileName, targetFileName, overWrite);
                    }
                }
                else
                {
                    File.Copy(sourceFileName, targetFileName, overWrite);
                }
            }
        }
        #endregion

        #region 移动指定目录的所有文件
        /// <summary>
        /// 移动指定目录的所有文件
        /// </summary>
        /// <param name="sourceDir">原始目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="overWrite">如果为true,覆盖同名文件,否则不覆盖</param>
        /// <param name="moveSubDir">如果为true,包含目录,否则不包含</param>
        public static void MoveFiles(string sourceDir, string targetDir, bool overWrite, bool moveSubDir)
        {
            //移动当前目录文件
            foreach (string sourceFileName in Directory.GetFiles(sourceDir))
            {
                string targetFileName = Path.Combine(targetDir, sourceFileName.Substring(sourceFileName.LastIndexOf(PATH_SPLIT_CHAR) + 1));
                if (File.Exists(targetFileName))
                {
                    if (overWrite == true)
                    {
                        File.SetAttributes(targetFileName, FileAttributes.Normal);
                        File.Delete(targetFileName);
                        File.Move(sourceFileName, targetFileName);
                    }
                }
                else
                {
                    File.Move(sourceFileName, targetFileName);
                }
            }
            if (moveSubDir)
            {
                foreach (string sourceSubDir in Directory.GetDirectories(sourceDir))
                {
                    string targetSubDir = Path.Combine(targetDir, sourceSubDir.Substring(sourceSubDir.LastIndexOf(PATH_SPLIT_CHAR) + 1));
                    if (!Directory.Exists(targetSubDir))
                        Directory.CreateDirectory(targetSubDir);
                    MoveFiles(sourceSubDir, targetSubDir, overWrite, true);
                    Directory.Delete(sourceSubDir);
                }
            }
        }
        #endregion

        #region 删除指定目录的所有文件和子目录
        /// <summary>
        /// 删除指定目录的所有文件和子目录
        /// </summary>
        /// <param name="TargetDir">操作目录</param>
        /// <param name="delSubDir">如果为true,包含对子目录的操作</param>
        public static void DeleteDirectoryFiles(string TargetDir, bool delSubDir)
        {
            foreach (string fileName in Directory.GetFiles(TargetDir))
            {
                File.SetAttributes(fileName, FileAttributes.Normal);
                File.Delete(fileName);
            }
            if (delSubDir)
            {
                DirectoryInfo dir = new DirectoryInfo(TargetDir);
                foreach (DirectoryInfo subDi in dir.GetDirectories())
                {
                    DeleteDirectoryFiles(subDi.FullName, true);
                    subDi.Delete();
                }
            }
        }
        #endregion

        #region 删除指定目录下的指定文件
        /// <summary>
        /// 删除指定目录下的指定文件
        /// </summary>
        /// <param name="TargetFileDir">指定文件的目录</param>
        public static void DeleteFiles(string TargetFileDir)
        { 
            File.Delete(TargetFileDir);
        }
        #endregion

        #region 创建指定目录
        /// <summary>
        /// 创建指定目录
        /// </summary>
        /// <param name="targetDir"></param>
        public static void CreateDirectory(string targetDir)
        {
            DirectoryInfo dir = new DirectoryInfo(targetDir);
            if (!dir.Exists)
                dir.Create();
        }
        #endregion

        #region 建立子目录
        /// <summary>
        /// 建立子目录
        /// </summary>
        /// <param name="parentDir">目录路径</param>
        /// <param name="subDirName">子目录名称</param>
        public static void CreateDirectory(string parentDir, string subDirName)
        {
            CreateDirectory(parentDir + PATH_SPLIT_CHAR + subDirName);
        }
        #endregion

        #region 重命名文件夹
        /// <summary>
        /// 重命名文件夹
        /// </summary>
        /// <param name="OldFloderName">原路径文件夹名称</param>
        /// <param name="NewFloderName">新路径文件夹名称</param>
        /// <returns></returns>
        public static bool ReNameFloder(string OldFloderName, string NewFloderName)
        {
            try
            {
                if (Directory.Exists(GetFullPath("//") + OldFloderName))
                {
                    Directory.Move(GetFullPath("//") + OldFloderName, GetFullPath("//") + NewFloderName);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 删除指定目录
        /// <summary>
        /// 删除指定目录
        /// </summary>
        /// <param name="targetDir">目录路径</param>
        public static void DeleteDirectory(string targetDir)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(targetDir);
            if (dirInfo.Exists)
            {
                DeleteDirectoryFiles(targetDir, true);
                dirInfo.Delete(true);
            }
        }
        #endregion

        #region 检测目录是否存在
        /// <summary>
        /// 检测目录是否存在
        /// </summary>
        /// <param name="StrPath">路径</param>
        /// <returns></returns>
        public static bool DirectoryIsExists(string StrPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(StrPath);
            return dirInfo.Exists;
        }
        /// <summary>
        /// 检测目录是否存在
        /// </summary>
        /// <param name="StrPath">路径</param>
        /// <param name="Create">如果不存在，是否创建</param>
        public static void DirectoryIsExists(string StrPath, bool Create)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(StrPath);
            //return dirInfo.Exists;
            if (!dirInfo.Exists)
            {
                if (Create) dirInfo.Create();
            }
        }
        #endregion

        #region 删除指定目录的所有子目录,不包括对当前目录文件的删除
        /// <summary>
        /// 删除指定目录的所有子目录,不包括对当前目录文件的删除
        /// </summary>
        /// <param name="targetDir">目录路径</param>
        public static void DeleteSubDirectory(string targetDir)
        {
            foreach (string subDir in Directory.GetDirectories(targetDir))
            {
                DeleteDirectory(subDir);
            }
        }
        #endregion

        #region 获取文件最后修改时间
        /// <summary>
        /// 获取文件最后修改时间
        /// </summary>
        /// <param name="FileUrl">文件真实路径</param>
        /// <returns></returns>
        public DateTime GetFileWriteTime(string FileUrl)
        {
            return File.GetLastWriteTime(FileUrl);
        }
        #endregion

        #region 返回指定路径的文件的扩展名
        /// <summary>
        /// 返回指定路径的文件的扩展名
        /// </summary>
        /// <param name="PathFileName">完整路径的文件</param>
        /// <returns></returns>
        public string GetFileExtension(string PathFileName)
        {
            return Path.GetExtension(PathFileName);
        }
        #endregion

        #region 判断是否是隐藏文件
        /// <summary>
        /// 判断是否是隐藏文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public bool IsHiddenFile(string path)
        {
            FileAttributes MyAttributes = File.GetAttributes(path);
            string MyFileType = MyAttributes.ToString();
            if (MyFileType.LastIndexOf("Hidden") != -1) //是否隐藏文件
            {
                return true;
            }
            else
                return false;
        }
        #endregion

        #region 以只读方式读取文本文件
        /// <summary>
        /// 以只读方式读取文本文件
        /// </summary>
        /// <param name="FilePath">文件路径及文件名</param>
        /// <returns></returns>
        public static string ReadTxtFile(string FilePath)
        {
            string content = "";//返回的字符串
            using (FileStream fs = new FileStream(FilePath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
                {
                    string text = string.Empty;
                    while (!reader.EndOfStream)
                    {
                        text += reader.ReadLine() + "\r\n";
                        content = text;
                    }
                }
            }
            return content;
        }
        /// <summary>
        /// 以只读方式读取文本文件前N条数据
        /// </summary>
        /// <param name="FilePath">文件路径及文件名</param>
        /// <returns></returns>
        public static string ReadTxtFileNum(string FilePath, int num)
        {
            string content = "";//返回的字符串
            using (FileStream fs = new FileStream(FilePath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
                {
                    string text = string.Empty;
                    for (int i = 0; i < num; i++)
                    {
                        if (!reader.EndOfStream)
                        {
                            text += reader.ReadLine() + "\r\n";
                            content = text;
                        }
                    }
                }
            }
            return content;
        }
        /// <summary>
        /// 以只读方式读取文本文件后N条数据
        /// </summary>
        /// <param name="FilePath">文件路径及文件名</param>
        /// <returns></returns>
        public static string ReadTxtFileNumE(string FilePath, int num)
        {
            string content = "";//返回的字符串
            try
            {
                if (!IsFileInUse(FilePath))
                {
                    Encoding r = GetType(FilePath);
                    using (FileStream fs = new FileStream(FilePath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(fs, r))
                        {
                            if (reader.ReadLine() != null)
                            {
                                string str = reader.ReadToEnd();
                                string[] aryStr = Regex.Split(str, "\r\n");
                                int allnum = aryStr.Length;
                                if (allnum > num)
                                {
                                    string text = string.Empty;
                                    int startnum = allnum - num;
                                    int j = 1;
                                    for (int i = 0; i < allnum; i++)
                                    {
                                        if (aryStr[allnum - j] != null && aryStr[allnum - j].Trim() != "")
                                        {
                                            startnum -= 1;
                                        }
                                        if (startnum <= i)
                                        {
                                            if (aryStr[allnum - j] != null && aryStr[allnum - j].Trim() != "")
                                            {
                                                text += aryStr[allnum - j] + "\r\n";
                                                content = text;
                                            }
                                            j++;

                                        }
                                    }
                                }
                                else
                                {
                                    int j = 1;
                                    string text = string.Empty;
                                    for (int i = 0; i < allnum; i++)
                                    {
                                        text += aryStr[allnum - j] + "\r\n";
                                        content = text;
                                        j++;
                                    }
                                }
                            }
                        }
                    }
                }
                return content;
            }
            catch (Exception ex)
            {
                LogHelp.logHelp.WriteLog(ex.Message, Model.Enums.LogType.Error);
                return content;
            }
        }
        #endregion

        #region 将内容写入文本文件(如果文件path存在就打开，不存在就新建)
        /// <summary>
        /// 将内容写入文本文件(如果文件path存在就打开，不存在就新建)
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="WriteStr">要写入的内容</param>
        /// <param name="FileModes">写入模式：append 是追加写, CreateNew 是覆盖</param>
        public static void WriteStrToTxtFile(string FilePath, string WriteStr, FileMode FileModes)
        {
            FileStream fst = new FileStream(FilePath, FileModes);
            StreamWriter swt = new StreamWriter(fst, System.Text.Encoding.GetEncoding("utf-8"));
            swt.WriteLine(WriteStr);
            swt.Close();
            fst.Close();
        }
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="namespaces">路径</param>
        /// <param name="WriteStr">写入数据</param>
        public static void WirteStr(string FilePath, string WriteStr)
        {
            try
            {
                //判断文件是否被使用
                if (!IsFileInUse(FilePath))
                {
                    FileInfo finfo = new FileInfo(FilePath);
                    using (FileStream fs = finfo.OpenWrite())
                    {
                        //根据上面创建的文件流创建写数据流 
                        StreamWriter strwriter = new StreamWriter(fs);
                        //设置写数据流的起始位置为文件流的末尾 
                        strwriter.BaseStream.Seek(0, SeekOrigin.End);
                        //写入相关记录信息
                        strwriter.WriteLine(WriteStr);
                        //清空缓冲区内容，并把缓冲区内容写入基础流 
                        strwriter.Flush();
                        strwriter.Close();
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelp.logHelp.WriteLog(ex.Message, Model.Enums.LogType.Error);
            }
        }
        /// <summary>
        /// 写入回调数据文件
        /// </summary>
        /// <param name="namespaces">命名空间名称</param>
        /// <param name="WriteStr">写入数据</param>
        public static void WirteDatas(string namespaces, string WriteStr)
        {
            string files = DATAPATH + @"\" + namespaces + @"\" + DateTime.Now.ToString(DATANAME) + ".dat";
            if (!DirectoryIsExists(DATAPATH))
            {
                CreateDirectory(DATAPATH);
            }
            if (!DirectoryIsExists(DATAPATH + @"\" + namespaces))
            {
                CreateDirectory(DATAPATH + @"\" + namespaces);
            }
            //判断文件是否被使用 
            if (!IsFileInUse(files))
            {
                FileInfo finfo = new FileInfo(files);
                using (FileStream fs = finfo.OpenWrite())
                {
                    //根据上面创建的文件流创建写数据流 
                    StreamWriter strwriter = new StreamWriter(fs);
                    //设置写数据流的起始位置为文件流的末尾 
                    strwriter.BaseStream.Seek(0, SeekOrigin.End);
                    //写入相关记录信息
                    strwriter.WriteLine(WriteStr);
                    //清空缓冲区内容，并把缓冲区内容写入基础流 
                    strwriter.Flush();
                    strwriter.Close();
                    fs.Close();
                }
            }
        }
        #endregion

        #region 获取文件大小并以B，KB，GB，TB方式表示[+2 重载]
        /// <summary>
        /// 获取文件大小并以B，KB，GB，TB方式表示
        /// </summary>
        /// <param name="File">文件(FileInfo类型)</param>
        /// <returns></returns>
        public static string GetFileSize(FileInfo File)
        {
            string Result = "";
            long FileSize = File.Length;
            if (FileSize >= 1024 * 1024 * 1024)
            {
                if (FileSize / 1024 * 1024 * 1024 * 1024 >= 1024) Result = string.Format("{0:############0.00} TB", (double)FileSize / 1024 * 1024 * 1024 * 1024);
                else Result = string.Format("{0:####0.00} GB", (double)FileSize / 1024 * 1024 * 1024);
            }
            else if (FileSize >= 1024 * 1024) Result = string.Format("{0:####0.00} MB", (double)FileSize / 1024 * 1024);
            else if (FileSize >= 1024) Result = string.Format("{0:####0.00} KB", (double)FileSize / 1024);
            else Result = string.Format("{0:####0.00} Bytes", FileSize);
            return Result;
        }

        /// <summary>
        /// 获取文件大小并以B，KB，GB，TB方式表示
        /// </summary>
        /// <param name="FilePath">文件的具体路径</param>
        /// <returns></returns>
        public static string GetFileSize(string FilePath)
        {
            string Result = "";
            FileInfo File = new FileInfo(FilePath);
            long FileSize = File.Length;
            if (FileSize >= 1024 * 1024 * 1024)
            {
                if (FileSize / 1024 * 1024 * 1024 * 1024 >= 1024) Result = string.Format("{0:########0.00} TB", (double)FileSize / 1024 * 1024 * 1024 * 1024);
                else Result = string.Format("{0:####0.00} GB", (double)FileSize / 1024 * 1024 * 1024);
            }
            else if (FileSize >= 1024 * 1024) Result = string.Format("{0:####0.00} MB", (double)FileSize / 1024 * 1024);
            else if (FileSize >= 1024) Result = string.Format("{0:####0.00} KB", (double)FileSize / 1024);
            else Result = string.Format("{0:####0.00} Bytes", FileSize);
            return Result;
        }

        #endregion

        #region 获取指定目录下所有文件夹列表
        /// <summary>
        /// 获取指定目录下所有文件夹列表
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetDirectoryList(string folderFullName)
        {
            ArrayList arry = new ArrayList();
            DirectoryInfo TheFolder = new DirectoryInfo(folderFullName);

            foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
            {
                arry.Add(NextFolder.Name);
            }
            return arry;
        }
        #endregion

        #region 获取指定目录下所有文件列表
        /// <summary>
        /// 获取指定目录下所有文件列表
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetFileslist(string folderFullName)
        {
            ArrayList arry = new ArrayList();
            DirectoryInfo TheFolder = new DirectoryInfo(folderFullName);
            if (DirectoryIsExists(folderFullName))
            {
                FileInfo[] files = TheFolder.GetFiles();
                if (files != null && files.Count() > 0)
                {
                    foreach (FileInfo NextFile in files)
                    {
                        arry.Add(NextFile.Name);
                    }
                }
            }
            return arry;
        }
        #endregion

        #region 判断文件是否正在被使用 +static bool IsFileInUse(string fileName)
        /// <summary>
        /// 判断文件是否正在被使用
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsFileInUse(string fileName)
        {
            bool inUse = true;
            if (File.Exists(fileName))
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                    inUse = false;
                }
                catch (Exception e)
                {
                    LogHelp.logHelp.WriteLog(e.Message, Model.Enums.LogType.Error);
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
                return inUse;           //true表示正在使用,false没有使用
            }
            else
            {
                return false;           //文件不存在则一定没有被使用
            }
        }
        #endregion

        #region 通过给定的文件流，判断文件的编码类型
        /// <summary> 
        /// 通过给定的文件流，判断文件的编码类型 
        /// </summary> 
        /// <param name=“fs“>文件流</param> 
        /// <returns>文件的编码类型</returns> 
        public static System.Text.Encoding GetType(string FilePath)
        {
            Encoding reVal = Encoding.Default;
            if (!IsFileInUse(FilePath))
            {
                using (FileStream fs = new FileStream(FilePath, FileMode.Open))
                {
                    byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
                    byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
                    byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM

                    BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
                    int i;
                    int.TryParse(fs.Length.ToString(), out i);
                    byte[] ss = r.ReadBytes(i);
                    if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
                    {
                        reVal = Encoding.UTF8;
                    }
                    else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
                    {
                        reVal = Encoding.BigEndianUnicode;
                    }
                    else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
                    {
                        reVal = Encoding.Unicode;
                    }
                    r.Close();
                }
            }
            return reVal;
        }



        /// <summary>
        /// 判断是否是不带 BOM 的 UTF8 格式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1; //计算当前正分析的字符应还有的字节数
            byte curByte; //当前分析的字节.
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }

        #endregion


    }
}
