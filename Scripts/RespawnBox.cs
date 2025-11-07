using UnityEngine;

public class RespawnBox : MonoBehaviour
{
    public Vector3 RespawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnDrawGizmosSelected(){
        Gizmos.DrawSphere(RespawnPoint, 0.4f);
    }
}
