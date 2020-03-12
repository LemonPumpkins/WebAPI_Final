using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
public class NetworkMove : MonoBehaviour
{

    public SocketIOComponent socket;
    // Start is called before the first frame update
    public void OnMove(Vector3 pos)
    {
        Debug.Log("sending pos to server: " + pos);


        socket.Emit("move", new JSONObject(Network.VectorToJson(pos)));
    }

   
}
