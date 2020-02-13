using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiveAidCharity.Models.Main
{
    public class Donation
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("Project")]
        public string ProjectId { get; set; }
        public virtual Project Project { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Amount { get; set; }

        public PaymentMethodEnum PaymentMethod { get; set; }
        public enum PaymentMethodEnum
        {
            Paypal = 0,
            VnPay = 1,
            DirectBankTransfer = 2
        }
        public DonationStatusEnum Status { get; set; }
        public enum DonationStatusEnum
        {
            Pending = 0,
            Success = 1,
            Fail = -1,
            Cancel = -2
        }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DeletedAt { get; set; }

        public Donation()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Status = DonationStatusEnum.Pending;
        }
    }

    public class Follow
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("Project")]
        public string ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public StatusEnum Status { get; set; }
        public enum StatusEnum
        {
            Followed = 0,
            Unfollowed = 1
        }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DeletedAt { get; set; }

        public Follow()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Status = StatusEnum.Followed;
        }
    }
}