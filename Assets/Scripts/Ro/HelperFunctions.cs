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

    public static float CalculateProjectilePositionAngle(int index, Group group){
        
        switch(group.pattern){
            case(GroupType.Ring):
                return CalculateRingPositionAngle(index, group);
            case(GroupType.Spread):
                return CalculateSpreadPositionAngle(index, group);
            case(GroupType.Stack):
                return CalculateStackPositionAngle(index, group);
            default:
                return 0f;
        }
    }

    private static float CalculateRingPositionAngle(int i, Group ring){
        bool isStartingAngleLess = ring.startingAngle < ring.endingAngle;
        float angleDiff = Mathf.Abs(ring.startingAngle - ring.endingAngle);

        if(ring.positionAngle == PositionAngle.FixedPosition){
            float angleIncrement = angleDiff/ring.projectileCount;
            float angleToCalculate = 0f;
            float rawAngle = angleIncrement * i + ring.startingAngle;
            angleToCalculate =  rawAngle > 360 ? rawAngle - 360 : rawAngle;
            float angleRad = Mathf.Deg2Rad * angleToCalculate;
            return angleRad;
        }

        if(ring.positionAngle == PositionAngle.RandomPosition){
            float angleIncrement = Random.Range(0f, angleDiff);
            float angleToCalculate = 0f;
            float rawAngle = angleIncrement + ring.startingAngle;
            angleToCalculate = rawAngle > 360 ? rawAngle - 360: rawAngle;
            return angleToCalculate * Mathf.Deg2Rad;
        }

        return 0f;

    }

    private static float CalculateSpreadPositionAngle(int i, Group ring){
        return 0f;
    }

    private static float CalculateStackPositionAngle(int i, Group ring){
        return 0f;
    }

    public static float CaluclateProjectileMovementAngle(int i, Group ring, Vector3 position){
        return 0f;
    }

}