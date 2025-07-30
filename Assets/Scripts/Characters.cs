using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Characters
{
    public string name;

    public Sprite characterSprite;
    //public Sprite toungeSprite;

    public int health;

    public float grappelForce;

    public float abilityCooldown;

    public bool canFart;
    public bool canShield;
    public bool canTeleport;
    //
    public bool canHeal;
    public float gravityScale;
}
