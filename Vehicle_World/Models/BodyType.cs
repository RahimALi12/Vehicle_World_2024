﻿using System.ComponentModel.DataAnnotations;

namespace Vehicle_World.Models
{
    public class BodyType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
