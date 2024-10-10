using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class StartNode : MonoBehaviour
{
    [Header("New Node")]
    [SerializeField]
    private GameObject DynamiqueNode;
    [SerializeField]
    private GameObject FirstNode;
    [SerializeField]
    private GameObject LinkDynamique;
    [SerializeField]
    private GameObject NodeUpdate;

    //node
    private GameObject node;
    //nodeManager
    private NodeManager node_M;
    //link
    private int numberLink;
    //Drag
    private Vector3 offset;
    private Vector3 screenPoint;
    private Vector3 curPosition;
    //HinjeJoint
    private HingeJoint[] HJ;

    void OnMouseDown()
    {
        //instanciate Node and Drag
        node = (GameObject)Instantiate(DynamiqueNode);
        node.transform.position = FirstNode.transform.position;        
        offset = node.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,0));

        //instanciate NodeManager and Add Node to the listNode
        GameObject[] Node_Manager = GameObject.FindGameObjectsWithTag("NodeManager");
        node_M = Node_Manager[0].GetComponent<NodeManager>();
        node_M.listNode.Add(node);

        //Remove old Hinge Joints
        numberLink = 0;
        HingeJoint[] HJ_old = node.GetComponents<HingeJoint>();
        if(HJ_old != null){
            foreach(HingeJoint h in HJ_old)
            {
                Destroy(h);
            }
        }

        //Remove old Spring Join 
        SpringJoint s = node.GetComponent<SpringJoint>();
        if (s != null) Destroy(s);

        Rigidbody rb = node.GetComponent<Rigidbody>();
        if (rb != null) Destroy(rb);

        //Fixe Sphere Collider
        SphereCollider sp = node.GetComponent<SphereCollider>();
        sp.enabled = false;
    }    

    void OnMouseDrag()
    {
        if(node != null) {
            //Drag
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            node.transform.position = curPosition;

            //instenciate links
            foreach (GameObject e in node_M.listNode)
            {
                if (node.transform.position.x < e.transform.position.x + 2.5f && node.transform.position.x > e.transform.position.x - 2.5f &&
                   node.transform.position.y < e.transform.position.y + 2.5f && node.transform.position.y > e.transform.position.y - 2.5f)
                {
                    //Check link doesn't exist yet
                    if (!node_M.ExistLink(e, node))
                    {
                        //Instanciate Link
                        GameObject lien = (GameObject)Instantiate(LinkDynamique);
                        Link l = lien.GetComponent<Link>();
                        l.Init(e, node);
                        node_M.lisLink.Add(l);
                        numberLink += 1;
                    }
                }
                //Destroy Link
                if (node.transform.position.x > e.transform.position.x + 5.0f || node.transform.position.x < e.transform.position.x - 5.0f ||
                   node.transform.position.y > e.transform.position.y + 5.0f || node.transform.position.y < e.transform.position.y - 5.0f)
                {
                    if (node_M.ExistLink(e, node))
                    {
                        node_M.DestroyLink(e, node);
                        numberLink -= 1;
                    }
                }
            }
        }
    }
    
    private void OnMouseUp()
    {
        if (node != null)
        {
            //Destroy Node if number of link is not enought
            if (numberLink <= 1)
            {
                node_M.listNode.Remove(node);
                Destroy(node);
                foreach (GameObject e in node_M.listNode)
                {
                    if (node_M.ExistLink(e, node))
                    {
                        node_M.DestroyLink(e, node);
                    }
                }
            }
            //Attribuate Hinge Joins
            else
            {
                Vector3 PositionFixe = node.transform.position;
                //Add SrpingJoint          
                node.AddComponent(typeof(SpringJoint));
                SpringJoint sj = node.GetComponent<SpringJoint>();
                sj.autoConfigureConnectedAnchor = false;
                sj.anchor = new Vector3(0, 0, 0);
                sj.connectedAnchor = PositionFixe;
                sj.maxDistance = 1;


                //Add Hinge Joints to node
                for (int i = 0; i != numberLink; i++)
                {
                    node.AddComponent(typeof(HingeJoint));
                }

                //HingeJoint list
                int index = 0;
                HJ = node.GetComponents<HingeJoint>();

                foreach (GameObject e in node_M.listNode)
                {
                    if (node_M.ExistLink(e, node) && e != node)
                    {
                        HingeJoint h = HJ[index];
                        h.connectedBody = e.GetComponent<Rigidbody>();
                        h.autoConfigureConnectedAnchor = false;
                        h.enableCollision = false;
                        h.connectedAnchor = new Vector3(0, 0, 0);
                        h.anchor = e.transform.position - PositionFixe;
                        h.axis = new Vector3(0, 0, -1); ;

                        JointLimits limits = h.limits;
                        limits.max = 20;
                        limits.min = -20;
                        limits.bounciness = 65;
                        limits.bounceMinVelocity = 0;
                        h.limits = limits;
                        h.useLimits = true;

                        index += 1;
                    }
                }

                //Fixe z position
                Rigidbody r = node.GetComponent<Rigidbody>();
                node.transform.position = new Vector3(PositionFixe.x, PositionFixe.y, PositionFixe.z);
                r.constraints = RigidbodyConstraints.FreezePositionZ;
                r.mass = 2;

                //Fixe Sphere Collider
                SphereCollider s = node.GetComponent<SphereCollider>();
                s.enabled = true;

                // startUpdate = true;
                GameObject nodeU = (GameObject)Instantiate(NodeUpdate);
                Node n = nodeU.GetComponent<Node>();
                n.Init(node);

                //Decrement number of Node you can use
                //Check Victory or GameOver
                bool checkEndPointReach = node_M.EndPointReach();
                if (!checkEndPointReach)
                {
                    EventManager.Instance.Raise(new NodeHasBeenCreateEvent());
                }
            }
        }
    }       
}
