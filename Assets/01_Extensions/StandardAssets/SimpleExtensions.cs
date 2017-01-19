using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FloatUnityEvent : UnityEvent<float> { }

[System.Serializable]
public class IntUnityEvent : UnityEvent<int> { }

[System.Serializable]
public class StringUnityEvent : UnityEvent<string> { }

public static class SimpleExtensions
{
    public static void SendEvent(this Object target, UnityEngine.Events.UnityEvent action)
    {
        if (action != null)
            action.Invoke();
    }

    public static void SendEvent(this Object target, UnityEngine.Events.UnityAction action)
    {
        if (action != null)
            action.Invoke();
    }

    public static void SendEvent(this Object target, System.Action action)
    {
        if (action != null)
            action.Invoke();
    }

    public static string GetName<T>(this T obj, Expression<System.Func<T>> propertyAccessor)
    {
        return GetName(propertyAccessor.Body);
    }

    private static string GetName(Expression expression)
    {
        if (expression.NodeType == ExpressionType.MemberAccess)
        {
            var memberExpression = expression as MemberExpression;
            if (memberExpression == null)
                return null;
            return memberExpression.Member.Name;
        }
        return null;
    }
}