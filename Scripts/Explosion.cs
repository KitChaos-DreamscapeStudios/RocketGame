using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float force;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.GetComponent<Explosion>()){
            force *= 2;
        }
    }
}
