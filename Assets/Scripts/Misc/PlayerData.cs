using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

[System.Serializable]
public class PlayerData
{
    public bool UserNameEntered, SlimeEggTapped, SlimeHatched, LoggedInFirstTime;

    public int wins, losts, NameLength,SlimeNameLength;
    public string pName = "";
    public Slime mySlime;

    public Quest thisQuest;
    public float timer;
    public bool leftAdventure, timerActivated, inQuest;
    public Inventory inventory;
    public PlayerData()
    {
        wins = -1;
        losts = -1;
        pName = string.Empty;
        mySlime = null;
    }

    public PlayerData(PlayerInfo player, MyQuestSaver mqs)
    {
        //PlayerInfo
        UserNameEntered = player.UserNameEntered;
        SlimeEggTapped = player.SlimeEggTapped;
        SlimeHatched = player.SlimeHatched;
        LoggedInFirstTime = player.LoggedInFirstTime;

        wins = player.wins;
        losts = player.losts;
        pName = player.pName;
        mySlime = player.mySlime;

        inventory = player.myInventory;

        //MyQuestSaver
        thisQuest = mqs.thisQuest;
        timer = mqs.timer;

        leftAdventure = mqs.leftAdventure;
        timerActivated = mqs.timerActivated;
        inQuest = mqs.inQuest;
    }

    public static byte[] Serialize(Object obj)
    {
        PlayerData data = (PlayerData)obj;
        //MyPName
        data.NameLength = data.pName.Length;
        byte[] myPNameByte = Encoding.ASCII.GetBytes(data.pName);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(myPNameByte);
        //MyPNameLength
        byte[] myPNameLengthsByte = BitConverter.GetBytes(data.NameLength);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(myPNameLengthsByte);
        //MyPWins
        byte[] myPWinsByte = BitConverter.GetBytes(data.wins);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(myPWinsByte);
        //MyPLoss
        byte[] myPLossByte = BitConverter.GetBytes(data.losts);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(myPLossByte);
        //Slime Serialize
        byte[] myPSlimeByte = SerializeSlime(data.mySlime);
        return JoinBytes(myPWinsByte, myPLossByte, myPNameLengthsByte, myPNameByte,myPSlimeByte);
    }

    public static Object Deserialize(byte[] bytes)
    {
        PlayerData data = new PlayerData();

        //PWins
        byte[] myWinsByt = new byte[4];
        Array.Copy(bytes, 0, myWinsByt, 0, myWinsByt.Length);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(myWinsByt);
        data.wins = BitConverter.ToInt32(myWinsByt, 0);
        //PLoss
        byte[] myLossByt = new byte[4];
        Array.Copy(bytes, 4, myLossByt, 0, myLossByt.Length);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(myWinsByt);
        data.losts = BitConverter.ToInt32(myLossByt, 0);
        //NameLength
        byte[] myNameByt = new byte[4];
        Array.Copy(bytes, 8, myNameByt, 0, myNameByt.Length);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(myNameByt);
        data.NameLength = BitConverter.ToInt32(myNameByt, 0);
        //PName
        byte[] myNameBytes = new byte[data.NameLength];
        if (myNameBytes.Length > 0)
        {
            Array.Copy(bytes, 12, myNameBytes, 0, myNameBytes.Length);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(myNameBytes);
            data.pName = Encoding.UTF8.GetString(myNameBytes);
        }
        else
        {
            data.pName = string.Empty;
        }
        //mySlime
        byte[] mySlimeBytes = new byte[bytes.Length - data.NameLength - 12];
        data.mySlime = new Slime((Slime)DeserializeSlime(mySlimeBytes));
        return data;
    }

    public static byte[] SerializeSlime(Object slimeobj)
    {
        Slime data = (Slime)slimeobj;
        //SlimeName
        data.NameLength = data.getNme().Length;
        byte[] SNameByte = Encoding.ASCII.GetBytes(data.getNme());
        if (BitConverter.IsLittleEndian)
            Array.Reverse(SNameByte);
        //MyPNameLength
        byte[] mySNameLengthsByte = BitConverter.GetBytes(data.NameLength);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(mySNameLengthsByte);
        //SlimeAttack
        byte[] SAtkByte = BitConverter.GetBytes(data.getAtk());
        if (BitConverter.IsLittleEndian)
            Array.Reverse(SAtkByte);
        //SlimeDefense
        byte[] SDefByte = BitConverter.GetBytes(data.getDef());
        if (BitConverter.IsLittleEndian)
            Array.Reverse(SDefByte);
        //SlimeSpeed
        byte[] SSpdByte = BitConverter.GetBytes(data.getSpd());
        if (BitConverter.IsLittleEndian)
            Array.Reverse(SSpdByte);
        //SlimeElement
        //own
        byte[] SOElemByte = BitConverter.GetBytes(data.getElement().OwnElement);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(SOElemByte);

        return JoinBytes(SAtkByte, SDefByte, SSpdByte, SOElemByte, mySNameLengthsByte, SNameByte);
    }

    public static Object DeserializeSlime(byte[] bytes)
    {
        Slime data = new Slime();

        //SATK
        byte[] mySATKByt = new byte[4];
        Array.Copy(bytes, 0, mySATKByt, 0, mySATKByt.Length);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(mySATKByt);
        data.setAtk(BitConverter.ToInt32(mySATKByt, 0));
        //SDEF
        byte[] mySDEFByt = new byte[4];
        Array.Copy(bytes, 4, mySDEFByt, 0, mySDEFByt.Length);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(mySDEFByt);
        data.setDef(BitConverter.ToInt32(mySDEFByt, 0));
        //SSPD
        byte[] mySSPDByt = new byte[4];
        Array.Copy(bytes, 8, mySSPDByt, 0, mySSPDByt.Length);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(mySSPDByt);
        data.setSpd(BitConverter.ToInt32(mySSPDByt, 0));
        //SOELEM
        byte[] mySOELEMByt = new byte[4];
        Array.Copy(bytes, 12, mySOELEMByt, 0, mySOELEMByt.Length);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(mySOELEMByt);
        data.setElement(BitConverter.ToInt32(mySOELEMByt, 0));
        //SlimeNameLength
        byte[] mySNameByt = new byte[4];
        Array.Copy(bytes, 16, mySNameByt, 0, mySNameByt.Length);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(mySNameByt);
        data.NameLength = BitConverter.ToInt32(mySNameByt, 0);
        //PName
        byte[] myNameBytes = new byte[data.NameLength];
        if (myNameBytes.Length > 0)
        {
            Array.Copy(bytes, 20, myNameBytes, 0, myNameBytes.Length);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(myNameBytes);
            data.setNme(Encoding.UTF8.GetString(myNameBytes));
        }
        else
        {
            data.setNme(string.Empty);
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
