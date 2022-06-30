using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Globals{
    public static float gravity = 10f;
}

public class GameField : MonoBehaviour{
    
    public Element[,,] field = new Element[25, 25, 25];

    public Material[] elementMaterials;
    public GameObject elementModel;
    [NonSerialized] public MeshRenderer elementModelMeshRenderer;

    public GameObject fieldHighlighting;

    public UpdateManager updateManager = new UpdateManager();

    public int creatingElementTypeNumber = 0;

    public bool isPaused = false;

    public int creationFieldSize = 0;

    private void Awake() {
        fieldHighlighting.transform.localScale = new Vector3(field.GetLength(0), field.GetLength(1), field.GetLength(2));
        Instantiate(fieldHighlighting, new Vector3(field.GetLength(0)/2, field.GetLength(1)/2, field.GetLength(2)/2), Quaternion.Euler(0,0,0));

        elementModelMeshRenderer = elementModel.GetComponent<MeshRenderer>();
        elementModelMeshRenderer.material = elementMaterials[creatingElementTypeNumber];

        Time.fixedDeltaTime = 1 / 60f;
    }

    private void Update(){
        CreateElements();
        DeleteElements();
        ChangeCreatingElement();
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

    private void ChangeCreatingElement(){
        if (Input.GetKey(KeyCode.Alpha1)){
            creatingElementTypeNumber = 0;
            elementModelMeshRenderer.material = elementMaterials[creatingElementTypeNumber];
        }
        else if (Input.GetKey(KeyCode.Alpha2)){
            creatingElementTypeNumber = 1;
            elementModelMeshRenderer.material = elementMaterials[creatingElementTypeNumber];
        }
        else if (Input.GetKey(KeyCode.Alpha3)){
            creatingElementTypeNumber = 2;
            elementModelMeshRenderer.material = elementMaterials[creatingElementTypeNumber];
        }
        else if (Input.GetKey(KeyCode.Alpha4)){
            creatingElementTypeNumber = 3;
            elementModelMeshRenderer.material = elementMaterials[creatingElementTypeNumber];
        }
        else if (Input.GetKey(KeyCode.Alpha5)){
            creatingElementTypeNumber = 4;
            elementModelMeshRenderer.material = elementMaterials[creatingElementTypeNumber];
        }
    }

    public bool checkCoordsRelevance(int xPos, int yPos, int zPos){
        if ((xPos > -1 && xPos < field.GetLength(0)) && 
            (yPos > -1 && yPos < field.GetLength(1)) &&
            (zPos > -1 && zPos < field.GetLength(2)))
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
                            switch (creatingElementTypeNumber){
                                case 0:
                                    field[xPos, yPos, zPos] = new Sand(xPos, yPos, zPos, Instantiate(elementModel, new Vector3(xPos, yPos, zPos), Quaternion.Euler(0,0,0)));
                                    break;
                                case 1:
                                    field[xPos, yPos, zPos] = new Water(xPos, yPos, zPos, Instantiate(elementModel, new Vector3(xPos, yPos, zPos), Quaternion.Euler(0,0,0)));
                                    break;
                                case 2:
                                    field[xPos, yPos, zPos] = new Smoke(xPos, yPos, zPos, Instantiate(elementModel, new Vector3(xPos, yPos, zPos), Quaternion.Euler(0,0,0)));
                                    break;
                                case 3:
                                    field[xPos, yPos, zPos] = new Stone(xPos, yPos, zPos, Instantiate(elementModel, new Vector3(xPos, yPos, zPos), Quaternion.Euler(0,0,0)));
                                    break;
                                case 4:
                                    field[xPos, yPos, zPos] = new Oil(xPos, yPos, zPos, Instantiate(elementModel, new Vector3(xPos, yPos, zPos), Quaternion.Euler(0,0,0)));
                                    break;
                            }
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

    public void ClearField(){
        if (Input.GetKey(KeyCode.R)){
            foreach (Element element in field)
                if (element != null){
                    Destroy(element.elementModel);
                }
            field = new Element[25, 25, 25];
        }
    }
}