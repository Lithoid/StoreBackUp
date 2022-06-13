using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{

    [Table("Products")]
    public class Order :DbEntity
    {

        public string Status { get; set; }


    }
}
