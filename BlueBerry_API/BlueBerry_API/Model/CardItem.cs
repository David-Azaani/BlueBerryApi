﻿using System.ComponentModel.DataAnnotations.Schema;

namespace BlueBerry_API.Model;

public class CardItem
{
    public int Id { get; set; }
    public int MenuItemId { get; set; }
    [ForeignKey("MenuItemId")] 
    public MenuItem MenuItem { get; set; } = new MenuItem();

    public int Quantity { get; set; }
    public int ShoppingCardId { get; set; }


}