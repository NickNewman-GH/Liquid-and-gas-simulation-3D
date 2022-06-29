using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Static
{
    public Stone(int posX, int posY, int posZ, GameObject obj) : base(posX, posY, posZ, obj){
        density = 5;
    }
}
