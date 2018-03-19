using System;
using System.IO;
using System.Net.Http;

namespace DJS.Core.Common.Net
{
    public class HttpHelp
    {
        /// <summary>
        /// 下载并保存
        /// </summary>
        /// <param name="url">网络路径</param>
        /// <param name="savePath">保存本地的文件夹</param>
        public static void FileDownSave(string url, string savePath, out string filePath)
        {
            try
            {
                filePath = string.Empty;
                HttpClient httpClient = new HttpClient();
                if (!string.IsNullOrWhiteSpace(url))
                {
                    string[] strArry = url.Split('/');
                    savePath = savePath + "/" + strArry[strArry.Length - 1];
                }
                var t = httpClient.GetByteArrayAsync(url);
                t.Wait();
                using (Stream responseStream = new MemoryStream(t.Result))
                {
                    using (Stream stream = new FileStream(savePath, FileMode.Create))
                    {
                        byte[] bArr = new byte[1024];
                        int size = responseStream.Read(bArr, 0, bArr.Length);
                        while (size > 0)
                        {
                            stream.Write(bArr, 0, size);
                            size = responseStream.Read(bArr, 0, bArr.Length);
                        }
                        stream.Close();
                        responseStream.Close();
                        filePath = savePath;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
