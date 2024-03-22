using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Niantic.Lightship.AR;
using Unity.Collections;

public class CubeScript : NetworkBehaviour
{
    //private NetworkVariable<float> testNetworkVariable =
    //new NetworkVariable<float>
    //(
    //    default,
    //    NetworkVariableReadPermission.Everyone,
    //    NetworkVariableWritePermission.Owner
    //);
    public override void OnNetworkSpawn()
    {
        //if (IsOwner)
        //{
        //    var testNetworkVariableTemp = this.gameObject.transform.position.x;
        //    testNetworkVariable.Value = testNetworkVariableTemp;
        //}
        //else
        //{
        //    testNetworkVariable.OnValueChanged += OnTestNetworkVariabeChanged;
        //}

        Debug.Log("Cube Script: apple");
        Debug.Log("Cube Script | X: " + this.gameObject.transform.position.x + " Y: " + this.gameObject.transform.position.y + " Z: " + this.gameObject.transform.position.z);
        Debug.Log("Cube Script: On Network Spawn: Network Object ID " + this.NetworkObjectId);
        Debug.Log("Cube Script: On Network Spawn: Instance ID " + this.GetInstanceID());

        //Debug.Log("TRANSFORM -V network: " + this.gameObject.transform.position);
        //Debug.Log("TRANSFORM -V mono: " + this.gameObject.transform.TransformPoint(this.gameObject.transform.position));
        //Debug.Log("TRANSFORM -V mono: " + this.gameObject.transform.InverseTransformPoint(this.gameObject.transform.position));
        //Debug.Log("TRANSFORM -V network: " + this.gameObject.transform.forward);
        //Debug.Log("TRANSFORM -V network: " + this.gameObject.transform.right);
        //Debug.Log("TRANSFORM -V network: " + this.gameObject.transform.up);
        //Debug.Log("TRANSFORM -V network: " + this.gameObject.transform.root.transform.root.name);
        //Debug.Log("TRANSFORM -V network: " + this.gameObject.transform.localPosition);
        //Debug.Log("TRANSFORM -V network: " + this.gameObject.transform.localToWorldMatrix.ToPosition());
        //Debug.Log("TRANSFORM -V network: " + this.gameObject.transform.worldToLocalMatrix.ToPosition());

        //Debug.Log("Transform Pos network: " + this.transform.position + " | " + this.gameObject.GetComponent<NetworkObject>().transform.position);

        //Instantiate(this.gameObject, this.gameObject.transform.position, Quaternion.identity);

        //Debug.Log("Instantiating again!");

        base.OnNetworkSpawn();
    }

    private void Update()
    {
        //if (IsOwner) {
        //    testNetworkVariable.Value = this.gameObject.transform.GetChild(0).transform.position.y;
        //    //Debug.Log($"Updated TestNetworkVariable In Server: {testNetworkVariable.Value}");
        //    //Debug.Log("X Position of THIS in IsServer of Update(): " + this.gameObject.transform.position.x);
        //    //Debug.Log("Transform Pos network: " + this.gameObject.transform.GetChild(0).name + " | " + this.name + " | " + this.transform.position + " | " + this.gameObject.GetComponent<NetworkObject>().transform.position);
        //    Debug.Log("Transform Pos network: " + this.gameObject.transform.GetChild(0).name + " | " + this.gameObject.transform.GetChild(0).transform.position);
        //}
    }
    private void OnTestNetworkVariabeChanged(float previous, float current)
    {
        Debug.Log($"Detected TestNetworkVariable Change: Previous: {previous} | Current: {current}");
    }

    //public GameObject SpawnObject(Vector3 position)
    //{
    //    GameObject placedObject = Instantiate(this.gameObject, position, Quaternion.identity);
    //    placedObject.GetComponent<NetworkObject>().Spawn();
    //    return placedObject;
    //}

    public override void OnNetworkObjectParentChanged(NetworkObject parentNetworkObject)
    {
        Debug.Log("Cube Script: On Parent Changed Ran");
    }
}
