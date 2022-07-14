using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Globals{
    public static float gravity = 10f;
    public static double worldTemperature = 25;
    public static Element[] elements;
    public static int xSize = 25, ySize = 25, zSize = 25;
}

public class GameField : MonoBehaviour{
    
    public Element[,,] field = new Element[Globals.xSize, Globals.ySize, Globals.zSize];

    public Material[] elementMaterials;

    public Element[] elements;

    public GameObject elementModel;
    [NonSerialized] public MeshRenderer elementModelMeshRenderer;

    public GameObject fieldHighlighting;

    public UpdateManager updateManager = new UpdateManager();

    public int creatingElementTypeNumber = 0;

    public bool isPaused = false;

    public int creationFieldSize = 0;

    public int tempChangeType = 0;
    public double tempChangePerSec = 250;

    private void ElementsDeclaration(){
        elements = new Element[]{
            new Sand(-1, -1, -1, null, elementMaterials[0]),
            new Water(-1, -1, -1, null, elementMaterials[1]),
            new Smoke(-1, -1, -1, null, elementMaterials[2]),
            new Stone(-1, -1, -1, null, elementMaterials[3]),
            new Oil(-1, -1, -1, null, elementMaterials[4]),
            new Steam(-1, -1, -1, null, elementMaterials[5]),
            new Ice(-1, -1, -1, null, elementMaterials[6]),
            new Lava(-1, -1, -1, null, elementMaterials[7]),
            new SolidStone(-1, -1, -1, null, elementMaterials[3])
        };
    }

    private void Awake() {
        ElementsDeclaration();
        Globals.elements = elements;

        fieldHighlighting.transform.localScale = new Vector3(field.GetLength(0), field.GetLength(1), field.GetLength(2));
        Instantiate(fieldHighlighting, new Vector3(field.GetLength(0)/2, field.GetLength(1)/2, field.GetLength(2)/2), Quaternion.Euler(0,0,0));

        elementModelMeshRenderer = elementModel.GetComponent<MeshRenderer>();
        elementModelMeshRenderer.material = elements[creatingElementTypeNumber].material;

        Time.fixedDeltaTime = 1 / 60f;
    }

    private void Update(){
        if (tempChangeType == 0)
            CreateElements();
        else ChangeTemperature();
        DeleteElements();
        ChangeInputType();
        Pause();
        ChangeCreationFieldSize();
        ClearField();

        // if (!isPaused)
        //     FieldUpdate();

        //Debug.Log("Apx FPS: " + 1.0f / Time.deltaTime);
    }

    private void FixedUpdate(){
        if (!isPaused)
            FieldUpdate();
    }

    private void FieldUpdate(){
        TemperatureTransmission();
        updateManager.GetUpdates(field);
        for (int updateTypeNumber = 0; updateTypeNumber < updateManager.updates.Length; updateTypeNumber++){
            List<int[]> updateType = updateManager.updates[updateTypeNumber];
            foreach (int[] updateCoords in updateType){
                if (!field[updateCoords[0], updateCoords[1], updateCoords[2]].isUpdated)
                    field[updateCoords[0], updateCoords[1], updateCoords[2]].Update(field, (UpdateType)updateTypeNumber);
            }
        }
    }

    private void ChangeCreationFieldSize(){
        var mouseWheelAxis = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWheelAxis > 0 && creationFieldSize < 6)
            creationFieldSize++;
        else if (mouseWheelAxis < 0 && creationFieldSize > 0)
            creationFieldSize--;
    }

    private void Pause(){
        if (Input.GetKeyDown(KeyCode.P))
            isPaused = !isPaused;
        if (isPaused && Input.GetKeyDown(KeyCode.RightArrow))
            FieldUpdate();
    }

    private void ChangeInputType(){
        if (Input.GetKey(KeyCode.LeftShift)){
            if (Input.GetKeyDown(KeyCode.Alpha1))
                tempChangeType = 1;
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                tempChangeType = -1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1)){
            creatingElementTypeNumber = GetIndexOfElementType(typeof(Sand));
            elementModelMeshRenderer.material = elements[creatingElementTypeNumber].material;
            tempChangeType = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)){
            creatingElementTypeNumber = GetIndexOfElementType(typeof(Water));
            elementModelMeshRenderer.material = elements[creatingElementTypeNumber].material;
            tempChangeType = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)){
            creatingElementTypeNumber = GetIndexOfElementType(typeof(Stone));
            elementModelMeshRenderer.material = elements[creatingElementTypeNumber].material;
            tempChangeType = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)){
            creatingElementTypeNumber = GetIndexOfElementType(typeof(Oil));
            elementModelMeshRenderer.material = elements[creatingElementTypeNumber].material;
            tempChangeType = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)){
            creatingElementTypeNumber = GetIndexOfElementType(typeof(Smoke));
            elementModelMeshRenderer.material = elements[creatingElementTypeNumber].material;
            tempChangeType = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6)){
            creatingElementTypeNumber = GetIndexOfElementType(typeof(Steam));
            elementModelMeshRenderer.material = elements[creatingElementTypeNumber].material;
            tempChangeType = 0;
        }
    }

    public bool checkCoordsRelevance(int xPos, int yPos, int zPos){
        if ((xPos > -1 && xPos < Globals.xSize) && 
            (yPos > -1 && yPos < Globals.ySize) &&
            (zPos > -1 && zPos < Globals.zSize))
            return true;
        else return false;
    }

    private void CreateElements(){
        if (Input.GetKey(KeyCode.Mouse0)){
            var cameraPosition = transform.position;
            int cameraX = (int)cameraPosition[0], cameraY = (int)cameraPosition[1], cameraZ = (int)cameraPosition[2];
            for (int xPos = cameraX - creationFieldSize; xPos < cameraX + (creationFieldSize + 1); xPos++){
                for (int yPos = cameraY - creationFieldSize; yPos < cameraY + (creationFieldSize + 1); yPos++){
                    for (int zPos = cameraZ - creationFieldSize; zPos < cameraZ + (creationFieldSize + 1); zPos++){
                        if (checkCoordsRelevance(xPos, yPos, zPos) && field[xPos, yPos, zPos] == null){
                            Element tmpElement = (Element)elements[creatingElementTypeNumber].Clone();
                            tmpElement.x = xPos; tmpElement.y = yPos; tmpElement.z = zPos;
                            tmpElement.elementModel = Instantiate(elementModel, new Vector3(xPos, yPos, zPos), Quaternion.Euler(0,0,0));
                            tmpElement.elementModelMeshRenderer = tmpElement.elementModel.GetComponent<MeshRenderer>();
                            field[xPos, yPos, zPos] = tmpElement;
                        }
                    }
                }
            }
        }
    }

    private void DeleteElements(){
        if (Input.GetKey(KeyCode.Mouse1)){
            var cameraPosition = transform.position;
            int cameraX = (int)cameraPosition[0], cameraY = (int)cameraPosition[1], cameraZ = (int)cameraPosition[2];
            for (int xPos = cameraX - creationFieldSize; xPos < cameraX + (creationFieldSize + 1); xPos++){
                for (int yPos = cameraY - creationFieldSize; yPos < cameraY + (creationFieldSize + 1); yPos++){
                    for (int zPos = cameraZ - creationFieldSize; zPos < cameraZ + (creationFieldSize + 1); zPos++){
                        if (checkCoordsRelevance(xPos, yPos, zPos) && field[xPos, yPos, zPos] != null){
                            Destroy(field[xPos, yPos, zPos].elementModel);
                            field[xPos, yPos, zPos] = null;
                        }
                    }
                }
            }
        }
    }

    public void ChangeTemperature(){
        if (Input.GetKey(KeyCode.Mouse0)){
            /////////
            //Debug.Log(tempChangePerSec * Time.deltaTime * tempChangeType);
            /////////
            var cameraPosition = transform.position;
            int cameraX = (int)cameraPosition[0], cameraY = (int)cameraPosition[1], cameraZ = (int)cameraPosition[2];
            for (int xPos = cameraX - creationFieldSize; xPos < cameraX + (creationFieldSize + 1); xPos++){
                for (int yPos = cameraY - creationFieldSize; yPos < cameraY + (creationFieldSize + 1); yPos++){
                    for (int zPos = cameraZ - creationFieldSize; zPos < cameraZ + (creationFieldSize + 1); zPos++){
                        if (checkCoordsRelevance(xPos, yPos, zPos) && field[xPos, yPos, zPos] != null){
                            field[xPos, yPos, zPos].temperature += tempChangePerSec * Time.deltaTime * tempChangeType;
                        }
                    }
                }
            }
        }
    }

    public void ClearField(){
        if (Input.GetKey(KeyCode.R)){
            foreach (Element element in field)
                if (element != null){
                    Destroy(element.elementModel);
                }
            field = new Element[Globals.xSize, Globals.ySize, Globals.zSize];
        }
    }

    public void TemperatureTransmission(){
        foreach (Element element in field)
            if (element != null)
                element.AroundTemperatureTransmission(field);
    }

    public int GetIndexOfElementType(Type type){
        for (int i = 0; i < Globals.elements.Length; i++)
            if (object.ReferenceEquals(Globals.elements[i].GetType(), type))
                return i;
        return -1;
    }
}