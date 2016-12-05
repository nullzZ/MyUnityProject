using UnityEngine;
using System.Collections;
using System;

public class AbsEncorder
{
    //List<byte[]> msgList;

    public byte[] encorder(byte[] bytes, int index)
    {
        byte[] head = new byte[2];
        int headLengthIndex = index + 2;
        Array.Copy(bytes, index, head, 0, 2);
        short length = BitConverter.ToInt16(head, 0);
        if (length > 0)
        {
            byte[] data = new byte[length];
            Array.Copy(bytes, headLengthIndex, data, 0, length);
            //JFPackage.WorldPackage wp = new JFPackage.WorldPackage ();
            //wp = (JFPackage.WorldPackage)BytesToStruct (data, wp.GetType ());	
            //worldpackage.Add (data);
            index = headLengthIndex + length;
            return data;
        }
        else
        {
            return null;
        }
    }

    private void SplitPackage(byte[] bytes, int index)
    {
        while (true)
        {
            byte[] head = new byte[2];
            int headLengthIndex = index + 2;
            Array.Copy(bytes, index, head, 0, 2);
            short length = BitConverter.ToInt16(head, 0);
            if (length > 0)
            {
                byte[] data = new byte[length];
                Array.Copy(bytes, headLengthIndex, data, 0, length);
                //JFPackage.WorldPackage wp = new JFPackage.WorldPackage ();
                //wp = (JFPackage.WorldPackage)BytesToStruct (data, wp.GetType ());	
                //worldpackage.Add (data);
                index = headLengthIndex + length;
            }
            else
            {
                break;
            }
        }
    }


}
