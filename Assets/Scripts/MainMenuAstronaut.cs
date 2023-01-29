using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAstronaut : MonoBehaviour
{
    public float speed = .5f;
    public float rotSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float sin = Mathf.Sin( Time.time );
        Vector3 position = transform.position;
        position.y += sin * speed * Time.deltaTime;
        transform.position = position;

        transform.Rotate(transform.up * rotSpeed * Time.deltaTime);
    }
}
