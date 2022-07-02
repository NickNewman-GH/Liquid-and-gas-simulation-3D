using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : Bulk {

    public Sand(int posX, int posY, int posZ, GameObject obj, Material material) : base(posX, posY, posZ, obj, material){
        density = 1500;
    }

}
