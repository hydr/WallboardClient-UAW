using System.Threading.Tasks;

namespace Wallboard.Model.Repos
{
    public interface IDeviceConfigurationRepo
    {
        Task<DeviceConfiguration> Get();

        Task Save(DeviceConfiguration deviceConfiguration);
    }
}