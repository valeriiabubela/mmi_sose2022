using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using System.Linq;

public class Player : MonoBehaviour
{

    public Transform groundCheckTransform ;
    public LayerMask playerMask;
    private bool jumpKeyPressed;
    private bool collection;
    private float horizontalInput;
    private Rigidbody rigidbodyComponent;
    
    public string[] keywords = new string[] { "forward", "back", "jump", "collect" };
    private int superJumpsRemaining;
    protected KeywordRecognizer keywordRecognizer;
    //private Dictionary<string, System.Action> actions = new Dictionary<string, System.Action>();
    protected string word = "forward";

     // Start is called before the first frame update
    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
       /* //we add the jump function to the dictionnary
        actions.Add("jump", () => rigidbodyComponent.AddForce(UnityEngine.Vector3.up * 9, ForceMode.VelocityChange));
        actions.Add("forward", () => rigidbodyComponent.AddForce(UnityEngine.Vector3.right * 25, ForceMode.VelocityChange));
        actions.Add("back", () => rigidbodyComponent.AddForce(UnityEngine.Vector3.left * 25, ForceMode.VelocityChange));
        actions.Add("collect", Collect);
        */

        //we set the speech recognition function and start it
        keywordRecognizer = new KeywordRecognizer(keywords, ConfidenceLevel.Low);
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
        Debug.Log( keywordRecognizer.IsRunning );

    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs args)
    {
        word = args.text;

        Debug.Log("You told the bullet to: " + word);
        switch (word)
        {
            case "forward":
                if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey("a")){
                    rigidbodyComponent.AddForce(UnityEngine.Vector3.right * 30, ForceMode.VelocityChange);
                }
                break;
            case "back":
                if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey("d")){
                    rigidbodyComponent.AddForce(UnityEngine.Vector3.left * 30, ForceMode.VelocityChange);
                }
                break;
            case "jump":
                rigidbodyComponent.AddForce(UnityEngine.Vector3.up * 12, ForceMode.VelocityChange);
                break;
            case "collect":
                collection = true;
                break;

        }
        //actions[speech.text].Invoke();
    }

    public void EndListening()
    {
        //actions.Clear();
       if (keywordRecognizer != null && keywordRecognizer.IsRunning )
        {
            //keywordRecognizer.OnPhraseRecognized -= RecognizedSpeech;
            keywordRecognizer.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyPressed = true;

        }
        if (Input.GetKeyDown("k"))
        {
            collection = true;

        }
        horizontalInput = Input.GetAxis("Horizontal");
    }


//Fixed Update is called once every physics update
    private void FixedUpdate()
    {
        rigidbodyComponent.velocity = new UnityEngine.Vector3(horizontalInput, rigidbodyComponent.velocity.y, 0);

        if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask ).Length == 0)
        {
          return;  
        }
        
        if (jumpKeyPressed == true)
        {
            float jumpPower = 5f;

            if (superJumpsRemaining>0)
            {
                jumpPower*=2;
                superJumpsRemaining --;
            }
            rigidbodyComponent.AddForce(UnityEngine.Vector3.up * 7, ForceMode.VelocityChange);
            jumpKeyPressed = false;
            collection = false;


        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9 && collection==true)
        {
            ScoreScript.scoreValue +=1;
            collection = false;
            Destroy(other.gameObject);
            superJumpsRemaining++;
        }
        collection = false;
    }
    
}
