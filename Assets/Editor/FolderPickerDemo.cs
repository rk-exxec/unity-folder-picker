using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.Search;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;


public class FolderPickerDemo : EditorWindow
{
    [SerializeField] private VisualTreeAsset mainUITemplate;
    
    /// <summary>
    /// Button to open the editor window Tools in the toolbar
    /// </summary>
    [MenuItem("Tools/FolderPickerDemo", priority = 0)]
    public static void ShowExample()
    {
        FolderPickerDemo wnd = GetWindow<FolderPickerDemo>(typeof(UnityEditor.SceneView));
        wnd.titleContent = new GUIContent("FolderPickerDemo");
    }

    public void CreateGUI()
    {
        rootVisualElement.Clear();
        // Import UXML
        var content = mainUITemplate.Instantiate();
        // make sure UI fills entire window
        content.style.height = Length.Percent(100);
        rootVisualElement.Add(content);
        var objPickrButton = rootVisualElement.Q<Button>("openObjectPckrBtn");
        var objectField = rootVisualElement.Q<ObjectField>("objectSelect");
        objPickrButton.RegisterCallback<ClickEvent>(e => SpritePickerClicked(e, objectField));
        
    }
    
    /// <summary>
    /// show a custom ObjectPicker for selecting sprites at a certain path
    /// uses custom search provider to filter for sprites -> FindInFolderSearchProvider
    /// </summary>
    /// <param name="e"></param>
    /// <param name="element">element that was target of event</param>
    private void SpritePickerClicked(ClickEvent e, VisualElement element)
    {
        // Here you can define your custom path after the "fif:" prefix
        var context = SearchService.CreateContext("infolder", "fif:Assets/Editor t:Sprite");
        var picker = SearchService.ShowPicker(
            context, 
            (item,cancelled) => SelectHandler(item, cancelled, element ), 
            (item) => TrackingHandler(item, element),
            title: ((ObjectField)element).label
            );
    }
    
    /// <summary>
    /// handler for when the SpritePicker is closed or made a definite selection
    /// </summary>
    /// <param name="searchItem"></param>
    /// <param name="canceled"></param>
    /// <param name="element"></param>
    static void SelectHandler(SearchItem searchItem, bool canceled, VisualElement element)
    {
        // action on double click or enter on the selected item
        // if(canceled) return;
    }

    /// <summary>
    /// handler for single clicking on an item in the search results of the SpritePicker
    /// updates the targeted element with the selected item
    /// </summary>
    /// <param name="searchItem">selected item</param>
    /// <param name="element">element to update</param>
    static void TrackingHandler(SearchItem searchItem, VisualElement element)
    {
        // action on single click or selection via keyboard
        ((ObjectField)element).value = (Sprite)searchItem.ToObject();
    }
    
}
