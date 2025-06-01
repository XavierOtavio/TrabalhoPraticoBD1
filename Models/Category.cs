using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrabalhoFinal3.Models;

namespace TrabalhoFinal3.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<Area> Areas { get; set; } = new List<Area>();
    }
}