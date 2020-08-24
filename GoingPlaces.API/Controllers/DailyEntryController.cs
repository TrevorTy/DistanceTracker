
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GoingPlaces.API.Data;
using GoingPlaces.API.Dtos;
using GoingPlaces.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GoingPlaces.API.Controllers
{
    
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DailyEntryController : ControllerBase
    {
        private readonly IGoingPlacesRepository _repo;
        private readonly DataContext _context;
        private readonly ILogger<DailyEntryController> _logger;
        private readonly UserManager<User> _userManager;
         

        public DailyEntryController(IGoingPlacesRepository repo, ILogger<DailyEntryController> logger
        , UserManager<User> userManager)
        {
            _repo = repo;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet("distances/{year}")]
        public async Task<IActionResult> GetDistances(int year)
        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var myDistances = await _repo.GetAllDistances(year, userId);
                return  Ok(myDistances);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get distances: {ex}");
                return BadRequest("Failed to get distances");
            }
        } 

        [HttpPost]
        public IActionResult AddDailyDistance(DailyDistanceEntry model)
        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
             if (userId == null)
                 return Unauthorized();
            try
            {
                var entry = new DailyDistanceEntry
                {
                    //CurrentDay = model.CurrentDay,
                    DailyDistance = model.DailyDistance,
                    Month = model.Month,
                    Day = model.Day,
                    Year = model.Year,
                    UserId = userId
                };
                if(_repo.AddDailyEntry(entry))
                {
                return Created($"/api/dailyentry/{model.Id}", model);
                }
                else{
                    BadRequest("This date already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"failed to save a new distance: {ex}");
               
            }
            return BadRequest("Failed to save the Daily Distance");
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<DailyDistanceEntry>> DeleteDailyDistance(int id)
        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
            var distanceToBeDeleted = await _repo.GetDailyDistanceByID(id);

             if (distanceToBeDeleted == null)
        {
                return NotFound();
        }
            if(distanceToBeDeleted.UserId == userId)
            {
             _repo.Delete(distanceToBeDeleted);
           // await _context.SaveChangesAsync();
              _repo.Save();
            }
                return Ok(); // special http delete statuscode?
        }

        [HttpPost("destination")]
        public  ActionResult AddEndDestination(EndDestination model)
        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                 return Unauthorized();
            
            try
            {
                var destinationToSave = new EndDestination 
                {
                    Goal = model.Goal,
                    DistanceOfGoal = model.DistanceOfGoal,
                    Year = model.Year,
                    UserId = userId
                };

                 _repo.AddEntity(destinationToSave);
                _repo.Save();
                return Created($"/api/dailyentry/destination{model.Id}", model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"could not add End Destination{ex}");
                
            }
             return BadRequest("Failed to save the End Destination");
        }

        [HttpGet("destination/{year}")]
        public async Task<IActionResult> GetDestination(int year)
        {
              var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);

            
             try
            {
                var myDestination = await _repo.GetDestination(year, userId);
                return  Ok(myDestination);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get distances: {ex}");
                return BadRequest("Failed to get distances");
                
            }
        }

         [HttpGet("totalDistance/{year}")]
        public async Task<IActionResult> GetTotalCoveredDistance(int year)
        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
            var totaldistance = await _repo.CalculateTotalCoveredDistance(year, userId);

            return Ok(totaldistance);
        }

        [HttpGet("totalDistanceMonth/{year}")]
        public async Task<IActionResult> GetTotalDistanceMonth(int year)
        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append($"\"title\": \"Total\", \"year\": {year}, \"day\": null ");

            var distanceMonth = await _repo.CalculateTotalDistanceMonth(year, userId);

            foreach(var distance in distanceMonth)
            {
                sb.Append($", \"{distance.Month}\": {distance.Total.ToString(CultureInfo.GetCultureInfo("en-US"))}");
            }
            sb.Append("}");

            return Ok(sb.ToString());
        }

        [HttpGet("averageDistanceMonth/{year}")]
        public async Task<IActionResult> GetAverageDistanceMonth(int year)
        {
           var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append($"\"title\": \"Average\", \"year\": {year}, \"day\": null ");

            var averageDistanceMonth = await _repo.CalculateAverageDistanceMonth(year, userId);

            foreach(var distance in averageDistanceMonth)
            {
                sb.Append($", \"{distance.Month}\": {distance.Average.ToString(CultureInfo.GetCultureInfo("en-US"))}");
            }
            sb.Append("}");
            
            return Ok(sb.ToString());
        }

        [HttpGet("distanceToCoverInMonth/{year}")]
        public async Task<IActionResult> DistanceToCoverInMonth(int year)

        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);

            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            sb.Append($"\"title\": \"Monthly target\", \"year\": {year}, \"day\": null ");

           //LeapYear 
           int currentYear = DateTime.Now.Year;
            double daysInYear = DateTime.IsLeapYear(currentYear) ? 366.00 : 365.00;
           // een andere functie  van de week delen door (365/366) x 7
           var distanceToGoal =   await _repo.GetDistanceOfGoal(year, userId);

            for (int i = 1; i < 13; i++)
            {

               var month =  CultureInfo.GetCultureInfo("en-US").DateTimeFormat.GetMonthName(i).ToLower();
                double calculation = (distanceToGoal / daysInYear) * DateTime.DaysInMonth(year, i);
                sb.Append($", \"{month}\": {(calculation.ToString(CultureInfo.GetCultureInfo("en-US")))}");
            }

            sb.Append("}");
               
            return Ok(sb.ToString());
        }

        [HttpGet("deltaDistanceToCoverInMonth/{year}")]
        public async Task<IActionResult> DeltaDistanceToCoverInMonth(int year)
        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append($"\"title\": \"Delta Target\", \"year\": {year}, \"day\": null ");
           //LeapYear 
            int currentYear = DateTime.Now.Year;
            double daysInYear = DateTime.IsLeapYear(currentYear) ? 366.00 : 365.00;

        
            var distanceToGoal =   await _repo.GetDistanceOfGoal(year, userId);
            var distanceMonth = await _repo.CalculateTotalDistanceMonth(year, userId);
            for (int i = 1; i < 13; i++)
            {
               var month =  CultureInfo.GetCultureInfo("en-US").DateTimeFormat.GetMonthName(i).ToLower();
               var totalDistanceDto = distanceMonth.SingleOrDefault(d => d.Month == month && d.Year == year);
                double  totalDistanceToCover = (distanceToGoal / daysInYear) * DateTime.DaysInMonth(year, i);
                double calculation =  (totalDistanceDto == null  ?  0 : totalDistanceDto.Total ) - totalDistanceToCover; //(this has to be distance.Total);

                sb.Append($", \"{month}\": {calculation.ToString(CultureInfo.GetCultureInfo("en-US"))}");
                //  sb.Append($", \"{month}\": {(distanceToGoal / daysInYear) * DateTime.DaysInMonth(year, i)}");
            }
            sb.Append("}");
            return Ok(sb.ToString());
        }

         [HttpGet("destination/dailyAverageToCover/{year}")]
        public async Task<IActionResult> GetDailyAverageToCoverFromDestination(int year)
        {
            //Do a not null or empty check/no value
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
             try
            {
                 var myDestination = await _repo.CalculateDailyDistanceToCover(year, userId);
                
                return  Ok(myDestination);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Destination Distance: {ex}");
                return BadRequest("Failed to get Destination Distance");
                
            }
        }

        [HttpGet("destination/CalculateMonthlyDistanceToCover/{year}")]
        public async Task<IActionResult> GetCalculateMonthlyDistanceToCover(int year)
        {
           
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
            var DistanceToCoverInMonth = await _repo.CalculateMonthlyDistanceToCover(year, userId);
             return Ok(DistanceToCoverInMonth);
        }

        [HttpGet("destination/CalculateWeeklyDistanceToCover/{year}")]
        public async Task<IActionResult> GetCalculateWeeklyDistanceToCover(int year)
        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await _repo.CalculateWeeklyDistanceToCover(year, userId));
        }

        [HttpGet("totalWalkedDays/{year}")]
        public async Task<IActionResult> GetCalculateTotalWalkedDays(int year)
        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await _repo.CalculateTotalWalkedDays(year, userId));
        }

        [HttpGet("distanceTillDestination/{year}")]
        public async Task<IActionResult> GetlculateDistanceTillDestination(int year)
        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await _repo.CalculateDistanceTillDestination(year, userId));
        }

        [HttpGet("daysLeftOfYear/{year}")]
        public async Task<IActionResult> GetCalculateDaysLeftOfYear(int year)
        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
            return  Ok(await _repo.CalculateDaysLeftOfYear(year));
        }
    }
}