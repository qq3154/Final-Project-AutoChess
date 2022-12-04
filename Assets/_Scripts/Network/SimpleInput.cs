using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SimpleInput : MonoBehaviour
{
    [SerializeField] private PhotonView view;
    public float speed = 1;

    public int id = 0;
    // Start is called before the first frame update
    void Start()
    {
        id = PhotonNetwork.LocalPlayer.ActorNumber;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (view.IsMine)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector2.up * speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector2.up * speed * Time.deltaTime * -1);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
            }
        
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime * -1);
            }
        }
       
    }
    
}