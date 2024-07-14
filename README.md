
# Neodymium 🧲

Move and resize windows using keyboard shortcuts!

The app is inspired by tools like Rectangle for MacOS. Although the Windows OS supports window snapping and keyboard shortcuts, this app aims to complement those.

Some of Neodymium features are:

 - Instant snapping without animations
 - Manually resize windows using keyboard shortcuts
 - Move windows to next monitor
 - Customizable shortcuts
 - Uses less than ~20MB of RAM and 1% of CPU even on older machines

## Installation

Download & install the latest version from Releases. After the installation is complete, you can launch the app from the Start menu. The app is automatically minimized into its taskbar icon. To quit Neodymium, double click the icon and close the window.

If you want to run Neodymium on startup, copy a shortcut to the app to:

`C:\Users\[user name]\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup`

Neodymium requires .NET 8.0.

## Usage

Use a modifier key (*Default is Ctrl+Win+Alt*) with the combination of following keys:

| Shortcut    | Action (Move and/or resize window to)  |
|-------------|----------------------------------------|
| Left arrow  | Left half                              |
| Right arrow | Right half                             |
| Up arrow    | Top half                               |
| Down arrow  | Bottom half                            |
| U           | Top left                               |
| I           | Top right                              |
| J           | Bottom left                            |
| K           | Bottom right                           |
| Enter       | Full screen                            |
| X           | Next display (half the size of screen) |
| C           | Center                                 |
| W           | Make window wider                      |
| S           | Make window narrower                   |
| E           | Make window taller                     |
| D           | Make window shorter                    |

The default shortcuts can be changed by modifying the `.neodymium.json` configuration file inside `C:\Users\[user name]` and restarting. 

## TODO

Since this app is something of a weekend project of mine, there are some bugs that needs to be fixed:

 - Improve resize guards (Don't allow windows to be manually resized past the display size)
 - Unsnap to default size
 - Start as an admin by default to allow resizing of all windows
 - Remove the resized window's margin

## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

## License

[GNU GPL v3.0](https://www.gnu.org/licenses/gpl-3.0.en.html)
