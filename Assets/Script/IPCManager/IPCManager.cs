using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Security.Principal;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class IPCManager : MonoBehaviour
{
    public static MemoryMappedFile sharedBuffer;
    public static MemoryMappedViewAccessor sharedBufferAccessor;
    public static bool isInitialized = false;
    public static bool[] TouchData = new bool[240];

    private void Awake() 
    {
        EnsureInitialization();
    }
    private static void EnsureInitialization()
    {
        if (!isInitialized)
            InitializeIPC("Local\\WACVR_SHARED_BUFFER", 2164);
    }

    private IEnumerator ReconnectWait()
    {
        yield return new WaitForSeconds(5);
        InitializeIPC("Local\\WACVR_SHARED_BUFFER", 2164);
    }

    private void Reconnect()
    {
        InitializeIPC("Local\\WACVR_SHARED_BUFFER", 2164);
    }

    private static void InitializeIPC(string sharedMemoryName, int sharedMemorySize)
    {
        MemoryMappedFileSecurity CustomSecurity = new MemoryMappedFileSecurity();
        SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
        var acct = sid.Translate(typeof(NTAccount)) as NTAccount;
        CustomSecurity.AddAccessRule(new System.Security.AccessControl.AccessRule<MemoryMappedFileRights>(acct.ToString(), MemoryMappedFileRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));
        sharedBuffer = MemoryMappedFile.CreateOrOpen(sharedMemoryName, sharedMemorySize, MemoryMappedFileAccess.ReadWrite, MemoryMappedFileOptions.None, CustomSecurity, System.IO.HandleInheritability.Inheritable);
        sharedBufferAccessor = sharedBuffer.CreateViewAccessor();
        isInitialized = true;
    }

    public static byte[] GetLightData()
    {
        EnsureInitialization();
        //byte[] bytes = new byte[1920];
        //IPCManager.sharedBufferAccessor.ReadArray<byte>(244, bytes, 0, 1920);
        //if (bytes[3] == 0)
            //return null;
        return ReadBytes(244, 1920);
    }

    // Source: https://stackoverflow.com/questions/7956167/how-can-i-quickly-read-bytes-from-a-memory-mapped-file-in-net/7956222#7956222
    private static unsafe byte[] ReadBytes(int offset, int num)
    {
        byte[] arr = new byte[num];
        byte *ptr = (byte*)0;
        sharedBufferAccessor.SafeMemoryMappedViewHandle.AcquirePointer(ref ptr);
        Marshal.Copy(IntPtr.Add(new IntPtr(ptr), offset), arr, 0, num);
        sharedBufferAccessor.SafeMemoryMappedViewHandle.ReleasePointer();
        return arr;
    }

    private void OnDestroy() {
        Debug.Log("Disposing IPC");
        //CallAfterDelay.Create(0.1f, () => {
            //StartCoroutine(DisposeWait());
       //});
       Dispose();
    }

    private static IEnumerator DisposeWait()
    {
        IPCManager.sharedBufferAccessor.Write(244 + 3, 0); // Clear the flag
        yield return new WaitForSeconds(0.1f); // Wait if the IPC is still in use
        IPCManager.sharedBufferAccessor.Read<byte>(244 + 3, out byte flag); // Get the flag again
        if (flag == 0)
        {
            Dispose();
        }
    }

    private static void Dispose()
    {
        if (sharedBuffer != null)
        {
            IPCManager.sharedBuffer.Dispose();
            IPCManager.sharedBuffer = null;
            IPCManager.sharedBufferAccessor = null;
            IPCManager.isInitialized = false;
        }
        Debug.Log("IPC Disposed");
    }

    private static void SetTouchData(bool[] datas)
    {
        EnsureInitialization();
        IPCManager.sharedBufferAccessor.WriteArray<bool>(4, datas, 0, 240);
    }

    public static void SetTouch(int Area, bool State)
    {
        Area -= 1; //0-239

        if (Area < 120) //right side
            TouchData[Area + 120] = State;
        else if (Area >= 120) //left side
            TouchData[Area - 120] = State;

        SetTouchData(TouchData);
    }
}
