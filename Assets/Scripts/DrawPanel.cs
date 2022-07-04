using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawPanel : MonoBehaviour
{
    [SerializeField] private float PointFrequency = 1f;
    [SerializeField] private GameObject PaintingPanel;
    [SerializeField] private DrawRenderer PanelDrawer;
    [SerializeField] private Canvas Canvas;

    private bool _isDrawing = false;
    private Vector2 _lastPosition = Vector2.zero;
    private Vector2 offset = Vector2.zero;
    private Vector2 size = Vector2.zero;
    private bool isTouching = false;
    private List<Vector2> _touchPoints;
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = PaintingPanel.GetComponent<RectTransform>();
        
        offset = new Vector2(_rectTransform.position.x, _rectTransform.position.y);
        size = _rectTransform.rect.size;
    }

    private void Update()
    {
        Vector2 pos = ScreenToLocalSpace(Input.mousePosition);
        bool inputDetected = Input.GetMouseButton(0) && InBounds(pos);
        if (inputDetected && isTouching == false)
        {
            isTouching = true;
            OnTouchStart(pos);
        }
        else if (inputDetected && isTouching)
        {
            OnTouchContinue(pos);
        }
        else if (inputDetected == false && isTouching)
        {
            isTouching = false;
            OnTouchStop(pos);
        }
    }

    private bool InBounds(Vector2 position)
    {
        Vector2 t = RectTransformUtility.PixelAdjustPoint(position, PaintingPanel.transform, Canvas);
        return Mathf.Abs(t.x) < size.x / 2 && Mathf.Abs(t.y) < size.y / 2;
    }

    private void OnTouchStart(Vector2 touchPos)
    {
        _isDrawing = true;

        _touchPoints = new List<Vector2>();
        _touchPoints.Add(touchPos);
        _lastPosition = touchPos;
    }

    private void OnTouchContinue(Vector2 touchPos)
    {
        PanelDrawer.DrawLine(_lastPosition, touchPos);
        _lastPosition = touchPos;
        _touchPoints.Add(touchPos);
    }

    private void OnTouchStop(Vector2 touchPos)
    {
        _isDrawing = false;
        PanelDrawer.Clear();
        if (PlayerSpace.instance.IsMoving)
        {
            PlayerSpace.instance.Restructurize(_touchPoints);
        }
        else
        {
            PlayerSpace.instance.StartLevel(_touchPoints);
        }
    }

    private Vector2 ScreenToLocalSpace(Vector2 screen)
    {
        //Vector2 res = new Vector2();
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, screen, Camera.main, out res);
        //return res;

        float offsetX = screen.x - offset.x;
        float offsetY = screen.y - offset.y;

        return new Vector2(offsetX, offsetY);
    }

}
