using System.Device.Gpio;

namespace raspberrypi.tool.gpio.Services
{
    public interface IGpioService
    {
        bool SetPinValue(int pinNumber, bool value);
        bool? GetPinValue(int pinNumber);
        void Cleanup();
    }

    public class GpioService : IGpioService, IDisposable
    {
        private readonly GpioController _gpioController;
        private readonly Dictionary<int, PinMode> _managedPins;

        public GpioService()
        {
            _gpioController = new GpioController();
            _managedPins = new Dictionary<int, PinMode>();
        }

        public bool SetPinValue(int pinNumber, bool value)
        {
            try
            {
                if (!_gpioController.IsPinOpen(pinNumber))
                {
                    _gpioController.OpenPin(pinNumber, PinMode.Output);
                    _managedPins[pinNumber] = PinMode.Output;
                }

                _gpioController.Write(pinNumber, value ? PinValue.High : PinValue.Low);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting pin {pinNumber}: {ex.Message}");
                return false;
            }
        }

        public bool? GetPinValue(int pinNumber)
        {
            try
            {
                if (!_gpioController.IsPinOpen(pinNumber))
                {
                    _gpioController.OpenPin(pinNumber, PinMode.Input);
                    _managedPins[pinNumber] = PinMode.Input;
                }

                var value = _gpioController.Read(pinNumber);
                return value == PinValue.High;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading pin {pinNumber}: {ex.Message}");
                return null;
            }
        }

        public void SetPinMode(int pinNumber, PinMode mode)
        {
            try
            {
                if (_gpioController.IsPinOpen(pinNumber))
                {
                    _gpioController.ClosePin(pinNumber);
                }

                _gpioController.OpenPin(pinNumber, mode);
                _managedPins[pinNumber] = mode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting pin mode {pinNumber}: {ex.Message}");
            }
        }

        public void Cleanup()
        {
            foreach (var pin in _managedPins.Keys)
            {
                if (_gpioController.IsPinOpen(pin))
                {
                    _gpioController.Write(pin, PinValue.Low);
                    _gpioController.ClosePin(pin);
                }
            }
            _managedPins.Clear();
        }

        public void Dispose()
        {
            Cleanup();
            _gpioController?.Dispose();
        }
    }
}
