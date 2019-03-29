namespace ProductShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Category
    {
        public Category()
        {
            this.CategoryProducts = new List<CategoryProduct>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(15, MinimumLength =3)]
        public string Name { get; set; }

        public ICollection<CategoryProduct> CategoryProducts { get; set; }

        internal bool IsValid()
        {
            if (this.Name != null && this.Name.Length >=3 && this.Name.Length <=15 )
            {
                return true;
            }
            return false;
        }
    }
}
