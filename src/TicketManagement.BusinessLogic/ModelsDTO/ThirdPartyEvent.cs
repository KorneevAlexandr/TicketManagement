using System;

namespace TicketManagement.BusinessLogic.ModelsDTO
{
    /// <summary>
    /// Describe third-party event, provided third-party event editor.
    /// </summary>
	public class ThirdPartyEvent
	{
        public string VenueName { get; set; }

        public string LayoutName { get; set; }

        public double Price { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        public string PosterImage { get; set; }

        public string NameImage { get; set; }

        public bool Imported { get; set; }
    }
}
