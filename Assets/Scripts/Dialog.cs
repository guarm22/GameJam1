using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    
    public List<string> sentences = new List<string>();
    public int currentSentence = 0;
    public GameObject dialogPrefab;
    private GameObject openDialog = null;

    public bool autoTrigger = false;
    public bool oneTime = false;

    void Start() {
        if(sentences.Count == 0) {
            sentences.Add("I have nothing to say.");
        }

        if(dialogPrefab == null) {
            dialogPrefab = Resources.Load<GameObject>("GameObjects/DialogBox");
        }
    }

    // Update is called once per frame
    void Update() {

        if(isPlayerNear() && (Input.GetKeyDown(KeyCode.E) || autoTrigger) ) {
            StartCoroutine(ShowDialog());
        }
        else if(openDialog != null && !isPlayerNear()) {
            StartCoroutine(DestroyDialog());
        }
        
    }

    private IEnumerator ShowDialog() {
        if(openDialog == null) {
            GameObject dialog = Instantiate(dialogPrefab);
            dialog.transform.position = new Vector3(transform.position.x+1, transform.position.y + 1.5f, transform.position.z);
            openDialog = dialog;
        }
        TMP_Text text = openDialog.GetComponentInChildren<TMP_Text>();

        text.text = sentences[currentSentence];
        currentSentence++;

        if(currentSentence >= sentences.Count) {
            currentSentence = 0;
        }
        yield return null;
    }

    private IEnumerator DestroyDialog() {
        yield return new WaitForSeconds(.5f);
        Destroy(openDialog);
        openDialog = null;
        currentSentence = 0;

        if(oneTime) {
            Destroy(gameObject);
        }
    }

    private bool isPlayerNear() {
        GameObject player = GameObject.Find("Player");
        return Vector3.Distance(player.transform.position, transform.position) < 1.5f;
    }
}
