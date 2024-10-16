using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorWithKeyInteractable : MonoBehaviour ,IInteractable
{
    public string requiredKey; // The key required to open the door
    private bool isOpen = false;
    public Animator doorKeyAnimator;
    [SerializeField] private GameObject dialogueManager;
    [SerializeField] private TextMeshProUGUI interactText;
    private int waitTime = 3;
    public bool isSceneChanger = false;
    private SceneChanger sceneChanger;
    [SerializeField] private string sceneToLoad;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        doorKeyAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact(Transform interacterTransform)
    {
        //PLAY AUDIO
        Inventory inventory = interacterTransform.GetComponent<Inventory>();
        sceneChanger = interacterTransform.GetComponent<SceneChanger>();

        if (inventory != null && inventory.HasItem(requiredKey))
        {
            OpenDoor();
        }
        else
        {
            StartCoroutine(displayText());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator displayText(){
            interactText.text = "To open this door you need the " + requiredKey;
            dialogueManager.gameObject.SetActive(true);
            yield return new WaitForSeconds(waitTime);
            dialogueManager.gameObject.SetActive(false);
    }

    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }

    public string getInteractText()
    {
        return "OPEN DOOR WITH KEY";
    }

    private void OpenDoor()
    {
        if (!isOpen)
        {
            //animation, changing position
            if(sceneChanger != null && sceneToLoad != null){
                sceneChanger.LoadGameScene(sceneToLoad);
            }
            Debug.Log("The door is now open.");
            isOpen = true;
        }
    }

}
