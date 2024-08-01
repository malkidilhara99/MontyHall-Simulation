using Microsoft.AspNetCore.Mvc;
using System;

namespace MontyHallSimulationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MontyHallController : ControllerBase
    {
        [HttpPost("simulate")]
        public IActionResult Simulate([FromBody] SimulationRequest request)
        {
            int wins = 0;
            int losses = 0;
            Random random = new Random();

            for (int i = 0; i < request.NumberOfGames; i++)
            {
                bool result = MontyHallPick(random.Next(3), request.SwitchDoor, random.Next(3), random.Next(2));
                if (result)
                    wins++;
                else
                    losses++;
            }

            // Calculate probabilities
            var totalGames = request.NumberOfGames;
            var switchProbability = wins ;
            var stayProbability = losses ;

            return Ok(new
            {
                WinningProbability = switchProbability,
                StayingProbability = stayProbability
            });
        }

        private static bool MontyHallPick(int initialPick, bool switchDoor, int carDoor, int goatDoorToReveal)
        {
            int leftGoatDoor = 0;
            int rightGoatDoor = 2;
            switch (initialPick)
            {
                case 0: leftGoatDoor = 1; rightGoatDoor = 2; break;
                case 1: leftGoatDoor = 0; rightGoatDoor = 2; break;
                case 2: leftGoatDoor = 0; rightGoatDoor = 1; break;
            }

            int remainingGoatDoor = goatDoorToReveal == 0 ? rightGoatDoor : leftGoatDoor;
            return switchDoor ? carDoor != remainingGoatDoor : carDoor == initialPick;
        }
    }

    public class SimulationRequest
    {
        public int NumberOfGames { get; set; }
        public bool SwitchDoor { get; set; }
    }
}
