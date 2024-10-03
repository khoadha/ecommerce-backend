using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Shared {
    public class CalculateFeeRequestDto {

        public int from_district_id { get; set; }
        public string from_ward_code { get; set; }
        public int to_district_id { get;set; }
        public string to_ward_code { get; set; }
        public int service_id { get; set; }
        public int weight { get; set; }

    }
}
