using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public float speed;
    Rigidbody2D rigid;
    float h;
    float v;
    bool isHorizonMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        if (h != 0)
            isHorizonMove = true;
        else if (v != 0)
            isHorizonMove = false;
        if (Input.GetKey(KeyCode.Alpha1))
        {
            PlayerTool.currentTool = "Hoe";
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            PlayerTool.currentTool = "Seed";
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            PlayerTool.currentTool = "Water";
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            PlayerTool.currentTool = "Hand";
        }
    }

    void FixedUpdate()
    {
        Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
        Vector2 nextPos = rigid.position + moveVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(nextPos);
        
    }
}