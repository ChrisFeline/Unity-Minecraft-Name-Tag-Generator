/*
MIT License

Copyright (c) 2021 Kittenji

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.IO;
using UnityEngine;
using UnityEditor;

namespace Kittenji {
    public class MCNameTag : EditorWindow
    {
        private static bool initiated;
        private static Texture2D font_texture;
        private static GUIStyle style_BoldLabel;
        private static GUIStyle style_CenterLabel;

        private static Color color_background;
        private static Color color_font;
        
        private static string nametag_content;

        [MenuItem("Tools/Kittenji/Minecraft Name Tag Generator")]
        private static void Init()
        {
            var window = EditorWindow.GetWindow<MCNameTag>();
            window.Show();

            window.titleContent = new GUIContent("Name Tag Generator");
            window.minSize = new Vector2(300,125);
            window.maxSize = window.minSize;

            style_BoldLabel = new GUIStyle(EditorStyles.label);
            style_CenterLabel = new GUIStyle(EditorStyles.label);
            style_BoldLabel.fontStyle = FontStyle.Bold;

            style_CenterLabel.alignment = TextAnchor.MiddleCenter;
            style_CenterLabel.normal.textColor = new Color ( 0.35f,0.35f,0.35f, 1 );

            color_background = new Color(0,0,0,0.25f);
            color_font = Color.white;

            nametag_content = "Creeper";

            initiated = true;
        }

        private void OnGUI()
        {
            if (!initiated) Init();
            DrawSeparator(1, 5.0f);

            nametag_content = GUILayout.TextField(nametag_content);
            DrawSeparator(1, 5.0f);
            color_background = EditorGUILayout.ColorField("Background Color", color_background);
            color_font = EditorGUILayout.ColorField("Font Color", color_font);
            DrawSeparator(1, 5.0f);
            GUI.enabled = !string.IsNullOrEmpty(nametag_content) && !string.IsNullOrWhiteSpace(nametag_content);
            if (GUILayout.Button("Export")) Execute();
            DrawSeparator(1, 5.0f);
            GUILayout.Label("Kittenji's Minecraft Name Tag Generator", style_CenterLabel);
            DrawSeparator(1, 5.0f);
        }

        private void OnEnable() {
            font_texture = KFile.Base64ToTexture2D("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");
        }

        private void Execute() {
            int len = nametag_content.Length;
            Print("Processing " + len + " characters.");
            Character[] characters = new Character[len];
            
            int w = font_bound.x;
            int h = font_bound.y;
            int s = 1;
            for (int i = 0; i < len; i++) {
                char c = nametag_content[i];
                int a = (int)c;
                int b = (a*w);
                int x = b % (w * font_bound.z);
                int y = (b / (h * font_bound.w)) * w;
                int _y = (font_texture.height - y) - h;
                
                var chh = (characters[i] = new Character(x,_y,w,h));
                s += chh.width + 1;
            }

            Texture2D temp = new Texture2D(s+1,10);
            temp.filterMode = FilterMode.Point;
            Color[] colors = new Color[temp.width * temp.height];
            for (int i = 0; i < colors.Length; i++) colors[i].a = 0;
            temp.SetPixels(0,0,temp.width,temp.height,colors);

            int j = 1;
            for (int i = 0; i < characters.Length; i++) {
                Character chr = characters[i];
                temp.SetPixels(j, 1, chr.width, chr.height, chr.pixels);
                j += 1+chr.width;
            }

            colors = temp.GetPixels(0,0,temp.width,temp.height);
            for (int i = 0; i < colors.Length; i++) colors[i] = colors[i].a > 0.5 ? color_font : color_background;
            temp.SetPixels(0,0,temp.width,temp.height,colors);
            temp.Apply();

            string save_path = EditorUtility.SaveFilePanelInProject("Save New Texture", "MC " + SanitizeFileName(nametag_content), "png", "Select where to save this new texture.");
            if (string.IsNullOrEmpty(save_path) || string.IsNullOrWhiteSpace(save_path)) return;

            string savedat = KFile.SaveTextureAsPNG(temp, save_path);
            string filenam = Path.GetFileName(save_path);

            EditorUtility.DisplayDialog("File saved.", Print($"Exported \"{savedat}\" as \"{filenam}\""), "Okay");

            AssetDatabase.Refresh();
        }

        private struct Character {
            public int x, y, width, height;
            public Character(int _x, int _y, int w, int h) {
                width = 0; height = h;
                x = _x; y = _y;
                
                Color[] pxs = font_texture.GetPixels(x,_y,w,h);
                for (int i = 0; i < pxs.Length; i++) {
                    if (pxs[i].a > 0.5) width = Mathf.Max(width,(i%w)+1);
                }
                if (width < 1) width = (w/2);
            }
            public Color[] pixels { get { return font_texture.GetPixels(x,y,width,height); } }
        }

        #region Methods
        private static string SanitizeFileName( string name )
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape( new string( System.IO.Path.GetInvalidFileNameChars() ) );
            string invalidRegStr = string.Format( @"([{0}]*\.+$)|([{0}]+)", invalidChars );

            return System.Text.RegularExpressions.Regex.Replace( name, invalidRegStr, "_" );
        }
        private void DrawSeparator( int i_height = 1, float space = 0)
        {
            GUILayout.Space(space/2.0f);
            Rect rect = EditorGUILayout.GetControlRect(false, i_height );
            rect.height = i_height;
            EditorGUI.DrawRect(rect, new Color ( 0.5f,0.5f,0.5f, 1 ) );
            GUILayout.Space(space/2.0f);
        }

        // Stuff?
        public static class KFile
        {
            static readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
            public static string Format(int bytes)
            {
                int counter = 0;
                float number = (float)bytes;
                while (Mathf.Round(number / (float)1024) >= 1)
                {
                    number = (float)number / (float)1024;
                    counter++;
                }
                return string.Format("{0:n1} {1}", number, suffixes[counter]);
            }
            public static string SaveTextureAsPNG(Texture2D _texture, string _fullPath)
            {
                byte[] _bytes = _texture.EncodeToPNG();
                File.WriteAllBytes(_fullPath, _bytes);
                return KFile.Format(_bytes.Length);
            }
            public static Texture2D Base64ToTexture2D(string encodedData)
            {
                byte[] imageData = System.Convert.FromBase64String(encodedData);
                
                int width, height;
                GetImageSize(imageData, out width, out height);
                
                Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false, true);
                texture.hideFlags = HideFlags.HideAndDontSave;
                texture.filterMode = FilterMode.Point;
                texture.LoadImage(imageData);
                
                return texture;
            }
            private static void GetImageSize(byte[] imageData, out int width, out int height)
            {
                width = ReadInt(imageData, 3 + 15);
                height = ReadInt(imageData, 3 + 15 + 2 + 2);
            }
            private static int ReadInt(byte[] imageData, int offset)
            {
                return (imageData[offset] << 8) | imageData[offset + 1];
            }
        }
        private static string Print(string content) {
            Debug.Log("[MC-Name-Tag] " + content);
            return content;
        }
        #endregion

        private struct V4Int {
            public int x, y, z, w;
            public V4Int(int _x, int _y, int _z, int _w) { x = _x; y = _y; z = _z; w = _w; }
        }
        private static readonly V4Int font_bound = new V4Int(8,8,16,16);
    }
}