using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using GoingPlaces.API.Dtos;
using GoingPlaces.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;

namespace GoingPlaces.API.Data
{
    //Authorize most of this
    //Change this stuff do Async
    public class GoingPlacesRepository : IGoingPlacesRepository
    {
         private readonly DataContext _context;
        public GoingPlacesRepository(DataContext context)
        {
            _context = context;
        }

        public void AddEntity(object model)
        {
            _context.Add(model);
        }

        public bool AddDailyEntry(DailyDistanceEntry entry)
        {
              var dateAlreadyExists = _context.DailyDistanceEntries
            .Any(x => x.Day == entry.Day 
                    && x.Month == entry.Month 
                    && x.Year == entry.Year
                    && x.UserId == entry.UserId);
                    if(!dateAlreadyExists) 
                    {
                        _context.Add(entry);

                        _context.SaveChanges();
                        return true;
                    }
                        return false;
        }



      

        public async Task<IEnumerable> GetAllDistances(int year, string userId)
        {
            
            return await _context.DailyDistanceEntries
                        .Where(x => x.UserId == userId 
                        && x.Year == year)
                        .ToListAsync();
        }

        public async Task<DailyDistanceEntry> GetDailyDistanceByID(int id)
        {
            return await _context.DailyDistanceEntries.FindAsync(id);
        }

        

        public void Save()
        {
            _context.SaveChanges();
        }


        public void UpdateDailyDistance(DailyDistanceEntry dailyDistance)
        {
            throw new System.NotImplementedException();
        }


        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async  Task<IEnumerable> GetDestination(int year, string userId)
        {
           return await _context.EndDestinations
                        .Where(x => x.UserId == userId 
                        && x.Year == year)
                        .ToListAsync();
        }

          public async  Task<List<EndDestinationDto>> CalculateDailyDistanceToCover(int year, string userId)
        {
            int currentYear = DateTime.Now.Year;
            int daysOfYear = DateTime.IsLeapYear(currentYear) ? 366 : 365;

            var  goal = await _context.EndDestinations
                        .Where(x => x.UserId == userId 
                        && x.Year == year)
                        .AsQueryable()
                        .Select(e => new EndDestinationDto {
                            Distance = (e.DistanceOfGoal / daysOfYear).ToString("#.##"),
                            Name = e.Goal
                           
                        })
                        .ToListAsync();

                    return goal;        
        }


             public async  Task<List<EndDestinationDto>> CalculateWeeklyDistanceToCover(int year, string userId)
        {
            // Average number of week in a year
            //  // een andere functie  van de week delen door (365/366) x 7
            int weeksOfYear = 52;
            var  goal = await _context.EndDestinations
                         .Where(x => x.UserId == userId 
                        && x.Year == year)
                        .AsQueryable()
                        .Select(e => new EndDestinationDto {
                            Distance = (e.DistanceOfGoal / weeksOfYear).ToString("#.#"),
                            Name = e.Goal
                           
                        })
                        .ToListAsync();

                    return goal;
                        
                        
        }

            public async  Task<List<EndDestinationDto>> CalculateMonthlyDistanceToCover(int year, string userId)
            {
                double monthsOfYear = 12;
           
                var  goal = await _context.EndDestinations
                            .Where(x => x.UserId == userId 
                            && x.Year == year)
                            .AsQueryable()
                            .Select(e => new EndDestinationDto {
                                Distance = (e.DistanceOfGoal / monthsOfYear).ToString("#.#"),
                                Name = e.Goal
                            })
                            .ToListAsync();

                        return goal;
                }

          


        public async Task<double> CalculateTotalCoveredDistance(int year, string userId)
        {
            //getuserid
            var totalDistance = await _context.DailyDistanceEntries
                                     .Where(x => x.UserId == userId 
                                    && x.Year == year)
                                    .SumAsync(t => t.DailyDistance);

            return totalDistance;
        }

        //Filter for user
        public async Task<List<MonthTotalsDto>> CalculateTotalDistanceMonth(int year, string userId)
        {
            // var distanceOftheMonth = await _context.DailyDistanceEntries
            //                         .Where(d => d.Month.Contains("may"))
            //                         .ToListAsync();

           // var iQuaryable = _context.DailyDistanceEntries.AsQueryable();
          // .Where(d => d.Month == "may")
            var distance =  await _context.DailyDistanceEntries
                        .Where(d => d.Year == year 
                        && d.UserId == userId )
                        .GroupBy(d => new {d.Year, d.Month})
                        .Select(k => new MonthTotalsDto {
                            Month = k.Key.Month,
                            Year = k.Key.Year,
                            Total = k.Sum(d => d.DailyDistance)
                        })
                        .ToListAsync();

                return  distance;
        }

        public async Task<List<MonthAveragesDto>> CalculateAverageDistanceMonth(int year, string userId)
        {
           var distance =  await _context.DailyDistanceEntries
                        .Where(d => d.Year == year && d.UserId == userId)
                        .GroupBy(d => new {d.Year, d.Month})
                        .Select(k => new MonthAveragesDto {
                            Month = k.Key.Month,
                            Year = k.Key.Year,
                            Average = k.Average(d => d.DailyDistance)
                        })
                        .ToListAsync();

                return  distance;
        }




        // public void InsertFinalDistance(EndDestination endDestination)
        // {
        //     _context.EndDestinations.AddAsync(endDestination);
        // }

        public async Task<int> CalculateTotalWalkedDays(int year, string userId)
        {
            var totalWalkedDays = await _context.DailyDistanceEntries
                                        .Where(d => d.Year == year 
                                        && d.UserId == userId)
                                        .CountAsync();
            return totalWalkedDays;
        }

        public async Task<double> CalculateDistanceTillDestination(int year, string userId)
        {
            //Do it in one query?
            var destination = await _context.EndDestinations
                            .Where(d => d.Year == year && d.UserId == userId)
                            .Select(e => e.DistanceOfGoal)
                            .FirstOrDefaultAsync();

            var totalCoveredDistance  = await _context.DailyDistanceEntries
                                        .Where(d => d.Year == year && d.UserId == userId)
                                        .SumAsync(t => t.DailyDistance);

            double distanceTillDestination = destination - totalCoveredDistance;

            return distanceTillDestination;
        }

        //Do this inside a helper class or controller
        public async Task<int> CalculateDaysLeftOfYear(int year)
        {
            int daysInYear = DateTime.IsLeapYear(year) ? 366 : 365;
            if(year == DateTime.Now.Year)
            {
                       DateTime date = DateTime.Today;
            
                     int daysLeftInYear = daysInYear - date.DayOfYear; 

                     return daysLeftInYear;
            }
            else{
                if(year > DateTime.Now.Year)
                {
                    return daysInYear;
                }
                 else
                 {
                    return 0;
                }
            }
        }

        public async Task<int> GetDistanceOfGoal(int year, string userId)
        {
              var goalDistance = await _context.EndDestinations
                            .Where(d => d.Year == year && d.UserId == userId)
                            .Select(e => e.DistanceOfGoal)
                            .FirstOrDefaultAsync();
                        
            return goalDistance;
        }

         public async Task<List<MonthTotalsDto>> CalculateDeltaDistanceMonth(int year, 
         string userId)
        {
            var distance =  await _context.DailyDistanceEntries
                        .Where(d => d.Year == year && d.UserId == userId)
                        .GroupBy(d => new {d.Year, d.Month})
                        .Select(k => new MonthTotalsDto {
                            Month = k.Key.Month,
                            Year = k.Key.Year,
                            Total = k.Sum(d => d.DailyDistance)
                        })
                        .ToListAsync();
                        

                return  distance;
        }
    }
}