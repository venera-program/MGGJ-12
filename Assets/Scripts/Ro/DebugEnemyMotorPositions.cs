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

        PrintScreenMarkers();
    }
    public void SetFloatyPositionMarker(int id, Vector3 position){
        #if DEVELOPMENT_BUILD || UNITY_EDITOR
            if(currentFloatyPositionMarkers.ContainsKey(id)){
                currentFloatyPositionMarkers[id].transform.position = position;
            } else {
                currentFloatyPositionMarkers.Add(id, Instantiate<GameObject>(floatyPositionMarker, position, Quaternion.identity));
            }
        #endif
    }

    public static void RemoveFloatyPositionMarker(int id){
        #if DEVELOPMENT_BUILD || UNITY_EDITOR
            if(currentFloatyPositionMarkers.ContainsKey(id)){
                Destroy(currentFloatyPositionMarkers[id]);
                currentFloatyPositionMarkers.Remove(id);
            }
        #endif
    }

    public static void PrintScreenMarkers(){
        #if DEVELOPMENT_BUILD || UNITY_EDITOR
        Vector3[] screenCorners = HelperFunctions.ScreenCorners();
        // topleft, top right, bottom left, bottom right
        Debug.Log($"Top Left Corner : {screenCorners[0]}");
        Debug.Log($"Top Right Corner : {screenCorners[1]}");
        Debug.Log($"Bottom Left Corner : {screenCorners[2]}");
        Debug.Log($"Bottom Right Corner : {screenCorners[3]}");
        #endif
    }
}