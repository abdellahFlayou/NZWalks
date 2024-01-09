using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController (NZWalksDbContext dbContext, IRegionRepository regionRepository,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain = await regionRepository.GetAllAsync();

            //var regionDto = new List <RegionDto>();
            //foreach(var regiondomain in regionsDomain) 
            //{
            //    regionDto.Add(new RegionDto()
            //    {
            //        Id= regiondomain.Id,
            //        Name= regiondomain.Name, 
            //        Code= regiondomain.Code,
            //        RegionImageId= regiondomain.RegionImageId
            //    });
            //}

            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }

            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageId = regionDomain.RegionImageId
            };
            return Ok(regionDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto) 
        {
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,    
                RegionImageId= addRegionRequestDto.RegionImageId
            };

            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageId = regionDomainModel.RegionImageId
            };
            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDomainModel);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id,UpdateRegionsRequestDto updateRegionsRequestDto ) 
        {

            var regionModel = new Region
            {
                Code = updateRegionsRequestDto.Code,
                Name = updateRegionsRequestDto.Name,
                RegionImageId = updateRegionsRequestDto.RegionImageId
            };

            var regionDomainModel = await regionRepository.UpdateAsync(id, regionModel);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            var regionsDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageId = regionDomainModel.RegionImageId
            };
            return Ok(regionsDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) 
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            var regionsDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageId = regionDomainModel.RegionImageId
            }; 
            
            return Ok(regionsDto);

        }

    }
}
