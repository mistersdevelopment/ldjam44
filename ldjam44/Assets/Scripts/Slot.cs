using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {

    private SpriteRenderer body;
    private SpriteRenderer screen;

	// Use this for initialization
	void Start () {
        body = gameObject.transform.Find("Slots_Body").gameObject.GetComponent<SpriteRenderer>();
        screen = gameObject.transform.Find("Slots_Screen").gameObject.GetComponent<SpriteRenderer>();
        body.color = PrettyColor(0.6f);

        StartCoroutine(BlinkyScreen());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator BlinkyScreen()
    {
        while (true)
        {
            screen.color = PrettyColor(1f);
            yield return new WaitForSeconds(Random.Range(0.3f, 1.0f));
        }
    }

    Color PrettyColor(float value)
    {
        return Color.HSVToRGB(Random.Range(0.0f, 1.0f), value * 0.5f, value);
    }
}
