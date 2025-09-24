sudo cp ./gpio-api.service /etc/systemd/system/gpio-api.service
sudo systemctl enable gpio-api.service
sudo systemctl start gpio-api.service
sudo usermod -a -G gpio $USER