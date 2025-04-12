using UnityEngine;
using Unity.Netcode;

public class PersistentNetworkManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // No destruir el objeto al cargar una nueva escena
    }
}
