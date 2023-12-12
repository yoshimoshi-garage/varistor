using Meadow.Hardware;

namespace Varistor;

public interface IDisplayService
{
    void Initialize(int rheostatCount);
    void Update();
    void UpdateRheostat(int index, IRheostat rheostat);
}
