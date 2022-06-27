using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : Bulk {

    public Sand(int posX, int posY, int posZ, GameObject obj) : base(posX, posY, posZ, obj){
        density = 5;
    }

}
