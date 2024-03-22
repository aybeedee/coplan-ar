using Niantic.Lightship.AR;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ChairNetworkSynchronization : NetworkBehaviour
{
    private NetworkVariable<Vector3> position = new NetworkVariable<Vector3>();
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Debug.Log("OnNetworkSpawn: I am the owner of this object");
        }
        Debug.Log("apple");
        Debug.Log("NETWORK | X: " + this.gameObject.transform.position.x + " Y: " + this.gameObject.transform.position.y + " Z: " + this.gameObject.transform.position.z);
        Debug.Log("Cube Script: On Network Spawn: Network Object ID " + this.NetworkObjectId);
        Debug.Log("Cube Script: On Network Spawn: Instance ID " + this.GetInstanceID());

        Debug.Log("TRANSFORM -V network: " + this.gameObject.transform.position);
        Debug.Log("TRANSFORM -V mono: " + this.gameObject.transform.TransformPoint(this.gameObject.transform.position));
        Debug.Log("TRANSFORM -V mono: " + this.gameObject.transform.InverseTransformPoint(this.gameObject.transform.position));
        Debug.Log("TRANSFORM -V network: " + this.gameObject.transform.forward);
        Debug.Log("TRANSFORM -V network: " + this.gameObject.transform.right);
        Debug.Log("TRANSFORM -V network: " + this.gameObject.transform.up);
        Debug.Log("TRANSFORM -V network: " + this.gameObject.transform.root.transform.root.name);
        Debug.Log("TRANSFORM -V network: " + this.gameObject.transform.localPosition);
        Debug.Log("TRANSFORM -V network: " + this.gameObject.transform.localToWorldMatrix.ToPosition());
        Debug.Log("TRANSFORM -V network: " + this.gameObject.transform.worldToLocalMatrix.ToPosition());

        Debug.Log("Transform Pos network: " + this.transform.position + " | " + this.gameObject.GetComponent<NetworkObject>().transform.position);

        //Instantiate(this.gameObject, this.gameObject.transform.position, Quaternion.identity);

        //Debug.Log("Instantiating again!");

        base.OnNetworkSpawn();
    }
}
