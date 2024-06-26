﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AudioStore.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public int? SuperCategoryID { get; set; }
        [ForeignKey("SuperCategoryID")]
        [ValidateNever]
        public virtual Category SuperCategory { get; set; }
        [ValidateNever]

        public virtual ICollection<Category> SubCategories { get; set; }
    }
}