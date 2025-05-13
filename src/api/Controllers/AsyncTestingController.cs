using System.Collections;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class AsyncTestingController : ControllerBase
{
    [HttpGet]
    [Route("asynctesting/sleep")]

    public async Task<IActionResult> DelayTaskAsync()
    {
        await Task.Delay(100); 
        return Ok("Async Task Completed");
    }
    [HttpGet]
    [Route("synctesting/sleep")]
    public IActionResult DelayThreadSync()
    {
        Thread.Sleep(100); 
        return Ok("Sync Task Completed");
    }


    
    [HttpGet]
    [Route("asynctesting/calculate")]
    public async Task<IActionResult> CalculateTaskAsync()
    {
        ArrayList resultList = new ArrayList() ;
        for (int i = 0; i < 50; i++)
        {
            var result = await Task.Run(() => CalculateFactorial(i));
            resultList.Add(result);

        }
        return Ok(resultList);
    }
    
    [HttpGet]
    [Route("/synctesting/calculate")]
    public IActionResult CalculateTasksync()
    {
        
        ArrayList resultList = new ArrayList() ;
        for (int i = 0; i < 50; i++)
        {
            var result = CalculateFactorial(i);
            resultList.Add(result);

        }
        return Ok(resultList);
    }

    
    private long CalculateFactorial(int n)
    {
        long result = 1;
        for (int i = 1; i <= n; i++)
        {
            result *= i;
        }
        return result;
    }
}
