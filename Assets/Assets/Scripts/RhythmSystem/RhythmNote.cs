using UnityEngine;

public class RhythmNote : MonoBehaviour
{
    [SerializeField]
    private Sprite[] DirectionSprites;
    float speed = 0f;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private GameManager.DirEnum noteDirection;
    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        moveNote();
    }

    public float CalculateNoteSpeed() {
        //TODO calculate note speed based on song BPM and distance to travel
        float speed = 5f;
        return speed;
    }
    public void InitaliseNote(GameManager.DirEnum direction)
    {
        //Set the direction of the sprite
        SetNoteSprite(direction);

        //Calculate the speed of this note
        speed = CalculateNoteSpeed();
    }

    public void SetNoteSprite(GameManager.DirEnum direction) { 
        spriteRenderer.sprite = DirectionSprites[(int)direction];
    }

    private void moveNote() { 
        transform.transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}