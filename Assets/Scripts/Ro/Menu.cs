using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject firstSelectedItem;

    void OnEnable(){
        EventSystem.current.SetSelectedGameObject(firstSelectedItem);
    }
}
