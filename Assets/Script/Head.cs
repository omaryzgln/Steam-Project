using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    bool go;

    GameObject player;
    GameObject head;

    Transform headToRotate;

    Vector3 locationInFrontOfPlayer;

    Rigidbody rb;
    void Start()
    {
        go = false;
        rb = gameObject.GetComponent<Rigidbody>();
        player = GameObject.Find("Puppet");
        head = GameObject.Find("Head");

        head.GetComponent<MeshRenderer>().enabled = false;

      //  headToRotate = gameObject.transform.GetChild(0);

        locationInFrontOfPlayer = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z) + player.transform.forward * 10f;

        rb.AddForce(transform.forward * 150);

        StartCoroutine(Headd());
    }

    IEnumerator Headd()
    {
        go = true;
        yield return new WaitForSeconds(2f);
        go = false;
    }

    void Update()
    {
//        headToRotate.transform.Rotate(0, Time.deltaTime * 500, 0);

        if (go)
        {
            
           
            //transform.position = Vector3.MoveTowards(transform.position, locationInFrontOfPlayer, Time.deltaTime * 2 );
        }

        if (!go)
        {
            Debug.Log("timetoback");
            rb.isKinematic = true;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z), Time.deltaTime * 3.5f);
        }

        if(!go && Vector3.Distance(player.transform.position,transform.position) < .1)
        {
            head.GetComponent<MeshRenderer>().enabled = true;
            Destroy(this.gameObject);
            player.GetComponent<AnimAndMoveController>().attack = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
      //  go = false;
    }
}
