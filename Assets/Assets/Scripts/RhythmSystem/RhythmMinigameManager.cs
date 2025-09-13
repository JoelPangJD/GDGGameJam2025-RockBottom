using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RhythmMinigameManager : MonoBehaviour
{
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
    //The left spawn points for the rhythm notes
    [SerializeField]
    private Transform[] leftSpawnPoints = new Transform[4];
    //The right spawn points for the rhythm notes
    [SerializeField]
    private Transform[] rightSpawnPoints = new Transform[4];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Create a list to store inactive rhythm notes for object pooling
        rhythmNotePool = new List<RhythmNote>();
        for (int i = 0; i < rhythmNotePoolSize; i++) {
            //Create a new rhythm note and add it to the list as an inactive object
            GameObject newNote = Instantiate(RhythmNotePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newNote.gameObject.SetActive(false);
        }

        //test spawn code
        StartCoroutine(TestSpawn());
    }

    // Update is called once per frame
    void Update()
    {
        //Gets the inputs for the rhythm minigame
        if (Input.GetKeyDown(inputKeys[(int)GameManager.DirEnum.DirUp])) { 
            Debug.Log("Up Key Pressed");
        }
        if (Input.GetKeyDown(inputKeys[(int)GameManager.DirEnum.DirRight]))
        {
            Debug.Log("Up Right Pressed");
        }
        if (Input.GetKeyDown(inputKeys[(int)GameManager.DirEnum.DirLeft]))
        {
            Debug.Log("Up Left Pressed");
        }
        if (Input.GetKeyDown(inputKeys[(int)GameManager.DirEnum.DirDown]))
        {
            Debug.Log("Up Down Pressed");
        }
    }

    private void SpawnRhythmNote(GameManager.DirEnum noteDirection, GameManager.DirEnum moveDirection)
    {
        //Spawns a rhythm note at the top of the screen and moves it down
        RhythmNote newNote = GetNoteFromPool();
        //Initialise the note with the correct direction
        newNote.InitaliseNote(noteDirection);
        //Set it to the correct positions, if moving right, spawn on the left
        if (moveDirection == GameManager.DirEnum.DirRight)
        {
            newNote.transform.position = leftSpawnPoints[(int)noteDirection].position;
        }
        else {             //TODO add right spawn points
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
            SpawnRhythmNote((GameManager.DirEnum)randomDirection, GameManager.DirEnum.DirRight);

            yield return new WaitForSeconds(0.3f); // Wait 
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
}
