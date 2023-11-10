using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;
    public static bool isFollowing = true;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }
    private void Update()
    {
        if (isFollowing)
            transform.position = new Vector3(player.transform.position.x + offset.x, transform.position.y, player.transform.position.z + offset.z);
    }
}
