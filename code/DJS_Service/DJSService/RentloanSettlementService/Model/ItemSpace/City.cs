using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentloanSettlementService.ItemSpace
{
    public class City
    {
        public int ID { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string CityArea { get; set; }
        public int? Sequence { get; set; }
    }
}
