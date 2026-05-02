using Ink.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueVariables
{
    private Dictionary<string, Ink.Runtime.Object> _variables;

    public DialogueVariables()
    {
        _variables = new Dictionary<string, Ink.Runtime.Object>();
    }

    #region GET
    public int GetInt(string variableName) => GetValue(variableName, 0);

    public float GetFloat(string variableName) => GetValue(variableName, 0f);

    public bool GetBool(string variableName) => GetValue(variableName, false);

    private T GetValue<T>(string variableName, T defaultValue)
    {
        if (_variables.ContainsKey(variableName))
        {
            Ink.Runtime.Value inkValue = _variables[variableName] as Ink.Runtime.Value;
            if (inkValue.valueObject is T casted)
            {
                return casted;
            }
            else
            {
                Debug.LogError($"The variable {variableName} is not of type {typeof(T)}.");
            }
        }
        else
        {
            Debug.LogError($"The variable {variableName} does not exist");
        }
        return defaultValue;
    }
    #endregion

    #region SET
    public void SetInt(string variableName, int value)
    {
        Ink.Runtime.IntValue integer = GetOrCreateVariableValue<Ink.Runtime.IntValue>(variableName);
        integer.value = value;
    }

    public void SetFloat(string variableName, float value)
    {
        Ink.Runtime.FloatValue floatValue = GetOrCreateVariableValue<Ink.Runtime.FloatValue>(variableName);
        floatValue.value = value;
    }

    public void SetBool(string variableName, bool value)
    {
        Ink.Runtime.BoolValue boolean = GetOrCreateVariableValue<Ink.Runtime.BoolValue>(variableName);
        boolean.value = value;
    }    

    private T GetOrCreateVariableValue<T>(string variableName) where T : Ink.Runtime.Value, new()
    {
        T variable;
        if (_variables.ContainsKey(variableName))
        {
            variable = _variables[variableName] as T;
        }
        else
        {
            variable = new T();
            _variables[variableName] = variable;
        }
        return variable;
    }
    #endregion

    #region ADD AND CHANGE VARIABLES
    public void AddNewGlobalVariablesFromStory(Story story)
    {
        foreach (var variableName in story.variablesState)
        {
            if (!_variables.ContainsKey(variableName))
            {
                Ink.Runtime.Object value = story.variablesState.GetVariableWithName(variableName);
                _variables.Add(variableName, value);
                Debug.Log($"Initialized global dialogue variable: {variableName} = {value}");
            }
        }
    }

    public void StartListening(Story story)
    {
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
    }

    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    private void VariableChanged(string variableName, Ink.Runtime.Object value)
    {
        Debug.Log($"Variable Changed! {variableName}:{value}");

        if (_variables.ContainsKey(variableName))
        {
            _variables[variableName] = value;
        }
    }

    private void VariablesToStory(Story story)
    {
        foreach (var variable in _variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
    #endregion
}