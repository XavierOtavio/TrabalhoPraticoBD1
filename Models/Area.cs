using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrabalhoFinal3.Models;

namespace TrabalhoFinal3.Models
{
    public class Area
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int CategoryId { get; set; }
        public List<Topic> Topics { get; set; } = new List<Topic>();
    }
}