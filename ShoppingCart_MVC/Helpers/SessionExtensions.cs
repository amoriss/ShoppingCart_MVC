﻿using System.Text.Json;

namespace ShoppingCart_MVC.Helpers;

public static class SessionExtensions
{
    public static void SetObject(this ISession session, string key, object value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T GetObject<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }
}
