using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steam : Gas
{
    public Steam(int posX, int posY, int posZ, GameObject obj, Material material) : base(posX, posY, posZ, obj, material){
        density = 4;
        temperatureBounds.Add(TemperatureBoundType.Lower, new TemperatureBound(70, typeof(Water)));
    }
}