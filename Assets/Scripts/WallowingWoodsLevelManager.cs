using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WallowingWoodsLevelManager : MonoBehaviour {

    // ----------------------------------------------------------------- PROPERTIES ----------------------------------------------------------------------------------

    public GameObject Player;
    public GameObject ChatBox;
    public Text ChatBoxText;
    public Text ChatBoxTextName;
    public GameObject deadOverlay;

    public Button SelectionOne;
    public Button SelectionTwo;
    public Button SelectionThree;
    public Button SelectionFour;
    public Text textSelectionOne;
    public Text textSelectionTwo;
    public Text textSelectionThree;
    public Text textSelectionFour;

    public GameObject Witch;

    PlayerController playerControls;
    PlayerGround playerMovement;
    PlayerHealth playerHealth;
    GameObject playerUI;

    List<string> introText;
    List<string> witchIntroText;
    int TextPosition = 0;
    int chatLimit = 4;
    int storySet = 1;

    public GameObject[] enemies;
    GameObject firstEnemy;

    bool chatActive = false;
    bool bossFight = false;

    int AffectionPoints = 0;

    void Awake()
    {
        introText = new List<string>();
        witchIntroText = new List<string>();
        introText.Add("We shouldn't be here you know.");
        introText.Add("Bad things happen to witches in the Wrong Places. Any City-born knows that.");
        introText.Add("...and you're still going aren't you. Fine, don't listen to me. I'm only the voice in your head.");
        introText.Add("WASD or Arrow to Walk. Left Ctrl to Crouch.");
        introText.Add("(A shadow appears).");
        introText.Add("A shadow. I've heard about them, but i've never though...well. Whatever you do, don't let it touch you.");
        introText.Add("What are you waiting for? You're not carrying that Spellblaster around for no reason.");
        introText.Add("Left Mouse to Fire.");
        introText.Add("F to Punch.");
        introText.Add("Nice shot.");
        introText.Add("You've heard the stories, right? How shadows like that used to be...like us?");
        introText.Add("Witches that get lost in the Wrong Places eventually lose themselves, too. Or Never leave.");
        introText.Add("Is that why you're here?");
        introText.Add("Whatever you're looking for, I hope you find it.");

        introText.Add("Would you look at that. A City-born witch hunter? In my woods?");
        introText.Add("Well, not my woods, per se. I don’t physically own the deed to them or anything and I suppose they’re technically public property NEVERMIND THAT IS BESIDES THE POINT.");
        introText.Add("Leave now if you know what’s good for you, City slicker. You won’t like what’s waiting for you if you don’t.");
        // Last two then boss fight
        introText.Add("I see you didn't take my rather excellent advice. Pretty bold of you. Stupid. But bold.");
        introText.Add("Any last words, before I let the forest have it's wicked way with you?");

        playerControls = Player.transform.GetChild(1).gameObject.GetComponent<PlayerController>();
        playerMovement = Player.GetComponent<PlayerGround>();
        playerUI = Player.transform.GetChild(0).gameObject;
        playerHealth = Player.transform.GetChild(1).gameObject.GetComponent<PlayerHealth>();

        
    }

    void Start()
    {
        TextPosition = 0;
        chatLimit = 4;
        storySet = 1;
        StartCoroutine(BeginGame());
        chatActive = true;
    }

    // Update is called once per frame
    void Update () {
        // Exit Button
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (chatActive)
        {
            if (!bossFight)
            {
                if (Input.anyKeyDown)
                {
                    NextText();
                }
            }
        }

        if(playerHealth.isDead == true)
        {
            Player.SetActive(false);
            Time.timeScale = 0;

            deadOverlay.SetActive(true);

            if (Input.anyKeyDown)
            {
                Application.Quit();
            }
        }

        // first set
        if(TextPosition >= chatLimit)
        {
            DeActivateChat();

            // if the second set, re-enable enemy movement
            if (storySet == 2 && chatActive == false)
            {
                firstEnemy.GetComponent<Animator>().enabled = true;
                firstEnemy.GetComponent<AIController>().enabled = true;

                storySet = 3;
            }
        }
        
        if(storySet == 1)
        {
            if (Player.transform.position.x >= 2)
            {
                storySet = 2;
                ActivateChat();
                firstEnemy = Instantiate(enemies[0], new Vector3(25, -12.6f, 0), Quaternion.identity);
                firstEnemy.GetComponent<Animator>().enabled = false;
                firstEnemy.GetComponent<AIController>().enabled = false;
                chatLimit = 9;
            }
        }
        
        if(firstEnemy == null && storySet == 3)
        {
            storySet = 4;
            chatLimit = 14;
            ActivateChat();
        }

        // Witch Encounter
        if(storySet == 4 && Player.transform.position.x >= 50)
        {
            chatLimit = 17;
            storySet = 5;
            ActivateChat();
        }
        
        // sample fight encounter
        if(storySet == 5 && Player.transform.position.x >= 100)
        {
            Instantiate(enemies[1], new Vector3(130, -10.5f, 0), Quaternion.identity);
            Instantiate(enemies[0], new Vector3(130, -12.8f, 0), Quaternion.identity);
            Instantiate(enemies[1], new Vector3(130, -15.74f, 0), Quaternion.identity);
            storySet = 6;
        }

        if(storySet == 6 && Player.transform.position.x > 200)
        {
            ChatBoxTextName.text = "Witch";
            Witch.SetActive(true);
            storySet = 7;
            chatLimit = 19;
            ActivateChat();
        }

	}

    IEnumerator BeginGame()
    {
        yield return new WaitForSeconds(.1f);
        ActivateChat();
        ChatBoxText.text = introText[TextPosition];
    }

    void NextText()
    {
        if (TextPosition + 1 >= 19)
            if (storySet == 7)
                BossSelectionOne();
            else
                DeActivateChat();
        else
            TextPosition += 1;
        ChatBoxText.text = introText[TextPosition];
    }

    void ActivateChat()
    {
        chatActive = true;
        ChatBox.SetActive(true);
        playerControls.walkSpeed = 0;
        playerControls.enabled = false;
        playerMovement.enabled = false;
        playerUI.transform.GetChild(0).gameObject.SetActive(false);
    }

    void DeActivateChat()
    {
        chatActive = false;
        ChatBox.SetActive(false);
        playerControls.enabled = true;
        playerMovement.enabled = true;
        playerUI.transform.GetChild(0).gameObject.SetActive(true);
    }

    // Boss Fight Options
    void BossSelectionOne()
    {
        bossFight = true;
        ChatBox.transform.GetChild(2).gameObject.SetActive(false);
        ChatBoxText.gameObject.SetActive(false);
        ChatBoxTextName.text = "";
        SelectionOne.gameObject.SetActive(true);
        SelectionTwo.gameObject.SetActive(true);
        playerHealth.gameObject.SetActive(true);
        textSelectionOne.text = "> Before we get to the murdering me, could we...talk?";
        textSelectionTwo.text = "> You're really pretty...";

        Button btn1 = SelectionOne.GetComponent<Button>();
        btn1.onClick.AddListener(BossSelectionOneOptionOne);

        Button btn2 = SelectionTwo.GetComponent<Button>();
        btn2.onClick.AddListener(BossSelectionOneOptionOne);

    }

    void BossSelectionOneOptionOne()
    {
        textSelectionOne.gameObject.SetActive(false);
        textSelectionTwo.gameObject.SetActive(false);
        ChatBoxText.gameObject.SetActive(true);
        ChatBoxText.text = "...I must say, this is not how I was expecting this evening to go. (And that's all we had time for in the jam. Sorry!)";
        ChatBoxTextName.text = "Witch";
        StartCoroutine(EndGame());
    }


    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(10);
        Application.Quit();
    }
}
