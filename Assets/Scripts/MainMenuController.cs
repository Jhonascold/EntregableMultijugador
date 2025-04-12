using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void OnClickPlayHost()
    {
        NetworkManager.Singleton.StartHost();

        NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single); //Carga la escena de manera sincronizada para todos los clientes y el servidor
    }

    public void OnClickPlayClient()
    {
        NetworkManager.Singleton.StartClient();

    }

    public void OnClickExit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
