using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartApp.Models
{
    public class MartItem
    {
        public int ProductId { get; set; } // 기본키, 자동증가, 상품아이디
        public string Product { get; set; } // 상품이름
        public int Price { get; set; } // 가격
        public string Category { get; set; } // 카테고리
        public string Image { get; set; } // 이미지(경로)

    }
}
