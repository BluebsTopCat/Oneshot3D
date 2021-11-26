using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class Inputthing : MonoBehaviour
{
    public EventSystem e;
    public TMP_InputField[] inputs;
    public string nums;
    public string correctpassword;
    public InMemoryVariableStorage vars;
    public Computer pc;
    public GameObject textright;
    public GameObject textwrong;
    public AudioClip fail;
    public AudioClip succed;
    public AudioClip start;
    public AudioSource se;
    private bool donebefore;
    private IEnumerator Start()
    {

        vars = FindObjectOfType<InMemoryVariableStorage>();
        yield return new WaitForSeconds(.05f);
        if (PlayerPrefs.GetString("Passwordoproom", "").Length < 4) 
        {
            List<int> numbers = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                int num = Random.Range(0, 10);
                while (numbers.Contains(num)) 
                    num = Random.Range(0, 10);
                numbers.Add(num);
                Debug.Log(num);
            }

            int red = numbers[0];
            int green = numbers[1];
            int blue = numbers[2];
            int yellow = numbers[3];
            correctpassword = red + green.ToString() + blue + yellow;
            vars.SetValue("$Red", red.ToString());
            vars.SetValue("$Green", green.ToString());
            vars.SetValue("$Blue", blue.ToString());
            vars.SetValue("$Yellow", yellow.ToString());        
            PlayerPrefs.SetString("Passwordoproom", red.ToString() + green.ToString() + blue.ToString() + yellow.ToString());
        }
        else
        {
            string s = PlayerPrefs.GetString("Passwordoproom");
            Debug.Log(s);
            int i = int.Parse(s);
            correctpassword = s;
            string red = s[0] +"";
            string green = s[1] +"";
            string blue= s[2] +"";
            string yellow = s[3] +"";
           vars.SetValue("$Red", int.Parse(red).ToString());
            vars.SetValue("$Green",int.Parse(green).ToString());
            vars.SetValue("$Blue", int.Parse(blue).ToString());
            vars.SetValue("$Yellow", int.Parse(yellow).ToString());
        }
        gameObject.SetActive(false);
    }

    public void highlight(GameObject G)
    {
        e.SetSelectedGameObject(G);
    }

    public void dothething()
    {
        if (!donebefore)
            StartCoroutine(check());
        donebefore = !donebefore;
    }

    public IEnumerator check()
    {
        yield return new WaitForSeconds(.1f);
        nums = null;
        for (var i = 0; i < inputs.Length; i++)
        {
            nums += inputs[i].text;
            inputs[i].text = null;
        }

        if (nums == correctpassword)
        {
            textright.SetActive(true);
            se.clip = succed;
            se.Play();
            GameObject.Find("ComputerInputList").SetActive(false);
            yield return new WaitForSeconds(1f);
            textright.SetActive(false);
            pc.initiatecomputerdialogue();

            Debug.Log("Right Password!");
        }
        else
        {
            textwrong.SetActive(true);
            var g = GameObject.Find("ComputerInputList");
            g.SetActive(false);
            se.clip = fail;
            se.Play();
            yield return new WaitForSeconds(.5f);
            textwrong.SetActive(false);
            g.SetActive(true);
            Debug.Log("Wrong Password :(");
            pc.wrongpassword();
        }


        gameObject.SetActive(false);
    }
}