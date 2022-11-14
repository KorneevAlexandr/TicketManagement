using System;
using System.ComponentModel.DataAnnotations;
using ThirdPartyEventEditor.Annotations;

namespace ThirdPartyEventEditor.Models
{
    /// <summary>
    /// Describes entity third party event for that application.
    /// </summary>
    public class ThirdPartyEvent
    {
        public int Id { get; set; }

        public string VenueName { get; set; }

        [Required]
        [MaxLength(120, ErrorMessage = "Length layout name can not be more than 120 characters")]
        public string LayoutName { get; set; }

        [Required(ErrorMessage = "Price can not be empty")]
        [Double(0, ErrorMessage = "Invalid format price")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Name can not be empty")]
        [MaxLength(120, ErrorMessage = "Length name can not be more than 120 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Start date can not be empty")]
        [DateTime(ErrorMessage = "Invalid format start date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date can not be empty")]
        [DateTime(ErrorMessage = "Invalid format start date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Description can not be empty")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Poster image can not be empty")]
        [InnerImage(ErrorMessage = "This image does not exist")]
        public string NameImage { get; set; }

        public string PosterImage { get; set; }

        public bool Exported { get; set; }
    }
}