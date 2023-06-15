﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlueBerry_API.Utility;

namespace BlueBerry_API.Model.Dto
{
    public class OrderHeaderCreateDTO
    {
        
        [Required]
        public string PickUpName { get; set; }
        [Required]
        public string PickUpPhoneNumber { get; set; }
        [Required]
        public string PickUpEmail { get; set; }
        public string ApplicationUserId { get; set; }
       
        public double OrderTotal { get; set; }

        
        public string StripePaymentIntentID { get; set; }
        public string Status { get; set; } 
        public int TotalItems { get; set; }

        public ICollection<OrderDetailsCreateDTO> OrderDetailsDTO { get; set; }
    }
}
