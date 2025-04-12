using Unity.Netcode;
using UnityEngine;

public class NetworkUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        //Botones de prueba en pantalla para iniciar como Host, Servidor o Cliente.
        if(GUILayout.Button("Iniciar Host"))
        {
            NetworkManager.Singleton.StartHost();   //Inicia como Host (servidor + cliente local)
        }

        if(GUILayout.Button("Iniciar Solo Servidor"))
        {
            NetworkManager.Singleton.StartServer(); //Inicia solo el Servidor dedicado.
        }

        if(GUILayout.Button("Iniciar Cliente"))
        {
            NetworkManager.Singleton.StartClient(); //Inicia como Cliente y busca conectarse a un servidor/host.
        }
    }
}
