# WACVR

A VR arcade emulator

## Current stage

- Successfully started the game itself
- Successfully initialized touch
- Successfully send touch signal
- Successfully enabled Freeplay and tested the touch signal in-game
- Successfully got led data from the game

## Current issue
- capture display white screen issue (set game priority in taskmanager to real-time may solve)

## Quick guide

- Port binding is the same as my other repo [MaiDXR](https://github.com/xiaopeng12138/MaiDXR)
- add "[touch] enable=0" to ini file
- The LED requires a special fork of the tools. Currently, it's under my fork of the tools but it's not in the Github! You need to build by yourself and replace the things inside ``mercuryio`` folder whit the one under this repo.

## Configuration

A ``config.json`` is automatically created in the WACVR root on startup

- You can change the ``config`` now via the in-game config panel
- The ``config panel`` is still in the WIP stage, so some functions may not work.

## When release?

- Currently, you can get the latest build-in actions (in artifacts)
- The first release version will release after 8.31

Huge thanks to everyone that helped with this project!
If you want to add any function pls commit PR, I will accept it as soon as possible!