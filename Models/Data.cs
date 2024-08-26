using System.ComponentModel.DataAnnotations;

namespace MQTT.Models
{
	public class Data
	{
	//	[Required]
	//	[MaxLength(100)]
	//	[Key]
		public string name { get; set; }
	//	[Required]
	//	[MaxLength(100)]
		public string value { get; set; }
	//	[Required]
		public DateTime timestamp { get; set; } = DateTime.Now.AddHours(7);
	}
}
