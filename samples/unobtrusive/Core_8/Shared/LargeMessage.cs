﻿namespace Messages;

public class LargeMessage
{
    public Guid RequestId { get; set; }

    public byte[]? LargeDataBus { get; set; }
}
