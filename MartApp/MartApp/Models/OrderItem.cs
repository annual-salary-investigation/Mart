using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartApp.Models
{
    public class OrderItem
    {
        public bool IsSelected { get; set; } // 체크박스
        public int ProductId { get; set; } // mart테이블 외래키
        public string Id { get; set; } // user테이블 외래키
        public string Product { get; set; }
        public int Price { get; set; }
        public int Count {set; get; } // 수량
        public string Category { get; set; }
        public string Image { get; set; }
        public DateTime DateTime { get; set; } // 주문시간
        public bool Chedcked { get; set; } // 결제 유무 파악을 위함
    }
}
