using Meadow.Hardware;

namespace VaristorDisplay;

public interface IDisplayService
{
    void Update();
    void UpdateRheostat(int index, IRheostat rheostat);
    void SelectRheostat(int index);
}
