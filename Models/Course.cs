using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrabalhoFinal3.Models;

namespace TrabalhoFinal3.Models
    {
        public class Course
        {
            public int Id { get; set; }
            public string Titulo { get; set; }
            public string Descricao { get; set; }
            public string Formador { get; set; }
            public DateTime DataInicio { get; set; }
            public DateTime DataFim { get; set; }
        }
    }
