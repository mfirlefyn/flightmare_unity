using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using System;


public class TerrainEditor : MonoBehaviour
{
    // Constructor to hold terrain object
    public Terrain Terrain;
    // Constructor to hold texture
    //public Texture2D texture;  //= Resources.Load<Texture2D>("Black_Sand_BaseColor");
    private Object[] textures_diffuse;
    private Object[] textures_normal;
    private int textures_idx = 0;

    // heightmap variables
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool updateTerrainHeight;
    public bool updateTerrainTexture;

    public bool massPlaceTrees;

    public bool massPlaceGrass;

    public bool massPlaceBushes;
    private bool bushesPlaced = false;

    public bool massPlaceFerns;
    private bool fernsPlaced = false;

    public bool massPlacePlants;
    private bool plantsPlaced = false;

    public float textureSize;

    public static GameObject treePrefab0;
    public static GameObject treePrefab1;
    public static GameObject treePrefab2;
    public static GameObject treePrefab3;

    [Range(0,100)]
    public int chanceTrees = 40;     // chance that trees spawn in out of 1000
    public int treeSeed = 0;

    public static int nrTreesGenerated = 15000;

    public static Texture2D grassTexture0;
    public static Texture2D grassTexture1;
    public static Texture2D grassTexture2;
    public static Texture2D grassTexture3;
    public static Texture2D grassTexture4;
    public static Texture2D grassTexture5;
    public static Texture2D grassTexture6;
    public static Texture2D grassTexture7;
    public static Texture2D grassTexture8;
    public static Texture2D grassTexture9;
    public static Texture2D grassTexture10;
    public static Texture2D grassTexture11;

    [Range(0,100)]
    public int chanceGrass = 40;     // chance that grass spawn in out of 1000
    public int grassSeed = 0;

    public static GameObject bushPrefab0;
    public static GameObject bushPrefab1;
    public static GameObject bushPrefab2;
    public static GameObject bushPrefab3;

    [Range(0,100)]
    public int chanceBushes = 40;     // chance that bushes spawn in out of 1000
    public int bushSeed = 0;

    public static GameObject fernPrefab0;
    public static GameObject fernPrefab1;
    public static GameObject fernPrefab2;

    [Range(0,100)]
    public int chanceFerns = 40;     // chance that ferns spawn in out of 1000
    public int fernSeed = 0;

    public static GameObject plantPrefab0;
    public static GameObject plantPrefab1;
    public static GameObject plantPrefab2;
    public static GameObject plantPrefab3;

    [Range(0,100)]
    public int chancePlants = 40;     // chance that plants spawn in out of 1000
    public int plantSeed = 0;

    void Start()
    {
        //Load textures from Resources folder
        //Texture2D tex = Resources.Load<Texture2D>("Textures/ground1/ground1_Diffuse");
        //Texture2D tex = Resources.Load<Texture2D>("Textures/Grass & dead leafs 02/diffuse");
        //Texture2D norm =  Resources.Load<Texture2D>("Textures/Grass & dead leafs 02/normal");
        //GenerateLayer(tex,norm);
        textures_diffuse = Resources.LoadAll("Textures/Diffuse", typeof(Texture2D));
        textures_normal = Resources.LoadAll("Textures/Normal", typeof(Texture2D));

        treePrefab0 = Resources.Load("Prefabs/PF Conifer Bare BOTD") as GameObject;
        treePrefab1 = Resources.Load("Prefabs/PF Conifer Medium BOTD") as GameObject;
        treePrefab2 = Resources.Load("Prefabs/PF Conifer Small BOTD") as GameObject;
        treePrefab3 = Resources.Load("Prefabs/PF Conifer Tall BOTD") as GameObject;

        grassTexture0 = Resources.Load<Texture2D>("GrassTextures/grass01");
        grassTexture1 = Resources.Load<Texture2D>("GrassTextures/grass02");
        grassTexture2 = Resources.Load<Texture2D>("GrassTextures/grassFlower01");
        grassTexture3 = Resources.Load<Texture2D>("GrassTextures/grassFlower02");
        grassTexture4 = Resources.Load<Texture2D>("GrassTextures/grassFlower03");
        grassTexture5 = Resources.Load<Texture2D>("GrassTextures/grassFlower04");
        grassTexture6 = Resources.Load<Texture2D>("GrassTextures/grassFlower05");
        grassTexture7 = Resources.Load<Texture2D>("GrassTextures/grassFlower06");
        grassTexture8 = Resources.Load<Texture2D>("GrassTextures/grassFlower07");
        grassTexture9 = Resources.Load<Texture2D>("GrassTextures/grassFlower08");
        grassTexture10 = Resources.Load<Texture2D>("GrassTextures/grassFlower09");
        grassTexture11 = Resources.Load<Texture2D>("GrassTextures/grassFlower10");

        bushPrefab0 = Resources.Load("Prefabs/Bush_A") as GameObject;
        bushPrefab1 = Resources.Load("Prefabs/Bush_B") as GameObject;
        bushPrefab2 = Resources.Load("Prefabs/BushDry_A") as GameObject;
        bushPrefab3 = Resources.Load("Prefabs/BushDry_B") as GameObject;

        fernPrefab0 = Resources.Load("Prefabs/Fern_A") as GameObject;
        fernPrefab1 = Resources.Load("Prefabs/Fern_B") as GameObject;
        fernPrefab2 = Resources.Load("Prefabs/Fern_C") as GameObject;

        plantPrefab0 = Resources.Load("Prefabs/Plant_A") as GameObject;
        plantPrefab1 = Resources.Load("Prefabs/Plant_B") as GameObject;
        plantPrefab2 = Resources.Load("Prefabs/Plant_C") as GameObject;
        plantPrefab3 = Resources.Load("Prefabs/Plant_D") as GameObject;
    }

    void Update()
    {
        if (updateTerrainHeight) {
            GenerateHeight();
            updateTerrainHeight = false;
        }

        if (updateTerrainTexture) {
            Texture2D tex = (Texture2D)textures_diffuse[textures_idx];
            Texture2D norm = (Texture2D)textures_normal[textures_idx];
            ReplaceTerrainTexture(Terrain.terrainData, tex, norm, textureSize);
            updateTerrainTexture = false;
            textures_idx++;
        }

        if (massPlaceTrees) {
            AddTrees(Terrain,treeSeed,chanceTrees,treePrefab0,treePrefab1,treePrefab2,treePrefab3);
            massPlaceTrees = false;
        }

        if (massPlaceGrass) {
            AddGrass(Terrain,grassSeed,chanceGrass,grassTexture0,grassTexture1,grassTexture2,grassTexture3,grassTexture4,grassTexture5,grassTexture6,grassTexture7,grassTexture8,grassTexture9,grassTexture10,grassTexture11);
            massPlaceGrass = false;
        }

        if (massPlaceBushes && !bushesPlaced) {
            AddBushes(Terrain,bushSeed,chanceBushes,bushPrefab0,bushPrefab1,bushPrefab2,bushPrefab3);
            massPlaceBushes = false;
            bushesPlaced = true;
        }

        if (massPlaceFerns && !fernsPlaced) {
            AddFerns(Terrain,fernSeed,chanceFerns,fernPrefab0,fernPrefab1,fernPrefab2);
            massPlaceFerns = false;
            fernsPlaced = true;
        }

        if (massPlacePlants && !plantsPlaced) {
            AddPlants(Terrain,plantSeed,chancePlants,plantPrefab0,plantPrefab1,plantPrefab2,plantPrefab3);
            massPlacePlants = false;
            plantsPlaced = true;
        }
    }

    // check whether the noise map values are valid
    void OnValidate() 
    {
        if (mapWidth < 1) {
            mapWidth = 1;
        }
        if (mapHeight < 1) {
            mapHeight = 1;
        }
        if (lacunarity < 1) {
            lacunarity = 1;
        }
        if (octaves < 0) {
            octaves = 0;
        }

        if (textureSize <= 0) {
            textureSize = 1;
        }

        if (textures_idx == textures_diffuse.Length) {
            textures_idx = 0;
        }
    }

    void OnApplicationQuit()
    {
        //RmTerrainLayer(Terrain.terrainData);
        //RmTrees(Terrain.terrainData);
        //RmGrass(Terrain.terrainData);
    }

    // reference to be used in editor without hitting play
    public void GenerateLayer(Texture2D texture, Texture2D normal) {
        float[,] noiseMap = GenerateHeightmap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        // make the terrain attain to the heightmap
        this.Terrain.terrainData.SetHeights(0,0,noiseMap);

        //TerrainLayer terrainLayer = new TerrainLayer();
        SetTerrainTexture(Terrain.terrainData, texture, normal, textureSize);
    }

    public void GenerateHeight() {
        float[,] noiseMap = GenerateHeightmap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
        Debug.Log(noiseMap);

        //MapDisplay display = FindObjectOfType<MapDisplay> ();
        //display.DrawNoiseMap(noiseMap);
        // make the terrain attain to the heightmap
        this.Terrain.terrainData.SetHeights(0,0,noiseMap);
    }

    // generate the perlin noise map for heightmap (taken from seblague tut2)
    public static float[,] GenerateHeightmap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset) {
        float[,] noiseMap = new float[mapWidth,mapHeight];

        System.Random prng = new System.Random(seed);       // random seed to set new sequence
        Vector2[] octaveOffsets = new Vector2[octaves];     // phase differences for different frequencies of noise

        // setting the different used octaves
        for (int i = 0; i < octaves; i++) {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }
        // check positivity of scale
        if (scale <= 0) {
            scale = 0.0001f;
        }

        // set-up heightmap
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        // constructing heightmap
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
        
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++) {
                    float sampleX = (x-halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y-halfHeight) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY)*2 - 1;
                    noiseHeight += perlinValue*amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight) {
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap [x, y] = 0.01f*noiseHeight;
            }
        }

        // clip noisemap in between max and minimum values
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                noiseMap[x,y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x,y]);
            }
        }

        return noiseMap;
    }

    /// <summary>
    /// Adds the given texture as an extra layer to the given terrain.
    /// </summary>
    /// <param name="terrainData"><see cref="TerrainData"/> to modify the texture of.</param>
    /// <param name="texture">Texture to be used.</param>
    /// <param name="size">Size of the <see cref="Terrain"/> in meters.</param>
    public static void SetTerrainTexture(TerrainData terrainData, Texture2D texture, Texture2D normal, float size) {
        var newTextureLayer = new TerrainLayer();
        newTextureLayer.diffuseTexture = texture;
        newTextureLayer.normalMapTexture = normal;
        newTextureLayer.tileOffset = Vector2.zero;
        newTextureLayer.tileSize = Vector2.one * size;

        AddTerrainLayer(terrainData, newTextureLayer);
    }

    public static void ReplaceTerrainTexture(TerrainData terrainData, Texture2D texture, Texture2D normal, float size) {
        RmTerrainLayer(terrainData);

        var newTextureLayer = new TerrainLayer();
        newTextureLayer.diffuseTexture = texture;
        newTextureLayer.normalMapTexture = normal;
        newTextureLayer.tileOffset = Vector2.zero;
        newTextureLayer.tileSize = Vector2.one * size;

        AddTerrainLayer(terrainData, newTextureLayer);
    }

    /// <summary>
    /// Adds new <see cref="TerrainLayer"/> to the given <see cref="TerrainData"/> object.
    /// </summary>
    /// <param name="terrainData"><see cref="TerrainData"/> to add layer to.</param>
    /// <param name="inputLayer"><see cref="TerrainLayer"/> to add.</param>
    public static void AddTerrainLayer(TerrainData terrainData, TerrainLayer inputLayer) {
        if (inputLayer == null)
            return;

        var layers = terrainData.terrainLayers;
        for (var idx = 0; idx < layers.Length; ++idx)
        {
            if (layers[idx] == inputLayer)
                return;
        }

        int newIndex = layers.Length;
        var newarray = new TerrainLayer[newIndex + 1];
        System.Array.Copy(layers, 0, newarray, 0, newIndex);
        newarray[newIndex] = inputLayer;

        terrainData.terrainLayers = newarray;
    }
    
    public static void RmTerrainLayer(TerrainData terrainData) {
        terrainData.terrainLayers = null;
    }

    // from: https://forum.unity.com/threads/adding-trees-to-a-terrain-with-c.829560/
    // from: https://stackoverflow.com/questions/53880451/add-trees-to-terrain-c-sharp-with-treeinstance
    // tree generation
    public static void AddTrees(Terrain terrain, int seed, int prob, GameObject treePrefab0, GameObject treePrefab1, GameObject treePrefab2, GameObject treePrefab3) {
        TreePrototype treePrototype0 = new TreePrototype();
        treePrototype0.prefab = treePrefab0;
        TreePrototype treePrototype1 = new TreePrototype();
        treePrototype1.prefab = treePrefab1;
        TreePrototype treePrototype2 = new TreePrototype();
        treePrototype2.prefab = treePrefab2;
        TreePrototype treePrototype3 = new TreePrototype();
        treePrototype3.prefab = treePrefab3;
         
        TreePrototype[] treePrototypeCollection = new TreePrototype[4]{treePrototype0,treePrototype1,treePrototype2,treePrototype3};
        terrain.terrainData.treePrototypes = treePrototypeCollection;
         
        TreeInstance[] treeInstanceCollection = new TreeInstance[nrTreesGenerated];
        int idx = 0;

        UnityEngine.Random.InitState(seed);

        for (float x = 0; x < terrain.terrainData.heightmapResolution; x++) {
            //heightmapHeight not heightmapWidth
            for (float z = 0; z < terrain.terrainData.heightmapResolution; z++) {
                if (idx < treeInstanceCollection.Length) {
                    int r = UnityEngine.Random.Range(0, 1000);
                    if (r <= prob) {
                        //int posX = Array.IndexOf(position_lst_x,x);
                        //int posZ = Array.IndexOf(position_lst_z,z);
                        //if (posX <= -1 || posZ <= -1) {
                            TreeInstance treeTemp = new TreeInstance();
                            treeTemp.position = new Vector3(x/terrain.terrainData.heightmapResolution, 0, z/terrain.terrainData.heightmapResolution);
                            int selected = UnityEngine.Random.Range(0,4);
                            treeTemp.prototypeIndex = selected;
                            float scaling = UnityEngine.Random.Range(0.8f,1f);
                            treeTemp.widthScale = scaling;
                            treeTemp.heightScale = scaling;
                            float angle = UnityEngine.Random.Range(0f,2*Mathf.PI);
                            treeTemp.rotation = angle;
                            treeTemp.color = Color.white;
                            treeTemp.lightmapColor = Color.white;

                            treeInstanceCollection[idx] = treeTemp;
                            idx++;
                        //}
                    }
                }
            }
        }
         
        //TreeInstance[] treeInstanceCollection = new TreeInstance[1]{treeTemp};
        terrain.terrainData.SetTreeInstances(treeInstanceCollection, true);
    }

    public static void RmTrees(TerrainData terrainData) {
        TreeInstance[] treeInstanceCollection = new TreeInstance[0];
        terrainData.treeInstances = treeInstanceCollection;
        terrainData.treePrototypes = null;
    }

    // from: https://answers.unity.com/questions/1141423/placing-grass-on-terrain-in-script-on-a-certain-he.html
    // from: https://forum.unity.com/threads/trying-to-remove-terrain-details-vegetation-getdetaillayer-and-setdetaillayer-doesnt-work.925679/
    // grass generation
    public static void AddGrass(Terrain terrain, int seed, int prob, Texture2D grassTex0, Texture2D grassTex1, Texture2D grassTex2, Texture2D grassTex3, Texture2D grassTex4, Texture2D grassTex5, Texture2D grassTex6, Texture2D grassTex7, Texture2D grassTex8, Texture2D grassTex9, Texture2D grassTex10, Texture2D grassTex11) {
        DetailPrototype grassPrototype0 = new DetailPrototype();
        grassPrototype0.prototypeTexture = grassTex0;
        grassPrototype0.renderMode = DetailRenderMode.GrassBillboard;
        grassPrototype0.minWidth = 0.49f;
        grassPrototype0.maxWidth = 0.5f;
        grassPrototype0.minHeight = 0.19f;
        grassPrototype0.maxHeight = 0.2f;
        grassPrototype0.noiseSeed = 0;
        grassPrototype0.noiseSpread = 0.1f;
        //grassPrototype.holeEdgePadding = 0f;
        //Color healthyColor = Color.green;
        //grassPrototype.healthyColor = healthyColor;
        //Color dryColor = Color.yellow;
        //grassPrototype.dryColor = dryColor;
        DetailPrototype grassPrototype1 = new DetailPrototype();
        grassPrototype1.prototypeTexture = grassTex1;
        grassPrototype1.renderMode = DetailRenderMode.GrassBillboard;
        grassPrototype1.minWidth = 0.49f;
        grassPrototype1.maxWidth = 0.5f;
        grassPrototype1.minHeight = 0.19f;
        grassPrototype1.maxHeight = 0.2f;
        grassPrototype1.noiseSeed = 1;
        grassPrototype1.noiseSpread = 0.1f;

        DetailPrototype grassPrototype2 = new DetailPrototype();
        grassPrototype2.prototypeTexture = grassTex2;
        grassPrototype2.renderMode = DetailRenderMode.GrassBillboard;
        grassPrototype2.minWidth = 0.49f;
        grassPrototype2.maxWidth = 0.5f;
        grassPrototype2.minHeight = 0.3f;
        grassPrototype2.maxHeight = 0.7f;
        grassPrototype2.noiseSeed = 2;
        grassPrototype2.noiseSpread = 0.1f;

        DetailPrototype grassPrototype3 = new DetailPrototype();
        grassPrototype3.prototypeTexture = grassTex3;
        grassPrototype3.renderMode = DetailRenderMode.GrassBillboard;
        grassPrototype3.minWidth = 0.49f;
        grassPrototype3.maxWidth = 0.5f;
        grassPrototype3.minHeight = 0.3f;
        grassPrototype3.maxHeight = 0.7f;
        grassPrototype3.noiseSeed = 3;
        grassPrototype3.noiseSpread = 0.1f;

        DetailPrototype grassPrototype4 = new DetailPrototype();
        grassPrototype4.prototypeTexture = grassTex4;
        grassPrototype4.renderMode = DetailRenderMode.GrassBillboard;
        grassPrototype4.minWidth = 0.49f;
        grassPrototype4.maxWidth = 0.5f;
        grassPrototype4.minHeight = 0.3f;
        grassPrototype4.maxHeight = 0.7f;
        grassPrototype4.noiseSeed = 4;
        grassPrototype4.noiseSpread = 0.1f;

        DetailPrototype grassPrototype5 = new DetailPrototype();
        grassPrototype5.prototypeTexture = grassTex5;
        grassPrototype5.renderMode = DetailRenderMode.GrassBillboard;
        grassPrototype5.minWidth = 0.49f;
        grassPrototype5.maxWidth = 0.5f;
        grassPrototype5.minHeight = 0.3f;
        grassPrototype5.maxHeight = 0.7f;
        grassPrototype5.noiseSeed = 5;
        grassPrototype5.noiseSpread = 0.1f;

        DetailPrototype grassPrototype6 = new DetailPrototype();
        grassPrototype6.prototypeTexture = grassTex6;
        grassPrototype6.renderMode = DetailRenderMode.GrassBillboard;
        grassPrototype6.minWidth = 0.49f;
        grassPrototype6.maxWidth = 0.5f;
        grassPrototype6.minHeight = 0.3f;
        grassPrototype6.maxHeight = 0.7f;
        grassPrototype6.noiseSeed = 6;
        grassPrototype6.noiseSpread = 0.1f;

        DetailPrototype grassPrototype7 = new DetailPrototype();
        grassPrototype7.prototypeTexture = grassTex7;
        grassPrototype7.renderMode = DetailRenderMode.GrassBillboard;
        grassPrototype7.minWidth = 0.49f;
        grassPrototype7.maxWidth = 0.5f;
        grassPrototype7.minHeight = 0.3f;
        grassPrototype7.maxHeight = 0.7f;
        grassPrototype7.noiseSeed = 7;
        grassPrototype7.noiseSpread = 0.1f;

        DetailPrototype grassPrototype8 = new DetailPrototype();
        grassPrototype8.prototypeTexture = grassTex8;
        grassPrototype8.renderMode = DetailRenderMode.GrassBillboard;
        grassPrototype8.minWidth = 0.49f;
        grassPrototype8.maxWidth = 0.5f;
        grassPrototype8.minHeight = 0.3f;
        grassPrototype8.maxHeight = 0.7f;
        grassPrototype8.noiseSeed = 8;
        grassPrototype8.noiseSpread = 0.1f;

        DetailPrototype grassPrototype9 = new DetailPrototype();
        grassPrototype9.prototypeTexture = grassTex9;
        grassPrototype9.renderMode = DetailRenderMode.GrassBillboard;
        grassPrototype9.minWidth = 0.49f;
        grassPrototype9.maxWidth = 0.5f;
        grassPrototype9.minHeight = 0.3f;
        grassPrototype9.maxHeight = 0.7f;
        grassPrototype9.noiseSeed = 9;
        grassPrototype9.noiseSpread = 0.1f;

        DetailPrototype grassPrototype10 = new DetailPrototype();
        grassPrototype10.prototypeTexture = grassTex10;
        grassPrototype10.renderMode = DetailRenderMode.GrassBillboard;
        grassPrototype10.minWidth = 0.49f;
        grassPrototype10.maxWidth = 0.5f;
        grassPrototype10.minHeight = 0.3f;
        grassPrototype10.maxHeight = 0.7f;
        grassPrototype10.noiseSeed = 10;
        grassPrototype10.noiseSpread = 0.1f;

        DetailPrototype grassPrototype11 = new DetailPrototype();
        grassPrototype11.prototypeTexture = grassTex11;
        grassPrototype11.renderMode = DetailRenderMode.GrassBillboard;
        grassPrototype11.minWidth = 0.49f;
        grassPrototype11.maxWidth = 0.5f;
        grassPrototype11.minHeight = 0.3f;
        grassPrototype11.maxHeight = 0.7f;
        grassPrototype11.noiseSeed = 11;
        grassPrototype11.noiseSpread = 0.1f;

        DetailPrototype[] detailPrototypeCollection = new DetailPrototype[12]{grassPrototype0,grassPrototype1,grassPrototype2,grassPrototype3,grassPrototype4,grassPrototype5,grassPrototype6,grassPrototype7,grassPrototype8,grassPrototype9,grassPrototype10,grassPrototype11};
        terrain.terrainData.detailPrototypes = detailPrototypeCollection;

        int grassDensity = 1500;
        int patchDetail = 8;

        terrain.terrainData.SetDetailResolution(grassDensity, patchDetail);

        // Get all of layer zero.
        int[,] map0 = terrain.terrainData.GetDetailLayer(0, 0, terrain.terrainData.detailWidth, terrain.terrainData.detailHeight, 0);
        int[,] map1 = terrain.terrainData.GetDetailLayer(0, 0, terrain.terrainData.detailWidth, terrain.terrainData.detailHeight, 1);
        int[,] map2 = terrain.terrainData.GetDetailLayer(0, 0, terrain.terrainData.detailWidth, terrain.terrainData.detailHeight, 2);
        int[,] map3 = terrain.terrainData.GetDetailLayer(0, 0, terrain.terrainData.detailWidth, terrain.terrainData.detailHeight, 3);
        int[,] map4 = terrain.terrainData.GetDetailLayer(0, 0, terrain.terrainData.detailWidth, terrain.terrainData.detailHeight, 4);
        int[,] map5 = terrain.terrainData.GetDetailLayer(0, 0, terrain.terrainData.detailWidth, terrain.terrainData.detailHeight, 5);
        int[,] map6 = terrain.terrainData.GetDetailLayer(0, 0, terrain.terrainData.detailWidth, terrain.terrainData.detailHeight, 6);
        int[,] map7 = terrain.terrainData.GetDetailLayer(0, 0, terrain.terrainData.detailWidth, terrain.terrainData.detailHeight, 7);
        int[,] map8 = terrain.terrainData.GetDetailLayer(0, 0, terrain.terrainData.detailWidth, terrain.terrainData.detailHeight, 8);
        int[,] map9 = terrain.terrainData.GetDetailLayer(0, 0, terrain.terrainData.detailWidth, terrain.terrainData.detailHeight, 9);
        int[,] map10 = terrain.terrainData.GetDetailLayer(0, 0, terrain.terrainData.detailWidth, terrain.terrainData.detailHeight, 10);
        int[,] map11 = terrain.terrainData.GetDetailLayer(0, 0, terrain.terrainData.detailWidth, terrain.terrainData.detailHeight, 11);

        UnityEngine.Random.InitState(seed);

        for (int x = 0; x < terrain.terrainData.detailWidth; x++) {
            //heightmapHeight not heightmapWidth
            for (int z = 0; z < terrain.terrainData.detailHeight; z++) {
                map0[x,z] = 1;

                int r1 = UnityEngine.Random.Range(0, 1000);
                if (r1 <= prob) {
                    map1[x,z] = 1;
                }
                int r2 = UnityEngine.Random.Range(0, 1000);
                if (r2 <= prob) {
                    map2[x,z] = 1;
                }
                int r3 = UnityEngine.Random.Range(0, 1000);
                if (r3 <= prob) {
                    map3[x,z] = 1;
                }
                int r4 = UnityEngine.Random.Range(0, 1000);
                if (r4 <= prob) {
                    map4[x,z] = 1;
                }
                int r5 = UnityEngine.Random.Range(0, 1000);
                if (r5 <= prob) {
                    map5[x,z] = 1;
                }
                int r6 = UnityEngine.Random.Range(0, 1000);
                if (r6 <= prob) {
                    map6[x,z] = 1;
                }
                int r7 = UnityEngine.Random.Range(0, 1000);
                if (r7 <= prob) {
                    map7[x,z] = 1;
                }
                int r8 = UnityEngine.Random.Range(0, 1000);
                if (r8 <= prob) {
                    map8[x,z] = 1;
                }
                int r9 = UnityEngine.Random.Range(0, 1000);
                if (r9 <= prob) {
                    map9[x,z] = 1;
                }
                int r10 = UnityEngine.Random.Range(0, 1000);
                if (r10 <= prob) {
                    map10[x,z] = 1;
                }
                int r11 = UnityEngine.Random.Range(0, 1000);
                if (r11 <= prob) {
                    map11[x,z] = 1;
                }
            }
        }

        terrain.terrainData.SetDetailLayer(0,0,0,map0);
        terrain.terrainData.SetDetailLayer(0,0,1,map1);
        terrain.terrainData.SetDetailLayer(0,0,2,map2);
        terrain.terrainData.SetDetailLayer(0,0,3,map3);
        terrain.terrainData.SetDetailLayer(0,0,4,map4);
        terrain.terrainData.SetDetailLayer(0,0,5,map5);
        terrain.terrainData.SetDetailLayer(0,0,6,map6);
        terrain.terrainData.SetDetailLayer(0,0,7,map7);
        terrain.terrainData.SetDetailLayer(0,0,8,map8);
        terrain.terrainData.SetDetailLayer(0,0,9,map9);
        terrain.terrainData.SetDetailLayer(0,0,10,map10);
        terrain.terrainData.SetDetailLayer(0,0,11,map11);
    }

    public static void RmGrass(TerrainData terrainData) {
        int[,] map = new int[terrainData.detailWidth,terrainData.detailHeight];
        terrainData.SetDetailLayer(0,0,0,map);
        terrainData.SetDetailLayer(0,0,1,map);
        terrainData.SetDetailLayer(0,0,2,map);
        terrainData.SetDetailLayer(0,0,3,map);
        terrainData.SetDetailLayer(0,0,4,map);
        terrainData.SetDetailLayer(0,0,5,map);
        terrainData.SetDetailLayer(0,0,6,map);
        terrainData.SetDetailLayer(0,0,7,map);
        terrainData.SetDetailLayer(0,0,8,map);
        terrainData.SetDetailLayer(0,0,9,map);
        terrainData.SetDetailLayer(0,0,10,map);
        terrainData.SetDetailLayer(0,0,11,map);
        terrainData.detailPrototypes = null;
    }

    public static void AddBushes(Terrain terrain, int seed, int prob, GameObject treePrefab0, GameObject treePrefab1, GameObject treePrefab2, GameObject treePrefab3) {
        TreePrototype treePrototype0 = new TreePrototype();
        treePrototype0.prefab = treePrefab0;
        TreePrototype treePrototype1 = new TreePrototype();
        treePrototype1.prefab = treePrefab1;
        TreePrototype treePrototype2 = new TreePrototype();
        treePrototype2.prefab = treePrefab2;
        TreePrototype treePrototype3 = new TreePrototype();
        treePrototype3.prefab = treePrefab3;

        TreePrototype[] prototypes = terrain.terrainData.treePrototypes;

        int newProtoIndex = prototypes.Length;
        TreePrototype[] treePrototypeCollection = new TreePrototype[newProtoIndex+4];
        System.Array.Copy(prototypes, 0, treePrototypeCollection, 0, newProtoIndex);
        treePrototypeCollection[newProtoIndex] = treePrototype0;
        treePrototypeCollection[newProtoIndex+1] = treePrototype1;
        treePrototypeCollection[newProtoIndex+2] = treePrototype2;
        treePrototypeCollection[newProtoIndex+3] = treePrototype3;
         
        //TreePrototype[] treePrototypeCollection = new TreePrototype[4]{treePrototype0,treePrototype1,treePrototype2,treePrototype3};
        terrain.terrainData.treePrototypes = treePrototypeCollection;
        
        TreeInstance[] treeInstances = terrain.terrainData.treeInstances;

        int newTreeIndex = treeInstances.Length;
        TreeInstance[] treeInstanceCollection = new TreeInstance[newTreeIndex+nrTreesGenerated];
        System.Array.Copy(treeInstances, 0, treeInstanceCollection, 0, newTreeIndex);

        UnityEngine.Random.InitState(seed);
        

        for (float x = 0; x < terrain.terrainData.heightmapResolution; x++) {
            //heightmapHeight not heightmapWidth
            for (float z = 0; z < terrain.terrainData.heightmapResolution; z++) {
                if (newTreeIndex < treeInstanceCollection.Length) {
                    int r = UnityEngine.Random.Range(0, 1000);
                    if (r <= prob) {
                        //int posX = Array.IndexOf(position_lst_x,x);
                        //int posZ = Array.IndexOf(position_lst_z,z);
                        //if (posX <= -1 || posZ <= -1) {
                            TreeInstance treeTemp = new TreeInstance();
                            treeTemp.position = new Vector3(x/terrain.terrainData.heightmapResolution, 0, z/terrain.terrainData.heightmapResolution);
                            int selected = UnityEngine.Random.Range(newProtoIndex,newProtoIndex+3);
                            treeTemp.prototypeIndex = selected;
                            float scaling = UnityEngine.Random.Range(0.8f,1f);
                            treeTemp.widthScale = scaling;
                            treeTemp.heightScale = scaling;
                            float angle = UnityEngine.Random.Range(0f,2*Mathf.PI);
                            treeTemp.rotation = angle;
                            treeTemp.color = Color.white;
                            treeTemp.lightmapColor = Color.white;

                            treeInstanceCollection[newTreeIndex] = treeTemp;
                            newTreeIndex++;
                        //}
                    }
                }
            }
        }

    terrain.terrainData.SetTreeInstances(treeInstanceCollection, true);
    }

    public static void AddFerns(Terrain terrain, int seed, int prob, GameObject treePrefab0, GameObject treePrefab1, GameObject treePrefab2) {
        TreePrototype treePrototype0 = new TreePrototype();
        treePrototype0.prefab = treePrefab0;
        TreePrototype treePrototype1 = new TreePrototype();
        treePrototype1.prefab = treePrefab1;
        TreePrototype treePrototype2 = new TreePrototype();
        treePrototype2.prefab = treePrefab2;

        TreePrototype[] prototypes = terrain.terrainData.treePrototypes;

        int newProtoIndex = prototypes.Length;
        TreePrototype[] treePrototypeCollection = new TreePrototype[newProtoIndex+3];
        System.Array.Copy(prototypes, 0, treePrototypeCollection, 0, newProtoIndex);
        treePrototypeCollection[newProtoIndex] = treePrototype0;
        treePrototypeCollection[newProtoIndex+1] = treePrototype1;
        treePrototypeCollection[newProtoIndex+2] = treePrototype2;
         
        //TreePrototype[] treePrototypeCollection = new TreePrototype[4]{treePrototype0,treePrototype1,treePrototype2,treePrototype3};
        terrain.terrainData.treePrototypes = treePrototypeCollection;
        
        TreeInstance[] treeInstances = terrain.terrainData.treeInstances;

        int newTreeIndex = treeInstances.Length;
        TreeInstance[] treeInstanceCollection = new TreeInstance[newTreeIndex+nrTreesGenerated];
        System.Array.Copy(treeInstances, 0, treeInstanceCollection, 0, newTreeIndex);

        UnityEngine.Random.InitState(seed);
        

        for (float x = 0; x < terrain.terrainData.heightmapResolution; x++) {
            //heightmapHeight not heightmapWidth
            for (float z = 0; z < terrain.terrainData.heightmapResolution; z++) {
                if (newTreeIndex < treeInstanceCollection.Length) {
                    int r = UnityEngine.Random.Range(0, 1000);
                    if (r <= prob) {
                        //int posX = Array.IndexOf(position_lst_x,x);
                        //int posZ = Array.IndexOf(position_lst_z,z);
                        //if (posX <= -1 || posZ <= -1) {
                            TreeInstance treeTemp = new TreeInstance();
                            treeTemp.position = new Vector3(x/terrain.terrainData.heightmapResolution, 0, z/terrain.terrainData.heightmapResolution);
                            int selected = UnityEngine.Random.Range(newProtoIndex,newProtoIndex+2);
                            treeTemp.prototypeIndex = selected;
                            float scaling = UnityEngine.Random.Range(0.8f,1f);
                            treeTemp.widthScale = scaling;
                            treeTemp.heightScale = scaling;
                            float angle = UnityEngine.Random.Range(0f,2*Mathf.PI);
                            treeTemp.rotation = angle;
                            treeTemp.color = Color.white;
                            treeTemp.lightmapColor = Color.white;

                            treeInstanceCollection[newTreeIndex] = treeTemp;
                            newTreeIndex++;
                        //}
                    }
                }
            }
        }

    terrain.terrainData.SetTreeInstances(treeInstanceCollection, true);
    }

    public static void AddPlants(Terrain terrain, int seed, int prob, GameObject treePrefab0, GameObject treePrefab1, GameObject treePrefab2, GameObject treePrefab3) {
        TreePrototype treePrototype0 = new TreePrototype();
        treePrototype0.prefab = treePrefab0;
        TreePrototype treePrototype1 = new TreePrototype();
        treePrototype1.prefab = treePrefab1;
        TreePrototype treePrototype2 = new TreePrototype();
        treePrototype2.prefab = treePrefab2;
        TreePrototype treePrototype3 = new TreePrototype();
        treePrototype3.prefab = treePrefab3;

        TreePrototype[] prototypes = terrain.terrainData.treePrototypes;

        int newProtoIndex = prototypes.Length;
        TreePrototype[] treePrototypeCollection = new TreePrototype[newProtoIndex+4];
        System.Array.Copy(prototypes, 0, treePrototypeCollection, 0, newProtoIndex);
        treePrototypeCollection[newProtoIndex] = treePrototype0;
        treePrototypeCollection[newProtoIndex+1] = treePrototype1;
        treePrototypeCollection[newProtoIndex+2] = treePrototype2;
        treePrototypeCollection[newProtoIndex+3] = treePrototype3;
         
        //TreePrototype[] treePrototypeCollection = new TreePrototype[4]{treePrototype0,treePrototype1,treePrototype2,treePrototype3};
        terrain.terrainData.treePrototypes = treePrototypeCollection;
        
        TreeInstance[] treeInstances = terrain.terrainData.treeInstances;

        int newTreeIndex = treeInstances.Length;
        TreeInstance[] treeInstanceCollection = new TreeInstance[newTreeIndex+nrTreesGenerated];
        System.Array.Copy(treeInstances, 0, treeInstanceCollection, 0, newTreeIndex);

        UnityEngine.Random.InitState(seed);
        

        for (float x = 0; x < terrain.terrainData.heightmapResolution; x++) {
            //heightmapHeight not heightmapWidth
            for (float z = 0; z < terrain.terrainData.heightmapResolution; z++) {
                if (newTreeIndex < treeInstanceCollection.Length) {
                    int r = UnityEngine.Random.Range(0, 1000);
                    if (r <= prob) {
                        //int posX = Array.IndexOf(position_lst_x,x);
                        //int posZ = Array.IndexOf(position_lst_z,z);
                        //if (posX <= -1 || posZ <= -1) {
                            TreeInstance treeTemp = new TreeInstance();
                            treeTemp.position = new Vector3(x/terrain.terrainData.heightmapResolution, 0, z/terrain.terrainData.heightmapResolution);
                            int selected = UnityEngine.Random.Range(newProtoIndex,newProtoIndex+3);
                            treeTemp.prototypeIndex = selected;
                            float scaling = UnityEngine.Random.Range(0.8f,1f);
                            treeTemp.widthScale = scaling;
                            treeTemp.heightScale = scaling;
                            float angle = UnityEngine.Random.Range(0f,2*Mathf.PI);
                            treeTemp.rotation = angle;
                            treeTemp.color = Color.white;
                            treeTemp.lightmapColor = Color.white;

                            treeInstanceCollection[newTreeIndex] = treeTemp;
                            newTreeIndex++;
                        //}
                    }
                }
            }
        }

    terrain.terrainData.SetTreeInstances(treeInstanceCollection, true);
    }
}