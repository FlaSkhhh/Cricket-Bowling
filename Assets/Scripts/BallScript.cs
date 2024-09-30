using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallScript : MonoBehaviour
{
    [SerializeField] Transform bounceSpot;
    [SerializeField] Transform cameraPos;

    Rigidbody rb;

    [SerializeField] Slider swingSlider;
    [SerializeField] Slider speedSlider;
    [SerializeField] GameObject switchButton;


    float speedx;
    float speedy;
    float speedz;

    float ballSpeed;
    float swing;         //value is between -5 to 5

    bool onSwing = true;
    bool firstTouch;

    float fallTime;
    float finalTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fallTime = Mathf.Sqrt(2 * cameraPos.position.y / 10);
        switchButton.transform.GetChild(0).GetComponent<Text>().text = "Now Swinging";
    }

    void Update()
    {
        Bowl();
        WicketSideChange();
    }

    void Bowl()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ResetBall();
            SpeedCalc();

            rb.AddForce(new Vector3(speedx, -speedy, speedz+ballSpeed), ForceMode.VelocityChange);
            if (onSwing) { StartCoroutine(Swing()); }
        }
    }

    IEnumerator Swing()
    {
        float timer = 0;
        //rb.AddForce(new Vector3(swing, 0, 0), ForceMode.VelocityChange);
        while(timer<finalTime/2)
        {
            rb.AddForce(new Vector3(swing/20, 0, 0) * Time.deltaTime * 50, ForceMode.VelocityChange);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //yield return new WaitForSeconds(finalTime/2f);
        while (timer>finalTime/2 && timer < finalTime)
        {
            rb.AddForce(new Vector3(-3*swing/20, 0, 0)*Time.deltaTime*50, ForceMode.VelocityChange);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //rb.AddForce(new Vector3(-swing*(1f+ballSpeed/100f), 0, 0), ForceMode.VelocityChange);
        //yield return new WaitForSeconds(finalTime/2f);
        //rb.AddForce(new Vector3(-swing*0.7f, 0, 0), ForceMode.VelocityChange);
    }

    void OnCollisionEnter()
    {
        if (!onSwing && !firstTouch)
        {
            rb.AddForce(new Vector3(swing, 0, 0), ForceMode.VelocityChange); //spin
            firstTouch = true;
        }
    }

    void ResetBall()
    {
        transform.position = cameraPos.position;
        rb.velocity = Vector3.zero;
        if (onSwing) { StopCoroutine(Swing()); }
        firstTouch = false;
    }

    void SpeedCalc()
    {
        float distancez = bounceSpot.position.z - cameraPos.position.z+0.3f;     // little offset for higer speeds
        speedz = distancez / fallTime;                                           // forward speed required
        finalTime = distancez / (speedz + ballSpeed);

        speedy = (cameraPos.position.y - 5 * finalTime * finalTime) / finalTime; // downwards speed required

        float distancex = bounceSpot.position.x - cameraPos.position.x;
        speedx = distancex / finalTime;                                          // sideways speed required
    }

    void WicketSideChange()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            cameraPos.position = new Vector3(-0.5f, cameraPos.position.y, cameraPos.position.z);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            cameraPos.position = new Vector3(0.5f, cameraPos.position.y, cameraPos.position.z);
        }
    }

    public void SwingSlider()
    {
        swing = swingSlider.value;
    }

    public void SpeedSlider()
    {
        ballSpeed = speedSlider.value;
    }

    public void ModeSwitch()
    {
        onSwing = !onSwing;
        if (onSwing)
        {
            switchButton.transform.GetChild(0).GetComponent<Text>().text = "Now Swinging";
        }
        else
        {
            switchButton.transform.GetChild(0).GetComponent<Text>().text = "Now Spinning";
        }
    }
}
