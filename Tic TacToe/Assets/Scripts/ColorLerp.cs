using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ColorValues
{
    public float Min;
    public float Max;
}

public class ColorLerp : MonoBehaviour {

    [SerializeField] private Image image;
	[SerializeField] private float fadeDuration = 2;
    public ColorValues colorValuesR;
    public ColorValues colorValuesG;
    public ColorValues colorValuesB;

    private float r;
    private float g;
    private float b;

    private Color previousColor = Color.white;
    private Color newColor = Color.white;
    private float time = 0;
	// Use this for initialization
	void Start () {
        
        StartCoroutine(LerpColor());
	}
	
    IEnumerator LerpColor()
    {
        time = 0;
        r = Random.Range(colorValuesR.Min, colorValuesR.Max);
        g = Random.Range(colorValuesG.Min, colorValuesG.Max);
        b = Random.Range(colorValuesB.Min, colorValuesB.Max);
        newColor = new Color(r/255, g/255, b/255);

        while(time < 1)
        {
            image.color = Color.Lerp(previousColor, newColor, time);
            time += Time.deltaTime / fadeDuration;
            yield return null;
        }
        yield return new WaitForSeconds(fadeDuration);
		previousColor = newColor;
        StartCoroutine(LerpColor());
    }
}
