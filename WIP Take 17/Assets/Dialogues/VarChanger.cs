using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class VarChanger : MonoBehaviour
{
    private Dictionary<string, Ink.Runtime.Object> variables;

    public VarChanger(TextAsset globalsFilePath)
    {
        Story globalStory = new Story(globalsFilePath.text);
        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalStory.variablesState)
        {
            Ink.Runtime.Object value = globalStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            
        }
    }
    private void VarChanged(string Name, Ink.Runtime.Object value)
    {
        if (variables.ContainsKey(Name))
        {
            variables.Remove(Name);
            variables.Add(Name, value);
        }
    }


    private void VariablesToStory(Story story)
    {
        foreach(KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }

    public void StartListen(Story story)
    {
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VarChanged;
    }

    public void StopListen(Story story)
    {
        story.variablesState.variableChangedEvent -= VarChanged;
    }
}
