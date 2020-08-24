using System;
using System.ComponentModel.DataAnnotations;

namespace GoingPlaces.API.Models
{
    public class DailyDistanceEntry
    {
        public int Id { get; set; }
        
        public DateTime CurrentDay { get; set; }// currentDate
        public double DailyDistance { get; set; }
        public int Day { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}