using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class HelperFunctions {

    public static bool IsOnScreen(Vector2 position){
        Rect CameraRect = Camera.main.pixelRect;

        Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(CameraRect.x, CameraRect.y + CameraRect.height, Camera.main.nearClipPlane));
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(CameraRect.x + CameraRect.width, CameraRect.y + CameraRect.height, Camera.main.nearClipPlane));
        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(CameraRect.x , CameraRect.y, Camera.main.nearClipPlane));
        Vector3 bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(CameraRect.x + CameraRect.width , CameraRect.y , Camera.main.nearClipPlane));
    
        bool betweenX = (position.x >= topLeft.x) && (position.x <= topRight.x);
        bool betweenY = (position.y <= topRight.y) && (position.y >= bottomRight.y);

        // Debug.Log($"topLeft: {topLeft}");
        // Debug.Log($"topRight: {topRight}");
        // Debug.Log($"bottomLeft: {bottomLeft}");
        // Debug.Log($"bottomRight: {bottomRight}");
        // Debug.Log($"betweenX : {betweenX}");
        // Debug.Log($"betweenY : {betweenY}");

        // Doesn't work because position is in worldspace and pixelRect is in screenspace
        //Camera.main.pixelRect.Contains(position)
        return betweenX && betweenY;
    }

    public static float CalculateProjectileAngle(int index, Group group, Vector3 position){
        
        switch(group.pattern){
            case(GroupType.Ring):
                return CalculateRingAngle(index, group, position);
            case(GroupType.Spread):
                return CalculateSpreadAngle(index, group, position);
            case(GroupType.Stack):
                return CalculateStackAngle(index, group, position);
            default:
                return 0f;
        }
    }

    private static float CalculateRingAngle(int i, Group ring, Vector3 position){
        bool isStartingAngleLess = ring.startingAngle < ring.endingAngle;
        float angleDiff = Mathf.Abs(ring.startingAngle - ring.endingAngle);

        if(ring.movementAngle == MovementAngle.Fixed){
            float angleIncrement = angleDiff/ring.projectileCount;
            float angleToCalculate = 0f;
            float rawAngle = angleIncrement * i + ring.startingAngle;
            angleToCalculate =  rawAngle > 360 ? rawAngle - 360 : rawAngle;
            float angleRad = Mathf.Deg2Rad * angleToCalculate;
            return angleRad;
        }

        if(ring.movementAngle == MovementAngle.Random){
            float angleIncrement = Random.Range(0f, angleDiff);
            float angleToCalculate = 0f;
            float rawAngle = angleIncrement + ring.startingAngle;
            angleToCalculate = rawAngle > 360 ? rawAngle - 360: rawAngle;
            return angleToCalculate * Mathf.Deg2Rad;
        }

        if(ring.movementAngle == MovementAngle.TowardsPlayer){
            Vector3 playerPosition = Vector3.zero; // replace with player position
            Vector3 direction = playerPosition - position;
            direction = direction.normalized;

        }


        return 0f;

    }

    private static float CalculateSpreadAngle(int i, Group ring, Vector3 position){
        return 0f;
    }

    private static float CalculateStackAngle(int i, Group ring, Vector3 position){
        return 0f;
    }

}