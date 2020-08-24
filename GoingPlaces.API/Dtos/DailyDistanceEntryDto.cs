using System;
using System.ComponentModel.DataAnnotations;
using GoingPlaces.API.Models;

namespace GoingPlaces.API.Dtos
{
    public class DailyDistanceEntryDto
    {
        public int Id { get; set; }
        public string CurrentDay { get; set; }
        public double DailyDistance { get; set; }
        public string MonthOfTheYear { get; set; }
        public int UserId { get; set; }

        public DailyDistanceEntryDto()
        {
            CurrentDay = DateTime.Now.ToString("MM/dd/yyyy");
        }
    }
}