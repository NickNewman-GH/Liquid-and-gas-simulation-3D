using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class Element : ICloneable{

    public int x, y, z;
    public int density;
    public double thermalConductivity = 3;
    public GameObject elementModel;
    public MeshRenderer elementModelMeshRenderer;
    public Material material;
    public bool isUpdated = false;
    public bool canBeMoved = true;
    public double temperature = Globals.worldTemperature;
    public Dictionary<TemperatureBoundType, TemperatureBound> temperatureBounds = new Dictionary<TemperatureBoundType, TemperatureBound>();

    public Element(int posX, int posY, int posZ, GameObject obj, Material material){
        elementModel = obj; this.material = material; x = posX; y = posY; z = posZ;
        if (elementModel != null)
            elementModelMeshRenderer = elementModel.GetComponent<MeshRenderer>();
    }

    abstract public void Update(Element[,,] field, UpdateType updateType);
    abstract public UpdateType GetUpdateType(Element[,,] field);

    public bool checkCoordsRelevance(int xPos, int yPos, int zPos){
        if ((xPos > -1 && xPos < Globals.xSize) && 
            (yPos > -1 && yPos < Globals.ySize) &&
            (zPos > -1 && zPos < Globals.zSize))
            return true;
        else return false;
    }

    public void AroundTemperatureTransmission(Element[,,] field){
        bool isContactWithOutside = false;
        for (int xPos = x - 1; xPos < x + 2; xPos++){
            for (int yPos = y - 1; yPos < y + 2; yPos++){
                for (int zPos = z - 1; zPos < z + 2; zPos++){
                    if (!(xPos == x && yPos == y && zPos == z) && checkCoordsRelevance(xPos, yPos, zPos)){
                        Element tmpElem = field[xPos, yPos, zPos];
                        if (tmpElem == null)
                            isContactWithOutside = true;
                        else{
                            double tmpTemp = tmpElem.temperature;
                            field[xPos, yPos, zPos].temperature = tmpTemp + (temperature - tmpTemp)/tmpElem.thermalConductivity * Time.fixedDeltaTime;
                        }
                    }
                }
            }
        }
        if (isContactWithOutside)
            temperature -= (temperature - Globals.worldTemperature)/thermalConductivity * Time.fixedDeltaTime * 0.25;
    }

    public object Clone(){
        return this.MemberwiseClone();
    }

    public int GetIndexOfElementType(Type type){
        for (int i = 0; i < Globals.elements.Length; i++)
            if (object.ReferenceEquals(Globals.elements[i].GetType(), type))
                return i;
        return -1;
    }

    public void ReplaceElement(Element[,,] field, int index){
        Element tmpElement = (Element)Globals.elements[index].Clone();
        tmpElement.x = x; tmpElement.y = y; tmpElement.z = z; tmpElement.temperature = temperature;
        tmpElement.elementModel = elementModel;
        tmpElement.elementModelMeshRenderer = elementModelMeshRenderer;
        tmpElement.elementModelMeshRenderer.material = tmpElement.material;
        tmpElement.isUpdated = true;
        field[x, y, z] = tmpElement;
    }
}

public enum TemperatureBoundType {
    Lower,
    Upper
}

public struct TemperatureBound {
    public double boundTemperature;
    public Type elementToCreate;
    
    public TemperatureBound(double boundTemperature, Type elementToCreate){
        this.boundTemperature = boundTemperature;
        this.elementToCreate = elementToCreate;
    }
}