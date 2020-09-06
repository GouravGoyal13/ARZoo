using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using System;
using UnityEngine.UI;

public class QRReader : MonoBehaviour
{
    private WebCamTexture camTexture;
    public RectTransform canvasSize;
    private Rect screenRect;
    public RawImage backgrounfTexture;

    public GameObject Scanner;
    public GameObject questionBoard;
    public Text QuestionText;
    public Text optionA;
    public Text optionB;
    public Text optionC;
    public Text optionD;
    public Button[] optionBtn;
    Questions[] question;
    public QuestionDataLoader questionData;
    void Start()
    {
        foreach (Button btn in optionBtn)
        {
            btn.onClick.AddListener(() => EvaluteAnswer());
        }

        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        Debug.Log(Screen.height);
        camTexture = new WebCamTexture();
        camTexture.requestedHeight = Screen.height;
        camTexture.requestedWidth = Screen.width;
        if (camTexture != null)
        {
            camTexture.Play();
        }
        backgrounfTexture.texture = camTexture;
//        question = QuestionDataLoader.instance.quest;
    }
//    public Questions GetQuestion(string questionId)
//    {
//        foreach (Questions q in question)
//        {
//            if (q.Id == questionId)
//            {
//                return q;
//            }
//        }
//        //        Questions q = quest.Where(item => item.Id == questionId).FirstOrDefault();
//        return null;
//    }
    void Update()
    {
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            // decode the current frame
            var result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);
            if (result != null)
            {
                q =questionData.GetQuestion(result.Text);
                if(q!=null)
                {
                    questionBoard.SetActive(true);
                    Scanner.SetActive(false);
                    setQuestionBoard();
                }
                Debug.Log("DECODED TEXT FROM QR: " + result.Text);
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.Message);
        }
    }

    Questions q;
    void EvaluteAnswer()
    {
        foreach (Button btn in optionBtn)
        {
            if (btn.name.Equals(q.Answer))
            {
                btn.image.color = Color.green;
            }
            else
            {
                btn.image.color = Color.red;
            }
        }
    }

    void setQuestionBoard()
    {
        if (q != null)
        {
            QuestionText.text = q.Question;
            optionA.text = q.OptionA;
            optionB.text = q.OptionB;
            optionC.text = q.OptionC;
            optionD.text = q.OptionD;
        }
    }
}
