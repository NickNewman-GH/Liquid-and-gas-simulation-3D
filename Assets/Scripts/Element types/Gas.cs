using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : Element{

    public Gas(int posX, int posY, int posZ, GameObject obj, Material material) : base(posX, posY, posZ, obj, material){}
    
    public override void Update(Element[,,] field, UpdateType updateType){       
        if (updateType == UpdateType.Move){
            if (y < field.GetLength(1) - 1){
                int yPos = y + 1;
                if (field[x, yPos, z] != null){
                    List<int[]> availableCells = GetAvailableCellsAroundY(field, yPos);
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
                    y = yPos;
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
        } else if (updateType == UpdateType.Swap) {
            if (y < field.GetLength(1) - 1){
                int yPos = y + 1;
                if (field[x, yPos, z] != null && field[x, yPos, z].canBeMoved && field[x, yPos, z].density > density){
                    SwapElements(field, new int[]{x, yPos, z});
                } else {
                    List<int[]> availableCells = GetCellsAroundYWithUpperDensity(field, yPos);
                    int[] randomAvailableCoords;
                    if (availableCells.Count == 0){
                        availableCells = GetCellsAroundYWithUpperDensity(field, y);
                        if (availableCells.Count == 0){
                            isUpdated = true;
                            return;
                        }
                        randomAvailableCoords = availableCells[Random.Range(0, availableCells.Count)];
                        SwapElements(field, randomAvailableCoords);                    
                    }
                    randomAvailableCoords = availableCells[Random.Range(0, availableCells.Count)];
                    SwapElements(field, randomAvailableCoords);
                }
            } else{
                List<int[]> availableCells = GetCellsAroundYWithUpperDensity(field, y);
                int[] randomAvailableCoords;
                if (availableCells.Count == 0){
                    isUpdated = true;
                    return;
                }
                randomAvailableCoords = availableCells[Random.Range(0, availableCells.Count)];
                SwapElements(field, randomAvailableCoords); 
            }
        } else if (updateType == UpdateType.Replace){
            if (temperatureBounds.ContainsKey(TemperatureBoundType.Upper) && temperature > temperatureBounds[TemperatureBoundType.Upper].boundTemperature){
                int index = GetIndexOfElementType(temperatureBounds[TemperatureBoundType.Upper].elementToCreate);
                ReplaceElement(field, index);
                return;
            } else if (temperatureBounds.ContainsKey(TemperatureBoundType.Lower) && temperature < temperatureBounds[TemperatureBoundType.Lower].boundTemperature){
                int index = GetIndexOfElementType(temperatureBounds[TemperatureBoundType.Lower].elementToCreate);
                ReplaceElement(field, index);
                return;
            }
        }
        isUpdated = true;
    }

    public override UpdateType GetUpdateType(Element[,,] field){
        if (temperatureBounds.Count > 0)
            if ((temperatureBounds.ContainsKey(TemperatureBoundType.Upper) && temperature > temperatureBounds[TemperatureBoundType.Upper].boundTemperature) || 
            (temperatureBounds.ContainsKey(TemperatureBoundType.Lower) && temperature < temperatureBounds[TemperatureBoundType.Lower].boundTemperature))
                return UpdateType.Replace;
        if (y < field.GetLength(1) - 1){
            int yPos = y + 1;
            if (field[x, yPos, z] == null)
                return UpdateType.Move;
            else if (field[x, yPos, z].canBeMoved && field[x, yPos, z].density > density)
                return UpdateType.Swap;
            
            for (int xPos = x - 1; xPos < x + 2; xPos++)
                for (int zPos = z - 1; zPos < z + 2; zPos++)
                    if (checkCoordsRelevance(field, xPos, yPos, zPos))
                        if (field[xPos, yPos, zPos] == null)
                            return UpdateType.Move;
            for (int xPos = x - 1; xPos < x + 2; xPos++)
                for (int zPos = z - 1; zPos < z + 2; zPos++)
                    if (checkCoordsRelevance(field, xPos, yPos, zPos))
                        if (field[xPos, yPos, zPos].canBeMoved && field[xPos, yPos, zPos].density > density)
                            return UpdateType.Swap;
            
            for (int xPos = x - 1; xPos < x + 2; xPos++)
                for (int zPos = z - 1; zPos < z + 2; zPos++)
                    if (checkCoordsRelevance(field, xPos, y, zPos))
                        if (field[xPos, y, zPos] == null)
                            return UpdateType.Move;
            for (int xPos = x - 1; xPos < x + 2; xPos++)
                for (int zPos = z - 1; zPos < z + 2; zPos++)
                    if (checkCoordsRelevance(field, xPos, y, zPos))
                        if (field[xPos, y, zPos].canBeMoved && field[xPos, y, zPos].density > density)
                            return UpdateType.Swap;

            return UpdateType.Stay;
        } else {
            for (int xPos = x - 1; xPos < x + 2; xPos++)
                for (int zPos = z - 1; zPos < z + 2; zPos++)
                    if (checkCoordsRelevance(field, xPos, y, zPos))
                        if (field[xPos, y, zPos] == null)
                            return UpdateType.Move;
            for (int xPos = x - 1; xPos < x + 2; xPos++)
                for (int zPos = z - 1; zPos < z + 2; zPos++)
                    if (checkCoordsRelevance(field, xPos, y, zPos))
                        if (field[xPos, y, zPos].canBeMoved && field[xPos, y, zPos].density > density)
                            return UpdateType.Swap;

            return UpdateType.Stay;
        }
    }

    public List<int[]> GetAvailableCellsAroundY(Element[,,] field, int yPos){
        List<int[]> availableCells = new List<int[]>();
                
        for (int xPos = x - 1; xPos < x + 2; xPos++)
            for (int zPos = z - 1; zPos < z + 2; zPos++)
                if (!(xPos == x && zPos == z) && checkCoordsRelevance(field, xPos, yPos, zPos) && (field[xPos, yPos, zPos] == null))
                    availableCells.Add(new int[]{xPos, yPos, zPos});
        
        return availableCells;
    }

    public void SwapElements(Element[,,] field, int[] randomAvailableCoords){
        int tmpX = x, tmpY = y, tmpZ = z;
        Element targetElement = field[randomAvailableCoords[0], randomAvailableCoords[1], randomAvailableCoords[2]];

        x = targetElement.x; y = targetElement.y; z = targetElement.z;
        elementModel.transform.position = new Vector3(x,y,z);

        targetElement.x = tmpX; targetElement.y = tmpY; targetElement.z = tmpZ;
        targetElement.elementModel.transform.position = new Vector3(tmpX,tmpY,tmpZ);

        field[tmpX, tmpY, tmpZ] = targetElement; field[randomAvailableCoords[0], randomAvailableCoords[1], randomAvailableCoords[2]] = this;
        field[tmpX, tmpY, tmpZ].isUpdated = true;
    }

    public List<int[]> GetCellsAroundYWithUpperDensity(Element[,,] field, int yPos){
        List<int[]> availableCells = new List<int[]>();
        for (int xPos = x - 1; xPos < x + 2; xPos++)
            for (int zPos = z - 1; zPos < z + 2; zPos++)
                if (!(xPos == x && zPos == z) && checkCoordsRelevance(field, xPos, yPos, zPos) && (field[xPos, yPos, zPos] != null) && field[xPos, yPos, zPos].canBeMoved && field[xPos, yPos, zPos].density > density)
                    availableCells.Add(new int[]{xPos, yPos, zPos});
        return availableCells;
    }
}
