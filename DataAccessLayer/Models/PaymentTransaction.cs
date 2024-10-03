﻿using DataAccessLayer.Enums;
namespace DataAccessLayer.Models {
    public class PaymentTransaction : BaseEntity {
        public string? VnPayTransactionId { get; set; }
        public string? AccountId { get; set; }
        public double Amount { get; set; }
        public string? Description { get; set; }
        public TransactionStatus? TransactionStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
