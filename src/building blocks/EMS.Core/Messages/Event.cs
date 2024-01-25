﻿namespace EMS.Core.Messages;

public class Event : Message
{
    public DateTime Timestamp { get; private set; }

    protected Event()
    {
        Timestamp = DateTime.Now;
    }
}