using UnityEngine;
public class SafeArea : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Rect _safeArea;
    private Vector2 _minAnchor;
    private Vector2 _maxAnchor;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _safeArea = Screen.safeArea;
        _minAnchor = _safeArea.position;
        _maxAnchor = _minAnchor + _safeArea.size;
        _minAnchor.x /= Screen.width;
        _minAnchor.y /= Screen.height;
        _maxAnchor.x /= Screen.width;
        _maxAnchor.y /= Screen.height;
        _rectTransform.anchorMin = _minAnchor;
        _rectTransform.anchorMax = _maxAnchor;
    }
}