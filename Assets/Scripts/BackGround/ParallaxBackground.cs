using Unity.VisualScripting;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float parallaxValue;
    private float xPos;
    private float length;
    private GameObject cam;
    private Transform child;

    private void Start()
    {
        cam = GameObject.Find("MainCamera");
        if (cam == null) Debug.LogError("找不到相机GameObject,背景移动需要相机!");

        child = transform.GetChild(0);

        var render = child.GetComponent<SpriteRenderer>();
        length = render.bounds.size.x;

        var left = child.position + new Vector3(length * -1, 0, 0);
        var right = child.position + new Vector3(length, 0, 0);
        Instantiate(child, left, child.rotation).SetParent(transform);
        Instantiate(child, right, child.rotation).SetParent(transform);

        xPos = transform.position.x;
    }

    private void LateUpdate()
    {
        var distanceToMove = cam.transform.position.x * parallaxValue;
        transform.position = new Vector3(xPos + distanceToMove, transform.position.y);

        var distanceMoved = cam.transform.position.x * (1 - parallaxValue);

        if (distanceMoved > xPos + length / 2)
            xPos += length;
        else if (distanceMoved < xPos - length / 2)
            xPos -= length;
    }
}
