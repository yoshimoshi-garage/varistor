using Meadow.Foundation.Graphics;
using Meadow.Hardware;

namespace Varistor;

public interface IVaristorHardware
{
    void Initialize();
    IRheostat[] GetRheostats();
    IGraphicsDisplay? GetDisplay();
}