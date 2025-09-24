# Turn on GPIO pin 17
curl -X POST http://localhost:5000/api/gpio/pin/17/on
sleep 30
# Turn off GPIO pin 17
curl -X POST http://localhost:5000/api/gpio/pin/17/off
sleep 30
# Get status of GPIO pin 17
curl http://localhost:5000/api/gpio/pin/17
sleep 30
# Blink GPIO pin 17
curl -X POST "http://localhost:5000/api/gpio/blink/17?durationMs=500&count=10"
sleep 30