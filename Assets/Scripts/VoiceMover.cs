using HoloToolkit.Unity;
using UnityEngine;

public class VoiceMover : MonoBehaviour
{
    public float MovementVelocity = 30f;

    private TextToSpeechManager textToSpeechManager;

    public void MoveForward()
    {
        this.transform.Translate(Vector3.forward * Time.deltaTime * this.MovementVelocity);
        this.textToSpeechManager.SpeakText("Moving forward, sir!");
    }

    public void MoveBackward()
    {
        this.transform.Translate(Vector3.back * Time.deltaTime * this.MovementVelocity);
        this.textToSpeechManager.SpeakText("Moving backward, sir!");
    }

    // Use this for initialization
    void Start()
    {
        this.textToSpeechManager = this.GetComponent<TextToSpeechManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
