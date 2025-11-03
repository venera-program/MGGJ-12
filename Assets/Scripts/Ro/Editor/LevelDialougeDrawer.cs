using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(LevelDialogueSO))]
public class LevelDialogueDrawer : Editor {
    public LevelDialogueSO test;
    public VisualTreeAsset inspector_UXML;
    private List<SerializedKeyValuePair<int, DialogueSO>> levelDialogueList;
    private SerializedProperty propertyList;

    public override VisualElement CreateInspectorGUI(){
        VisualElement myInspector = new VisualElement();
        SerializedObject levelDialogue = serializedObject;
        InspectorElement.FillDefaultInspector(myInspector, levelDialogue , this);
        propertyList = levelDialogue.FindProperty("_duringLevelDialogue").FindPropertyRelative("KVP");
        Func<VisualElement> makeItem = MakeListItem;
        Action<VisualElement, int> bindItem = bindItemFunc;
        levelDialogueList = getCurrentList();
        const int itemHeight = 125;
        var listView = new ListView(levelDialogueList, itemHeight, makeItem, bindItem);
        listView.style.flexGrow = 1.0f;
        listView.virtualizationMethod = CollectionVirtualizationMethod.FixedHeight;
        listView.reorderMode = ListViewReorderMode.Animated;
        listView.reorderable = true;
        listView.showBoundCollectionSize = true;
        listView.showFoldoutHeader = true;
        listView.showAddRemoveFooter = true;
        listView.headerTitle = "Level Dialogue";
        listView.BindProperty(propertyList);
        myInspector.RemoveAt(2);
        myInspector.Insert(2, listView);

        return myInspector;
    }

    private VisualElement MakeListItem (){
        var window = new UnityEngine.UIElements.PopupWindow();
        window.Add(inspector_UXML.CloneTree());
        return window;
    }

    private void bindItemFunc (VisualElement element, int index){
        element.Q<IntegerField>("Key").BindProperty(propertyList.GetArrayElementAtIndex(index).FindPropertyRelative("KEY"));
        element.Q<ObjectField>("Value").objectType = typeof(DialogueSO);
        element.Q<ObjectField>("Value").BindProperty(propertyList.GetArrayElementAtIndex(index).FindPropertyRelative("VALUE"));
        element.Q<UnityEngine.UIElements.PopupWindow>().text = "Dialogue Item " + index;
    }

    private List<SerializedKeyValuePair<int, DialogueSO>> getCurrentList(){
        int size = propertyList.arraySize;
        List<SerializedKeyValuePair<int, DialogueSO>> list = new List<SerializedKeyValuePair<int, DialogueSO>>();
        for(int i = 0; i < size ; i++){
            SerializedKeyValuePair<int,DialogueSO> kvp = new SerializedKeyValuePair<int, DialogueSO>(propertyList.GetArrayElementAtIndex(i).FindPropertyRelative("KEY").intValue, (DialogueSO)propertyList.GetArrayElementAtIndex(i).FindPropertyRelative("VALUE").boxedValue);
            list.Add(kvp);
        }

        return list;
    }
}