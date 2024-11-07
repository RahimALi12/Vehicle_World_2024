using Microsoft.DotNet.Scaffolding.Shared.Project;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vehicle_World.Models
{
    public class CarDetail
    {
        [Key]
        public int Id { get; set; }


        public int MakeTypeId { get; set; }

        [ForeignKey("MakeTypeId")]
        public virtual MakeType MakeType { get; set; }

        public int ModelTypeId { get; set; }

        [ForeignKey("ModelTypeId")]
        public virtual ModelType ModelType { get; set; }

        [Required]
        [Range(1886, 2024, ErrorMessage = "Please enter a valid year between 1886 and 2024")]
        public int Year { get; set; }

        public int BodyTypeId { get; set; }

        [ForeignKey("BodyTypeId")]
        public virtual BodyType BodyType { get; set; }

        public int EngineTypeId { get; set; }

        [ForeignKey("EngineTypeId")]
        public virtual EngineType EngineType { get; set; }

        public int FuelTypeId { get; set; }

        [ForeignKey("FuelTypeId")]
        public virtual FuelType FuelType { get; set; }

        public int TransmissionTypeId { get; set; }

        [ForeignKey("TransmissionTypeId")]
        public virtual TransmissionType TransmissionType { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessage = "Please enter a valid mileage between 0 and 1,000,000 kilometers.")]

        public float Mileage { get; set; }

        [Required]
        [Range(2, 72, ErrorMessage = "Please enter a valid seating capacity between 2 and 72 seats.")]
        public int SeatingCapacity { get; set; }

        [Required]
        public int Price { get; set; }

        public int ConditionTypeId { get; set; }

        [ForeignKey("ConditionTypeId")]
        public virtual ConditionType ConditionType { get; set; }


        [Required]
        public DateTime CreatedDate { get; set; } // Date when the car was uploaded

        public string Seller_Id { get; set; }

        [ForeignKey("Seller_Id")]
        public virtual AppUser Seller { get; set; }




        public virtual ICollection<Feedback> Feedbacks { get; set; } // Add this line

        [Required]
        public string Image { get; set; }

        [NotMapped]
        public IFormFile CarImage { get; set; }


        public string ApprovalStatus { get; set; } = "Pending"; // Default status will be 'Pending'



        public decimal? MinimumBidAmount { get; set; } // Minimum bidding amount set by seller

        public bool IsBiddingEnabled { get; set; }
        public bool IsBiddingRequested { get; set; } // Whether bidding request is sent

        public bool BiddingRequestPending { get; set; }
        public string BiddingStatus { get; set; } // Status of the bidding request (Pending, Approved, Rejected)


        public bool IsFeatured { get; set; }  // Admin-approved featured status
        public bool IsFeatureRequested { get; set; }  // Seller requests this car to be featured
       public bool FeatureRequestPending { get; set; }
        public string Status { get; set; } // Approved, Pending, etc.

        //More Features

        public bool BlindspotMonitor { get; set; }
        public bool Adaptivecruisecontrol { get; set; }
        public bool BackupCamera { get; set; }
        public bool ForwardCollisionwarning { get; set; }
        public bool Heatedseats { get; set; }
        public bool Hillassist { get; set; }
        public bool Sunroof { get; set; }
        public bool AutoPark { get; set; }
        public bool Automaticemergencybraking { get; set; }
        public bool EvasiveSteering { get; set; }
        public bool Leatherseats { get; set; }
        public bool Remotestart { get; set; }
        public bool USBoutlets { get; set; }
        public bool Drivercommunicationassistance { get; set; }
        public bool AirConditioning { get; set; }
        public bool Battery { get; set; }
        public bool Bluetooth { get; set; }
    }
}
