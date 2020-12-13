using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SecAuth;

public class GameManager : MonoBehaviour
{
    public string username;

    public int maxMessage = 25;

    public GameObject chatPanel, textObject;
    public InputField chatBox;

    public Color playerMessage, info;


    [SerializeField]
    List<Message> messageList = new List<Message>();
    // Start is called before the first frame update

    void Start()
    {
        username = SceneInstanceControl.User.userName;
    }

    // Update is called once per frame
    void Update()
    {
        if (chatBox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToChat(username + ": " + chatBox.text, Message.MessageType.playerMessage);

                if (chatBox.text.ToLower() == "hi" || chatBox.text.ToLower() == "hello" || chatBox.text.ToLower() == "hello?")
                {
                    SendMessageToChat("Admin: Hello", Message.MessageType.info);
                }else if (chatBox.text.ToLower() == "how are you doing?" || chatBox.text.ToLower() == "how are you doing")
                {
                    SendMessageToChat("Admin: I am doing well, how about you?", Message.MessageType.info);
                }

                chatBox.text = "";
            }
        }
        else
        {
            if (!chatBox.isFocused && Input.GetKeyDown(KeyCode.Return))
            {
                chatBox.ActivateInputField();
            }
        }

        /*
        if (!chatBox.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendMessageToChat("SPACE Bar", Message.MessageType.info);
            }
        }
        */
    }

    public void SendMessageToChat(string text, Message.MessageType messageType)
    {
        SceneInstanceControl.User.messages.Add(text);
        if (messageList.Count >= maxMessage)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }
        Message newMessage = new Message();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();

        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(messageType);

        messageList.Add(newMessage);
    }

    Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = info;

        switch (messageType)
        {
            case Message.MessageType.playerMessage:
                color = playerMessage;
                break;
            case Message.MessageType.info:
                color = info;
                break;
            default:
                break;
        }

        return color;
    }


}


[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;

    public enum MessageType
    {
        playerMessage,
        info
    }
}
