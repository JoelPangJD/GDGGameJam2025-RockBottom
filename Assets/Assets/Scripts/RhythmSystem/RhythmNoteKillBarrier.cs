using UnityEngine;
using UnityEngine.Audio;

public class RhythmNoteKillBarrier : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Rhythm Note Hit Kill Barrier");
        //Miss the note if it hits this barrier
        if (col.gameObject.CompareTag("RhythmNote"))
            col.gameObject.GetComponent<RhythmNote>().MissNote();
    }
}
