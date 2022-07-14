using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : Liquid
{
    public Lava(int posX, int posY, int posZ, GameObject obj, Material material) : base(posX, posY, posZ, obj, material){
        density = 1100;
        temperatureBounds.Add(TemperatureBoundType.Lower, new TemperatureBound(500, typeof(SolidStone)));
    }
}