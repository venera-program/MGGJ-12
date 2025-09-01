using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class HelperFunctions {
    // top/bottom/left/right are ALL in world units are in Pixels
    private static Camera main;
    public static bool IsOnScreen(Vector2 position, float topBorder = 0f, float bottomBorder = 0f, float leftBorder = 0f, float rightBorder = 0f){ // can modify this to take in borders
        if(!main){
            main = Camera.main;
        }
        Rect CameraRect = main.pixelRect;

        Vector3 topLeft = main.ScreenToWorldPoint(new Vector3(CameraRect.x + leftBorder, CameraRect.y + CameraRect.height - topBorder, main.nearClipPlane));
        Vector3 topRight = main.ScreenToWorldPoint(new Vector3(CameraRect.x + CameraRect.width - rightBorder, CameraRect.y + CameraRect.height - topBorder, main.nearClipPlane));
        Vector3 bottomLeft = main.ScreenToWorldPoint(new Vector3(CameraRect.x + leftBorder, CameraRect.y + bottomBorder, main.nearClipPlane));
        Vector3 bottomRight = main.ScreenToWorldPoint(new Vector3(CameraRect.x + CameraRect.width - rightBorder, CameraRect.y + bottomBorder, main.nearClipPlane));
    
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

    /// <summary> Returns topleft, top right, bottom left, bottom right screen corners</summary>

    public static Vector3[] ScreenCorners(float topBorder = 0f, float bottomBorder = 0f, float leftBorder = 0f, float rightBorder = 0f){
         if(!main){
            main = Camera.main;
        }
        Rect CameraRect = main.pixelRect;
        Vector3 topLeft = main.ScreenToWorldPoint(new Vector3(CameraRect.x + leftBorder, CameraRect.y + CameraRect.height - topBorder, main.nearClipPlane));
        Vector3 topRight = main.ScreenToWorldPoint(new Vector3(CameraRect.x + CameraRect.width - rightBorder, CameraRect.y + CameraRect.height - topBorder, main.nearClipPlane));
        Vector3 bottomLeft = main.ScreenToWorldPoint(new Vector3(CameraRect.x + leftBorder, CameraRect.y + bottomBorder, main.nearClipPlane));
        Vector3 bottomRight = main.ScreenToWorldPoint(new Vector3(CameraRect.x + CameraRect.width - rightBorder, CameraRect.y + bottomBorder, main.nearClipPlane));

        return new Vector3[]{topLeft, topRight, bottomLeft, bottomRight};
    }

    
    public static ScreenBorders CloseToBorder(Vector2 position){
        Vector3[] corners = HelperFunctions.ScreenCorners();
        //top left
        //top right
        //bottom left
        //bottom right

        // factor in if the enemy is on the screen when spawned
        float x = position.x;
        float y = position.y;
        if (y > corners[0].y ){ //|| (y - corners[0].y < y - corners[2].y)
            return ScreenBorders.Top;
        }
        if (y < corners[2].y ){ // || (y - corners[2].y < y - corners[0].y)
            return ScreenBorders.Bottom;
        }
        if (x < corners[0].x){ //  || (x - corners[0].x > x - corners[1].x)
            return ScreenBorders.Left;
        }
        if (x > corners[1].x){ // || (x - corners[1].x < x - corners[0].x)
            return ScreenBorders.Right;
        }

        return ScreenBorders.Default;
    }

    public static bool IsAtPosition(Vector2 position, Vector2 destination, float accuracy){
        if((position - destination).sqrMagnitude <= (accuracy * accuracy)){
            return true;
        } 
        
        return false;
    }

    // Returns the angle in radians to be used for position
    public static float CalculateProjectilePositionAngle(int index, Group group){
        
        switch(group.pattern){
            case(GroupType.Ring):
                return CalculateRingPositionAngle(index, group);
            case(GroupType.Spread):
                return CalculateRingPositionAngle(index, group);
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
        // using starting angle 
        return 0f;
    }

    private static float CalculateStackPositionAngle(int i, Group ring){
        return Mathf.Deg2Rad * ring.startingAngle;
    }

    // returns angle in degrees for rotation
    public static float CaluclateProjectileMovementAngle(int index, Group group, Vector3 position, Vector3 center){
        switch(group.pattern){
            case(GroupType.Ring):
                return CalculateRingMovementAngle(index, group, position, center);
            case(GroupType.Spread):
                return CalculateRingMovementAngle(index, group, position, center);
            case(GroupType.Stack):
                return CalculateStackMovementAngle(index, group, position);
            default:
                return 0f;
        }
    }

    private static float CalculateRingMovementAngle(int i, Group ring, Vector3 position, Vector3 center){
        // return the angle that the projectile should be rotated towards

        if(ring.movementAngle == MovementAngle.Fixed){
            return CalculateRingPositionAngle(i, ring) * Mathf.Rad2Deg;
        } else if (ring.movementAngle == MovementAngle.TowardsPlayerDistorted){
            Vector3 playerPosition = PlayerControllerScript.instance.transform.position; 
            Vector3 angleDiff = playerPosition - position;
            float angle = Vector2.SignedAngle(Vector2.right, (Vector2)angleDiff);
            return angle;
        } else if (ring.movementAngle == MovementAngle.TowardsPlayerRigid){
            Vector3 playerPosition = PlayerControllerScript.instance.transform.position;
            Vector3 angleDiff = playerPosition - center;
            float angle = Vector2.SignedAngle(Vector2.right, (Vector2)angleDiff);
            return angle;
        }
        return 0f;
    }

    private static float CalculateSpreadMovementAngle(int i, Group spread, Vector3 position){
        return 0f;
    }

    private static float CalculateStackMovementAngle(int i, Group stack, Vector3 position){
       if(stack.movementAngle == MovementAngle.Fixed){
        return CalculateStackPositionAngle(i, stack) * Mathf.Rad2Deg;
        } else if (stack.movementAngle == MovementAngle.TowardsPlayerDistorted){
            Vector3 playerPosition = PlayerControllerScript.instance.transform.position; 
            Vector3 angleDiff = playerPosition - position;
            float angle = Vector2.SignedAngle(Vector2.right, (Vector2)angleDiff);
            return angle;
        }
        return 0f;
    }

    public static float RoundToDecimal(float number, int decimalPlaces){
        float multiplicant = Mathf.Pow(10, (float)decimalPlaces);
        return (Mathf.Floor(number * multiplicant))/multiplicant;
    }

    public static bool IsContact(Vector2 mainCenter, Vector2 closestOtherColliderPoint, float mainRadius){
        // this if statement is needed because when a collider is deactivated, the "closest" point passed in ends up being the center;
        if(mainCenter == closestOtherColliderPoint) return false; 
        bool isInContact =  ((mainCenter - closestOtherColliderPoint).sqrMagnitude) <= (mainRadius * mainRadius);
        return isInContact;
    }

}