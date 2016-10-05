# ScoreKeeper

ScoreKeeper is a WPF application built for Windows 7/8/10. The application lets
you add a list of games with Blue/White team names. After adding and starting a
game, you can add/subtract goals and start/stop/restart/reset a game timer.

The application consists of two windows. MainWindow automatically starts on the
primary display, and GoalWindow is displayed full screen on a user configured
display.

ScoreKeeper will (if enabled in configuration) broadcast a struct containing
team names, team goals and timer information over UDP network.

Broadcast and display settings can be configured.


## ScoreHub

ScoreHub is a Windows Service that will listen for UDP broadcast data from
the ScoreKeeper application and serve as a CSV file on HTTP. The service
will update the CSV file on every UDP receive unless an update interval is
configured.


## ShowTime

ShowTime is a small application built for Windows 10 IOT. The application will
receive data from the ScoreKeeper application over UDP. When that data contains
an active timer, it will display the time over HDMI.

Display config can be changed by editing the config file on the device. 