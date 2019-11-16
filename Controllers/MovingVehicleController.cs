using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;
using AspNetCoreBustronic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharpPad;
using System.Diagnostics;
namespace AspNetCoreBustronic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovingVehicleController : ControllerBase
    {
        private readonly ILogger<MovingVehicleController> _logger;
        private readonly BustronicContext _context;

        public MovingVehicleController(ILogger<MovingVehicleController> logger, BustronicContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public void Get()
        {
            //go();
        }

    }
}