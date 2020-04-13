using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircles : MonoBehaviour
{
    private const float spreadingSpeed = 20f;
    private ObjectAdmin objectAdmin;
    public Shader EffectShader;
    public Material ppMaterial;
    private List<Material> regionMat;
    private List<float> timesSince = new List<float>();
    private string[] myHexColors = {
        "#E57373", //Red
        "#4DD0E1", //Cyan
        "#F06292", //Pink
        "#81C784", //Green
        "#BA68C8", //Purple
        "#AED581", //Light Green
        "#7986CB", //Indigo
        "#4DB6AC", //Teal
        "#FF8A65", //Deep Orange
        "#9575CD", //Deep Purple
        "#A1887F", //Brown
        "#4FC3F7", //Light Blue
        "#64B5F6", //Blue
        // "#DCE775",//lime
    };
    void Awake()
    {
        foreach (var hex in myHexColors)
        {
            Color color;
            ColorUtility.TryParseHtmlString(hex, out color);
            colorArray.Add(color);
        }
        ppMaterial = new Material(EffectShader);

        List<Vector4> fillArray = new List<Vector4>(200);
        for (int i = 0; i < 200; i++)
        {
            fillArray.Add(Vector4.zero);
        }
        List<float> fillArray2 = new List<float>(200);
        for (int i = 0; i < 200; i++)
        {
            fillArray2.Add(0f);
        }
        ppMaterial.SetVectorArray("_positionVec", fillArray);
        ppMaterial.SetFloatArray("_minRad", fillArray2);
        ppMaterial.SetVectorArray("_advantage", fillArray);
        ppMaterial.SetFloatArray("_factor", fillArray2);
        ppMaterial.SetVectorArray("_colors", colorArray);
    }
    void Start()
    {
        Camera main = Camera.main;
        objectAdmin = main.GetComponent<ObjectAdmin>();
    }
    private List<Vector4> colorArray = new List<Vector4>();

    void Update()
    {
        Camera main = Camera.main;
        var positionVec = new List<Vector4>();
        timesSince.Clear();
        foreach (var item in objectAdmin.objects)
        {
            positionVec.Add(main.WorldToViewportPoint(item.transform.position));
            if (objectAdmin.isPressed[item] >= 0f)
            {
                objectAdmin.isPressed[item] += Time.deltaTime * spreadingSpeed;
                timesSince.Add(objectAdmin.isPressed[item]);
            }
            else
            {
                timesSince.Add(0f);
            }
            // positionVec.Add(item.transform.position);
        }
        List<Vector4> advantages = new List<Vector4>();
        for (int i = 0; i < objectAdmin.objects.Count; i++)
        {
            GameObject item = objectAdmin.objects[i];
            var v = Time.time;
            if (objectAdmin.advantages.ContainsKey(item))
            {
                v = objectAdmin.advantages[item];
            }
            advantages.Add(new Vector2((Time.time - v) * spreadingSpeed, 1f));
        }

        List<float> factors = new List<float>();
        for (int i = 0; i < objectAdmin.objects.Count; i++)
        {
            factors.Add(i * 2f + 1f);
        }
        ppMaterial.SetVectorArray("_advantage", advantages);
        ppMaterial.SetFloatArray("_factor", factors);
        ppMaterial.SetVectorArray("_positionVec", positionVec);
        ppMaterial.SetFloatArray("_minRad", timesSince);
        ppMaterial.SetFloat("_positionVecCount", positionVec.Count);
    }
    // bool firstPass = true;
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, ppMaterial);
    }
}
