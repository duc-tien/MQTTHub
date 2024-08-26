using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MQTT.Models;
using OfficeOpenXml;

namespace MQTT.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExcelDataController : ControllerBase
	{
		private readonly MyData _myData;
		public ExcelDataController (MyData myData)
		{
			_myData = myData;
		}

		[HttpGet("{name_e}/{time1_e}/{time2_e}")]
		public IActionResult DownloadExcel(string name_e, DateTime time1_e, DateTime time2_e)
		{
			// Truy vấn dữ liệu từ database bằng Entity Framework
			var data = _myData.Datas
				.Where(p => p.name == name_e && p.timestamp >= time1_e && p.timestamp <= time2_e)
				.OrderBy(p => p.timestamp)
				.ToList();
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			using (var package = new OfficeOpenXml.ExcelPackage(new FileInfo("MyWorkbook.xlsx")))
			{
				var worksheet = package.Workbook.Worksheets.Add($"{data.First().name}");

				// Điền tiêu đề cột
				worksheet.Cells[1, 1].Value = "Name";
				worksheet.Cells[1, 2].Value = "Value";
				worksheet.Cells[1, 3].Value = "Timestamp";

				// Điền dữ liệu từ Entity Framework vào worksheet
				for (int row = 2; row <= data.Count + 1; row++)
				{
					worksheet.Cells[row, 1].Value = data[row - 2].name;
					worksheet.Cells[row, 2].Value = data[row - 2].value;
					worksheet.Cells[row, 3].Value = "\'"+data[row - 2].timestamp;
				}

				// Gửi file Excel như response
				var stream = new MemoryStream(package.GetAsByteArray());
				return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
				{
					FileDownloadName = "products.xlsx"
				};
			}
		}
	}
}
