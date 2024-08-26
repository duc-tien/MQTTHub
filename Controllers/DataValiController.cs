using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MQTT.Models;

namespace MQTT.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DataValiController : ControllerBase
	{
		private readonly MyData _myData;
		public DataValiController(MyData myData) 
		{
			_myData = myData;
		}

		[HttpGet("{name}/{time1}/{time2}")]
		public IActionResult GetDataVali(string name, DateTime time1, DateTime time2) 
		{
			var data = _myData.Datas
				.Where(p => p.name == name && p.timestamp >= time1 && p.timestamp <=time2)
				.OrderBy(p=> p.timestamp)
				.Select(p=> new { value= p.value, timestamp = p.timestamp })
				.ToList();

			if (data != null) { return Ok(data); }
			else { return BadRequest(); }
		}


		//[HttpGet("{name_e}/{time1_e}/{time2_e}")]
		//public IActionResult DownloadExcel(string name_e, DateTime time1_e, DateTime time2_e)
		//{
		//	// Truy vấn dữ liệu từ database bằng Entity Framework
		//	var data = _myData.Datas
		//		.Where(p => p.name == name_e && p.timestamp >= time1_e && p.timestamp <= time2_e)
		//		.OrderBy(p => p.timestamp)
		//		.ToList();

		//	using (var package = new OfficeOpenXml.ExcelPackage())
		//	{
		//		var worksheet = package.Workbook.Worksheets.Add($"{data.First().name}");

		//		// Điền tiêu đề cột
		//		worksheet.Cells[1, 1].Value = "Name";
		//		worksheet.Cells[1, 2].Value = "Value";
		//		worksheet.Cells[1, 3].Value = "Timestamp";

		//		// Điền dữ liệu từ Entity Framework vào worksheet
		//		for (int row = 2; row <= data.Count + 1; row++)
		//		{
		//			worksheet.Cells[row, 1].Value = data[row - 2].name;
		//			worksheet.Cells[row, 2].Value = data[row - 2].value;
		//			worksheet.Cells[row, 3].Value = data[row - 2].timestamp;
		//		}

		//		// Gửi file Excel như response
		//		var stream = new MemoryStream(package.GetAsByteArray());
		//		return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
		//		{
		//			FileDownloadName = "products.xlsx"
		//		};
		//	}
		//}

	}
}
