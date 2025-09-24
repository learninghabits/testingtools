using Microsoft.AspNetCore.Mvc;
using raspberrypi.tool.gpio.Services;

namespace raspberrypi.tool.gpio.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class GpioController : ControllerBase
    {
        private readonly IGpioService _gpioService;
        private readonly ILogger<GpioController> _logger;

        public GpioController(IGpioService gpioService, ILogger<GpioController> logger)
        {
            _gpioService = gpioService;
            _logger = logger;
        }

        [HttpPost("pin/{pinNumber}/on")]
        public IActionResult TurnOnPin(int pinNumber)
        {
            _logger.LogInformation($"Turning on pin {pinNumber}");
            var result = _gpioService.SetPinValue(pinNumber, true);

            return result ? Ok($"Pin {pinNumber} turned on") : BadRequest($"Failed to turn on pin {pinNumber}");
        }

        [HttpPost("pin/{pinNumber}/off")]
        public IActionResult TurnOffPin(int pinNumber)
        {
            _logger.LogInformation($"Turning off pin {pinNumber}");
            var result = _gpioService.SetPinValue(pinNumber, false);

            return result ? Ok($"Pin {pinNumber} turned off") : BadRequest($"Failed to turn off pin {pinNumber}");
        }

        [HttpPost("pin/{pinNumber}/toggle")]
        public IActionResult TogglePin(int pinNumber)
        {
            _logger.LogInformation($"Toggling pin {pinNumber}");
            var currentValue = _gpioService.GetPinValue(pinNumber);

            if (currentValue.HasValue)
            {
                var result = _gpioService.SetPinValue(pinNumber, !currentValue.Value);
                return result ? Ok($"Pin {pinNumber} toggled to {!currentValue.Value}")
                            : BadRequest($"Failed to toggle pin {pinNumber}");
            }

            return BadRequest($"Failed to read current value of pin {pinNumber}");
        }

        [HttpGet("pin/{pinNumber}")]
        public IActionResult GetPinStatus(int pinNumber)
        {
            var value = _gpioService.GetPinValue(pinNumber);

            if (value.HasValue)
            {
                return Ok(new { Pin = pinNumber, Status = value.Value ? "High" : "Low", Value = value.Value });
            }

            return BadRequest($"Failed to read pin {pinNumber}");
        }

        [HttpPost("blink/{pinNumber}")]
        public async Task<IActionResult> BlinkPin(int pinNumber, [FromQuery] int durationMs = 1000, [FromQuery] int count = 5)
        {
            _logger.LogInformation($"Blinking pin {pinNumber} {count} times with {durationMs}ms interval");

            for (int i = 0; i < count; i++)
            {
                _gpioService.SetPinValue(pinNumber, true);
                await Task.Delay(durationMs);
                _gpioService.SetPinValue(pinNumber, false);

                if (i < count - 1) // Don't wait after the last blink
                    await Task.Delay(durationMs);
            }

            return Ok($"Pin {pinNumber} blinked {count} times");
        }
    }
}
