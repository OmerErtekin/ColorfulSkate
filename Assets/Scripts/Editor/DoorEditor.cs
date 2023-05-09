#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(ColorDoor))]
public class DoorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Set door with parameters"))
        {
            foreach(Object target in targets)
            {
                ColorDoor door = (ColorDoor)target;
                door.SetDoorModel();
            }
        }
    }
}

#endif
