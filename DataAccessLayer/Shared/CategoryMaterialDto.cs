using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Shared {
    public class GetCategoryDto {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class GetMaterialDto {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AddCategoryDto {
        public string Name { get; set; }
    }

    public class AddMaterialDto {
        public string Name { get; set; }
    }
}
