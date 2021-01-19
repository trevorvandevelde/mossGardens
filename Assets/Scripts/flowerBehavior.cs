using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowerBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    //this script should be attached to flower parent object NOT PREFAB/MODEL

    AudioSource growingEffect;
    AudioSource musicEffect;
    private bool PlaySound;
    public float secondsWait; 

    IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);
        growingEffect = GetComponents<AudioSource>()[0];
        musicEffect = GetComponents<AudioSource>()[1];
        PlaySound = true; //bool to detect is sound is playing
        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(10f);
        PlaySound = true; 
        this.gameObject.GetComponentInChildren<Animator>().Play("flower_petal_opening", -1, 0f); 

    }

   
    // Update is called once per frame
    void Update()
    {
        if (PlaySound == true)
        {
            musicEffect.Play();
            PlaySound = false;
        }
        else
        {
            
        }
        if (musicEffect.isPlaying == false)
        {  
            //this.gameObject.GetComponentInChildren<Animator>().Play("flower_petal_closing", -1, 0f);
            StartCoroutine(Wait());
        }
    }


}
