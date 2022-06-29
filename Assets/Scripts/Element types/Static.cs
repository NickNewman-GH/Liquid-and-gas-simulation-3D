using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Static : Element
{
    public Static(int posX, int posY, int posZ, GameObject obj) : base(posX, posY, posZ, obj){
        canBeMoved = false;
    }

    public override void Update(Element[,,] field, UpdateType updateType){       
        
    }

    public override UpdateType GetUpdateType(Element[,,] field){
        return UpdateType.Stay;
    }
}
