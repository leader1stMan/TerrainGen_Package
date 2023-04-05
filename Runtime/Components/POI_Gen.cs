﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WorldGen))]
public class POI_Gen : MonoBehaviour
{
    public ulong worldRadius = 5000000;
    public uint worldResolution = 100;
    public uint[] cityCounts;
    
    [SerializeField]
    TerrainNoiseModifier[] modifier_templates;
    [SerializeField]
    WorldGen generator;

    [SerializeField]
    private TerrainNoiseModifier[] StaticModifiers;
    private List<TerrainNoiseModifier> ProcMods = new List<TerrainNoiseModifier>();
    public TerrainNoiseModifier[] Mods { 
        get {List<TerrainNoiseModifier> temp = new List<TerrainNoiseModifier>(ProcMods.Count + StaticModifiers.Length);
            temp.AddRange(ProcMods);
            temp.AddRange(StaticModifiers);
            return temp.ToArray();
            }
        }
    // Start is called before the first frame update
    void OnEnable()
    {
        SetUp();
    }

    [ContextMenu( "SetUp")]
    public void SetUp(){
        if (generator == null)
            generator = GetComponent<WorldGen>();
        else
            generator.WorldGenPreInit.RemoveListener(this.Gen_POIs);        
        generator.WorldGenPreInit.AddListener(this.Gen_POIs);
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        ProcMods.Clear();
        if(generator)
            generator.WorldGenPreInit.RemoveListener(this.Gen_POIs);
    }

    void OnValidate()
    {
        if(cityCounts.Length < modifier_templates.Length){
            cityCounts = new uint[modifier_templates.Length];
        }
    }


    public void Gen_POIs()
    {
        int seed = generator.Seed;
        List<TerrainNoiseModifier> NewMods = new List<TerrainNoiseModifier>();
        TerrainNoiseModifier mod = null;
        System.Random rand = new System.Random(seed);
        for(int j = 0; j < modifier_templates.Length; j++)
        {
            TerrainNoiseModifier modifier_template = modifier_templates[j];
            if(modifier_template == null) continue;
            for(int i = 0; i < cityCounts[j]; i++)
            {
                mod  = ScriptableObject.CreateInstance<TerrainNoiseModifier>();
                float x,y;
                x = ((float)rand.NextDouble() * 2 * worldResolution) - worldResolution;
                y = ((float)rand.NextDouble() * 2 * worldResolution) - worldResolution;
                mod.Position = new Vector3(x,generator.GetHeight(x,y),y);
                mod.innerRadius = 50f;
                mod.outerRadius = 25f;

                mod.Prefab = modifier_template.Prefab;
                NewMods.Add(mod);
            }
        }
        ProcMods.AddRange(NewMods);
    }

    public GameObject[] GetPOIinRect(Rect area)
    {
        List<GameObject> poi_OnTile = new List<GameObject>();
        foreach (TerrainNoiseModifier tMod in Mods)
        {
            Vector3 poiPos = tMod.GetPosition();
            GameObject obj = null;
            if (area.Contains(new Vector2(poiPos.x, poiPos.z)))
                obj = tMod.GetGameObject();
            if (obj != null)
                poi_OnTile.Add(obj);
        
            tMod.OnSpawned?.Invoke();
		}
        return poi_OnTile.ToArray();
	}
}


