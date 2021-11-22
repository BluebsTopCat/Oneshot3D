using System.Linq;
using UnityEditor;

[CustomEditor(typeof(SceneSwitcher))]
public class Sceditor : Editor
{
    private string[] scenes;
    
    
    public override void OnInspectorGUI()
    {
        SceneSwitcher myTarget = (SceneSwitcher)target;
        int _choiceindex = myTarget.sceneid;
        scenes = EditorBuildSettings.scenes
            .Where( scene => scene.enabled )
            .Select( scene => scene.path)
            .ToArray();
        DrawDefaultInspector();

        _choiceindex = EditorGUILayout.Popup("Scene", _choiceindex, scenes);
        // Update the selected choice in the underlying object
        myTarget.sceneid = _choiceindex;
    }
    
}