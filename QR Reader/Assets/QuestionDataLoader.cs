using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class QuestionDataLoader : MonoBehaviour {
    public TextAsset asset;
    public static QuestionDataLoader instance;
    public Questions[] quest;
	// Use this for initialization
	void Awake () {
        quest = JsonHelper.FromJson<Questions>(asset.text);
	}
	
    public Questions GetQuestion(string questionId)
    {
        foreach (Questions q in quest)
        {
            if (q.Id == questionId)
            {
                return q;
            }
        }
//        Questions q = quest.Where(item => item.Id == questionId).FirstOrDefault();
        return null;
    }

    public bool isValidQuestionId(string questionId)
    {
        return quest.Any(item => item.Id == questionId);
    }
}

[Serializable]
public class Questions
{
    public string Id ;
    public string Question;
    public string OptionA;
    public string OptionB;
    public string OptionC;
    public string OptionD;
    public string Answer;
}
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}