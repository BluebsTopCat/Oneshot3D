using UnityEngine;
using Yarn.Unity;

public class SetVar : MonoBehaviour
{
    [YarnCommand("setvar")]
    public void setvar(string name, string input)
    {
        float i;
        bool b;
        if (float.TryParse(input, out i))
            PlayerPrefs.SetFloat(name, i);
        else if (bool.TryParse(input, out b))
            PlayerPrefs.SetInt(name, toint(b));
    }

    private static int toint(bool b)
    {
        switch (b)
        {
            case true:
                return 1;
            
            case false:
                return 0;
             
        }
    }
}