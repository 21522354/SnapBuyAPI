﻿namespace CartService.Models
{
    public class Cart
    {
        public Guid UserId { get; set; }
        public List<CartItem> Items { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
