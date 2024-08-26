using Microsoft.AspNetCore.SignalR;

namespace MQTT
{
    public class MyHub: Hub
    {
		private readonly ManagedMqtt _managed;

		public MyHub(ManagedMqtt managed) 
        {
            _managed = managed;
        }
        public async Task SEND (string data)
        {
            try
            {
                await _managed.Publish("VTSauto/AR_project/Kep_write", data, true);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }
}
