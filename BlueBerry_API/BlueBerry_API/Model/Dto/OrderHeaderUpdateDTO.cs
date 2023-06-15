using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueBerry_API.Model.Dto
{
    public class OrderHeaderUpdateDTO
    {

        public int OrderHeaderId { get; set; }
        public string PickUpName { get; set; }
        public string PickUpPhoneNumber { get; set; }
        public string PickUpEmail { get; set; }



        public DateTime OrderDate { get; set; }
        public string StripePaymentIntentID { get; set; }
        public string Status { get; set; }



    }
}
