// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEditor;

namespace Fungus.EditorUtils
{
    [CustomEditor (typeof(ClickableCharacter))]
    public class ClickableCharacterEditor : CommandEditor 
    {
        //ClickableCharStartsHere
        protected SerializedProperty statusProp;
        protected SerializedProperty charProp;
        protected SerializedProperty camProp;
        protected SerializedProperty hitProp;
        protected SerializedProperty offsetProp;
        protected SerializedProperty debProp;
        //EndsHere

        protected SerializedProperty descriptionProp;
        protected SerializedProperty delayProp;
        protected SerializedProperty invokeTypeProp;
        protected SerializedProperty staticEventProp;
        protected SerializedProperty booleanParameterProp;
        protected SerializedProperty booleanEventProp;
        protected SerializedProperty integerParameterProp;
        protected SerializedProperty integerEventProp;
        protected SerializedProperty floatParameterProp;
        protected SerializedProperty floatEventProp;
        protected SerializedProperty stringParameterProp;
        protected SerializedProperty stringEventProp;

        public override void OnEnable()
        {
            base.OnEnable();
            statusProp = serializedObject.FindProperty("status");
            charProp = serializedObject.FindProperty("character");
            camProp = serializedObject.FindProperty("mainCam");
            hitProp = serializedObject.FindProperty("hitBoxSize");
            offsetProp = serializedObject.FindProperty("offsets");
            debProp = serializedObject.FindProperty("enableDebugLog");

            descriptionProp = serializedObject.FindProperty("description");
            delayProp = serializedObject.FindProperty("delay");
            invokeTypeProp = serializedObject.FindProperty("invokeType");
            staticEventProp = serializedObject.FindProperty("staticEvent");
            booleanParameterProp = serializedObject.FindProperty("booleanParameter");
            booleanEventProp = serializedObject.FindProperty("booleanEvent");
            integerParameterProp = serializedObject.FindProperty("integerParameter");
            integerEventProp = serializedObject.FindProperty("integerEvent");
            floatParameterProp = serializedObject.FindProperty("floatParameter");
            floatEventProp = serializedObject.FindProperty("floatEvent");
            stringParameterProp = serializedObject.FindProperty("stringParameter");
            stringEventProp = serializedObject.FindProperty("stringEvent");
        }

        public override void DrawCommandGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(statusProp);
            EditorGUILayout.PropertyField(charProp);
            EditorGUILayout.PropertyField(camProp);
            EditorGUILayout.PropertyField(hitProp);
            EditorGUILayout.PropertyField(offsetProp);
            EditorGUILayout.PropertyField(debProp);

            EditorGUILayout.PropertyField(descriptionProp);
            EditorGUILayout.PropertyField(delayProp);
            EditorGUILayout.PropertyField(invokeTypeProp);

            switch ((InvokeTypeClick)invokeTypeProp.enumValueIndex)
            {
            case InvokeTypeClick.Static:
                EditorGUILayout.PropertyField(staticEventProp);
                break;
            case InvokeTypeClick.DynamicBoolean:
                EditorGUILayout.PropertyField(booleanEventProp);
                EditorGUILayout.PropertyField(booleanParameterProp);
                break;
            case InvokeTypeClick.DynamicInteger:
                EditorGUILayout.PropertyField(integerEventProp);
                EditorGUILayout.PropertyField(integerParameterProp);
                break;
            case InvokeTypeClick.DynamicFloat:
                EditorGUILayout.PropertyField(floatEventProp);
                EditorGUILayout.PropertyField(floatParameterProp);
                break;
            case InvokeTypeClick.DynamicString:
                EditorGUILayout.PropertyField(stringEventProp);
                EditorGUILayout.PropertyField(stringParameterProp);
                break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
