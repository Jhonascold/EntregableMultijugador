using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using System.Numerics;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private Transform spawnedObjectPrefab; //Transform y GameObject son lo mismo, pero Transform es más ligero y no tiene componentes como Rigidbody o Collider.

    private Transform spawnedObjectTransform;

    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData{_int = 56, _bool = true}
        , NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public struct MyCustomData : INetworkSerializable{
        public int _int;
        public bool _bool;
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }

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
        /*randomNumber.OnValueChanged += (MyCustomData previousValue , MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + ";  " + newValue._int + "; " + newValue._bool + "; " + newValue.message);
        };*/

        if(IsLocalPlayer)
        {
            if(Camera.main != null)
                Camera.main.GetComponent<CameraFollow>().target = transform;
        }
        
    }

    private void Update()
    {
        
        if (!IsOwner) return; // Solo el propietario puede mover el objeto

        if(Input.GetKeyDown(KeyCode.T))
        {
            spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.gameObject.GetComponent<NetworkObject>().Spawn(true); //true = Spawn as PlayerObject
            //TestServerRpc("Hola mundo");
            //randomNumber.Value = new MyCustomData{_int = 10, _bool = false, message = "Hello"};
        }

        if(Input.GetKeyDown(KeyCode.Y))
        {
            spawnedObjectTransform.gameObject.GetComponent<NetworkObject>().Despawn(true); //true = Despawn as PlayerObject
            //Destroy(spawnedObjectTransform.gameObject);
        }

        Transform cameraTransform = Camera.main.transform;

        UnityEngine.Quaternion cameraRotationY = UnityEngine.Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0); 

        // Calcula el vector hacia adelante en el plano horizontal usando la rotación de la cámara
        UnityEngine.Vector3 forward = cameraRotationY * UnityEngine.Vector3.forward;
        UnityEngine.Vector3 right = cameraRotationY * UnityEngine.Vector3.right;

        // Asegúrate de que el movimiento esté en el plano horizontal
        //forward.y = 0f;
        //right.y = 0f;
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

    /*[ServerRpc]
    private void TestServerRpc(string message)
    {
        Debug.Log("TestServerRpc " + OwnerClientId + "; " + message);
    }

    [ClientRpc]
    private void TestClientRpc(string message)  //Para enviar mensajes desde el servidor a todos los clientes
    {
        Debug.Log("TestClientRpc " + OwnerClientId + "; " + message);
    }*/
}
