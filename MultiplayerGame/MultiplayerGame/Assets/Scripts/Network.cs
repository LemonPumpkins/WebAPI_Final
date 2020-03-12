using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SocketIO;
using System;

public class Network : MonoBehaviour
{
    static SocketIOComponent socket;
    public GameObject playerPrefab;
    public GameObject localPlayer;

    public GameObject[] boards;

    Dictionary<string, GameObject> players;

    // Start is called before the first frame update
    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("disconnect", OnDisconnected);
        /*socket.On("spawn", OnSpawned);
        socket.On("move", OnMoved);
        
        socket.On("requestPos", OnRequestPos);
        socket.On("updatePos", OnUpdatePos);
        players = new Dictionary<string, GameObject>();
        */
        socket.On("login", OnLogin);
        socket.On("register", OnRegister);
        socket.On("PatMAgic", Magic);
        //socket.On("leaderboard", OnLeaderboard);

        socket.On("buildLeaders", OnPopulate);
    }

    private void Magic(SocketIOEvent obj)
    {
        Debug.Log("ok2");
       // Debug.Log("AHHHHHHH");
      //  Console.WriteLine("AHHHHHHHG!");
        JSONObject players = obj.data;
        
        Console.WriteLine(players.ToString());
        //Console.WriteLine("alaca");
        
        foreach(GameObject board in boards)
        {
            
            Text[] text= board.GetComponentsInChildren<Text>();

            text[1].text = players[1].ToString();
        }
    }

    private void OnPopulate(SocketIOEvent e)
    {
        Debug.Log("AHHHHHHH");
        Console.WriteLine("AHHHHHHHG!");
        JSONObject players = e.data;
        Debug.Log(players);


  
    }

    private void OnLogin(SocketIOEvent e)
    {



    }

    public void OnRegister(SocketIOEvent e)
    {
        Debug.Log("register");

        var username = GameObject.FindGameObjectWithTag("usernameField").GetComponent<Text>().text;
        var password = GameObject.FindGameObjectWithTag("passwordField").GetComponent<Text>().text;

        var score = 0;

        /*e.data.AddField("username", username);
        e.data.AddField("password", password);
        e.data.AddField("score", score);*/

        socket.Emit("register", new JSONObject(e.data));
    }
    /*private void OnLeaderboard(SocketIOEvent e)
    {

    }*/
    private void OnUpdatePos(SocketIOEvent e)
    {
        var id = e.data["id"].ToString();
        var player = players[id];
        var pos = new Vector3(GetFloatFromJson(e.data, "x"), 0, GetFloatFromJson(e.data, "z"));
        player.transform.position = pos;

    }

    private void OnRequestPos(SocketIOEvent e)
    {
        socket.Emit("updatePos", new JSONObject(VectorToJson(localPlayer.transform.position)));
    }

    private void OnSpawned(SocketIOEvent e)
    {
        var player = Instantiate(playerPrefab);
        Debug.Log("spawned " + e.data);
        players.Add(e.data.ToString(), player);

    }

    private void OnMoved(SocketIOEvent e)
    {
        Debug.Log("Networked player is moving" + e.data);
        var id = e.data["id"].ToString();
        var player = players[id];

        var pos = new Vector3(GetFloatFromJson(e.data,"x"), 0, GetFloatFromJson(e.data,"z"));
        var navigateTo = GetComponent<NavigateTo>();
        navigateTo.NavigatePos(pos);

        Debug.Log(id);
    }

    void OnConnected(SocketIOEvent e)
    {
        Debug.Log("Connected");
        
        JSONObject data = new JSONObject();

        data.AddField("msg", "Howdy Partner");
        socket.Emit("yolo", data);
        Debug.Log("ok");
    }

    void OnDisconnected(SocketIOEvent e)
    {
        var player = players[e.data["id"].ToString()];
        Destroy(player);
        players.Remove(e.data["id"].ToString());
       // Debug.Log("ok2");

    }
    //HelperFunctions

    float GetFloatFromJson(JSONObject data, string key)
    {
        return float.Parse(data[key].ToString().Replace("\"",""));
    }
    public static string VectorToJson(Vector3 vector)
    {
        return string.Format(@"{{""x"":""{0}"":""z"":""{1}""}}", vector.x, vector.z);
    }
    public static string UserPassToJson(string user, string pass)
     {
        UserClass newuser = new UserClass();
        newuser.username = user;
        newuser.password = pass;
        Debug.Log(user + " " + pass);
        //return string.Format(@"{{""username"":{0}:""password"":{1}}}", user, pass);
        
        return JsonUtility.ToJson(newuser);
    }
    public class UserClass
    {
        public string username; 
        public string password;
    }

}
