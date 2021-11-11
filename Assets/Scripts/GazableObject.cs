using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazableObject : MonoBehaviour
{
    public bool beingGazedAt = false;
    [SerializeField] private Color activatedColor;
    [SerializeField] private TextMesh intense;
    Rigidbody r; 
    
    Material material;
    bool activated = false;

    float seconds = Mathf.FloorToInt(5.0f);

    private void Awake()
    {
        material = transform.GetComponent<Renderer>().material;

        r = gameObject.GetComponent<Rigidbody>();

        gameObject.layer = LayerMask.NameToLayer("GazableObjects");
    }

    // Called when the camera is gazing on this object
    public void OnGaze(float delay)
    {
       
        // If the object is already changing its color,
        // don't call the function again.
        if (activated) {
            return; 
        }

        CountDownTimer();
        StartCoroutine(ColorChange(material.color, activatedColor, material, 1, delay));
        activated = true;
    }

    void OnIntenseGaze() {

        Debug.Log("OnIntenseGaze()");
        //bounce();
        intense.gameObject.SetActive(true);
    }

    void bounce() {

        r.AddForce(0, 100.0f, 0);
    
    }

    void CountDownTimer()
    {
    
        if (seconds > 0)
        {

            seconds--;
            Invoke("CountDownTimer", 1.0f);
        }

        else {

            OnIntenseGaze();
        }
       
    }


    // When you stop looking at a gazable object
    public void OnStopGaze()
    {
        if(!activated) { return; }
        CancelInvoke("CountDownTimer");
        StopAllCoroutines();
        StartCoroutine(ColorChange(material.color, Color.black, material, 1, 0));
        activated = false;
        intense.gameObject.SetActive(false);
        //reset timer
        seconds = Mathf.FloorToInt(5.0f);
        //this.gameObject.GetComponent<SphereCollider>().material.bounciness = 0; 
    }

    // Change the color of the object
    IEnumerator ColorChange(Color from, Color to, Material mat, float time, float delay)
    {
        float t = 0;

        yield return new WaitForSeconds(delay);

        while (t < time)
        {
            mat.color = Color.Lerp(from, to, t);

            yield return new WaitForSeconds(0.05f);

            t += 0.05f;
        }

    }

}
