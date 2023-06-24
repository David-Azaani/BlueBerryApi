using System.ComponentModel.DataAnnotations.Schema;

namespace BlueBerry_API.Model
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ICollection<CartItem> CartItems { get; set; }


        [NotMapped]
        public double CartTotal { get; set; }
        [NotMapped]
        public int CartTotalItems { get; set; }
        [NotMapped]
        public string ClientSecret { get; set; }
        [NotMapped]
        public string StripPaymentIntentId { get; set; }

  
        



    }
}
