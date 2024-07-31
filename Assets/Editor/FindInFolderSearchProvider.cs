using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;

static class FindInFolderSearchProvider
{
    internal static string id = "infolder";
    internal static string name = "Find in folder";

    [SearchItemProvider]
    internal static SearchProvider CreateProvider()
    {
        return new SearchProvider(id, name)
        {
            filterId = "fif:",
            priority = 99999, // put example provider at a low priority
            fetchItems = (context, items, provider) =>
            {
                // That provider searches for tree prefabs in the project
                //context.searchQuery;+
                var querySplit = context.searchQuery.Split(" ", 2);
        
                var results = AssetDatabase.FindAssets(querySplit[^1], new []{querySplit[0]});
                foreach (var guid in results)
                {
                    items.Add(provider.CreateItem(context, AssetDatabase.GUIDToAssetPath(guid), null, null, null, null));
                }
                return null;
            },
            #pragma warning disable UNT0008 // Null propagation on Unity objects
            // Use fetch to load the asset asynchronously on display
            fetchThumbnail = (item, context) => AssetDatabase.GetCachedIcon(item.id) as Texture2D,
            fetchPreview = (item, context, size, options) => AssetDatabase.GetCachedIcon(item.id) as Texture2D,
            fetchLabel = (item, context) => AssetDatabase.LoadMainAssetAtPath(item.id)?.name,
            fetchDescription = (item, context) => AssetDatabase.LoadMainAssetAtPath(item.id)?.name,
            toObject = (item, type) => AssetDatabase.LoadAssetAtPath(item.id, typeof(Sprite)),
            #pragma warning restore UNT0008 // Null propagation on Unity objects
            // Shows handled actions in the preview inspector
            // Shows inspector view in the preview inspector (uses toObject)
            // showDetailsOptions = ShowDetailsOptions.Inspector | ShowDetailsOptions.Preview,
            // trackSelection = (item, context) =>
            // {
            //     var obj = AssetDatabase.LoadMainAssetAtPath(item.id);
            //     if (obj != null)
            //         EditorGUIUtility.PingObject(obj.GetInstanceID());
            // },
            // startDrag = (item, context) =>
            // {
            //     var obj = AssetDatabase.LoadMainAssetAtPath(item.id);
            //     if (obj != null)
            //     {
            //         DragAndDrop.PrepareStartDrag();
            //         DragAndDrop.objectReferences = new Object[] { obj };
            //         DragAndDrop.StartDrag(item.label);
            //     }
            // }
        };
    }
}
