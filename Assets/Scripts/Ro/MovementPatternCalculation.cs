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
}