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

        //After Ouside Clicked
        protected SerializedProperty delayProp2;
        protected SerializedProperty invokeTypeProp2;
        protected SerializedProperty staticEventProp2;
        protected SerializedProperty booleanParameterProp2;
        protected SerializedProperty booleanEventProp2;
        protected SerializedProperty integerParameterProp2;
        protected SerializedProperty integerEventProp2;
        protected SerializedProperty floatParameterProp2;
        protected SerializedProperty floatEventProp2;
        protected SerializedProperty stringParameterProp2;
        protected SerializedProperty stringEventProp2;
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
            
            delayProp2 = serializedObject.FindProperty("delay2");
            invokeTypeProp2 = serializedObject.FindProperty("invokeType2");
            staticEventProp2 = serializedObject.FindProperty("staticEvent2");
            booleanParameterProp2 = serializedObject.FindProperty("booleanParameter2");
            booleanEventProp2 = serializedObject.FindProperty("booleanEvent2");
            integerParameterProp2 = serializedObject.FindProperty("integerParameter2");
            integerEventProp2 = serializedObject.FindProperty("integerEvent2");
            floatParameterProp2 = serializedObject.FindProperty("floatParameter2");
            floatEventProp2 = serializedObject.FindProperty("floatEvent2");
            stringParameterProp2 = serializedObject.FindProperty("stringParameter2");
            stringEventProp2 = serializedObject.FindProperty("stringEvent2");
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

            EditorGUILayout.LabelField("Action When Clicked", EditorStyles.boldLabel);
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

            EditorGUILayout.LabelField("Action When Clicked Outside Character", EditorStyles.boldLabel);
            //After Clicked
            
            EditorGUILayout.PropertyField(delayProp2);
            EditorGUILayout.PropertyField(invokeTypeProp2);

            switch ((InvokeTypeClick2)invokeTypeProp2.enumValueIndex)
            {
            case InvokeTypeClick2.Static:
                EditorGUILayout.PropertyField(staticEventProp2);
                break;
            case InvokeTypeClick2.DynamicBoolean:
                EditorGUILayout.PropertyField(booleanEventProp2);
                EditorGUILayout.PropertyField(booleanParameterProp);
                break;
            case InvokeTypeClick2.DynamicInteger:
                EditorGUILayout.PropertyField(integerEventProp2);
                EditorGUILayout.PropertyField(integerParameterProp2);
                break;
            case InvokeTypeClick2.DynamicFloat:
                EditorGUILayout.PropertyField(floatEventProp2);
                EditorGUILayout.PropertyField(floatParameterProp2);
                break;
            case InvokeTypeClick2.DynamicString:
                EditorGUILayout.PropertyField(stringEventProp2);
                EditorGUILayout.PropertyField(stringParameterProp2);
                break;
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
