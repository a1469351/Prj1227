using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>

namespace ns
{
	public class UIAlphaEffects : MonoBehaviour
    {

#if UNITY_EDITOR
        [CanEditMultipleObjects]
        [CustomEditor(typeof(UIAlphaEffects))]

        public class Inspector : Editor
        {
            SerializedProperty fadeInSettings;
            SerializedProperty fadeOutSettings;
            SerializedProperty pingPongSettings;
            void OnEnable()
            {
                fadeInSettings = serializedObject.FindProperty("fadeInSettings");
                fadeOutSettings = serializedObject.FindProperty("fadeOutSettings");
                pingPongSettings = serializedObject.FindProperty("pingPongSettings");
            }

            override public void OnInspectorGUI()
            {
                EditorGUI.BeginChangeCheck();

                serializedObject.Update();

                var myScript = target as UIAlphaEffects;

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(myScript), typeof(UIAlphaEffects), false);
                EditorGUI.EndDisabledGroup();

                myScript.mode = (Mode)EditorGUILayout.EnumPopup("Mode", myScript.mode);

                if (myScript.mode == Mode.PingPong)
                {
                    EditorGUILayout.PropertyField(pingPongSettings, new GUIContent("PingPong Settings"));
                }
                else if (myScript.mode == Mode.FadeIn)
                {
                    EditorGUILayout.PropertyField(fadeInSettings, new GUIContent("Fade In Settings"));
                }
                else if (myScript.mode == Mode.FadeOut)
                {
                    EditorGUILayout.PropertyField(fadeOutSettings, new GUIContent("Fade Out Settings"));
                }

                myScript.img = (Image)EditorGUILayout.ObjectField("Image", myScript.img, typeof(Image), true);

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(myScript);

                    serializedObject.ApplyModifiedProperties();
                }
            }

        }
#endif

        [System.Serializable]
        // Fade in datatype.
        public class FadeInSettings
        {
            [Tooltip("Fade speed.")]
            public float speed = 1;
        }

        [System.Serializable]
        // Fade out datatype.
        public class FadeOutSettings
        {
            [Tooltip("Fade speed.")]
            public float speed = 1;
            [Tooltip("Timer.")]
            public float timer;
        }

        [System.Serializable]
        // Ping pong datatype.
        public class PingPongSettings
        {
            [Tooltip("Fade speed.")]
            public float speed = 1;
            [Tooltip("Lowest possible value.")]
            public float min = .25f;
            [Tooltip("Highest possible value.")]
            public float max = 1;
            [Tooltip("Whether to start at random value or not.")]
            public bool startRandom;
        }

        //[Tooltip("Mode of alpha effects.")]
        public enum Mode { PingPong, FadeOut, FadeIn }

        // Mode can be Mode.PingPong, Mode.FadeOut or Mode.FadeIn
        [Tooltip("Mode of the alpha effects.")]
        public Mode mode = Mode.PingPong;

        // Have a look at the fade in datatype for settings.
        [Tooltip("Settings for fading in.")]
        public FadeInSettings fadeInSettings = new FadeInSettings();

        // Have a look at the fade out datatype for settings.
        [Tooltip("Settings for fading out.")]
        public FadeOutSettings fadeOutSettings = new FadeOutSettings();

        // Have a look at the ping pong datatype for settings.
        [Tooltip("Settings for ping pong effect.")]
        public PingPongSettings pingPongSettings = new PingPongSettings();

        // spriteRenderer AlphaEffects is using.
        [Tooltip("Image component used for alpha effects.")]
        public Image img;

        void Awake()
        {
            if (img == null) img = GetComponent<Image>();

            if (img != null) _color = img.color;

            if (mode == Mode.PingPong)
            {
                if (pingPongSettings.startRandom)
                {
                    var color = img.color;

                    color.a = Random.Range(pingPongSettings.min, pingPongSettings.max);

                    if (color.a < pingPongSettings.min)
                    {
                        color.a = pingPongSettings.min;

                        pingPongSettings.speed = Mathf.Abs(pingPongSettings.speed);
                    }

                    if (color.a > pingPongSettings.max)
                    {
                        color.a = pingPongSettings.max;

                        pingPongSettings.speed = -Mathf.Abs(pingPongSettings.speed);
                    }

                    img.color = color;
                }
            }
            else if (mode == Mode.FadeIn)
            {
                var color = img.color;
                color.a = 0;
                img.color = color;
            }
        }

        void OnDisable()
        {
            if (img != null) img.color = _color;
        }

        void Update()
        {
            if (mode == Mode.PingPong)
            {
                _UpdatePingPong();
            }
            else if (mode == Mode.FadeOut)
            {
                _UpdateFadeOut();
            }
            else if (mode == Mode.FadeIn && img.color.a != 1)
            {
                _UpdateFadeIn();
            }
        }

        void _UpdateFadeIn()
        {
            var color = img.color;

            color.a += fadeOutSettings.speed * Time.deltaTime;

            if (color.a > 1)
            {
                color.a = 1;

                img.color = color;
            }
            else
            {
                img.color = color;
            }
        }

        void _UpdateFadeOut()
        {
            if (fadeOutSettings.timer > 0)
            {
                fadeOutSettings.timer -= Time.deltaTime;

                if (fadeOutSettings.timer <= 0) fadeOutSettings.timer = 0;
            }
            else
            {
                var color = img.color;

                color.a -= fadeOutSettings.speed * Time.deltaTime;

                if (color.a < 0)
                {
                    color.a = 0;

                    img.color = color;

                    Destroy(gameObject);
                }
                else
                {
                    img.color = color;
                }
            }
        }

        void _UpdatePingPong()
        {
            var color = img.color;

            color.a += pingPongSettings.speed * Time.deltaTime;

            if (color.a < pingPongSettings.min)
            {
                color.a = pingPongSettings.min;

                pingPongSettings.speed = Mathf.Abs(pingPongSettings.speed);
            }

            if (color.a > pingPongSettings.max)
            {
                color.a = pingPongSettings.max;

                pingPongSettings.speed = -Mathf.Abs(pingPongSettings.speed);
            }

            img.color = color;
        }

        Color _color;
    }
}