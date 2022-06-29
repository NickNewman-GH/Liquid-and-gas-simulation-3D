using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : Element{
    
    public Liquid(int posX, int posY, int posZ, GameObject obj) : base(posX, posY, posZ, obj){}

    public override void Update(Element[,,] field, UpdateType updateType){       
        if (updateType == UpdateType.Move){
            if (y > 0){
                int bottomY = y - 1;
                if (field[x, bottomY, z] != null){
                    List<int[]> availableCells = GetAvailableCellsAroundY(field, bottomY);
                    if (availableCells.Count == 0)
                        availableCells = GetAvailableCellsAroundY(field, y);
                        if (availableCells.Count == 0){
                            isUpdated = true;
                            return;
                        }
                    field[x, y, z] = null;
                    int[] randomAvailableCoords = availableCells[Random.Range(0, availableCells.Count)];
                    x = randomAvailableCoords[0]; y = randomAvailableCoords[1]; z = randomAvailableCoords[2];
                }
                else {
                    field[x, y, z] = null;
                    y = bottomY;
                }
            } else{
                List<int[]> availableCells = GetAvailableCellsAroundY(field, y);
                if (availableCells.Count == 0){
                    isUpdated = true;
                    return;
                }
                field[x, y, z] = null;
                int[] randomAvailableCoords = availableCells[Random.Range(0, availableCells.Count)];
                x = randomAvailableCoords[0]; y = randomAvailableCoords[1]; z = randomAvailableCoords[2];
            }
            field[x,y,z] = this;
            elementModel.transform.position = new Vector3(x,y,z);
        }
        isUpdated = true;
    }

    public override UpdateType GetUpdateType(Element[,,] field){
        int bottomY = y - 1;
        for (int xPos = x - 1; xPos < x + 2; xPos++)
            for (int zPos = z - 1; zPos < z + 2; zPos++)
                if ((checkCoordsRelevance(field, xPos, y, zPos) && (field[xPos, y, zPos] == null)) || 
                (checkCoordsRelevance(field, xPos, bottomY, zPos) && (field[xPos, bottomY, zPos] == null)))
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
