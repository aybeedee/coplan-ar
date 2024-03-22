using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class password : MonoBehaviour
{
    public static password Instance;
    private const string privateCode = "DjmLIHEcO0qkVLG0ojFrdgtf5PuRvVg0Wz3xUykDY2mQ";
    private const string publicCode = "65d5c8098f40bbbe8895099f";
    private const string webUrl = "http://dreamlo.com/lb/";
    public LeaderboardEntry[] leaderboardEntries;
    public UnityEvent OnUpdateLeaderboardEvent;
    const int score = 0;

    public Canvas LoginPage;
    public Canvas SignUp;
    public Canvas SelectionRoom;

    public TMP_InputField username;
    public TMP_InputField passw;
    public TMP_InputField nickname;

    public TMP_InputField login_username;
    public TMP_InputField login_passw;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DownEntries();
    }

    public void DownEntries()
    {
        StartCoroutine(DownEntriesC());
    }

    public IEnumerator DownEntriesC()
    {
        var uwr = new UnityWebRequest(webUrl + publicCode + "/pipe/");
        uwr.downloadHandler = new DownloadHandlerBuffer();

        yield return uwr.SendWebRequest();

        if (string.IsNullOrEmpty(uwr.error))
        {
            Debug.Log(uwr.downloadHandler.text);
            FormatEntries(uwr.downloadHandler.text);

            foreach (var entry in leaderboardEntries)
            {
                Debug.Log($"Username: {entry.uname}, Score: {entry.score}, Password: {entry.extra.password}, Nickname: {entry.extra.nickname}");
            }
            
        }
        else
        {
            Debug.Log("Error: " + uwr.error);
        }
    }

    private void FormatEntries(string textStream)
    {
        var entries = textStream.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        leaderboardEntries = new LeaderboardEntry[entries.Length];

        for (var i = 0; i < entries.Length; i++)
        {
            var entryInfo = entries[i].Split(new[] { '|' });
            var uname = entryInfo[0];
            var extra = entryInfo[3];

            extra = extra.Replace("=", ":");

            leaderboardEntries[i] = new LeaderboardEntry(uname, 0, extra);
        }

        OnUpdateLeaderboardEvent.Invoke();
    }

    public void AddOrUpdateEntry()
    {
        string name = username.text;
        string password = passw.text;
        string nickName = nickname.text;
        Extra extra = new Extra(password, nickName);

        var json = JsonConvert.SerializeObject(extra);
        json = json.Replace(":", "=");

        StartCoroutine(AddOrUpdateEntryCoroutine(name, json));
    }
    private bool UsernameExists(string username)
    {
        foreach (var entry in leaderboardEntries)
        {
            if (entry.uname.Equals(username, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator AddOrUpdateEntryCoroutine(string uname, string extra)
    {
        uname = uname.Replace("#", "=");

        if (UsernameExists(uname))
        {
            Debug.Log("Choose another username.");
            yield break; // Exit the coroutine if the username already exists
        }

        UnityWebRequest uwr = new UnityWebRequest(webUrl + privateCode + "/add/" + UnityWebRequest.EscapeURL(uname) + "/" + score + "/" + 0 + "/" + UnityWebRequest.EscapeURL(extra));
        yield return uwr.SendWebRequest();

        if (string.IsNullOrEmpty(uwr.error))
        {
            Debug.Log("Data Updated");
            DownEntries();
            SignUp.gameObject.SetActive(false);
            SelectionRoom.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Error: " + uwr.error);
        }
    }

    public void Login()
    {
        string enteredUsername = login_username.text;
        string enteredPassword = login_passw.text;

        // Check if the username exists in leaderboardEntries
        LeaderboardEntry matchingEntry = FindUserByUsername(enteredUsername);

        if (matchingEntry != null)
        {
            // Check if the entered password matches the stored password
            if (matchingEntry.extra.password.Equals(enteredPassword))
            {
                Debug.Log("Login successful");
                LoginPage.gameObject.SetActive(false);
                SelectionRoom.gameObject.SetActive(true);
                // Perform actions for a successful login
            }
            else
            {
                Debug.Log("Incorrect password");
                // Handle incorrect password scenario
            }
        }
        else
        {
            Debug.Log("Username not found");
            // Handle username not found scenario
        }
    }

    private LeaderboardEntry FindUserByUsername(string username)
    {
        foreach (var entry in leaderboardEntries)
        {
            if (entry.uname.Equals(username, StringComparison.OrdinalIgnoreCase))
            {
                return entry;
            }
        }
        return null;
    }

    [Serializable]
    public class LeaderboardEntry
    {
        public string uname;
        public int score;
        public Extra extra;

        public LeaderboardEntry(string _uname, int _score, string _extra)
        {
            uname = _uname;
            score = _score;
            extra = JsonConvert.DeserializeObject<Extra>(_extra);
        }
    }

    [Serializable]
    public class Extra
    {
        public string password;
        public string nickname;

        public Extra(string password, string nickname)
        {
            this.password = password;
            this.nickname = nickname;
        }
    }
}
