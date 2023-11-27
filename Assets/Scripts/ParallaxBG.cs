using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBG : MonoBehaviour
{

    Transform cam;
    Vector3 camStartPos;
    float distance;

    GameObject[] bg;
    Material[] mat;
    float[] backSpeed;
    float farthestBack;

    [Range(0.01f, 0.05f)]
    public float parallaxSpeed;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int backCount = transform.childCount;
        mat = new Material[backCount];
        backSpeed = new float[backCount];
        bg = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            bg[i] = transform.GetChild(i).gameObject;
            mat[i] = bg[i].GetComponent<Renderer>().material;
        }

        BackSpeedCalc(backCount);
    }

    void BackSpeedCalc(int backCount)
    {
        for (int i = 0; i < backCount; i++)
        {
            if ((bg[i].transform.position.z - cam.position.z) > farthestBack)
            {
                farthestBack = bg[i].transform.position.z - cam.position.z;
            }

        }

        for (int i = 0; i < backCount; i++)
        {
            backSpeed[i] = 1 - (bg[i].transform.position.z - cam.position.z) / farthestBack;
        }


    }

    void LateUpdate()
    {
        distance = cam.position.x - camStartPos.x;
        transform.position = new Vector3(cam.position.x, transform.position.y, 30);
        for (int i = 0; i < bg.Length; i++)
        {
            float speed = backSpeed[i] * parallaxSpeed;
            mat[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);
        }
    }
}
