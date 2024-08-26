using Microsoft.EntityFrameworkCore;
using System;

namespace MQTT.Models
{
	public class MyData : DbContext
	{
//#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public MyData(DbContextOptions options) : base(options) { }
//#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public DbSet<Data> Datas { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Xác định các thuộc tính làm khóa phức
			modelBuilder.Entity<Data>().HasKey(p => new { p.name, p.timestamp });
			modelBuilder.Entity<Data>(entity =>
			{
				entity.Property(p => p.name).IsRequired();
				entity.Property(p =>p.timestamp).IsRequired();
				entity.Property(p =>p.value).IsRequired();

			});
		}
	}
}
