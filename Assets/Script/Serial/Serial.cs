using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO.Ports;
using System.Linq;
using UnityEngine;
public class Serial : MonoBehaviour
{

    const byte CMD_GET_SYNC_BOARD_VER = 0xa0;
    const byte CMD_NEXT_READ = 0x72;
    const byte CMD_GET_UNIT_BOARD_VER = 0xa8;
    const byte CMD_MYSTERY1 = 0xa2;
    const byte CMD_MYSTERY2 = 0x94;
    const byte CMD_START_AUTO_SCAN = 0xc9;
    const byte CMD_BEGIN_WRITE = 0x77;
    const byte CMD_NEXT_WRITE = 0x20;

    private Thread _touchThread;
    private Queue _touchQueue;

    static SerialPort ComL = new SerialPort ("COM5", 115200);
    static SerialPort ComR = new SerialPort ("COM6", 115200);

    byte inByte;
    string SYNC_BOARD_VER = "190523";
    string UNIT_BOARD_VER = "190514";
    string read1 = "    0    0    1    2    3    4    5   15   15   15   15   15   15   11   11   11";
    string read2 = "   11   11   11  128  103  103  115  138  127  103  105  111  126  113   95  100";
    string read3 = "  101  115   98   86   76   67   68   48  117    0   82  154    0    6   35    4";

    byte[] SettingData_160 = new byte[8] {160, 49, 57, 48, 53, 50, 51, 44};
    byte[] SettingData_162 = new byte[3] {162, 63, 29};
    byte[] SettingData_148 = new byte[3] {148, 0, 20};
    byte[] SettingData_201 = new byte[3] {201, 0, 73};
    static byte[] TouchPackL = new byte[36];
    static byte[] TouchPackR = new byte[36];
    public static bool[] TouchPackAll = new bool[240];
    bool StartUp = false;
    void Start()
    {
        try
        {
            ComL.Open();
            ComR.Open();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to Open Serial Ports: {ex}");
        }
        //Debug.Log("Touch Serial Initializing..");
        //Send touch update periodically to keep "read" alive
        _touchQueue = Queue.Synchronized(new Queue());
        _touchThread = new Thread(TouchThreadLoop);
        InvokeRepeating("PingTouchThread", 0, 1);
        //Send touch updates whenever actual state changes to achieve desired update frequency without overloading
        ColliderToSerial.touchDidChange += PingTouchThread;
    }

    private void PingTouchThread()
    {
        _touchQueue.Enqueue(1);
    }

    private void TouchThreadLoop()
    {
        while(true)
        {
            if(_touchQueue.Count > 0)
            {
                _touchQueue.Dequeue();
                SendTouchState();
            }
        }
    }
    private void OnDestroy()
    {
        ComL.Close();
        ComR.Close();
    }
    void Update()
    {
        if(ComL.IsOpen)
            ReadHead(ComL, 0);
        if (ComR.IsOpen)
            ReadHead(ComR, 1);
        //  if (Input.GetKeyDown(KeyCode.M)) //this is a touch test code
        //      StartCoroutine(TouchTest(true));
        if (Input.GetKeyDown(KeyCode.M) && StartUp)
            SendTouchState();
    }

    private void SendTouchState()
    {
        if(StartUp)
        {
            //Debug.Log("Sending Touch State");
            // Debug.Log("Sending Left");
            SendTouch(ComL, TouchPackL);
            //  Debug.Log("Sending Right");
            SendTouch(ComR, TouchPackR);
        }
    }

    IEnumerator TouchTest(bool State) //this is a touch test code
    {
        for (int i = 0; i < 240; i++)
        {
            SetTouch(i, true);
            Debug.Log(i);
            yield return new WaitForSeconds(0.05f);
            SetTouch(i, false);
            yield return new WaitForSeconds(0.05f);
        }
    }

    void ReadHead(SerialPort Serial, int side) //Read head byte
    {
        if(Serial.BytesToRead > 0)
        {
            inByte = Convert.ToByte(Serial.ReadByte());
            var data = Serial.ReadExisting();
            SendResp(Serial, side, data);
        }
    }
    void SendResp(SerialPort Serial, int side, string data)
    {
        switch(inByte)
        {
            case CMD_GET_SYNC_BOARD_VER:
                //Response: cmd byte + sync board ver + checksum
                StartUp = false;
                List<byte> syncbytes = new List<byte>();
                syncbytes.Add(inByte);
                syncbytes.AddRange(ByteHelper.ConvertStringToByteArray(SYNC_BOARD_VER));
                byte syncCheckSum = (byte)44;
                syncbytes.Add(syncCheckSum);
                Serial.Write(syncbytes.ToArray(), 0, syncbytes.Count);
                //Debug.Log($"GET SYNC BOARD VER {side}");
                break;
            case CMD_NEXT_READ:
                //Response: corresponding read bytes + checksum
                StartUp = false;
                //Debug.Log($"Side {side} NEXT READ {Convert.ToByte(data[2])}");
                switch (Convert.ToByte(data[2]))
                {
                    case 0x30:
                        var bytes = ByteHelper.ConvertStringToByteArray(read1);
                        bytes.Add(ByteHelper.CalCheckSum(bytes.ToArray(), bytes.Count));
                        //Debug.Log("Read 1");
                        Serial.Write(bytes.ToArray(), 0, bytes.Count);
                        break;
                    case 0x31:
                        var bytes2 = ByteHelper.ConvertStringToByteArray(read2);
                        bytes2.Add(ByteHelper.CalCheckSum(bytes2.ToArray(), bytes2.Count));
                        //Debug.Log("Read 2");
                        Serial.Write(bytes2.ToArray(), 0, bytes2.Count);
                        break;
                    case 0x33:
                        var bytes3 = ByteHelper.ConvertStringToByteArray(read3);
                        bytes3.Add(ByteHelper.CalCheckSum(bytes3.ToArray(), bytes3.Count));
                        //Debug.Log("Read 3");
                        Serial.Write(bytes3.ToArray(), 0, bytes3.Count);
                        break;
                    default:
                        //Debug.Log("Extra Read");
                        break;
                }
                break;
            case CMD_GET_UNIT_BOARD_VER:
                //Response: cmd byte + sync board ver bytes + 'L'/'R' based on side + unit board ver bytes x6 + checksum
                StartUp = false;
                List<byte> unitBytes = new List<byte>();
                byte sideByte = (side == 0 ? Convert.ToByte('R') : Convert.ToByte('L'));
                byte unitCheckSum = (side == 0 ? (byte)118 : (byte)104);
                unitBytes.Add(inByte);
                unitBytes.AddRange(ByteHelper.ConvertStringToByteArray(SYNC_BOARD_VER));
                unitBytes.Add(sideByte);
                for (int i = 0; i < 6; i++)
                        unitBytes.AddRange(ByteHelper.ConvertStringToByteArray(UNIT_BOARD_VER));
                unitBytes.Add(unitCheckSum);
                Serial.Write(unitBytes.ToArray(), 0, unitBytes.Count);
                //Debug.Log($"GET UNIT BOARD VER {side}");
                break;
            case CMD_MYSTERY1:
                StartUp = false;
                Serial.Write(SettingData_162, 0, 3);
                //Debug.Log($"MYSTERY 1 SIDE {side}");
                break;
            case CMD_MYSTERY2:
                StartUp = false;
                Serial.Write(SettingData_148, 0, 3);
                //Debug.Log($"MYSTERY 2 SIDE {side}");
                break;
            case CMD_START_AUTO_SCAN:
                Serial.Write(SettingData_201.ToArray(), 0, 3);
                //Debug.Log($"START AUTO SCAN SIDE {side}");
                StartUp = true;
                if (!_touchThread.IsAlive)
                    _touchThread.Start();
                break;
            case CMD_BEGIN_WRITE:
          //      Debug.Log($"Begin Write For Side {side}");
                break;
            case CMD_NEXT_WRITE:
          //      Debug.Log($"Continue Write For Side {side}");
                break;
            case 154:
                StartUp = false;
                //Debug.Log("BAD");
                break;
        }
    }

    byte[] GetTouchPack(byte[] Pack) //convert touch to right format for game
    {
        Pack[0] = 129;
        Pack[34] = Pack[34]++;
        Pack[35] = 128;
        Pack[35] = ByteHelper.CalCheckSum(Pack, 36);
        if (Pack[34] > 127)
            Pack[34] = 0;
        return Pack;
    }
    void SendTouch(SerialPort Serial, byte[] Pack) //Send touch data
    {
        if (StartUp)
            Serial.Write(GetTouchPack(Pack), 0, 36);
    }
    public static void SetTouch(int Area, bool State) //set touch data 0-239
    {
        //Area +=1;
        if (Area < 121)
        {
            Area += (Area-1) / 5 * 3 + 7; 
            ByteHelper.SetBit(TouchPackR, Area, State);
        }
        else if (Area >= 120)
        {
            Area -= 120;
            Area += (Area-1) / 5 * 3 + 7; 
            ByteHelper.SetBit(TouchPackL, Area, State);
        }
        TouchPackAll[Area] = State;
    }
}

public static class ByteHelper
{
    public static byte[] SetBit(this byte[] self, int index, bool value)
    { 
        var bitArray = new BitArray(self);
        bitArray.Set(index, value);
        bitArray.CopyTo(self, 0);
        return self;
    }
    public static byte CalCheckSum(byte[] _PacketData,int PacketLength)
    {
        Byte _CheckSumByte = 0x00;
        for (int i = 0; i < PacketLength; i++)
            _CheckSumByte ^= _PacketData[i];
        return _CheckSumByte;
    }
    public static List<byte> ConvertStringToByteArray(string data)
    {
        List<byte> tempList = new List<byte>(100);
        for(int i = 0; i < data.Length; i++)
            tempList.Add(Convert.ToByte(data[i]));
        return tempList;
    }
}
