using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RhythmMinigameManager : MonoBehaviour
{
    struct RhythmNoteHitData
    {
        public GameObject hitNote;
        public float hitDistance;

        public RhythmNoteHitData(GameObject note, float distance)
        {
            hitNote = note;
            hitDistance = distance;
        }
        //I'm not sure why no paramter constructor doesn't work here, so this is a workaround
        public RhythmNoteHitData(int notParam)
        {
            hitNote = null;
            hitDistance = 0;
        }
    }

    //How the current notes should be moving
    public GameManager.DirEnum currentSpawnDirection = GameManager.DirEnum.DirRight;

    //The input keys for the rhythm minigame
    //Set this in the editor to change the input keys
    [SerializeField]
    private KeyCode[] inputKeys = new KeyCode[4];


    //Set the prefab for the rhythm notes in the editor
    [SerializeField]
    private GameObject RhythmNotePrefab;
    //Store a list of inactive rhythm notes to reuse
    private List<RhythmNote> rhythmNotePool;
    private int rhythmNotePoolSize = 50;
    //A game object to store the rhythm notes in the hierarchy for organization
    [SerializeField]
    private GameObject rhythmNoteParent;


    //The left spawn points for the rhythm notes
    [SerializeField]
    private Transform[] leftSpawnPoints = new Transform[4];
    //The right spawn points for the rhythm notes
    [SerializeField]
    private Transform[] rightSpawnPoints = new Transform[4];

    //Hit variables
    float maxHitRange = 1.5f;
    float goodHitRange = 1.0f;
    float perfectHitRange = 0.5f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Create a list to store inactive rhythm notes for object pooling
        rhythmNotePool = new List<RhythmNote>();
        for (int i = 0; i < rhythmNotePoolSize; i++) {
            //Create a new rhythm note and add it to the list as an inactive object
            GameObject newNote = Instantiate(RhythmNotePrefab, rhythmNoteParent.transform);
            newNote.gameObject.SetActive(false);
            rhythmNotePool.Add(newNote.GetComponent<RhythmNote>());
        }

        //test spawn code
        StartCoroutine(TestSpawn());
    }

    // Update is called once per frame
    void Update()
    {
        RhythmNoteHitData hitNote = new RhythmNoteHitData(0);
        //Gets the inputs for the rhythm minigame
        if (Input.GetKeyDown(inputKeys[(int)GameManager.DirEnum.DirUp])) {
            hitNote = HitNote(GameManager.DirEnum.DirUp);
        }
        if (Input.GetKeyDown(inputKeys[(int)GameManager.DirEnum.DirRight]))
        {
            hitNote = HitNote(GameManager.DirEnum.DirRight);
        }
        if (Input.GetKeyDown(inputKeys[(int)GameManager.DirEnum.DirLeft]))
        {
            hitNote = HitNote(GameManager.DirEnum.DirLeft);
        }
        if (Input.GetKeyDown(inputKeys[(int)GameManager.DirEnum.DirDown]))
        {
            hitNote = HitNote(GameManager.DirEnum.DirDown);
        }

        //If a note was hit, do something with it
        if (hitNote.hitNote != null)
        {
            //TODO do something with the hit note, like increase score or give feedback
            Debug.Log("Hit note at distance: " + hitNote.hitDistance);

            hitNote.hitNote.SetActive(false);
        }
    }

    private void SpawnRhythmNote(GameManager.DirEnum noteDirection)
    {
        //Spawns a rhythm note at the top of the screen and moves it down
        RhythmNote newNote = GetNoteFromPool();
        //Initialise the note with the correct direction
        newNote.InitaliseNote(noteDirection, currentSpawnDirection);
        //Set it to the correct positions, if moving right, spawn on the left
        if (currentSpawnDirection == GameManager.DirEnum.DirRight)
        {
            newNote.transform.position = leftSpawnPoints[(int)noteDirection].position;
        }
        else {             
            newNote.transform.position = rightSpawnPoints[(int)noteDirection].position;
        }
        //Then set the note to be active
        newNote.gameObject.SetActive(true);

        //newNote.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -5);
    }

    IEnumerator TestSpawn()
    {
        while (true) // Loop indefinitely
        {
            int randomDirection = Random.Range(0, 4);
            SpawnRhythmNote((GameManager.DirEnum)randomDirection);

            yield return new WaitForSeconds(1.0f); // Wait 
        }
    }

    //Gets an inactive rhythm note from the pool or creates a new one if none are available
    private RhythmNote GetNoteFromPool() {
        foreach (RhythmNote note in rhythmNotePool) {
            if (!note.gameObject.activeSelf) {
                return note;
            }
        }
        //If all notes are active, create a new note
        //Instantiate a new rhythm note 
        GameObject newNoteObj = Instantiate(RhythmNotePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newNoteObj.gameObject.SetActive(false);
        RhythmNote newNote = newNoteObj.GetComponent<RhythmNote>();
        //Add it to the pool and increment the pool size
        rhythmNotePool.Add(newNote);
        rhythmNotePoolSize = 0;
        return newNote;
    }

    //Try to catch a note and get the note
    private RhythmNoteHitData HitNote(GameManager.DirEnum directionInput)
    {
        RaycastHit2D hit;
        if (currentSpawnDirection == GameManager.DirEnum.DirRight) 
        {
            hit = Physics2D.Raycast(rightSpawnPoints[(int)directionInput].position, Vector2.left, maxHitRange);
            Debug.DrawRay(rightSpawnPoints[(int)directionInput].position, Vector2.left * maxHitRange, Color.red, maxHitRange);
        }
        else
        {
            hit = Physics2D.Raycast(leftSpawnPoints[(int)directionInput].position, Vector2.right, maxHitRange);
            Debug.DrawRay(leftSpawnPoints[(int)directionInput].position, Vector2.right * maxHitRange, Color.red, maxHitRange);
        }
        if (hit.collider == null || !hit.collider.gameObject.CompareTag("RhythmNote"))
        {
            return new RhythmNoteHitData(0);
        }
        return new RhythmNoteHitData(hit.collider.gameObject, hit.distance);
    }
}
