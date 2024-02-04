using UnityEngine;
using UnityEditor;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    [CustomPropertyDrawer(typeof(PBXFile))]
    public class PBXFileDrawer : PropertyDrawer 
    {
        #region Unity methods
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
        {
            return EditorGUIUtility.singleLineHeight;
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
        {
            // show property name label
            label       = EditorGUI.BeginProperty(position, label, property);
            position    = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), GUIContent.none);

            // show property attributes
            Rect    refRect         = new Rect(position.x, position.y, position.width * 0.6f, position.height);
            Rect    flagsRect       = new Rect(refRect.xMax + 5f, position.y, position.width - refRect.width - 5f, position.height);
            int     indentLevel     = EditorGUI.indentLevel;

            EditorGUI.indentLevel   = 0;
            EditorGUI.PropertyField(refRect, property.FindPropertyRelative("m_reference"), GUIContent.none);
            EditorGUI.PropertyField(flagsRect, property.FindPropertyRelative("m_compileFlags"), GUIContent.none);
            EditorGUI.indentLevel   = indentLevel;
            
            EditorGUI.EndProperty();
        }
        
        #endregion
    }
}