using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMove : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OnClick(Vector3 pos)
    {
        var moveTo = player.GetComponent<NavigateTo>();
        var netMove = player.GetComponent<NetworkMove>();

        moveTo.NavigatePos(pos);
        netMove.OnMove(pos);

    }

    
}
