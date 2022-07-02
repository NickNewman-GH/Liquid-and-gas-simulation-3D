using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : Gas
{
    public Smoke(int posX, int posY, int posZ, GameObject obj, Material material) : base(posX, posY, posZ, obj, material){
        density = 5;
    }
}