using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Wallboard.Model.Repos
{
    public class LocalDeviceConfigurationRepo : IDeviceConfigurationRepo
    {
        private string filename = "DeviceConfiguration.json";

        private StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

        public LocalDeviceConfigurationRepo()
        {
        }

        public async Task<DeviceConfiguration> Get()
        {
            var file = await storageFolder.TryGetItemAsync(filename);
            if (file == null)
            {
                return null;
            }

            var textContent = await FileIO.ReadTextAsync(file as IStorageFile);
            var deviceConfiguration = JsonConvert.DeserializeObject<DeviceConfiguration>(textContent);

            return deviceConfiguration;
        }

        public async Task Save(DeviceConfiguration deviceConfiguration)
        {
            var file = (IStorageFile) await storageFolder.TryGetItemAsync(filename);

            if(file == null)
            {
                file = await storageFolder.CreateFileAsync(filename);
            }

            var configString = JsonConvert.SerializeObject(deviceConfiguration);
            await FileIO.WriteTextAsync(file, configString);
        }
    }
}
