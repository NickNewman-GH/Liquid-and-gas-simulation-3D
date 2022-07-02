using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stone : Static{
    public Stone(int posX, int posY, int posZ, GameObject obj, Material material) : base(posX, posY, posZ, obj, material){
        density = 5000;
    }
}
