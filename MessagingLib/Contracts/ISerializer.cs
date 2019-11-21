﻿using System;

namespace MessagingLib.Contracts
{
    public interface ISerializer
    {
        string Serialize<T>(T obj);
        T Deserialize<T>(string json);
    }
}
