using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oil : Liquid
{
    public Oil(int posX, int posY, int posZ, GameObject obj, Material material) : base(posX, posY, posZ, obj, material){
        density = 600;
    }
}
