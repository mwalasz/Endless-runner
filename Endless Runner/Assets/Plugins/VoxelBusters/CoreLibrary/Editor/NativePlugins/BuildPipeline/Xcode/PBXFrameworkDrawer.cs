using UnityEngine;
using UnityEditor;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    [CustomPropertyDrawer(typeof(PBXFramework))]
    public class PBXFrameworkDrawer : PropertyDrawer 
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
            float   maxWidth        = position.width * .3f;
            float   offset          = position.width * .01f;
            Rect    nameRect        = new Rect(position.x, position.y, maxWidth, position.height);
            Rect    targetRect      = new Rect(nameRect.xMax + offset, position.y, maxWidth, position.height);
            Rect    optionalRect    = new Rect(targetRect.xMax + offset, position.y, maxWidth, position.height);
            int     indentLevel     = EditorGUI.indentLevel;

            EditorGUI.indentLevel   = 0;
            EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("m_name"), GUIContent.none);
            EditorGUI.PropertyField(targetRect, property.FindPropertyRelative("m_target"), GUIContent.none);
            EditorGUI.PropertyField(optionalRect, property.FindPropertyRelative("m_isOptional"), GUIContent.none);
            EditorGUI.indentLevel   = indentLevel;
            
            EditorGUI.EndProperty();
        }
        
        #endregion
    }
}