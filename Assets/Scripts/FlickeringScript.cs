using UnityEngine;
using UnityEngine.UI;

public class FlickeringScript : MonoBehaviour
{
    private Text txt;
    private bool isFading;

    private void Start()
    {
        txt = GetComponent<Text>();
    }

    private void Update()
    {
        if (isFading)
        {
            Color c = txt.color;
            c.a -= Time.deltaTime * 1.5f;
            txt.color = c;
            if (c.a < .2f) isFading = false;
        }
        else
        {
            Color c = txt.color;
            c.a += Time.deltaTime * 1.5f;
            txt.color = c;
            if (c.a > 1) isFading = true;
        }
    }
}