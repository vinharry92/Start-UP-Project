using System;
using System.ComponentModel.DataAnnotations;

namespace No_Core_Auth.Model
{
    public class ProductModel
    {
        [Key]
        public int Productid { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(150)]
        public string Description { get; set; }

        public bool Outofstock { get; set; }

        public string ImgUrl { get; set; }

        public double Price { get; set; }

    }
}
