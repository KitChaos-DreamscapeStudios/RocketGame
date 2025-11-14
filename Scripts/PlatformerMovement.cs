using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
//To Add this script, create a C# file, and name it PlatformerMovement(Exaclty that) and then paste this entire script in, from start to end.
//Alternatively, if you wish to name your file something else, only paste in the content below line 7, and remove the last curlybrace.

public class PlatformerMovement: MonoBehaviour
{
    
    public const float THRESHOLDVELOC = 10f;
    public Rigidbody2D body;
    public float horizontal;
    public float jumpPower = 5;
    public bool isOnGround;
    public LayerMask ground;
    public float moveSpeed = 5;
    float BaseSize;
    public GameObject Rocket;
    public GameObject Orient;
    public Vector3 KBVelocity;
    float ImpactTime;
    Vector3 RespawnPosition;
    public int RocketsLeft;
    public TMPro.TextMeshProUGUI AmmoCounter;
    float RocketCool;
    // Start is called before the first frame update
    //Additional Instructions
    //Make sure the object you attatch this to has a Rigidbody2D component attatched to it, and there is a square below it with the Layer "Ground"
    //Both player and floor should have BoxCollider2D's
    //Make sure "ground" in the object with this script attatched is set to the layer you made "Ground"
    //

    void Start()
    {
        
        BaseSize = Camera.main.orthographicSize;
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
        ImpactTime -= Time.unscaledDeltaTime;
        if(ImpactTime >0){
            Time.timeScale = 0;
        }
        else{
            Time.timeScale = 1;
        }
        if(ImpactTime <=0 && GetComponent<SpriteRenderer>().color.r < 0.5f){
            foreach(SpriteRenderer s in FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None)){
                s.color = new Color(1, 1, 1, 1);
            }
            Camera.main.backgroundColor = new Color(0.2f, 0.3f, 0.7f);
        }
        if(isOnGround&&ImpactTime<=-0.5f){
            RocketsLeft = 3;
        }
        //Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(transform.position.x, transform.position.y, -10), 0.1f);
        horizontal = Input.GetAxisRaw("Horizontal");
        isOnGround = Physics2D.OverlapBox(transform.position-new Vector3(0,1f), new Vector2(0.95f, 0.2f), 0, ground);
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround == true)
        {
             if(body.linearVelocityX >= THRESHOLDVELOC){
                body.linearVelocity += new Vector2(0, 1 * jumpPower);
            }
            else{
            if(ImpactTime <=-0.25f){
                 body.linearVelocity = new Vector2(body.linearVelocity.x, 1 * jumpPower);
            }
           
        }
            

        }
        
        
        if((Mathf.Abs(body.linearVelocityX)> THRESHOLDVELOC||Mathf.Abs(body.linearVelocityY) >THRESHOLDVELOC)&&Time.timeScale>0){
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, BaseSize + ((Mathf.Abs(body.linearVelocityX) + Mathf.Abs(body.linearVelocityY)) - THRESHOLDVELOC), 0.16f);
        }
        else{
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,BaseSize,0.2f);
        }
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        var pos1 = transform.position;
        pos1.z = -10;
        // get direction you want to point at
        Vector2 direction = (mouseScreenPosition - (Vector2)transform.position).normalized;
         if(Input.GetMouseButton(1)){
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, pos1 + ((Vector3)direction * 20), 0.1f);
        }
        else{
            var pos = transform.position;
            pos.z = -10;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, pos, 0.1f);
        }
        Orient.transform.eulerAngles = -(Orient.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if(Input.GetMouseButtonDown(0)&&RocketCool <=0&&RocketsLeft>0){
            RocketsLeft -= 1;
            RocketCool = 0.5f;
            var Rk = Instantiate(Rocket, transform.position + Orient.transform.forward.normalized * 1, Quaternion.identity);
            Rk.transform.position = (Vector2)transform.position + direction *1.1f;
            Rk.transform.right = direction;
            Rk.GetComponent<Rigidbody2D>().linearVelocity = Rk.transform.right.normalized * 10;
        }
        RocketCool -= Time.deltaTime;
        AmmoCounter.text = RocketsLeft.ToString();

    }
    private void FixedUpdate()
    {
       
        if(ImpactTime<=-0.5f){
            if(!isOnGround){
                if(horizontal!=0){
                     body.linearVelocity = new Vector2(horizontal * moveSpeed, body.linearVelocity.y);
                }
            }
            else{
                body.linearVelocity = new Vector2(horizontal * moveSpeed, body.linearVelocity.y);
            }
            
           
        }
       
    }
    bool HoriLeftRight(){
        if(horizontal > 0){
            return true;
        }
        else{
            return false;
        }
    }

    
    public void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.TryGetComponent(out Explosion e)){
            body.linearVelocity = ((transform.position - other.gameObject.transform.position).normalized)*e.force;
            ImpactTime = 0.4f;
            foreach(SpriteRenderer s in FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None)){
                Camera.main.backgroundColor = new Color(1, 1, 1);
                s.color = new Color(0.01f, 0.01f, 0.01f, 1);
            }

        }
       if(other.gameObject.TryGetComponent(out RespawnBox r)){
            RespawnPosition = r.RespawnPoint;
        }
        if(other.gameObject.TryGetComponent(out AmmoPack A)){
            RocketsLeft += A.AmmoFilled;
            if(RocketsLeft>3){
                RocketsLeft = 3;
            }
            A.RespawnTime = 5;
        }
    }
    public void OnCollisionEnter2D(Collision2D col){
       
        if(col.collider.gameObject.CompareTag("Kill")){
            Debug.Log("HitKillbox");
            transform.position = RespawnPosition;
        }
    }
}
