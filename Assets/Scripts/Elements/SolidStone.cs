using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidStone : Bulk {
    public SolidStone(int posX, int posY, int posZ, GameObject obj, Material material) : base(posX, posY, posZ, obj, material){
        density = 1500;
        temperatureBounds.Add(TemperatureBoundType.Upper, new TemperatureBound(1000, typeof(Lava)));
    }
}