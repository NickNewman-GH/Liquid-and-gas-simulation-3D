using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Liquid{
    public Water(int posX, int posY, int posZ, GameObject obj, Material material) : base(posX, posY, posZ, obj, material){
        density = 700;
        temperatureBounds.Add(TemperatureBoundType.Upper, new TemperatureBound(100, typeof(Steam)));
        temperatureBounds.Add(TemperatureBoundType.Lower, new TemperatureBound(0, typeof(Ice)));
    }
}