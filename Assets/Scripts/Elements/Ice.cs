using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ice : Static{
    public Ice(int posX, int posY, int posZ, GameObject obj, Material material) : base(posX, posY, posZ, obj, material){
        density = 1800;
        temperatureBounds.Add(TemperatureBoundType.Upper, new TemperatureBound(0, typeof(Water)));
    }
}