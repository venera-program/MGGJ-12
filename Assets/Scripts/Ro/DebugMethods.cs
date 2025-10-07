using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DebugMethods {

    private static string[] enemyPatternsToCheck = {"frogBoss Enemy"};
    public static void PrintTicks(int tick){
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.Log($"CURRENT TICK SINCE LEVEL STARTED : {tick}");
        #endif
    }

    public static void PrintGroupInformation(Group currGroup, float currSeconds, int index, string name){
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        // this is currently set only for the frog boss enemy. Can be extended for future use.
        foreach(string a in enemyPatternsToCheck){
        int length = a.Length;
        if (name.Length < length){
            continue;
        }
            if(a == name.Substring(0,length)){
                Debug.Log($"Group: {a} is generating a {currGroup.ToString()} group at index {index} at {currSeconds} seconds.");
            }
        }
        #endif
    }

    public static void PrintPatternInformation(int index, float currHealth, float maxHealth, string name){
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        foreach(string a in enemyPatternsToCheck){
            int length = a.Length;
            if (name.Length < length){
                continue;
            }
            if(a == name.Substring(0, length)){
                Debug.Log($"Pattern: {a} is now on phase {index}.");
                PrintHPValue(currHealth, maxHealth, name);
            }
        }
        #endif
    }

    public static void PrintHPValue(float currHealth, float maxHealth, string name){
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.Log($"Health: {name} has {currHealth}/{maxHealth} health.");
        #endif
    }

    public static void PrintGroupDetermination(float time, float truncTime, int index, Group group, string name){
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        foreach(string a in enemyPatternsToCheck){
            int length = a.Length;
            if (name.Length < length){
                continue;
            }
            if(a == name.Substring(0, length)){
                Debug.Log($"Enemy: {name} Current time: {time}, TruncTime: {truncTime}, Index: {index}, Delay: {group.delay}, SpawnInterval: {group.spawnInterval}");
            }
        }
        #endif
    }

}