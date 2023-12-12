using System;

namespace Varistor;

public interface IInputService
{
    event EventHandler SmallIncreaseRequested;
    event EventHandler LargeIncreaseRequested;
    event EventHandler SmallDecreaseRequested;
    event EventHandler LargeDecreaseRequested;
}
