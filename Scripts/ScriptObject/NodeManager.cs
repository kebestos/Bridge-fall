using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class NodeManager : MonoBehaviour
{
    public GameObject FirstNode;
    public GameObject FirstNode1;
    public GameObject FirstNode2;
    public GameObject FirstNode3;
    public GameObject EndPoint;

    public List<GameObject> listNode;
    public List<Link> lisLink;

    // Awake is called before Start
    void Awake()
    {        
        listNode.Add(FirstNode);
        listNode.Add(FirstNode1);
        listNode.Add(FirstNode2);
        listNode.Add(FirstNode3);
        listNode.Add(EndPoint);
    }  
    
    public bool ExistLink(GameObject n1, GameObject n2)
    {
        if (n1 == n2) return true;
        foreach(Link l in lisLink)
        {
            if(l.Node1 == n1 && l.Node2 == n2)
            {
                return true;
            }
        }
        return false;
    }

    public void DestroyLink(GameObject n1, GameObject n2)
    {
        foreach (Link l in lisLink)
        {
            if (l.Node1 == n1 && l.Node2 == n2)
            {
                lisLink.Remove(l);
                Destroy(l.gameObject);
                l.Destroy();
                break;
            }
        }
    }

    public void DestroyNode(GameObject n)
    {
        //remove links of node
        foreach (GameObject e in listNode)
        {
            if (ExistLink(e, n))
            {
                DestroyLink(e,n);
            }
        }

        //remove Hinge Joints connect to the node
        Rigidbody r = n.GetComponent<Rigidbody>();
        foreach (GameObject g in listNode)
        {
            HingeJoint[] hj = g.GetComponents<HingeJoint>();
            foreach(HingeJoint h in hj)
            {
                Rigidbody rConnectOfH = h.connectedBody;
                if (r == rConnectOfH)
                {
                    Destroy(h);
                    break;
                }
                if (rConnectOfH == null)
                {
                    Destroy(h);
                    break;
                }
            }
        }   

        //remove node
        listNode.Remove(n);
        Destroy(n);
    }

    public bool EndPointReach()
    {
        foreach(GameObject g in listNode)
        {
            if(ExistLink(EndPoint, g) && EndPoint != g)
            {
                EventManager.Instance.Raise(new EndPointHasBeenReachEvent());
                return true;
            }
        }
        return false;
    }
}
