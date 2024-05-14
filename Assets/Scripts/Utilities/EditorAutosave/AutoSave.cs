using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class AutoSave
{
    static AutoSave()
    {
        EditorApplication.playModeStateChanged += (state) =>
        {
            if (state != PlayModeStateChange.ExitingEditMode) return;
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
        };
    }
}
