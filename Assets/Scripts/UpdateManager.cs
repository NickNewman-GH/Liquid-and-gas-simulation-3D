using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum UpdateType {
    Replace,
    Swap,
    Move,
    Stay
}

public class UpdateManager{

    public List<int[]>[] updates = new List<int[]>[Enum.GetNames(typeof(UpdateType)).Length];
    
    public UpdateManager(){
        for(int i = 0; i < Enum.GetNames(typeof(UpdateType)).Length; i++){
            updates[i] = new List<int[]>();
        }
    }

    public void GetUpdates(Element[,,] field){
        ClearUpdates();
        foreach (Element element in field){
            if (element != null){
                element.isUpdated = false;
                UpdateType updateType = element.GetUpdateType(field);
                updates[(int)updateType].Add(new int[]{element.x, element.y, element.z});
                //Debug.Log(element.temperature);
            }
        }
        ShuffleUpdates();
    }

    private void ClearUpdates(){
        foreach(List<int[]> updateList in updates){
            updateList.Clear();
        }
    }

    private void ShuffleUpdates(){
        foreach(List<int[]> updateList in updates){
            updateList.Shuffle();
        }
    }
}

public static class ListExtensions  {
    public static void Shuffle<T>(this IList<T> list) {
        System.Random rnd = new System.Random();
        for (var i = 0; i < list.Count; i++)
            list.Swap(i, rnd.Next(i, list.Count));
    }
 
    public static void Swap<T>(this IList<T> list, int i, int j) {
        var temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }
}