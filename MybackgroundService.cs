
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using MQTT.Models;
using MQTTnet.Client;
using System.Text;
using Newtonsoft.Json;
using MQTTnet.Server;
//using Timer = System.Timers.Timer;

namespace MQTT
{
	public class MybackgroundService : BackgroundService
	{
		private readonly ManagedMqtt _managedMqtt;
		private readonly IHubContext<MyHub> _hubContext;
		private readonly IServiceScopeFactory _scopeFactory;

		//private readonly Timer _reconnectTimer;
		public MybackgroundService(ManagedMqtt managedMqtt, IHubContext<MyHub> hubContext, IServiceScopeFactory scopeFactory)
		{
			_managedMqtt = managedMqtt;
			_hubContext = hubContext;
			_scopeFactory = scopeFactory;

			//_reconnectTimer = new Timer(5000);
			//_reconnectTimer.Elapsed += ReconnectTimerElapsed;
			_managedMqtt.ApplicationMessageReceived += OnMqttClientMessageReceivedAsync;
			//_mqttdata = mqttdata;
		}

		protected async override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			ConnectAsync().Wait();
			//SignalR_1().Wait();

		}
		// thuc hien tai day
		private async Task OnMqttClientMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
		{
			//string topic = e.ApplicationMessage.Topic;
			var payloadMessage = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
			Console.WriteLine(payloadMessage);

			payloadMessage = payloadMessage.Replace("\\", "");
			payloadMessage = payloadMessage.Replace("\r", "");
			payloadMessage = payloadMessage.Replace("\n", "");
			payloadMessage = payloadMessage.Replace(" ", "");
			payloadMessage = payloadMessage.Replace("false", "\"FALSE\"");
			payloadMessage = payloadMessage.Replace("true", "\"TRUE\"");
			payloadMessage = payloadMessage.Replace("[", "");
			payloadMessage = payloadMessage.Replace("]", "");
			payloadMessage = payloadMessage.Replace("Channel1.Device1.Traffic_", "");
			payloadMessage = payloadMessage.Replace("PLC.Vali_IFM.", "");
			payloadMessage = payloadMessage.Replace("PLC.Vali_Siemens.", "");
			payloadMessage = payloadMessage.Replace("Channel1.Device1.", "");
			payloadMessage = payloadMessage.Replace("PLC.Vali_Siemens.", "");
			payloadMessage = payloadMessage.Replace("Channel1.Micro820.", "");
			payloadMessage = payloadMessage.Replace("PLC.Inverter.", "");
			payloadMessage = payloadMessage.Replace("Micro.Micro850.","");
			payloadMessage = payloadMessage.Replace("Micro.Micro820.", "");
			payloadMessage = payloadMessage.Replace("AB.CL5300.", "");

			//PLC.Vali_IFM PLC.Vali_Siemens. PLC.Inverter.
			string[] parts = payloadMessage.Split(';');
			//Channel1.Device1.Traffic_
			foreach (string part in parts)
			{
				if (!string.IsNullOrEmpty(part))
				{
					await _hubContext.Clients.All.SendAsync("TagChanged", part);
					//await _hubContext.Clients.All.SendAsync("TagChanged", DateTime.Now.AddHours(7));

					using (var scope = _scopeFactory.CreateScope())
					{
						var part1 = part.Replace("\\", "");
						try
						{
							var data1 = JsonConvert.DeserializeObject<Data>(part1);
							//data1.value= string.Format("{0:0.0000}", data1.value);

							//await _hubContext.Clients.All.SendAsync("TagChanged", data1);
							var dbContext = scope.ServiceProvider.GetRequiredService<MyData>();
							await dbContext.AddAsync(data1);
							await dbContext.SaveChangesAsync();
						}
						catch 
						{


							var data2 = JsonConvert.DeserializeObject<Data_N_V>(part1);
							var DATA = new Data
							{
								name = data2.name,
								value = data2.value,
								timestamp = DateTime.Now.AddHours(+7)
							};

							var dbContext = scope.ServiceProvider.GetRequiredService<MyData>();
							await dbContext.AddAsync(DATA);
							await dbContext.SaveChangesAsync();
						}

					}

				}

			}



		}

		private async Task ConnectAsync()
		{
			// _reconnectTimer.Enabled = false;
			try
			{
				await _managedMqtt.ConnectAsync();
				await _managedMqtt.Subscribe("qaz/123456");
				await _managedMqtt.Subscribe("micro850/Project/ab");
				//await _managedMqtt.Subscribe("VTSauto/AR_project/IOT_pub/+");
				await _managedMqtt.Subscribe("VTSauto/AR_project/Kep_pub");


			}
			catch (Exception ex)
			{
				Console.WriteLine($"MQTT connection failed: {ex.Message}");
				// _reconnectTimer.Enabled = true;
			}
		}

		private async Task SignalR_1()
		{

			var connection = new HubConnectionBuilder()
			   .WithUrl("https://vtsweb.vercel.app")
			   .Build();
			connection.On<string>("TagChanged", async data =>
			{
				await _managedMqtt.Publish("VTSauto/AR_project/IOT_pub/SEND", data, true);
			});

			await connection.StartAsync();
		}
	}
}
