# Reversi
### Made by Jameel Kelley

This project contains the Visual studio solution of all the code needed to run the Reversi game plus some extra because I wanted to make a web app and API that I could run on my web server. It may be running at *reversi.jamkelley.com* if im hosting it at the time you are viewing it.

### Run it
- Build is located at "Reversi\Reversi.exe"

### Folders
- client-app 		The React app that is the view layer for my full stack Revesri solution (run using npm install && npm start)
- Reversi 			The Unity project that was assigned
- Reversi.Core		The core Reversi data Model, Controller, and Data files
- Reversi.Managers	The overall Reversi game managers (seperate games) (only used in API)
- Reversi.Tests		Some select tests to run on Reversi.Core to check functionality
- Reversi.WebAPI	IIS web API to run on a backend to manage Reversi Games and expose Reversi functionality (run using "dotnet run")

### Notes
- If you want to test using specific board configurations there is a file in "Reversi\Assets\Resources" called "Board2.txt". Edit that to change the board. Additionally, In the Unity editor, toggle the DEBUG varable in the inspector when "reversi_board" is selected.
- Unique Animations are triggered 
 - On AI Win
 - On AI Lose
 - When no user move was taken in the last 30 seconds
 
### Todo 
- Get the board hash working correctly such that I can improve preformance and not need to update player moves so often