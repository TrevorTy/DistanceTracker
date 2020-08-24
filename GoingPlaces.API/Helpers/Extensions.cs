using Microsoft.AspNetCore.Http;

namespace GoingPlaces.API.Helpers
{
    public static class Extensions
    {
        //General purpose extension method class
        //u dont want to make a new instance of this class therefore its a static class
    //     public static void AddApplicationError(this HttpResponse response, string message)
    //      {
    //        //This is how to add additional headers to the response
    //         //The message will be the error message as the value
    //          response.Headers.Add("Application-Error", message);
    //        //These 2 headers will allow the above message to be displayed
    //      response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");

    //       response.Headers.Add("Access-Control-Allow-Origin", "*");
    //    }
    }
}