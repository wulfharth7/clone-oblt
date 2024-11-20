using Microsoft.AspNetCore.Mvc;

namespace clone_oblt.Helpers
{
    public static class ResponseUtil 
    {
        public static IActionResult Success(object data)
        {
            return new OkObjectResult(new
            {
                status = "Success",
                data
            });
        }

        public static IActionResult Error(string message, int statusCode = 400)
        {
            return new ObjectResult(new
            {
                status = "Error",
                message
            })
            {
                StatusCode = statusCode
            };
        }
    }
}
