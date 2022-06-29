using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Static : Element
{
    public Static(int posX, int posY, int posZ, GameObject obj) : base(posX, posY, posZ, obj){}

    public override void Update(Element[,,] field, UpdateType updateType){       
        if (updateType == UpdateType.Move){
        } else {}
    }

    public override UpdateType GetUpdateType(Element[,,] field){
        return UpdateType.Stay;
    }
}
