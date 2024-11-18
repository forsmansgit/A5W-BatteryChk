import rivalcfg
import clr

# Initialize .NET runtime
clr.AddReference("System.Drawing")
clr.AddReference("System.Windows.Forms")

from System import * # type: ignore
from System.Drawing import Icon # type: ignore
from System.Windows.Forms import Application, NotifyIcon, ToolTipIcon, ApplicationContext, Timer # type: ignore

class BatteryNotifier(ApplicationContext):
    def __init__(self):

        # Get the first mouse
        mouse = rivalcfg.get_first_mouse()

        # Get the battery information
        battery_info = mouse.battery

        # Extract the battery level and charging status
        is_charging = battery_info['is_charging']
        level = battery_info['level']

        # Format the battery information
        charging_status = "Charging" if is_charging else "Not Charging"
        battery_charging = f"Status: {charging_status}"
        battery_level = f"Battery level: {level}%"
        formatted_battery_info = f"Battery level: {level}%, Status: {charging_status}"

        # Print the formatted battery information
        print(f"{formatted_battery_info}")

        # Specify the path to the icon file
        icon_path = "C:\\Projects\\Python\\A5W-BatteryNotif\\media\\ssicon.ico"

        # Create a NotifyIcon
        notify_icon = NotifyIcon()
        notify_icon.Icon = Icon(icon_path)  # Path to your icon file
        notify_icon.Visible = True
        notify_icon.Text = "A5W Battery Notifier"
        notify_icon.BalloonTipTitle = battery_level
        notify_icon.BalloonTipText = battery_charging
        notify_icon.BalloonTipIcon = ToolTipIcon.Info

        # Show the balloon tip
        notify_icon.ShowBalloonTip(1500)  # Duration in milliseconds

        # Keep the application running to display the notification
        self.notify_icon = notify_icon

        # Set up a timer to stop the application after the notification
        self.timer = Timer()
        self.timer.Interval = 1600  # Duration in milliseconds (4 seconds)
        self.timer.Tick += self.on_timer_tick
        self.timer.Start()

    def on_timer_tick(self, sender, event):
        self.timer.Stop()
        self.notify_icon.Visible = False  # Hide the notification icon
        Application.Exit()  # Exit the application

# Run the application with the custom context
Application.Run(BatteryNotifier())