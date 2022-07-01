using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steam : Gas
{
    public Steam(int posX, int posY, int posZ, GameObject obj) : base(posX, posY, posZ, obj){
        density = 4;
    }
}