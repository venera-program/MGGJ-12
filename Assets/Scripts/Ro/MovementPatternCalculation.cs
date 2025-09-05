using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MovementPatternCalculation {
    /// <summary>
    /// A function that calculates the next accepted position for an actor to move towards to.
    ///</summary>
    public static Vector3 CalculateFloatyPosition(Vector3 currentPosition, float distance,  float topBorder, float bottomBorder, float leftBorder, float rightBorder){
        /*
            0 - Right
            1 - Up Right
            2 - Up
            3 - Up Left
            4 - Left
            5 - Down Left
            6 - Down
            7 - Down Right
        */
        List<int> directions = new List<int>();

        for(int i  = 0 ; i < 8 ; i++){
            directions.Add(i);
        }

        while (true){
            if(directions.Count <= 0){
                Debug.LogError("Not possible for actor to move within screen with given movement distance");
                break;
            }

            int direction = directions[Random.Range(0,directions.Count)];
            Vector3 newPosition;
            
            switch(direction){
                case 0: 
                    newPosition = currentPosition + new Vector3(distance, 0f, 0f);
                    break;
                case 1: 
                    newPosition = currentPosition + (distance * new Vector3(Mathf.Cos(45f * Mathf.Deg2Rad), Mathf.Sin(45f * Mathf.Deg2Rad), 0f));
                    break;
                case 2: 
                    newPosition = currentPosition + new Vector3(0f, distance, 0f);
                    break;
                case 3:
                    newPosition = currentPosition + (distance * new Vector3(Mathf.Cos(135f * Mathf.Deg2Rad), Mathf.Sin(135f * Mathf.Deg2Rad), 0f));
                    break;
                case 4:
                    newPosition = currentPosition + new Vector3(-distance, 0f, 0f);
                    break;
                case 5:
                    newPosition = currentPosition + (distance * new Vector3(Mathf.Cos(225f * Mathf.Deg2Rad), Mathf.Sin(225f * Mathf.Deg2Rad), 0f));
                    break;
                case 6:
                    newPosition = currentPosition + new Vector3(0f, -distance, 0f);
                    break;
                case 7:
                    newPosition = currentPosition + (distance * new Vector3(Mathf.Cos(315f  * Mathf.Deg2Rad), Mathf.Sin(315f * Mathf.Deg2Rad), 0f));
                    break;
                default:
                    newPosition = Vector3.zero;
                    break;
            }
            if(HelperFunctions.IsOnScreen(newPosition, topBorder, bottomBorder, leftBorder, rightBorder)){
                return newPosition;
            } else {
                directions.Remove(direction);
            }
        }

        //should never run into this.
        return Vector3.positiveInfinity;
    }

    public static Vector3 CalculateDirectedScreenPosition(Vector3 position, Rect imagebounds){
        ScreenBorders border = HelperFunctions.CloseToBorder(position);
        Rect CameraRect = Camera.main.pixelRect;
        float height, width;
        Vector3 cameraHeight, cameraWidth;
        switch(border){
            case(ScreenBorders.Top):
                height = CameraRect.y - imagebounds.height * 1.5f;
                cameraHeight = Camera.main.ScreenToWorldPoint(new Vector3(CameraRect.x, height, Camera.main.nearClipPlane));
                return new Vector3(position.x, cameraHeight.y, position.z);
            case (ScreenBorders.Bottom):
                height = CameraRect.y + CameraRect.height + imagebounds.height * 1.5f;
                cameraHeight = Camera.main.ScreenToWorldPoint(new Vector3(CameraRect.x, height, Camera.main.nearClipPlane));
                return new Vector3(position.x, cameraHeight.y, position.z);
            case (ScreenBorders.Left):
                width = CameraRect.width + imagebounds.width;
                cameraWidth = Camera.main.ScreenToWorldPoint(new Vector3(width, CameraRect.y, Camera.main.nearClipPlane));
                return new Vector3(cameraWidth.x, position.y, position.z);
            case (ScreenBorders.Right):
                width = CameraRect.x - imagebounds.width;
                cameraWidth = Camera.main.ScreenToWorldPoint(new Vector3(width, CameraRect.y, Camera.main.nearClipPlane));
                return new Vector3(cameraWidth.x, position.y, position.z);
            default:
                return Vector3.zero;
        }
    }

    public static Vector3 CalculateDirectedPlayerPosition(Vector3 position){
        return PlayerControllerScript.instance.transform.position;
    }
    public static Vector2 CalculateDirectedBossPosition(Vector3 position, float bossDistance){  
        GameObject boss = GameObject.FindWithTag("Boss");
        if(boss != null){
            return boss.transform.position;
        } else {
            return Vector2.zero;
        }
        
    }
}

public enum ScreenBorders {
    Top,
    Bottom,
    Left,
    Right,
    Default
}