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

    private void Awake() {
        fieldHighlighting.transform.localScale = new Vector3(field.GetLength(0), field.GetLength(1), field.GetLength(2));
        Instantiate(fieldHighlighting, new Vector3(field.GetLength(0)/2, field.GetLength(1)/2, field.GetLength(2)/2), Quaternion.Euler(0,0,0));

        elementModelMeshRenderer = elementModel.GetComponent<MeshRenderer>();
        elementModelMeshRenderer.material = elementMaterials[creatingElementTypeNumber];

        Time.fixedDeltaTime = 1 / 60f;
    }

    private void Update(){
        CreateElements();
        ChangeCreatingElement();
        //Debug.Log("Apx FPS: " + 1.0f / Time.deltaTime);
    }

    private void FixedUpdate(){

        FieldUpdate();    
        
    }

    private void FieldUpdate(){
        updateManager.GetUpdates(field);
        for (int updateTypeNumber = 0; updateTypeNumber < updateManager.updates.Length; updateTypeNumber++){
            foreach (int[] updateCoords in updateManager.updates[updateTypeNumber]){
                field[updateCoords[0], updateCoords[1], updateCoords[2]].Update(field, (UpdateType)updateTypeNumber);
            }
        }
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
    }

    public bool checkCoordsRelevance(int xPos, int yPos, int zPos){
        if ((xPos > -1 && xPos < field.GetLength(0)) && 
            (yPos > -1 && yPos < field.GetLength(1)) &&
            (zPos > -1 && zPos < field.GetLength(2)))
            return true;
        else return false;
    }

    private void CreateElements(){
        if (Input.GetKey(KeyCode.Space)){
            var cameraPosition = transform.position;
            
            int cameraX = (int)cameraPosition[0], cameraY = (int)cameraPosition[1], cameraZ = (int)cameraPosition[2];
            
            for (int xPos = cameraX - 1; xPos < cameraX + 2; xPos++){
                for (int yPos = cameraY - 1; yPos < cameraY + 2; yPos++){
                    for (int zPos = cameraZ - 1; zPos < cameraZ + 2; zPos++){
                        if (checkCoordsRelevance(xPos, yPos, zPos) && field[xPos, yPos, zPos] == null){
                            if (creatingElementTypeNumber == 0)
                                field[xPos, yPos, zPos] = new Sand(xPos, yPos, zPos, Instantiate(elementModel, new Vector3(xPos, yPos, zPos), Quaternion.Euler(0,0,0)));
                            else if (creatingElementTypeNumber == 1)
                                field[xPos, yPos, zPos] = new Water(xPos, yPos, zPos, Instantiate(elementModel, new Vector3(xPos, yPos, zPos), Quaternion.Euler(0,0,0)));
                            else if (creatingElementTypeNumber == 2)
                                field[xPos, yPos, zPos] = new Smoke(xPos, yPos, zPos, Instantiate(elementModel, new Vector3(xPos, yPos, zPos), Quaternion.Euler(0,0,0)));
                        }
                    }
                }
            }


            // if ((cameraX >= 0 && cameraX < field.GetLength(0)) &&
            //     (cameraY >= 0 && cameraY < field.GetLength(1)) && 
            //     (cameraZ >= 0 && cameraZ < field.GetLength(2))){
            //     if (field[cameraX, cameraY, cameraZ] == null){
            //         if (creatingElementTypeNumber == 0)
            //             field[cameraX, cameraY, cameraZ] = new Sand(cameraX, cameraY, cameraZ, Instantiate(elementModel, new Vector3(cameraX, cameraY, cameraZ), Quaternion.Euler(0,0,0)));
            //         else if (creatingElementTypeNumber == 1)
            //             field[cameraX, cameraY, cameraZ] = new Water(cameraX, cameraY, cameraZ, Instantiate(elementModel, new Vector3(cameraX, cameraY, cameraZ), Quaternion.Euler(0,0,0)));
            //     }
            // }
        }
    }
}