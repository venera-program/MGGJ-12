using UnityEngine; 
using System.Collections;
using System.Collections.Generic;

public class DebugEnemyMotorPositions : MonoBehaviour{
    [SerializeField] private GameObject floatyPositionMarker;
    private static Dictionary<int, GameObject> currentFloatyPositionMarkers = new Dictionary<int, GameObject>();
    public static DebugEnemyMotorPositions instance; 

    void Awake(){
        if(instance != null && instance != this){
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }
    public void SetFloatyPositionMarker(int id, Vector3 position){
        if(currentFloatyPositionMarkers.ContainsKey(id)){
            currentFloatyPositionMarkers[id].transform.position = position;
        } else {
            currentFloatyPositionMarkers.Add(id, Instantiate<GameObject>(floatyPositionMarker, position, Quaternion.identity));
        }
    }

    public static void RemoveFloatyPositionMarker(int id){
        if(currentFloatyPositionMarkers.ContainsKey(id)){
            Destroy(currentFloatyPositionMarkers[id]);
            currentFloatyPositionMarkers.Remove(id);
        }
    }
}