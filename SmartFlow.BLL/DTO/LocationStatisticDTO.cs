using System;

namespace SmartFlow.BLL.DTO
{
    public class LocationStatisticDTO
    {
        public int LocationID { set; get; }
        public string LocationName { set; get; }
        public int Visits { set; get; }
        public DateTime DateVisits { set; get; } 
    }
}