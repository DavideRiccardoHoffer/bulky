﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models1
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
		[DisplayName("Display Order")]

		//vedasi https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.rangeattribute

		[Range(1, 100, ErrorMessage = "{0} must be between {1} and {2}")]
		public int DisplayOrder { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}