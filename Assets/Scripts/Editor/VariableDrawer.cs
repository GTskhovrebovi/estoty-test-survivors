using Gameplay;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(Variable))]
    public class VariableDrawer : PropertyDrawer
    {
        private readonly string[] _popupOptions = { "Constant", "Variable"};
        private GUIStyle _popupStyle;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_popupStyle == null)
            {
                _popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
                _popupStyle.imagePosition = ImagePosition.ImageOnly;
            }

            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);
        
            EditorGUI.BeginChangeCheck();

            // Get properties
            SerializedProperty variableType = property.FindPropertyRelative("variableType");
            SerializedProperty constantValue = property.FindPropertyRelative("constantValue");
            SerializedProperty statType = property.FindPropertyRelative("statType");

            // Calculate rect for configuration button
            Rect buttonRect = new Rect(position);
            buttonRect.yMin += _popupStyle.margin.top;
            buttonRect.width = _popupStyle.fixedWidth + _popupStyle.margin.right;
            position.xMin = buttonRect.xMax + 30;

            // Store old indent level and set it to 0, the PrefixLabel takes care of it
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            int result = EditorGUI.Popup(buttonRect, variableType.intValue, _popupOptions, _popupStyle);

            variableType.intValue = result;
        
            switch (variableType.intValue)
            {
                case 0:
                    EditorGUI.PropertyField(position, constantValue, GUIContent.none);
                    break;
                case 1:
                    EditorGUI.PropertyField(position, statType, GUIContent.none);
                    break;
            }


            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}
