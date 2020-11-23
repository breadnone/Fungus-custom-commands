// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEditor;
using UnityEngine;

namespace Fungus.EditorUtils
{
    [CustomEditor (typeof(PortraitAnim))]
    public class PortraitAnimEditor : CommandEditor
    {
        protected SerializedProperty enblst;
        protected SerializedProperty stageProp;
        protected SerializedProperty displayProp;
        protected SerializedProperty characterProp;
        protected SerializedProperty portraitProp1;
        protected SerializedProperty portraitProp2;
        protected SerializedProperty portraitProp3;
        protected SerializedProperty portraitProp4;
        protected SerializedProperty portraitProp5;
        protected SerializedProperty framedelay;
        protected SerializedProperty reverseloop;        
        protected SerializedProperty usecyclesrange;
        protected SerializedProperty cycles;
        protected SerializedProperty randomenddelay;
        protected SerializedProperty endframedelay;
        public override void OnEnable()
        {
            base.OnEnable();
            enblst = serializedObject.FindProperty("enableAnimation");
            stageProp = serializedObject.FindProperty("stage");
            displayProp = serializedObject.FindProperty("display");
            characterProp = serializedObject.FindProperty("character");
            portraitProp1 = serializedObject.FindProperty("portrait1");
            portraitProp2 = serializedObject.FindProperty("portrait2");
            portraitProp3 = serializedObject.FindProperty("portrait3");
            portraitProp4 = serializedObject.FindProperty("portrait4");
            portraitProp5 = serializedObject.FindProperty("portrait5");
            framedelay = serializedObject.FindProperty("frameDelay");
            reverseloop = serializedObject.FindProperty("reverseLoop");            
            usecyclesrange = serializedObject.FindProperty("useCyclesRange");
            cycles = serializedObject.FindProperty("cycles");
            randomenddelay = serializedObject.FindProperty("RandomEndDelay");
            endframedelay = serializedObject.FindProperty("endFrameDelay");
        }
        
        public override void DrawCommandGUI() 
        {
            serializedObject.Update();
            
            PortraitAnim t = target as PortraitAnim;


            // Format Enum names
            string[] displayLabels = StringFormatter.FormatEnumNames(t.Display,"<None>");
            displayProp.enumValueIndex = EditorGUILayout.Popup("Display", (int)displayProp.enumValueIndex, displayLabels);

            string characterLabel = "Character";
            
            CommandEditor.ObjectField<Character>(characterProp, 
                                                 new GUIContent(characterLabel, "Character to display"), 
                                                 new GUIContent("<None>"),
                                                 Character.ActiveCharacters);

            // Only show optional portrait fields once required fields have been filled...
            if (t._Character != null)                // Character is selected
            {
                if (t._Character.Portraits == null ||    // Character has a portraits field
                    t._Character.Portraits.Count <= 0 )   // Character has at least one portrait
                {
                    EditorGUILayout.HelpBox("This character has no portraits. Please add portraits to the character's prefab before using this command.", MessageType.Error);
                }

            }
            if (t.Display != DisplayType.None && t._Character != null) 
            {
        
                    // PORTRAIT
                    CommandEditor.ObjectField<Sprite>(portraitProp1, 
                                                      new GUIContent("Portrait1", "Portrait representing character"), 
                                                      new GUIContent(""),
                                                      t._Character.Portraits);
                    CommandEditor.ObjectField<Sprite>(portraitProp2, 
                                                      new GUIContent("Portrait2", "Portrait representing character"), 
                                                      new GUIContent(""),
                                                      t._Character.Portraits);
                    CommandEditor.ObjectField<Sprite>(portraitProp3, 
                                                      new GUIContent("Portrait3", "Portrait representing character"), 
                                                      new GUIContent(""),
                                                      t._Character.Portraits);
                    CommandEditor.ObjectField<Sprite>(portraitProp4, 
                                                      new GUIContent("Portrait4", "Portrait representing character"), 
                                                      new GUIContent(""),
                                                      t._Character.Portraits);
                    CommandEditor.ObjectField<Sprite>(portraitProp5, 
                                                      new GUIContent("Portrait5", "Portrait representing character"), 
                                                      new GUIContent(""),
                                                      t._Character.Portraits);
   
                EditorGUILayout.PropertyField(stageProp);
                EditorGUILayout.PropertyField(enblst);
                EditorGUILayout.PropertyField(framedelay);
                EditorGUILayout.PropertyField(reverseloop);
                EditorGUILayout.PropertyField(usecyclesrange);
                EditorGUILayout.PropertyField(cycles);
                EditorGUILayout.PropertyField(randomenddelay);
                EditorGUILayout.PropertyField(endframedelay);
                


            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}