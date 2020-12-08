using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[System.Serializable]
public class SerializeSlimeData
{
    public Slime myPSlime = null;

    public static byte[] SerializeSlime(Object slimeobj)
    {
        SerializeSlimeData data = (SerializeSlimeData)slimeobj;
        //SlimeName
        byte[] SNameByte = Encoding.ASCII.GetBytes(data.myPSlime.getNme());
        if (BitConverter.IsLittleEndian)
            Array.Reverse(SNameByte);
        //SlimeAttack
        byte[] SAtkByte = BitConverter.GetBytes(data.myPSlime.getAtk());
        if (BitConverter.IsLittleEndian)
            Array.Reverse(SAtkByte);
        //SlimeDefense
        byte[] SDefByte = BitConverter.GetBytes(data.myPSlime.getDef());
        if (BitConverter.IsLittleEndian)
            Array.Reverse(SDefByte);
        //SlimeSpeed
        byte[] SSpdByte = BitConverter.GetBytes(data.myPSlime.getSpd());
        if (BitConverter.IsLittleEndian)
            Array.Reverse(SSpdByte);
        //SlimeElement
        //own
        byte[] SOElemByte = BitConverter.GetBytes(data.myPSlime.getElement().OwnElement);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(SOElemByte);

        return JoinBytes(SAtkByte, SDefByte, SSpdByte, SOElemByte, SNameByte);
    }

    public static Object DeserializeSlime(byte[] bytes)
    {
        SerializeSlimeData data = new SerializeSlimeData();

        //SATK
        byte[] mySATKByt = new byte[4];
        Array.Copy(bytes, 0, mySATKByt, 0, mySATKByt.Length);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(mySATKByt);
        data.myPSlime.setAtk(BitConverter.ToInt32(mySATKByt, 0));
        //SDEF
        byte[] mySDEFByt = new byte[4];
        Array.Copy(bytes, 4, mySDEFByt, 0, mySDEFByt.Length);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(mySDEFByt);
        data.myPSlime.setDef(BitConverter.ToInt32(mySDEFByt, 0));
        //SSPD
        byte[] mySSPDByt = new byte[4];
        Array.Copy(bytes, 8, mySSPDByt, 0, mySSPDByt.Length);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(mySSPDByt);
        data.myPSlime.setSpd(BitConverter.ToInt32(mySSPDByt, 0));
        //SOELEM
        byte[] mySOELEMByt = new byte[4];
        Array.Copy(bytes, 12, mySOELEMByt, 0, mySOELEMByt.Length);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(mySOELEMByt);
        data.myPSlime.setElement(BitConverter.ToInt32(mySOELEMByt, 0));
        //PName
        byte[] myNameBytes = new byte[bytes.Length - 16];
        if (myNameBytes.Length > 0)
        {
            Array.Copy(bytes, 16, myNameBytes, 0, myNameBytes.Length);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(myNameBytes);
            data.myPSlime.setNme(Encoding.UTF8.GetString(myNameBytes));
        }
        else
        {
            data.myPSlime.setNme(string.Empty);
        }

        return data;
    }

    public static byte[] JoinBytes(params byte[][] arrays)
    {
        byte[] rv = new byte[arrays.Sum(a => a.Length)];
        int offset = 0;
        foreach (byte[] array in arrays)
        {
            System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
            offset += array.Length;
        }
        return rv;
    }
}
