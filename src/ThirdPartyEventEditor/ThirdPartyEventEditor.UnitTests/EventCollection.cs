using System;
using System.Collections.Generic;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.UnitTests
{
	/// <summary>
	/// Represents the ThirdPartyEvents source in memory. 
	/// Used for testing.
	/// </summary>
	public static class EventCollection
	{
		/// <summary>
		/// Represents rhe ThirdPartyEvent collection, stored in memory.
		/// </summary>
		public static List<ThirdPartyEvent> Events = new List<ThirdPartyEvent>()
		{
			new ThirdPartyEvent
			{
				Id = 1,
				Name = "First",
				VenueName = "Venue",
				LayoutName = "Layout",
				Description = "Very well event",
				StartDate = DateTime.Now.AddDays(10),
				EndDate = DateTime.Now.AddDays(11),
				Exported = false,
				NameImage = "Poster",
				Price = 100,
				PosterImage = "data:image.png",
			},
			new ThirdPartyEvent
			{
				Id = 2,
				Name = "Second",
				VenueName = "Venue",
				LayoutName = "Layout",
				Description = "Bad event",
				StartDate = DateTime.Now.AddDays(12),
				EndDate = DateTime.Now.AddDays(13),
				Exported = false,
				NameImage = "Poster_2",
				Price = 10,
				PosterImage = "data:image.png",
			},
			new ThirdPartyEvent
			{
				Id = 1,
				Name = "Third",
				VenueName = "Venue",
				LayoutName = "Layout",
				Description = "Normal event",
				StartDate = DateTime.Now.AddDays(15),
				EndDate = DateTime.Now.AddDays(16),
				Exported = false,
				NameImage = "Poster_3",
				Price = 80,
				PosterImage = "data:image.png",
			},
		};

		/// <summary>
		/// Additional event, used to manipulate one event.
		/// </summary>
		public static ThirdPartyEvent DopEvent = new ThirdPartyEvent
		{
			Id = 4,
			Description = "Test description",
			Name = "X",
			VenueName = "",
			LayoutName = "",
			Price = 100,
			EndDate = DateTime.Now.AddDays(2),
			StartDate = DateTime.Now.AddDays(1),
			Exported = false,
			NameImage = "TestImage",
			PosterImage = "data:image.png/",
		};
	}
}
