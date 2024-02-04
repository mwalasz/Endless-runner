using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.Editor
{
    [CustomPropertyDrawer(typeof(RuntimePlatformConstant))]
    public class RuntimePlatformConstantDrawer : PropertyDrawer 
    {
        #region Unity methods
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
        {
            return EditorGUIUtility.singleLineHeight;
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
        {
            // show property name label
            label      = EditorGUI.BeginProperty(position, label, property);
            position   = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // show property attributes
            Rect    platformRect    = new Rect(position.x, position.y, 60f, position.height);
            Rect    idRect          = new Rect(position.x + 65f, position.y, position.width - 65f, position.height);
            int     indentLevel     = EditorGUI.indentLevel;

            EditorGUI.indentLevel   = 0;
            EditorGUI.PropertyField(platformRect, property.FindPropertyRelative("m_platform"), GUIContent.none);
            EditorGUI.PropertyField(idRect, property.FindPropertyRelative("m_value"), GUIContent.none);
            EditorGUI.indentLevel   = indentLevel;
            
            EditorGUI.EndProperty();
        }
        
        #endregion
    }
}