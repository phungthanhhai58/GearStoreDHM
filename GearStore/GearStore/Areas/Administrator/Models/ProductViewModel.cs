using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GearStore.Areas.Administrator.Models
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Text)]
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
        public int ManufacturerID { get; set; }
        [Required]
        [Range(0,double.PositiveInfinity)]
        public decimal Price { get; set; }
        public string PhotoFilePatch { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int UnitsInStock { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss.fff}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedDate { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int ReorderLevel { get; set; }
        [Required]
        [Range(0, 5)]
        public byte Rating { get; set; }
        public bool Discontinued { get; set; }
        [DataType(DataType.MultilineText)]
        public string Details { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public HttpPostedFileBase UploadImage { get; set; }
    }
}