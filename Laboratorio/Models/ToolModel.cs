using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laboratorio.Models
{
    public class ToolModel
    {

        public string Code { get; set; }
        public string Type { get; set; }
        public DateTimeOffset CalibrationDate { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public string Machine { get; set; }
        public short Available { get; set; } //1 disponible, 2 no disponible, 3 dado de baja
        public bool Measure { get; set; }
        public string Shared { get; set; }
        public string Plantilla { get; set; } = "";

        public bool isChecked { get; set; }
    
        public string ExpirationFlag { get; set; } //0 expirado, 1:proximo a expirar, 2: suficiente tiempo





    }
}