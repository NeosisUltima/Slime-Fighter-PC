using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Slime
{
    //ATTACK,DEFENSE,SPEED
    private static int MAX_STAT = 250;
    private static int MAX_HEALTH = 100;

    private int health;
    [SerializeField]
    private int atk, def, spd, evolutionstage;
    public int EVOLUTION_THRESHOLD;
    [SerializeField]
    private string nAme;
    [SerializeField]
    private Element elem;

    public int NameLength = 0;
    public int slimeIteration = 0;
    public int SlimeWins = 0;

    
    public Slime()
    {
        nAme = "";
        atk = 0;
        def = 0;
        spd = 0;
        evolutionstage = 1;
        NameLength = 0;
        health = MAX_HEALTH;
        setElement(10000);
        slimeIteration = 1;
        SlimeWins = 0;
        EVOLUTION_THRESHOLD = 15;
    }

    public Slime(Slime prev)
    {
        this.atk = prev.atk;
        this.def = prev.def;
        this.spd = prev.spd;
        this.elem = prev.elem;
        this.nAme = prev.nAme;
        this.evolutionstage = 1;
        health = MAX_HEALTH;
        this.NameLength = nAme.Length;
        this.slimeIteration = prev.slimeIteration;
        SlimeWins = 0;
        EVOLUTION_THRESHOLD = 15;
    }

    public Slime(string name, int atk, int spd, int def, int elem, int evol)
    {
        nAme = name;
        this.atk = atk;
        this.spd = spd;
        this.def = def;
        this.elem = new Element(elem);
        this.evolutionstage = evol;
        health = MAX_HEALTH;
        NameLength = nAme.Length;
        this.slimeIteration = 1;
        SlimeWins = 0;
        EVOLUTION_THRESHOLD = 15;
    }

    public Slime(Slime olderSlime, string name, int elemSet)
    {
        nAme = name;
        if (olderSlime.getAtk()/5 > 5) atk = UnityEngine.Random.Range(5,(olderSlime.getAtk() / 5)); else setAtk(5);
        if (olderSlime.getDef()/5 > 5) def = UnityEngine.Random.Range(5,(olderSlime.getDef() / 5)); else setDef(5);
        if (olderSlime.getSpd()/5 > 5) spd = UnityEngine.Random.Range(5,(olderSlime.getSpd() / 5)); else setSpd(5);
        int rand = (elemSet == -1) ? Mathf.FloorToInt(UnityEngine.Random.Range(0.0f, 10000)) % 6 : elemSet ;
        elem = new Element(rand);
        evolutionstage = 1;
        health = MAX_HEALTH;
        NameLength = nAme.Length;
        slimeIteration = olderSlime.slimeIteration++;
        SlimeWins = 0;
        EVOLUTION_THRESHOLD = 15;
    }

    public Slime(string nme,int elemSet)
    {
        nAme = nme;
        int rand = (elemSet == -1) ? Mathf.FloorToInt(UnityEngine.Random.Range(0.0f, 10000)) % 6 : elemSet;
        setAtk(5);
        setDef(5);
        setSpd(5);
        setElement(rand);
        evolutionstage = 1;
        health = MAX_HEALTH;
        NameLength = nAme.Length;
        slimeIteration = 1;
        SlimeWins = 0;
        EVOLUTION_THRESHOLD = 15;
    }

    public int getAtk() { return atk; }
    public int getDef() { return def; }
    public int getSpd() { return spd; }
    public string getNme() { return nAme; }
    public int getHlth() { return health; }
    public int getSlimeIndex() { return getSlimeElement(); }
    public int getSlimeElement() { return elem.OwnElement; }
    public int getSlimeStrElement() { return elem.ResistantElement; }
    public int getSlimeWkElement() { return elem.WeakElement; }
    public int getSlimeEvolutionStage() { return evolutionstage; }
    public Element getElement() { return elem; }

    public void setAtk(int attack) { atk = CheckNumber(attack); }
    public void setDef(int defense) { def = CheckNumber(defense); }
    public void setSpd(int speed) { spd = CheckNumber(speed); }
    public void setNme(string Name) { nAme = Name; NameLength = nAme.Length; }
    public void setHlth(int Health) { health = Health; }
    public void setElement(int n) {
        elem = new Element(n);
    }
    public int CheckNumber(int num) {
        if (num > MAX_STAT) return MAX_STAT;
        else if (num <= 5) return 5;
        else return num;
    }

    public void setSlimeEvolutionStage(int evol) { evolutionstage = evol; }

    //EVOLUTION AND REINCARNATION
    public void Evolve()
    {
        if (SlimeWins == EVOLUTION_THRESHOLD)
        {
            if (evolutionstage < 2)
            {
                evolutionstage++;
                setAtk((int)Math.Round(getAtk() * 1.5));
                setDef((int)Math.Round(getDef() * 1.5));
                setSpd((int)Math.Round(getSpd() * 1.5));
                EVOLUTION_THRESHOLD += 15;
            }
            else
            {
                int tempevol = UnityEngine.Random.Range(3, 5);
                while (tempevol == evolutionstage) tempevol = UnityEngine.Random.Range(3, 5);
                evolutionstage = tempevol ;

                setAtk((int)Math.Round(getAtk() * 1.5));
                setDef((int)Math.Round(getDef() * 1.5));
                setSpd((int)Math.Round(getSpd() * 1.5));
                EVOLUTION_THRESHOLD += 15;
            }
        }
    }
}
