using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oil : Liquid
{
    public Oil(int posX, int posY, int posZ, GameObject obj) : base(posX, posY, posZ, obj){
        density = 600;
    }
}
