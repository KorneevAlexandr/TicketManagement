using System;

namespace ThirdPartyEventEditor.Models
{
    /// <summary>
    /// Describes entity for export to third application.
    /// </summary>
    public class ExportThirdPartyEvent
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
    }
}