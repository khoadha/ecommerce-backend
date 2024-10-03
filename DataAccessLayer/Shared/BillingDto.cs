using DataAccessLayer.Enums;

namespace DataAccessLayer.Shared {
    public class CreateBillingRequestDto {
        public int StoreId { get; set; }
        public double TotalBill { get; set; }
    }
}
