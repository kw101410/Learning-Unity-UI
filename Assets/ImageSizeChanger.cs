using UnityEngine;

public class ImageSizeChanger : MonoBehaviour
{
    [SerializeField] private RectTransform imageRectTransform;

    public void UpdateImageScale(float value)
    {
        if (imageRectTransform != null)
        {
            imageRectTransform.localScale = new Vector3(value, value, 1f);
        }
    }
}
