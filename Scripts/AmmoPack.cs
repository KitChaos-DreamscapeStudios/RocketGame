using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    public float RespawnTime;
    Vector3 BasePos;
    public int AmmoFilled = 3;
    float bobSin;
    float randOffset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BasePos = transform.position;
        randOffset = Random.Range(0.5f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
        bobSin += Time.deltaTime*2;
        if(bobSin>=randOffset){
              transform.position = BasePos + new Vector3(0, Mathf.Sin(bobSin) * 1);
        }
      
        RespawnTime -= Time.deltaTime;
        if(RespawnTime>=0){
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
        else{
             GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Collider2D>().enabled = true;
        }
    }
}
