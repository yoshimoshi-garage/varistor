using Meadow.Foundation.Graphics;
using Meadow.Hardware;

namespace VaristorDisplay;

public interface IVaristorHardware
{
    void Initialize();
    IRheostat[] GetRheostats();
    IGraphicsDisplay? GetDisplay();
}