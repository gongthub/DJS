﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Model
{
    public class UserEntity : BaseEntity<UserEntity>
    {
        public string Account { get; set; }
        public string RealName { get; set; }
        public string NickName { get; set; }
        public string HeadIcon { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string WeChat { get; set; }
        public string Password { get; set; }
        public bool? IsAdministrator { get; set; }
        public int? SortCode { get; set; }
        public bool? EnabledMark { get; set; }
        public string Description { get; set; }
    }
}
