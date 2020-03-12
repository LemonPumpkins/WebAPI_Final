using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;
public class ButtonScript : MonoBehaviour
{
    Image[] images;
    Image popup;
    Network network;
    public SocketIOComponent socket;
    public GameObject leaderBoard;
    public Text username;
    public Text password;
    // Start is called before the first frame update
    void Start()
    {
        network = GameObject.Find("NetworkAsset").GetComponent<Network>();
        images = Resources.FindObjectsOfTypeAll<Image>();
        foreach(Image image in images)
        {
            if(image.gameObject.tag == "Popup")
            {
                popup = image;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoginPlay()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Buttons");
        foreach(GameObject button in buttons)
        {
            button.SetActive(false);
        }

        popup.gameObject.SetActive(true);         
        // EntryForm.SetActive(true);

    }
    public void LeaderBoard()
    {
        socket.Emit("leaderboard");
        
        leaderBoard.SetActive(true);
    }
    public void Quit()
    {

    }
    public void EnterData()
    {

    }
    public void Register()
    {
        Vector3 vect = new Vector3(120, 0, 60);
      //  Debug.Log(Network.VectorToJson(vect));

        Debug.Log(username.text + " " + password.text);
        var userPass = new JSONObject(Network.UserPassToJson(username.text, password.text));
        Debug.Log(userPass);
        socket.Emit("register", userPass);
    }
}
