using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace Varistor;

// Change F7FeatherV2 to F7FeatherV1 for V1.x boards
public class MeadowApp : App<F7CoreComputeV2>
{
    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var hardware = new VaristorProjLab();
        hardware.Initialize();

        var displayService = new DisplayService(hardware.GetDisplay());

        new AppLogic(hardware, displayService, hardware);

        return base.Initialize();
    }
}