using UnityEngine;
using Unity.Netcode;

public class CameraFollow : MonoBehaviour
{
    public Transform target;    // Objeto que debe seguir la cámara.
    public Vector3 offset = new Vector3(0, 5, -10);  // Ajusta la distancia según tu necesidad.
    public float smoothSpeed = 0.125f;  // Velocidad de la interpolación.

    void Start()
    {
        // Si el target no se asignó desde el Inspector, se busca el objeto local.
        if (target == null)
        {
            // Si tus jugadores tienen un tag "PlayerLocal" o similar, utilízalo.
            GameObject localPlayer = GameObject.FindGameObjectWithTag("Player");
            if(localPlayer != null)
            {
                target = localPlayer.transform;
            }
            else
            {
                Debug.LogWarning("No se encontró ningún objeto con tag 'Player'. Asegúrate de marcar al jugador local correctamente.");
            }
        }
    }

    void LateUpdate()
    {
        if (target == null) return;  // Evita errores si no se ha encontrado el target.

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        // Para que la cámara mire hacia el jugador, descomenta lo siguiente:
        // transform.LookAt(target);
    }
}
