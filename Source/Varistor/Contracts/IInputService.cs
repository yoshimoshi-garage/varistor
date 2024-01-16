using System;

namespace VaristorDisplay;

public interface IInputService
{
    event EventHandler IncreaseRequested;
    event EventHandler DecreaseRequested;
    event EventHandler PreviousStatRequested;
    event EventHandler NextStatRequested;
}
