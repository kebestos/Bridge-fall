using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    public Transform cylinderPrefab;
    public GameObject Node1;
    public GameObject Node2;

    private GameObject lien;

    public void Init(GameObject n1, GameObject n2)
    {
        Node1 = n1;
        Node2 = n2;
    }

    private void Start()
    {
        InstantiateCylinder(cylinderPrefab, Node1.transform.position, Node2.transform.position);
    }

    private void InstantiateCylinder(Transform cylinderPrefab, Vector3 beginPoint, Vector3 endPoint)
    {
        lien = Instantiate<GameObject>(cylinderPrefab.gameObject, Vector3.zero, Quaternion.identity);
        UpdateCylinderPosition(lien, beginPoint, endPoint);
    }

    private void UpdateCylinderPosition(GameObject cylinder, Vector3 beginPoint, Vector3 endPoint)
    {
        Vector3 offset = endPoint - beginPoint;
        Vector3 position = beginPoint + (offset / 2.0f);

        cylinder.transform.position = position;
        cylinder.transform.LookAt(beginPoint);
        Vector3 localScale = cylinder.transform.localScale;
        localScale.z = (endPoint - beginPoint).magnitude;
        cylinder.transform.localScale = localScale;

        //rescale box colider
       /* BoxCollider Bx = cylinder.GetComponent<BoxCollider>();
        Bx.size = new Vector3(Bx.size.x, Bx.size.y, 0.4f);
        SphereCollider s = Node2.GetComponent<SphereCollider>();
        if (s.enabled) Bx.enabled = false;*/
    }

    private void Update()
    {
        if (Node1 == null || Node2 == null)
        {
            Destroy();
        }
        else
        {
            UpdateCylinderPosition(lien, Node1.transform.position, Node2.transform.position);
        }
    }

    public void Destroy()
    {
        Destroy(lien);
        Destroy(this);
    }
}
