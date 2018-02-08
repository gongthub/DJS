
using System;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace EmailToDayReportSumExcel.Utils
{
    public class IniFile
    {
        //声明读写INI文件的API函数
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]

        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);

        public string FileName; //INI文件名

        /// <summary>
        /// 构造函数。
        /// 当指定的INI文件不存在时可选是否建立或抛出异常。
        /// </summary>
        /// <param name="IniFileName">文件名</param>
        /// <param name="ForceCreate">当文件不存在时是否建立</param>
        public IniFile(string IniFileName, bool ForceCreate)
        {
            // 判断文件是否存在
            FileInfo fileInfo = new FileInfo(IniFileName);
            //Todo:搞清枚举的用法
            if ((!fileInfo.Exists))
            {
                if (!ForceCreate)
                {
                    throw (new ApplicationException("Ini文件不存在"));
                }
                fileInfo.Directory.Create();
                fileInfo.Create();
            }
            //必须是完全路径，不能是相对路径
            FileName = fileInfo.FullName;
        }

        #region 各种数据类型的读写
        /// <summary>
        /// 将string型值写入ini。
        /// </summary>
        /// <param name="Section">小节名</param>
        /// <param name="Ident">关键字</param>
        /// <param name="Value">要写入的值</param>
        public void WriteString(string Section, string Ident, string Value)
        {
            if (!WritePrivateProfileString(Section, Ident, Value, FileName))
            {
                throw (new ApplicationException("写Ini文件出错"));
            }
        }

        /// <summary>
        /// 从ini文件中读取string型值。
        /// 当读取失败时返回缺省值。
        /// </summary>
        /// <param name="Section">小节名</param>
        /// <param name="Ident">关键字</param>
        /// <param name="Default">缺省值</param>
        /// <returns>string型值</returns>
        public string ReadString(string Section, string Ident, string Default)
        {
            Byte[] Buffer = new Byte[65535];
            int bufLen = GetPrivateProfileString(Section, Ident, Default, Buffer, Buffer.GetUpperBound(0), FileName);
            //必须设定0（系统默认的代码页）的编码方式，否则无法支持中文
            string s = Encoding.GetEncoding(0).GetString(Buffer);
            s = s.Substring(0, bufLen);
            return s.Trim();
        }
        /// <summary>
        /// 从ini文件中读取int型值。
        /// 当读取失败时返回缺省值。
        /// </summary>
        /// <param name="Section">小节名</param>
        /// <param name="Ident">关键字</param>
        /// <param name="Default">缺省值</param>
        /// <returns>int型值</returns>
        public int ReadInteger(string Section, string Ident, int Default)
        {
            string intStr = ReadString(Section, Ident, Default.ToString());
            try
            {
                return Convert.ToInt32(intStr);
            }
            catch (Exception)
            {
                //Console.WriteLine(ex.Message);
                return Default;
            }
        }

        /// <summary>
        /// 将int型值写入ini
        /// </summary>
        /// <param name="Section">小节名</param>
        /// <param name="Ident">关键字</param>
        /// <param name="Value">要写入的值</param>
        public void WriteInteger(string Section, string Ident, int Value)
        {
            WriteString(Section, Ident, Value.ToString());
        }

        /// <summary>
        /// 从ini文件中读取bool型值。
        /// 当读取失败时返回缺省值。
        /// </summary>
        /// <param name="Section">小节名</param>
        /// <param name="Ident">关键字</param>
        /// <param name="Default">缺省值</param>
        /// <returns>bool型值</returns>
        public bool ReadBool(string Section, string Ident, bool Default)
        {
            try
            {
                return Convert.ToBoolean(ReadString(Section, Ident, Default.ToString()));
            }
            catch (Exception)
            {
                //Console.WriteLine(ex.Message);
                return Default;
            }
        }

        /// <summary>
        /// 将bool型值写入ini
        /// </summary>
        /// <param name="Section">小节名</param>
        /// <param name="Ident">关键字</param>
        /// <param name="Value">要写入的值</param>
        public void WriteBool(string Section, string Ident, bool Value)
        {
            WriteString(Section, Ident, Value.ToString());
        }

        /// <summary>
        /// 将DateTime型值写入ini
        /// </summary>
        /// <param name="Section">小节名</param>
        /// <param name="Ident">关键字</param>
        /// <param name="Value">要写入的值</param>
        public void WriteDateTime(string Section, string Ident, DateTime Value)
        {
            WriteString(Section, Ident, Value.ToString());
        }

        /// <summary>
        /// 从ini文件中读取DateTime型值。
        /// 当读取失败时返回缺省值。
        /// </summary>
        /// <param name="Section">小节名</param>
        /// <param name="Ident">关键字</param>
        /// <param name="Default">缺省值</param>
        /// <returns>DateTime型值</returns>
        public DateTime ReadDateTime(string Section, string Ident, DateTime Default)
        {
            try
            {
                return Convert.ToDateTime(ReadString(Section, Ident, Default.ToString()));
            }
            catch (Exception)
            {
                return Default;
            }
        }

        public void WriteDouble(string Section, string Ident, double Value)
        {
            WriteString(Section, Ident, Value.ToString());
        }

        public double ReadFloat(string Section, string Ident, double Default)
        {
            try
            {
                return Convert.ToDouble(ReadString(Section, Ident, Default.ToString()));
            }
            catch (Exception)
            {
                return Default;
            }
        }
        #endregion
        //从Ini文件中，将指定的Section名称中的所有Ident添加到列表中
        public StringCollection ReadSection(string Section)
        {
            Byte[] Buffer = new Byte[16384];
            StringCollection Idents = new StringCollection();
            //Idents.Clear();
            int bufLen = GetPrivateProfileString(Section, null, null, Buffer, Buffer.GetUpperBound(0),
                                                 FileName);
            //对Section进行解析
            GetStringsFromBuffer(Buffer, bufLen, Idents);
            return Idents;
        }

        private void GetStringsFromBuffer(Byte[] Buffer, int bufLen, StringCollection Strings)
        {
            Strings.Clear();
            if (bufLen != 0)
            {
                int start = 0;
                for (int i = 0; i < bufLen; i++)
                {
                    if ((Buffer[i] == 0) && ((i - start) > 0))
                    {
                        String s = Encoding.GetEncoding(0).GetString(Buffer, start, i - start);
                        Strings.Add(s);
                        start = i + 1;
                    }
                }
            }
        }

        //从Ini文件中，读取所有的Sections的名称
        public StringCollection ReadSections()
        {
            StringCollection SectionList = new StringCollection();
            //Note:必须得用Bytes来实现，StringBuilder只能取到第一个Section
            byte[] Buffer = new byte[65535];
            int bufLen = 0;
            bufLen = GetPrivateProfileString(null, null, null, Buffer,
                                             Buffer.GetUpperBound(0), FileName);
            GetStringsFromBuffer(Buffer, bufLen, SectionList);
            return SectionList;
        }

        //读取指定的Section的所有Value到列表中
        public NameValueCollection ReadSectionValues(string Section)
        {
            NameValueCollection Values = new NameValueCollection();
            StringCollection KeyList = ReadSection(Section);
            Values.Clear();
            foreach (string key in KeyList)
            {
                Values.Add(key, ReadString(Section, key, ""));
            }
            return Values;
        }

        //清除某个Section
        public void EraseSection(string Section)
        {
            //
            if (!WritePrivateProfileString(Section, null, null, FileName))
            {
                throw (new ApplicationException("无法清除Ini文件中的Section"));
            }
        }

        //删除某个Section下的键
        public void DeleteKey(string Section, string Ident)
        {
            WritePrivateProfileString(Section, Ident, null, FileName);
        }
        //Note:对于Win9X，来说需要实现UpdateFile方法将缓冲中的数据写入文件
        //在Win NT, 2000和XP上，都是直接写文件，没有缓冲，所以，无须实现UpdateFile
        //执行完对Ini文件的修改之后，应该调用本方法更新缓冲区。
        public void UpdateFile()
        {
            WritePrivateProfileString(null, null, null, FileName);
        }
        /// <summary>
        /// 检查某个Section是否存在。
        /// </summary>
        /// <param name="Section">小节名</param>
        /// <returns>存在返回true，否则为false。</returns>
        public bool SectionExists(string Section)
        {
            StringCollection Sections = ReadSections();
            return Sections.IndexOf(Section) > -1;
        }
        /// <summary>
        /// 检查某个Section的某个键值是否存在。
        /// </summary>
        /// <param name="Section">小节名</param>
        /// <param name="Ident">关键字</param>
        /// <returns>存在返回true，否则为false。</returns>
        public bool ValueExists(string Section, string Ident)
        {
            //
            StringCollection Idents = ReadSection(Section);
            return Idents.IndexOf(Ident) > -1;
        }

        //确保资源的释放
        ~IniFile()
        {
            UpdateFile();
        }
    }
}
