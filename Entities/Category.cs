﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Entities
{
    [Table("Categories")]
    public class Category : DbEntity
    {
        [Column("Name")]
        [MaxLength(32)]
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}