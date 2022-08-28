
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

public class CubemapTextureBuilder : EditorWindow
{
    [MenuItem("Tools/Cubemap Builder")]
    public static void OpenWindow()
    {
        GetWindow<CubemapTextureBuilder>();
    }

    Texture2D[] textures = new Texture2D[6];
    string[] labels = new string[] {
        "Right",    "Left",
        "Top",      "Bottom",
        "Front",    "Back"
    };

    TextureFormat[] HDRFormats = new TextureFormat[] {
        TextureFormat.ASTC_HDR_10x10 ,
        TextureFormat.ASTC_HDR_12x12 ,
        TextureFormat.ASTC_HDR_4x4 ,
        TextureFormat.ASTC_HDR_5x5 ,
        TextureFormat.ASTC_HDR_6x6 ,
        TextureFormat.ASTC_HDR_8x8 ,
        TextureFormat.BC6H ,
        TextureFormat.RGBAFloat ,
        TextureFormat.RGBAHalf
    };

    Vector2Int[] placementRects = new Vector2Int[]
    {
        new Vector2Int(2, 1),
        new Vector2Int(0, 1),
        new Vector2Int(1, 2),
        new Vector2Int(1, 0),
        new Vector2Int(1, 1),
        new Vector2Int(3, 1),
    };


    private void OnGUI()
    {
        for (int i = 0; i < 6; i++)
        {
            textures[i] = EditorGUILayout.ObjectField(labels[i], textures[i], typeof(Texture2D), false) as Texture2D;
        }

        if (GUILayout.Button("Build Cubemap"))
        {
            // Missing Texture
            if (textures.Any(t => t == null))
            {
                EditorUtility.DisplayDialog("Cubemap Builder Error", "One or more texture is missing.", "Ok");
                return;
            }

            // Get size
            var size = textures[0].width;

            // Not all of the same size or square
            if (textures.Any(t => (t.width != size) || (t.height != size)))
            {
                EditorUtility.DisplayDialog("Cubemap Builder Error", "All the textures need to be the same size and square.", "Ok");
                return;
            }

            var isHDR = HDRFormats.Any(f => f == textures[0].format);
            var texturePaths = textures.Select(t => AssetDatabase.GetAssetPath(t)).ToArray();

            // Should be ok, ask for the file path.
            var path = EditorUtility.SaveFilePanel("Save Cubemap", Path.GetDirectoryName(texturePaths[0]), "Cubemap", isHDR ? "exr" : "png");

            if (string.IsNullOrEmpty(path)) return;

            // Save the readable flag to restore it afterwards
            var readableFlags = textures.Select(t => t.isReadable).ToArray();

            // Get the importer and mark the textures as readable
            var importers = texturePaths.Select(p => TextureImporter.GetAtPath(p) as TextureImporter).ToArray();

            foreach (var importer in importers)
            {
                importer.isReadable = true;
            }

            AssetDatabase.Refresh();

            foreach (var p in texturePaths)
            {
                AssetDatabase.ImportAsset(p);
            }

            // Build the cubemap texture
            var cubeTexture = new Texture2D(size * 4, size * 3, isHDR ? TextureFormat.RGBAFloat : TextureFormat.RGBA32, false);

            for (int i = 0; i < 6; i++)
            {
                cubeTexture.SetPixels(placementRects[i].x * size, placementRects[i].y * size, size, size, textures[i].GetPixels(0));
            }

            cubeTexture.Apply(false);

            // Save the texture to the specified path, and destroy the temporary object
            var bytes = isHDR ? cubeTexture.EncodeToEXR() : cubeTexture.EncodeToPNG();

            File.WriteAllBytes(path, bytes);

            DestroyImmediate(cubeTexture);

            // Reset the read flags, and reimport everything
            for (var i = 0; i < 6; i++)
            {
                importers[i].isReadable = readableFlags[i];
            }

            path = path.Remove(0, Application.dataPath.Length - 6);

            AssetDatabase.ImportAsset(path);

            var cubeImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            cubeImporter.textureShape = TextureImporterShape.TextureCube;
            cubeImporter.sRGBTexture = false;
            cubeImporter.generateCubemap = TextureImporterGenerateCubemap.FullCubemap;

            foreach (var p in texturePaths)
            {
                AssetDatabase.ImportAsset(p);
            }
            AssetDatabase.ImportAsset(path);

            AssetDatabase.Refresh();
        }
    }
}