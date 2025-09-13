using System.Collections;
using UnityEngine;

public class RhythmNote : MonoBehaviour
{

    [SerializeField]
    private Sprite[] DirectionSprites;
    public float speed = 5f;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Rigidbody2D rb;

    private GameManager.DirEnum noteDirection;

    [SerializeField]
    public ParticleSystem goodParticle;
    [SerializeField]
    public ParticleSystem perfectParticle;

    //The distance the note has to travel to reach the hit point
    public float travelDistance = 1f;
    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        MoveNote();
    }

    public void InitaliseNote(GameManager.DirEnum direction, GameManager.DirEnum noteDirection, float travelDistance, float speed)
    {
        //Reset rotations
        transform.rotation = Quaternion.identity;
        //Set the direction of the sprite
        SetNoteSprite(direction);
        //Set the note direction
        this.noteDirection = noteDirection;

        //Set the travel distance for calculations later
        this.travelDistance = travelDistance;

        //Reset the rigidbody
        rb.gravityScale = 0f;
        GetComponent<Collider2D>().enabled = true;

        this.speed = speed;
    }

    public void SetNoteSprite(GameManager.DirEnum direction) { 
        spriteRenderer.sprite = DirectionSprites[(int)direction];
    }

    private void MoveNote() { 
        if(noteDirection == GameManager.DirEnum.DirLeft)
            transform.transform.Translate(Vector3.left * speed * Time.deltaTime);
        else if(noteDirection == GameManager.DirEnum.DirRight)
            transform.transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public void HitNote()
    {
        Instantiate(goodParticle, transform.position, Quaternion.identity);
        StartCoroutine(SelfDestruct(0f));
    }

    public void HitPerfectNote()
    {
        Instantiate(perfectParticle, transform.position, Quaternion.identity);
        StartCoroutine(SelfDestruct(0f));
    }

    public void MissNote()
    {
        //Lose life on miss
        GameManager.instance.AddLife(-5);

        rb.gravityScale = Random.Range(0.8f,1.2f);
        rb.AddTorque(Random.Range(-0.1f,0.1f), ForceMode2D.Impulse);
        rb.AddForce(new Vector2(Random.Range(-0.2f,0.2f),1f) * Random.Range(2f,5f), ForceMode2D.Impulse);
        speed = 0;
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(SelfDestruct(5.0f));
    }

    
    IEnumerator SelfDestruct(float time)
    {
        //Wait for 5 seconds before deactivating the note
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}