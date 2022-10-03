using System.Collections;
using WindowsInput.Native;
using UnityEngine;

public class Config
{
    public captureMode CaptureMode = captureMode.BitBlt;
    public enum captureMode
    {
        None = 0,
        PrintWindow = 1,
        BitBlt = 2,
        WindowsGraphicCapture = 3,
        Auto = 4
    }
    public captureFPS CaptureFPS = captureFPS.FPS72;
    public enum captureFPS
    {
        FPS30 = 0,
        FPS60 = 1,
        FPS72 = 2,
        FPS90 = 3,
        FPS120 = 4,
        FPS144 = 5
    }
    public bool CaptureDesktop = false;
    public int CaptureDesktopNumber = 0;
    public spectatorMode SpectatorMode = spectatorMode.ThirdPerson;
    public enum spectatorMode
    {
        FirstPerson = 0,
        FirstPersonSmooth = 1,
        ThirdPerson = 2,
    }
    public spectatorFPS SpectatorFPS = spectatorFPS.FPS60;
    public enum spectatorFPS
    {
        FPS15 = 0,
        FPS30 = 1,
        FPS45 = 2,
        FPS60 = 3,
        FPS72 = 4,
        FPS90 = 5,
        FPS120 = 6,
        FPS144 = 7
    }
    public float SpectatorFOV = 40;
    public float SpectatorSmooth = 0.125f;
    public float[] TPCamPosition = new float[3] { -0.6f, 1.8f, -1.2f };
    public float[] TPCamRotation = new float[3] { 23, 35, 0 };
    public float HandSize = 8f;
    public float[] HandPosition = new float[3] { 0, 0, 0 };
    public int Skybox = 0;
    public float PlayerHeight = 0;
    public float HapticDuration = 0.1f;
    public float HapticAmplitude = 0.75f;
    public touchSampleRate TouchSampleRate  = touchSampleRate.FPS90;
    public enum touchSampleRate
    {
        FPS60 = 0,
        FPS72 = 1,
        FPS90 = 2,
        FPS120 = 3,
        FPS144 = 4,
        FPS160 = 5,
        FPS180 = 6,
        FPS200 = 7,
        FPS240 = 8,
        FPS280 = 9,
        FPS320 = 10,
    }
    public handStabilization HandStabilizationMode = handStabilization.None;
    public enum handStabilization
    {
        None = 0,
        Velocity = 1,
        Distance = 2,
        Smooth = 3,
    }
    public float HandStabilVelocity = 0.1f;
    public float HandStabilDistance = 0.1f;
    public float HandStabilSmooth = 0.1f;
    public bool useLight = true;
    public bool useIPCLighting = true;
    public bool useIPCTouch = true;
    public VirtualKeyCode TestKey = VirtualKeyCode.INSERT;
    public VirtualKeyCode ServiceKey = VirtualKeyCode.DELETE;
    public VirtualKeyCode CoinKey = VirtualKeyCode.HOME;
    public VirtualKeyCode CustomKey = VirtualKeyCode.NONAME;
}
