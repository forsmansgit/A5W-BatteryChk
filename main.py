import rivalcfg

def get_battery_info():
    # Get the first mouse
    mouse = rivalcfg.get_first_mouse()

    # Get the battery information
    battery_info = mouse.battery

    # Extract the battery level and charging status
    is_charging = battery_info['is_charging']
    level = battery_info['level']

    # Format the battery information
    charging_status = "Charging" if is_charging else "Not Charging"
    formatted_battery_info = f"Battery level: {level}%, Status: {charging_status}"

    # Print the formatted battery information
    print(formatted_battery_info)

if __name__ == "__main__":
    get_battery_info()