﻿namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionsRequestDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageId { get; set; }
    }
}
