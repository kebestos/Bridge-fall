using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    //nodeManager
    private NodeManager node_M;

    private void Start()
    {
        //instanciate NodeManager and Add Node to the listNode
        GameObject[] Node_Manager = GameObject.FindGameObjectsWithTag("NodeManager");
        node_M = Node_Manager[0].GetComponent<NodeManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {        
        if(collision.gameObject.tag == "Node")
        {
            node_M.DestroyNode(collision.gameObject);

            //if a StartNode OnMouseDrag
            foreach(GameObject g in node_M.listNode)
            {
                SphereCollider s = g.GetComponent<SphereCollider>();
                if (!s.enabled)
                {
                    node_M.listNode.Remove(g);
                    Destroy(g);
                    break;
                }
            }
        }        
    }
}
