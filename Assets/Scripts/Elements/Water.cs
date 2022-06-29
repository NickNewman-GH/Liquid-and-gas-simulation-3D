using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Liquid{

    public Water(int posX, int posY, int posZ, GameObject obj) : base(posX, posY, posZ, obj){
        density = 700;
    }
    
}
