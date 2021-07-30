using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIApplication2.DTOs
{
    public class OrderCreateDto
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public List<int> ProductIds { get; set; }
    }
}
