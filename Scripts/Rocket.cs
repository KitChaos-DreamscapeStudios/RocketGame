using UnityEngine;

public class Rocket : MonoBehaviour
{

    public GameObject Explosion;
    public LayerMask Ground;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 30);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCollisionEnter2D(Collision2D col){
        
        Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
        
    }
}
