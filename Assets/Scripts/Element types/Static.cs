using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Static : Element
{
    public Static(int posX, int posY, int posZ, GameObject obj, Material material) : base(posX, posY, posZ, obj, material){
        canBeMoved = false;
    }

    public override void Update(Element[,,] field, UpdateType updateType){       
        if (updateType == UpdateType.Replace){
            if (temperatureBounds.ContainsKey(TemperatureBoundType.Upper) && temperature > temperatureBounds[TemperatureBoundType.Upper].boundTemperature){
                int index = GetIndexOfElementType(temperatureBounds[TemperatureBoundType.Upper].elementToCreate);
                ReplaceElement(field, index);
            } else if (temperatureBounds.ContainsKey(TemperatureBoundType.Lower) && temperature < temperatureBounds[TemperatureBoundType.Lower].boundTemperature){
                int index = GetIndexOfElementType(temperatureBounds[TemperatureBoundType.Lower].elementToCreate);
                ReplaceElement(field, index);
            }
        } else {isUpdated = true;}
    }

    public override UpdateType GetUpdateType(Element[,,] field){
        if (temperatureBounds.Count > 0)
            if ((temperatureBounds.ContainsKey(TemperatureBoundType.Upper) && temperature > temperatureBounds[TemperatureBoundType.Upper].boundTemperature) || 
            (temperatureBounds.ContainsKey(TemperatureBoundType.Lower) && temperature < temperatureBounds[TemperatureBoundType.Lower].boundTemperature))
                return UpdateType.Replace;
        return UpdateType.Stay;
    }
}
