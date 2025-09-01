using UnityEngine;
using UnityEngine.UI;

public class SelectedBread : MonoBehaviour
{
    public Image breadIconImage;   // World Space Canvas 안 Image
    public Transform Selectedbread;   // 플레이어 머리 위치

    void Update()
    {
        if (breadIconImage.gameObject.activeSelf && Selectedbread != null)
        {
            // 플레이어 머리 위에 따라다니도록 위치 갱신
            breadIconImage.transform.position = Selectedbread.position + Vector3.up * 2.0f;
        }
    }

    public void ShowBreadIcon(Sprite icon)
    {
        if (icon != null)
        {
            breadIconImage.sprite = icon;
            breadIconImage.gameObject.SetActive(true);
        }
    }
}
