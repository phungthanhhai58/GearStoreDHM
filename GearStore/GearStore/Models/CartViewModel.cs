using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GearStore.Models
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
        [DisplayFormat(DataFormatString = "{0:N3}")]
        public decimal? Subtotal { get => Items.Sum(p => p.Total); }
        [DisplayFormat(DataFormatString = "{0:N3}")]
        public decimal? Shipping { get => Subtotal >= 1000000 ? 0 : 100000; }
        [DisplayFormat(DataFormatString = "{0:N3}")]
        public decimal? Tax { get => Subtotal * 10 / 100; }
        [DisplayFormat(DataFormatString = "{0:N3}")]
        public decimal? Total { get => Subtotal + Shipping + Tax; }
        public int? Count { get => Items.Sum(p => p.Quantity); }
    }
    public class CartItemViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get => Quantity * Price; }
        public string PhotoFilePatch { get; set; }
        public Category Category { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public Menu Menu { get; set; }

    }
}