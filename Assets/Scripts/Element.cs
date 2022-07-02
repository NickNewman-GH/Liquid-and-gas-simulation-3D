using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Element{

    public int x, y, z;
    public GameObject elementModel;
    public int density;
    public bool isUpdated = false;
    public bool canBeMoved = true;
    public double temperature = Globals.worldTemperature;

    public Element(int posX, int posY, int posZ, GameObject obj){
        elementModel = obj; x = posX; y = posY; z = posZ;
    }

    abstract public void Update(Element[,,] field, UpdateType updateType);
    abstract public UpdateType GetUpdateType(Element[,,] field);

    public bool checkCoordsRelevance(Element[,,] field, int xPos, int yPos, int zPos){
        if ((xPos > -1 && xPos < field.GetLength(0)) && 
            (yPos > -1 && yPos < field.GetLength(1)) &&
            (zPos > -1 && zPos < field.GetLength(2)))
            return true;
        else return false;
    }

    public void AroundTemperatureTransmission(Element[,,] field){
        
    }
}