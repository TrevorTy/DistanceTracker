using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoingPlaces.API.Dtos;
using GoingPlaces.API.Models;

namespace GoingPlaces.API.Data
{
    public interface IGoingPlacesRepository
    {
        
        Task<DailyDistanceEntry> GetDailyDistanceByID(int dailyDistanceId);
        bool AddDailyEntry(DailyDistanceEntry entry);
        void UpdateDailyDistance(DailyDistanceEntry dailyDistance);
        Task<IEnumerable> GetAllDistances(int year, string userId);
        Task<IEnumerable> GetDestination(int year, string userId);
        Task<double> CalculateTotalCoveredDistance(int year, string userId);
        Task<int> CalculateTotalWalkedDays(int year, string userId);
        Task<double> CalculateDistanceTillDestination(int year, string userId);
        Task<int> CalculateDaysLeftOfYear(int year);
        void Save();
        void AddEntity(object model);
        void Delete<T>(T entity) where T : class;
        Task<List<MonthTotalsDto>> CalculateTotalDistanceMonth(int year, string userId);
        Task<List<MonthTotalsDto>> CalculateDeltaDistanceMonth(int year, string userId);
        Task<List<MonthAveragesDto>> CalculateAverageDistanceMonth(int year, string userId);
        Task<List<EndDestinationDto>> CalculateMonthlyDistanceToCover(int year, string userId);
        Task<List<EndDestinationDto>> CalculateWeeklyDistanceToCover(int year, string userId);
        Task<List<EndDestinationDto>> CalculateDailyDistanceToCover(int year, string userId);
        Task<int> GetDistanceOfGoal(int year, string userId);
        
        
    }
}