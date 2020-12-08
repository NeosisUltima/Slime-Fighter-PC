using System;
using System.Linq;
using System.Text;

[System.Serializable]
public class SerializeData
{
    public string myPName = string.Empty;
    public int myPWins, myPLoss = -1;
    public Slime myPSlime = null;

    // Start is called before the first frame update
    public static byte[] Serialize(Object obj)
    {
        SerializeData data = (SerializeData)obj;
        //MyPName
        byte[] myPNameByte = Encoding.ASCII.GetBytes(data.myPName);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(myPNameByte);
        //MyPWins
        byte[] myPWinsByte = BitConverter.GetBytes(data.myPWins);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(myPWinsByte);
        //MyPLoss
        byte[] myPLossByte = BitConverter.GetBytes(data.myPLoss);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(myPLossByte);

        return JoinBytes(myPWinsByte, myPLossByte, myPNameByte);
    }

    public static Object Deserialize(byte[] bytes)
    {
        SerializeData data = new SerializeData();

        //PWins
        byte[] myWinsByt = new byte[4];
        Array.Copy(bytes, 0, myWinsByt, 0, myWinsByt.Length);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(myWinsByt);
        data.myPWins = BitConverter.ToInt32(myWinsByt, 0);
        //PLoss
        byte[] myLossByt = new byte[4];
        Array.Copy(bytes, 4, myLossByt, 0, myLossByt.Length);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(myWinsByt);
        data.myPWins = BitConverter.ToInt32(myWinsByt, 0);
        //PName
        byte[] myNameBytes = new byte[bytes.Length - 8];
        if (myNameBytes.Length > 0)
        {
            Array.Copy(bytes, 8, myNameBytes, 0, myNameBytes.Length);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(myNameBytes);
            data.myPName = Encoding.UTF8.GetString(myNameBytes);
        }
        else
        {
            data.myPName = string.Empty;
        }
        return data;
    }

    public static byte[] JoinBytes(params byte[][] arrays)
    {
        byte[] rv = new byte[arrays.Sum(a => a.Length)];
        int offset = 0;
        foreach(byte[] array in arrays)
        {
            System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
            offset += array.Length;
        }
        return rv;
    }

}
