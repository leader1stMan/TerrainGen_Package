using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = " New Constant Noise", menuName ="Terrain Noise/Constant Noise")]
public class ConstantNoiseObject : TerrainNoiseObject
{
    
    [SerializeField]
    private float height = 2;
    
    public override bool isValid {get => true;}

    public override float getHeight(Vector2 pos){

        return height;
    }

    public override void Intialize(int Seed, float Scale)
    {
        ;
    }


}

[CreateAssetMenuAttribute(fileName = " New Curve Noise", menuName ="Terrain Noise/Distance Curve Noise")]
public class DistanceCurveNoiseObject : TerrainNoiseObject
{
    
    [SerializeField]
    private float scale = 2, max = 1;
    [SerializeField]
    private AnimationCurve curve;
    public override bool isValid {get => true;}

    public override float getHeight(Vector2 pos)
    {
        float dis = Vector2.Distance(pos,Vector2.zero);
        //Treat scale as the distance that represents one curve.    
        return curve.Evaluate(dis/scale);
        
    }

    public override void Intialize(int Seed, float Scale)
    {
        
    }


}