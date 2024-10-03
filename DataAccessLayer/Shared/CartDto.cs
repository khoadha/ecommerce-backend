using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataAccessLayer.Shared {
    public class GetCartDto {
        public int Id { get; set; }
        public List<GetCartItemDto> Items { get; set; }
    }

    public class GetCartItemDto {
        public int Id { get; set; }
        public GetProductDto Product { get; set; }
        public int Quantity { get; set; }
        public decimal Cost {
            get { return (decimal)(Product.Price * Quantity); }
        }
    }

}
