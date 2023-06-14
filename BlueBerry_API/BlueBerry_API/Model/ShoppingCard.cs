using System.ComponentModel.DataAnnotations.Schema;

namespace BlueBerry_API.Model
{
    public class ShoppingCard
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string StripPaymentIntentId { get; set; }


        [NotMapped]
        public double CartTotal { get; set; }
        [NotMapped]
        public string ClientSecret { get; set; }
        [NotMapped]
        public ICollection<CardItem> CardItems { get; set; }



    }
}
