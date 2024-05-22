using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public GameObject player;
    public GameObject fx;
    public GameObject item;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;
    }
}
