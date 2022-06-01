using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public List<TileTransform> BodyList;
    public Vector3 TailLastPos;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform bodyTrans in transform)
        {
            BodyList.Add(bodyTrans.GetComponent<TileTransform>());
        }

        TailLastPos = BodyList[BodyList.Count - 1].Position;
    }

    // Update is called once per frame
    void Update()
    {
        GetMoveInput();
    }

    void GetMoveInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
            Move(Vector3.forward);

        if (Input.GetKeyDown(KeyCode.S))
            Move(Vector3.back);

        if (Input.GetKeyDown(KeyCode.A))
            Move(Vector3.left);

        if (Input.GetKeyDown(KeyCode.D))
            Move(Vector3.right);

        if (Input.GetKeyDown(KeyCode.Space))
            Move(Vector3.up);

        if (Input.GetKeyDown(KeyCode.LeftControl))
            Move(Vector3.down);

        if (Input.GetKeyDown(KeyCode.Q))
            AddNewBody();
    }

    void Move(Vector3 moveDir)
    {
        Vector3 _lastPos = Vector3.zero;

        for (int i = 0; i < BodyList.Count; i++)
        {
            Vector3 _oldPos = BodyList[i].Position;

            if (i == 0)
            {
                BodyList[i].Move(moveDir);
            }
            else
            {
                BodyList[i].Move(_lastPos - BodyList[i].Position);

                if (i == BodyList.Count - 1)
                    TailLastPos = _oldPos;
            }

            _lastPos = _oldPos;
        }
    }

    public void AddNewBody()
    {
        GameObject _newBody = Instantiate<GameObject>(BodyList[BodyList.Count - 1].gameObject);
        _newBody.GetComponent<TileTransform>().Position = TailLastPos;
        _newBody.transform.parent = transform;
        BodyList.Add(_newBody.GetComponent<TileTransform>());
    }
}
