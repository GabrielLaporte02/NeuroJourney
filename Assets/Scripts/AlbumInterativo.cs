
using UnityEngine;

public class AlbumInterativo : MonoBehaviour
{
    public GameObject painelAlbum;

    void OnMouseDown()
    {
        if (painelAlbum != null)
        {
            painelAlbum.SetActive(true);
        }
    }

    public void FecharAlbum()
    {
        painelAlbum.SetActive(false);
    }
}
