using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.Models.Entities
{
    public class DRT // you can skip it here
    {
        public int Id { get; set; }
        public string NAZWAPRODUKTU { get; set; }
        public string GTIN { get; set; }
        public string KLASYFIKACJAGPC { get; set; }
        public string DATA { get; set; }
        public string POBIERZ { get; set; }
        public int CategorySSSId { get; set; }
        public string Size { get; set; }
    }
}
