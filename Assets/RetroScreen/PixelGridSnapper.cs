using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1000)]
public class PixelGridSnapper : MonoBehaviour
{
    //TODO that won't change without a change of ppu, bake that into the RetroScreenSetting?
    float _unitPerPixel = 0.0f;
    Vector3 _originalPosition;

    void Start()
    {
        _unitPerPixel = 1.0f / RetroScreenSettings.instance.pixelPerUnits;
    }

    void OnPreRender()
    {
        int xRatio = Mathf.RoundToInt(transform.position.x / _unitPerPixel);
        int yRatio = Mathf.RoundToInt(transform.position.y / _unitPerPixel);

        _originalPosition = transform.position;
        transform.position = new Vector3(xRatio * _unitPerPixel, yRatio * _unitPerPixel, transform.position.z);
    }

    void OnPostRender()
    {
        transform.position = _originalPosition;
    }
}
