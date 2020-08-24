using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace GoingPlaces.API.Models
{
    public class User : IdentityUser
    {
      //  public DateTime DateOfBirth { get; set; }
       // public IEnumerable<DailyDistanceEntry> DailyDistanceEntries { get; set; }
        public IEnumerable<DailyDistanceEntry> DailyDistanceEntries { get; } = new HashSet<DailyDistanceEntry>();
    }
}