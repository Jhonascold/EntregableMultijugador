using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using System.Numerics;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private Transform spawnedObjectPrefab; 

    private Transform spawnedObjectTransform;


    public float empujeForce = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                UnityEngine.Vector3 direccion = (collision.transform.position - transform.position).normalized;

                rb.AddForce(direccion * empujeForce, ForceMode.Impulse);
            }
        }
    }

    public override void OnNetworkSpawn()
    {

        if(IsLocalPlayer)
        {
            if(Camera.main != null)
                Camera.main.GetComponent<CameraFollow>().target = transform;
        }
        
    }

    private void Update()
    {
        
        if (!IsOwner) return; // Solo el propietario puede mover el objeto

        Transform cameraTransform = Camera.main.transform;

        UnityEngine.Quaternion cameraRotationY = UnityEngine.Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0); 

        
        UnityEngine.Vector3 forward = cameraRotationY * UnityEngine.Vector3.forward;
        UnityEngine.Vector3 right = cameraRotationY * UnityEngine.Vector3.right;

        forward.Normalize();
        right.Normalize();

        UnityEngine.Vector3 moveDir = UnityEngine.Vector3.zero;

        if (Input.GetKey(KeyCode.W)) moveDir += forward;
        if (Input.GetKey(KeyCode.S)) moveDir -= forward;
        if (Input.GetKey(KeyCode.A)) moveDir -= right;
        if (Input.GetKey(KeyCode.D)) moveDir += right;

        float moveSpeed = 8f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    
}
