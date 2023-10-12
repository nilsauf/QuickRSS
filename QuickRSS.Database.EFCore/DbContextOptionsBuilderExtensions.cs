namespace QuickRSS.Database.EFCore
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Diagnostics;
	using System;

	public static class DbContextOptionsBuilderExtensions
	{
		public static DbContextOptionsBuilder ConfigureForEFCore(this DbContextOptionsBuilder builder, string connectionString)
		{
			if (builder is null)
				throw new ArgumentNullException(nameof(builder));
			if (string.IsNullOrWhiteSpace(connectionString))
				throw new ArgumentException($"'{nameof(connectionString)}' cannot be null or whitespace.", nameof(connectionString));

			return builder
				//.UseSqlServer(connectionString)
				.UseInMemoryDatabase("DevDB")
				.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
		}
	}
}
