using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

[System.Serializable]
public class Element
{
    /*
     * Fire: WK: Water STR: Nature
     * Water: WK: Electric STR: Fire
     * Nature: WK: Fire STR: Electric
     * Electric: WK: Nature STR: Water
     * Dark: WK: Light STR: Dark
     * Light: WK: Dark STR: Light
     */

    private static int FIRE = 0;
    private static int WATER = 1;
    private static int NATURE = 2;
    private static int ELECTRIC = 3;
    private static int DARK = 4;
    private static int LIGHT = 5;
    private static int NULL = 6;

    public int OwnElement, ResistantElement, WeakElement;
    public string MySlimeElement;
    public string MyWeakElement;
    public string MyStrongElement;

    public Element(int index)
    {
        switch (index)
        {
            case 0:
                OwnElement = FIRE;
                MySlimeElement = "Fire";
                break;
            case 1:
                OwnElement = WATER;
                MySlimeElement = "Fire";
                break;
            case 2:
                OwnElement = NATURE;
                MySlimeElement = "Nature";
                break;
            case 3:
                OwnElement = ELECTRIC;
                MySlimeElement = "Electric";
                break;
            case 4:
                OwnElement = DARK;
                MySlimeElement = "Dark";
                break;
            case 5:
                OwnElement = LIGHT;
                MySlimeElement = "Light";
                break;
            default:
                OwnElement = NULL;
                break;
        }

        SetElement(index);
    }

    public void SetElement(int elem)
    {
        ResistantElement = Resistance(elem);
        WeakElement = Weakness(elem);
    }

    public int Weakness(int elem) {
        int wk;
        switch (elem)
        {
            case 0:
                wk = WATER;
                MyWeakElement = "Water";
                break;
            case 1:
                wk = ELECTRIC;
                MyWeakElement = "Electric";
                break;
            case 2:
                wk = FIRE;
                MyWeakElement = "Fire";
                break;
            case 3:
                wk = NATURE;
                MyWeakElement = "Nature";
                break;
            case 4:
                wk = LIGHT;
                MyWeakElement = "Light";
                break;
            case 5:
                wk = DARK;
                MyWeakElement = "Dark";
                break;
            default: 
                wk = NULL;
                break;
        }
        return wk;
    }

    public int Resistance(int elem)
    {
        int wk;
        switch (elem)
        {
            case 0:
                wk = NATURE;
                MyStrongElement = "Nature";
                break;
            case 1:
                wk = FIRE;
                MyStrongElement = "Fire";
                break;
            case 2:
                wk = ELECTRIC;
                MyStrongElement = "Electric";
                break;
            case 3:
                wk = WATER;
                MyStrongElement = "Water";
                break;
            case 4:
                wk = DARK;
                MyStrongElement = "Dark";
                break;
            case 5:
                wk = LIGHT;
                MyStrongElement = "Light";
                break;
            default:
                wk = NULL;
                break;
        }
        return wk;
    }

    /*public int returnElement(int n)
    {
        int thisElement;

        if (n >= 0 && n <= 22) n = 0;
        else if (n >= 23 && n <= 45) n = 1;
        else if (n >= 46 && n <= 68) n = 2;
        else if (n >= 69 && n <= 90) n = 3;
        else if (n >= 91 && n <= 95) n = 4;
        else if (n >= 96 && n <= 100) n = 5;

        switch (n)
        {
            case 0:
                thisElement = FIRE;
                break;
            case 1:
                thisElement = WATER;
                break;
            case 2:
                thisElement = NATURE;
                break;
            case 3:
                thisElement = ELECTRIC;
                break;
            case 4:
                thisElement = DARK;
                break;
            case 5:
                thisElement = LIGHT;
                break;
            default:
                thisElement = NULL;
                break;
        }

        return thisElement;
    }*/
}
