using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Common.CommonModel
{
    public class UpFileDTO
    {
        /// <summary>
        /// Sys_FileName
        /// </summary>		
        public string Sys_FileName { get; set; }

        /// <summary>
        /// Sys_FileOldName
        /// </summary>		
        public string Sys_FileOldName { get; set; }

        /// <summary>
        /// Sys_ExtName
        /// </summary>		
        public string Sys_ExtName { get; set; }

        /// <summary>
        /// Sys_FilePath
        /// </summary>		
        public string Sys_FilePath { get; set; }
        /// <summary>
        /// IsNoFile
        /// </summary>		
        public bool IsNoFile { get; set; }
    }
}
