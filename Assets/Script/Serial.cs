using UnityEngine;
using System.IO.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
public class Serial : MonoBehaviour
{
    static SerialPort ComL = new SerialPort ("COM5", 115200);
    static SerialPort ComR = new SerialPort ("COM6", 115200);
    List<byte> inBytes;
    List<byte> Bytes;
    byte inByte;
    byte[] SettingData_160 = new byte[8];
    byte[] SettingData_114 = new byte[81];
    byte[] SettingData_168 = new byte[45];
    byte[] SettingData_162 = new byte[7];
    byte[] SettingData_148 = new byte[7];
    byte[] SettingData_201 = new byte[7];
    int TouchPackCounter = 0;
    static byte[] TouchPackL = new byte[36];
    static byte[] TouchPackR = new byte[36];
    public TextAsset SettingData_114_Text;
    public TextAsset SettingData_168_Text;
    bool StartUp = false;
    void Start()
    {
        ComL.Open();
        ComR.Open();
        Debug.Log("Touch Serial Started");
        SetSettingData_160();
        SetSettingData_201();
        SetSettingData_162();
        SetSettingData_148();
        SettingData_114 = ByteHelper.ConvertTextToByteArray(SettingData_114_Text);
        SettingData_168 = ByteHelper.ConvertTextToByteArray(SettingData_168_Text);
    }

    // Update is called once per frame
    void Update()
    {
        ReadHead(ComL);
        ReadHead(ComR);
        //SendTouch(ComL, TouchPackL);
        //SendTouch(ComR, TouchPackR);
        if (Input.GetKeyDown(KeyCode.M))
            StartCoroutine(Test(true));
    }

    private void FixedUpdate() {
        SendTouch(ComL, TouchPackL);
        SendTouch(ComR, TouchPackR);
    }

    IEnumerator Test(bool State)
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

    void ReadHead(SerialPort Serial)
    {
        while (Serial.BytesToRead > 0)
        {
            inByte = Convert.ToByte(Serial.ReadByte());
            if (inByte == 144 || inByte == 148 || inByte == 154 || inByte == 160  || inByte == 162 || inByte == 168 || inByte == 201)
            {
                SendResp(Serial);
                break;
            }
        }
    }
    void SendResp(SerialPort Serial)
    {
        switch(inByte)
        {
            case 160:
                StartUp = false;
                Serial.Write(SettingData_160, 0, SettingData_160.Length);
                Debug.Log(SettingData_160.Length);
                //Bytes.Clear();
                break;
            case 114:
                StartUp = false;
                Debug.Log(114);
                Serial.Write(SettingData_114, 0, 81);
                //Bytes.Clear();
                break;
            case 168:
                StartUp = false;
                Serial.Write(SettingData_168, 0, 45);
                Debug.Log(168);
                //Bytes.Clear();
                break;
            case 162:
                StartUp = false;
                Serial.Write(SettingData_162, 0, 3);
                Debug.Log(162);
                Debug.Log(SettingData_162.Length);
                Debug.Log("RX: "+SettingData_162[0]+"-"+
                                    SettingData_162[1]+"-"+
                                    SettingData_162[2]);
                //Bytes.Clear();
                break;
            case 148:
                StartUp = false;
                Serial.Write(SettingData_148, 0, 3);
                Debug.Log(148);
                //Bytes.Clear();
                break;
            case 201:
                Serial.Write(SettingData_201.ToArray(), 0, 3);
                Debug.Log(201);
                //Bytes.Clear();
                StartUp = true;
                break;
            case 154:
                StartUp = false;
                Debug.Log("BAD");
                //Bytes.Clear();
                break;
        }
    }

    byte[] GetTouchPack(byte[] Pack)
    {
        Pack[0] = 129;
        Pack[34] = Pack[34]++;
        Pack[35] = 128;
        Pack[35] = ByteHelper.CalCheckSum(Pack, 36);
        if (Pack[34] > 127)
            Pack[34] = 0;
        return Pack;
    }
    void SendTouch(SerialPort Serial, byte[] Pack)
    {
        if (StartUp)
            Serial.Write(GetTouchPack(Pack), 0, 36);
    }
    public static void SetTouch(int Area, bool State)
    {
        Area +=1;
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
    }

    void SetSettingData_160()
    {
        SettingData_160[0]=160;
        SettingData_160[1]=49;
        SettingData_160[2]=57;
        SettingData_160[3]=48;
        SettingData_160[4]=53;
        SettingData_160[5]=50;
        SettingData_160[6]=51;
        SettingData_160[7]=44;
    }
    void SetSettingData_201()
    {
        SettingData_201[0]=201;
        SettingData_201[1]=0;
        SettingData_201[2]=73;
    }
    void SetSettingData_162()
    {
        SettingData_162[0]=162;
        SettingData_162[1]=63;
        SettingData_162[2]=29;
    }
    void SetSettingData_148()
    {
        SettingData_148[0]=148;
        SettingData_148[1]=0;
        SettingData_148[2]=20;
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
    public static byte[] ConvertTextToByteArray(TextAsset TextObj)
    {
        var splitedData = TextObj.text.Split(Convert.ToChar("\n"));
        byte[] tempList = new byte[100];
        for (int i = 0; i < splitedData.Length; i++)
            tempList[i] = Convert.ToByte(splitedData[i]);
        return tempList;
    }
}
