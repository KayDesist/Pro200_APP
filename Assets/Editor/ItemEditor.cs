using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

// Ensure PetMeter enum is accessible. If not, adjust the using or define a mock enum for editor.
[CustomEditor(typeof(ItemBase))]
public class ItemEditor : Editor
{
    SerializedObject serializedItemBase;
    SerializedProperty meterAdjustmentsProp;

    void OnEnable()
    {
        serializedItemBase = new SerializedObject(target);
        meterAdjustmentsProp = serializedItemBase.FindProperty("meterAdjustments");
    }

    public override void OnInspectorGUI()
    {
        serializedItemBase.Update();

        ItemBase itemBase = (ItemBase)target;

        // Draw default inspector for other fields
        DrawDefaultInspector();

        EditorGUILayout.LabelField("Meter Adjustments", EditorStyles.boldLabel);

        // Get the dictionary via reflection (since Unity can't serialize Dictionary directly)
        var dictField = typeof(ItemBase).GetField("meterAdjustments", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var dict = dictField.GetValue(itemBase) as Dictionary<PetMeter, float>;

        if (dict == null)
        {
            dict = new Dictionary<PetMeter, float>();
            dictField.SetValue(itemBase, dict);
        }

        // Display existing entries
        List<PetMeter> keysToRemove = new List<PetMeter>();
        float newValue;
        foreach (var kvp in dict)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(kvp.Key.ToString(), GUILayout.Width(100));
            newValue = EditorGUILayout.FloatField(kvp.Value, GUILayout.Width(100));
            if (newValue != kvp.Value)
            {
                dict[kvp.Key] = newValue;
                EditorUtility.SetDirty(itemBase);
            }
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                keysToRemove.Add(kvp.Key);
            }
            EditorGUILayout.EndHorizontal();
        }
        foreach (var key in keysToRemove)
        {
            dict.Remove(key);
            EditorUtility.SetDirty(itemBase);
        }

        // Add new entry
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add New Meter Adjustment", EditorStyles.boldLabel);

        PetMeter newKey = (PetMeter)EditorGUILayout.EnumPopup("Meter", default(PetMeter));
        newValue = EditorGUILayout.FloatField("Value", 0f);

        if (GUILayout.Button("Add") && !dict.ContainsKey(newKey))
        {
            dict.Add(newKey, newValue);
            EditorUtility.SetDirty(itemBase);
        }

        serializedItemBase.ApplyModifiedProperties();
    }
}
