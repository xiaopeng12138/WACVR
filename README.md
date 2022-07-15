# WACVR

A VR arcade emulator

## Current stage

- Successfully started the game itself
- Successfully initialized touch
- Successfully send touch signal
- Successfully enabled Freeplay and tested the touch signal in game

## Current issue
- capture display white screen issue (set game priority in taskmanager to real-time may solve)

## Quick guide

- Port binding is same as my other repo MaiDXR
- add "[touch] enable=0" to ini file

## Configuration

A ``config.json`` is automatically created in the WACVR root on startup

- ``Skybox``: the current skybox selected for use (Default: 0)
- ``Height``: the offset from default height that the player is moved (Default: 0.0)
- ``ThirdPerson``: whether or not the camera is in third person (Default: true)
- ``CaptureMode``: the method uWindowCapture will use for window capture
  - ``0``: PrintScreen
  - ``1``: BitBlt
  - ``2``: Windows Graphics Capture
  - ``3``: Automatic (Recommended, Default)
- ``CaptureFramerate``: the framerate to capture the game at (Default: 60)
- ``CaptureDesktop``: whether to capture the specific window or a full monitor
- ``CaptureDesktopNumber``: the monitor to capture if you're capturing a full monitor

## Why this repo now?

I don't have much time and enough skills to make this project by myself. I'm not familiar with Unreal Engine etc. So I want this project to be a community project.
