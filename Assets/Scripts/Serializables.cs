using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Serializables : MonoBehaviour
{
    public enum DrawMode
    {
        ColorMap,
        Mesh,
        FallOff
    };

    [DataContract(Name = "FallOffDirection")]
    public enum FallOffDirection
    {
        [EnumMember] X,
        [EnumMember] Y
    };

    [DataContract(Name = "FalloffType")]
    public enum FallOffType
    {
        [EnumMember] Island,
        [EnumMember] Coast,
        [EnumMember] Lake
    }

    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color color;
    }

    [System.Serializable]
    public struct UIInputs
    {
        public Slider size;
        public TMP_InputField seed;
        public TMP_InputField offsetX;
        public TMP_InputField offsetY;
        public Toggle useFallOff;
        public TMP_Dropdown fallOffType;
        public Slider fallOffRate;
        public TMP_Dropdown fallOffDirection;
    }

    [System.Serializable]
    public struct MapArgs
    {
        public DrawMode drawMode;
        public int editorPreivewLevelOfDetail;
        public float noiseScale;
        public int octaves;
        public float persistance;
        public float lacunarity;
        public float meshHeightMultiplier;
        public AnimationCurve meshHeightCurve;
        public TerrainType[] regions;
    }

    [System.Serializable]
    public struct MapData
    {
        public float[,] heightMap;
        public Color[] colorMap;

        public MapData(float[,] heightMap, Color[] colorMap)
        {
            this.heightMap = heightMap;
            this.colorMap = colorMap;
        }
    }
}