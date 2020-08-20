using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGradient : MonoBehaviour
{

    MeshRenderer colormesh;

    [SerializeField] [Range(0f, 1f)] float lerpTime;

    [SerializeField] Color[] myColors;

    int colorIndex = 0;
    float t = 0f;
    int length;
    // Start is called before the first frame update
    void Start()
    {
        colormesh = GetComponent<MeshRenderer>();
        length = myColors.Length;
    }

    // Update is called once per frame
    void Update()
    {
        colormesh.material.color = Color.Lerp(colormesh.material.color, myColors[colorIndex], lerpTime);

        t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);

        if(t > 0.9f)
        {
            t = 0f;
            colorIndex++;
            colorIndex = (colorIndex >= length) ? 0 : colorIndex;
        }

    }
}
