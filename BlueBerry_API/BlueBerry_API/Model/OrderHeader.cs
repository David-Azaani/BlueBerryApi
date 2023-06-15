using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueBerry_API.Model
{
    public class OrderHeader

    {
        [Key]
        public int OrderHeaderId { get; set; }
        [Required]
        public string PickUpName { get; set; }
        [Required]
        public string PickUpPhoneNumber { get; set; }
        [Required]
        public string PickUpEmail { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser User { get; set; }

        public double OrderTotal { get; set; }

        public DateTime OrderType { get; set; }
        public string StripePaymentIntentID { get; set; }
        public string Status { get; set; }
        public int TotalItems { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; }


    }
}
