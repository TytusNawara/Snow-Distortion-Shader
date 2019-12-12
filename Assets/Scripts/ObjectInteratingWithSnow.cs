using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteratingWithSnow : MonoBehaviour
{
    [Range(0f, 1f)]
    public float brushSize;
    [Range(0f, 1f)]
    public float brushHardness;

    private static RenderTexture splatmap;
    public Shader drawShader;
    private Material drawShaderMaterial;

    public GameObject snowPlane;
    private Material snowMaterial;

    RaycastHit groundHit;
    int layerMask;
    void Start()
    {
        layerMask = LayerMask.GetMask("ground");

        drawShaderMaterial = new Material(drawShader);

        snowMaterial = snowPlane.GetComponent<MeshRenderer>().material;
        if(splatmap == null) {
            splatmap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        }
           
        snowMaterial.SetTexture("_Splat", splatmap);
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 4f, layerMask)) {
            drawShaderMaterial.SetFloat("_BrushSize", brushSize);
            drawShaderMaterial.SetFloat("_BrushHardness", brushHardness);
            drawShaderMaterial.SetVector("_Coordinate", new Vector4(groundHit.textureCoord.x, groundHit.textureCoord.y, 0, 0));
            RenderTexture temp = RenderTexture.GetTemporary(splatmap.width, splatmap.height, 0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit(splatmap, temp);
            Graphics.Blit(temp, splatmap, drawShaderMaterial);
            RenderTexture.ReleaseTemporary(temp);
        }
    }
}
