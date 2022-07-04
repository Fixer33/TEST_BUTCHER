using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DrawRenderer : MonoBehaviour
{
    [SerializeField] private Sprite LineImage;
    [SerializeField] private Transform Parent;

    private List<GameObject> _lines = new List<GameObject>();

    private void Start()
    {
        
    }

    public void DrawLine(Vector2 point1, Vector2 point2)
    {
        _lines.Add(MakeLine(point1.x, point1.y, point2.x, point2.y, Color.red));
    }

    public void Clear()
    {
        for (int i = 0; i < _lines.Count; i++)
        {
            Destroy(_lines[i]);
        }
        _lines.Clear();
    }

    private GameObject MakeLine(float ax, float ay, float bx, float by, Color col)
    {
        GameObject NewObj = new GameObject();
        NewObj.name = "line from " + ax + " to " + bx;
        Image NewImage = NewObj.AddComponent<Image>();
        NewImage.sprite = LineImage;
        NewImage.color = col;
        RectTransform rect = NewObj.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.zero;
        rect.SetParent(transform);
        rect.localScale = Vector3.one;

        Vector3 a = new Vector3(ax * 1, ay * 1, 0);
        Vector3 b = new Vector3(bx * 1, by * 1, 0);


        rect.localPosition = (a + b) / 2;
        Vector3 dif = a - b;
        rect.sizeDelta = new Vector3(dif.magnitude, 10);
        if (dif.y == 0)
            dif.y += 0.001f;
        if (dif.x == 0)
            dif.x += 0.001f;
        rect.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan(dif.y / dif.x));
        NewObj.transform.SetParent(Parent);
        return NewObj;
    }

}
