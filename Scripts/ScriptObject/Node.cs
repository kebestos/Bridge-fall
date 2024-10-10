using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{    
    public GameObject node;
    //HinjeJoint
    private HingeJoint[] HJ;
    //nodeManager
    private NodeManager node_M;

    private bool startUpdate = false;

    //Initiate the Node
    public void Init(GameObject N)
    {
        node = N;
        node.tag = "Node";
        //instanciate NodeManager and Add Node to the listNode
        GameObject[] Node_Manager = GameObject.FindGameObjectsWithTag("NodeManager");
        node_M = Node_Manager[0].GetComponent<NodeManager>();
        
        HJ = node.GetComponents<HingeJoint>();

        startUpdate = true;
    }

    //Freeze Rotation on x and y
    private void FixedUpdate()
    {
        if (node != null)
        {
            Quaternion rot = Quaternion.Euler(0, 0, node.transform.rotation.eulerAngles.z);
            node.transform.rotation = rot;
        }
        else Destroy(this.gameObject);
    }
    //update Anchor of Hinge Joins
    private void Update()
    {
        if (startUpdate && node != null)
        {
            int index = 0;            
            foreach (GameObject e in node_M.listNode)
            {
                if (node_M.ExistLink(e, node) && e != node && HJ[index] != null)
                {                    
                    HingeJoint h = HJ[index];
                    h.anchor = e.transform.position - node.transform.position;
                    index += 1;
                }
            }
            //Destroy the node if one link
            if(index <= 0)
            {
                node_M.DestroyNode(node);
                Destroy(this.gameObject);
            }
        }
    } 
}
