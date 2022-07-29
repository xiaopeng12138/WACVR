# WACVR

A VR arcade emulator

## Current stage

- Successfully started the game itself
- Successfully initialized touch
- Successfully send touch signal
- Successfully enabled Freeplay and tested the touch signal in game
- Successfully got led data from game

## Current issue
- capture display white screen issue (set game priority in taskmanager to real-time may solve)

## Quick guide

- Port binding is same as my other repo [MaiDXR](https://github.com/xiaopeng12138/MaiDXR)
- add "[touch] enable=0" to ini file
- The led requiere a special fork of the tools. Currently it's under my fork of the tools and it's not in the github! You need to build by your self and replace the things inside mercuryio folder whit the one under this repo.

## Configuration

A ``config.json`` is automatically created in the WACVR root on startup

- ``useSkybox``: Enable Skybox and hide the room (Default: false)
- ``Skybox``: the current skybox selected for use (Default: 0)
- ``Height``: the offset from default height that the player is moved (Default: 0.0)
- ``HandSize``: the size of hands (Default: 7.0)
- ``HandPosition``: the offset of hand position (Default: [1.0, 1.0, -3.0],)
- ``ThirdPerson``: whether or not the camera is in third person (Default: true)
- ``CaptureMode``: the method uWindowCapture will use for window capture
  - ``0``: PrintScreen
  - ``1``: BitBlt
  - ``2``: Windows Graphics Capture
  - ``3``: Automatic (Recommended, Default)
- ``CaptureFramerate``: the framerate to capture the game at (Default: 60)
- ``CaptureDesktop``: whether to capture the specific window or a full monitor
- ``CaptureDesktopNumber``: the monitor to capture if you're capturing a full monitor
- ``PhysicFPS``: the unity physic interval, lower value can prevent false touch but will also cause higher latency (Default: 90)
- ``useIPC``: the touch panle LED light mode, requiere mercuryio and new tools (Default: true)

## When release?
- currently you can get the latest build in actions (in artifacts)
- the first release version will release after 8.31

Huge thanks to everyone that helped this project!
If you want to add any function pls commit PR, I will accept it as soon as possible!