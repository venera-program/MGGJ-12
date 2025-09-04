using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class SpecialAttackCountUI : MonoBehaviour {

    [SerializeField] public TMPro.TMP_Text specialAttackTest;

    public void OnEnable(){
        ProjectilePool.instance.specialAttackCount.AddListener(UpdateText);
    }
    public void UpdateText(int count){
        specialAttackTest.text = "x" + count.ToString();
    }
}