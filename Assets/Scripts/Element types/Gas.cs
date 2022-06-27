using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : Element{

    public Gas(int posX, int posY, int posZ, GameObject obj) : base(posX, posY, posZ, obj){}
    
    public override void Update(Element[,,] field, UpdateType updateType){       
        if (updateType == UpdateType.Move){
            if (y < field.GetLength(1) - 1){
                int upperY = y + 1;
                if (field[x, upperY, z] != null){
                    List<int[]> availableCells = GetAvailableCellsAroundY(field, upperY);
                    if (availableCells.Count == 0)
                        availableCells = GetAvailableCellsAroundY(field, y);
                        if (availableCells.Count == 0)
                            return;
                    field[x, y, z] = null;
                    int[] randomAvailableCoords = availableCells[Random.Range(0, availableCells.Count)];
                    x = randomAvailableCoords[0]; y = randomAvailableCoords[1]; z = randomAvailableCoords[2];
                }
                else {
                    field[x, y, z] = null;
                    y = upperY;
                }
            } else{
                List<int[]> availableCells = GetAvailableCellsAroundY(field, y);
                if (availableCells.Count == 0)
                    return;
                field[x, y, z] = null;
                int[] randomAvailableCoords = availableCells[Random.Range(0, availableCells.Count)];
                x = randomAvailableCoords[0]; y = randomAvailableCoords[1]; z = randomAvailableCoords[2];
            }
            field[x,y,z] = this;
            elementModel.transform.position = new Vector3(x,y,z);
        } else {}
    }

    public override UpdateType GetUpdateType(Element[,,] field){
        int upperY = y + 1;
        for (int xPos = x - 1; xPos < x + 2; xPos++)
            for (int zPos = z - 1; zPos < z + 2; zPos++)
                if ((checkCoordsRelevance(field, xPos, y, zPos) && (field[xPos, y, zPos] == null)) || 
                (checkCoordsRelevance(field, xPos, upperY, zPos) && (field[xPos, upperY, zPos] == null)))
                    return UpdateType.Move;
        return UpdateType.Stay;
    }

    public List<int[]> GetAvailableCellsAroundY(Element[,,] field, int yPos){
        List<int[]> availableCells = new List<int[]>();
                
        for (int xPos = x - 1; xPos < x + 2; xPos++)
            for (int zPos = z - 1; zPos < z + 2; zPos++)
                if (!(xPos == x && zPos == z) && checkCoordsRelevance(field, xPos, yPos, zPos) && (field[xPos, yPos, zPos] == null))
                    availableCells.Add(new int[]{xPos, yPos, zPos});
        
        return availableCells;
    }
}
