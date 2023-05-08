using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartApp.Models
{
    public class User
    {
        public string Id { get; set; } // 기본키, 고객아이디
        public string Name { get; set; } // 고객명
        public string PassWord { get; set; } // 비밀번호
        public string PhoneNum { get; set; } // 휴대폰번호
    }
}
