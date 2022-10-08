# WACVR

Open Source VR Arcade Simulator

**About this project**
---

- Supports every game version
- The model is almost 1:1 to cabinet/framework
- Supports native touch input via serial (com0com required)
- Supports lights/LEDs (via hook)
- Customizable haptic feedback
- 3rd person camera and smooth camera
- 4 customizable buttons

**Supported platform**
---

- All SteamVR devices (Index，HTC，Oculus, etc.)
- All Oculus devices (Oculus Desktop App)
- Tested on: Quest 2 through Oculus link (Native and via SteamVR), ALVR and Virtual Desktop (via SteamVR).

**Used repository**
---

- [uWindowCapture](https://github.com/hecomi/uWindowCapture)
- [MaiDXR](https://github.com/xiaopeng12138/MaiDXR)

**Declaimer**
---

- This project is non-profit and some resources came from Internet!
- Although this is under the GPL-3.0 license, do not use any content of this repo in commercial/profitable scenarios without permission!
- Please support your local arcade if you can!

**How to use**
---

- Get the game somehow and make sure it will run properly. (DO NOT ASK ANYTHING THAT IS DIRECTLY RELATED TO THE GAME ITSELF)
- Download [the latest version of WACVR](https://github.com/xiaopeng12138/WACVR/actions)
- You have 2 ways to connect the touch to the game:

- **mercuryio:**
  - Download [mercuryio.dll](https://xpengs.com/s/wacvr/mercuryio.dll)
  - Put mercuryio.dll into ``bin`` folder
  - Add ``[mercuryio] path=mercuryio.dll`` to .ini file 
  - Start game and WACVR

- **Serial (not recommended):**

  - Download and install [com0com](https://storage.googleapis.com/google-code-archive-downloads/v2/code.google.com/powersdr-iq/setup_com0com_W7_x64_signed.exe).
  - Configure com0com to bind COM3 and COM5, COM4 and COM6.
  - You must enable the enable buffer option in com0com on both ports of all pairs. Otherwise, your WACVR will crash after the logo.
  - Add "[touch] enable=0" to .ini file
  - Start WACVR first then start the game.
  - If your touch is not working, try to enable somehow Test mod then exit Test mode.

- The lighting requires ``mercuryio.dll``. You must setup it up to get the light effect from the game. If you don't have the light effect, pls check if you are using the latest tools and if your LED hook works.

**Configuration**
---

A ``config.json`` is automatically created in the WACVR's root dir on startup

- You can change the ``config`` via the in-game config panel. Just please take a step back. The controller pointer will automatically be disabled when the controller is too close to the cabinet.
- Some extra option is only available in ``config.json``. For example:
    - ``"CaptureDesktopNumber:"``
    - ``"SpectatorSmooth:"``
- Some configs in ``config.json`` are only the index of the dropdown.
- You can use the pointer to point the third-person camera and grab it to the position where you want to be.

**Building guide**
---

- Current Unity version: 2021.3.11f1
- for mercuryio, just replace files in mercuryio folder with files in this repo.

**Current issue**
---

- display white screen issue
    **Solution:** Set game priority in the task manager to real-time may solve this issue. But the best way is just by capturing the entire screen.

Huge thanks to everyone that helped with this project!
If you want to add any function pls commit PR, I will accept it as soon as possible!
