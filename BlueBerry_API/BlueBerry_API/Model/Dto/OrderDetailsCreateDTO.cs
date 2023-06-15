﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueBerry_API.Model.Dto
{
    public class OrderDetailsCreateDTO
    {
     
        [Required]
        public int MenuItemId { get; set; }
      
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
