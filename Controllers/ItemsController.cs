using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc; //for controllerbase
using Catalog.Repositories;
using Catalog.Domain;
using Catalog.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Catalog.Controllers
{


    [ApiController]
    [Route("items")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository repository;
        private readonly IMapper _mapper;
        //Create IMemory instance using constructor dependancy injection
        private IMemoryCache _memoryCache;
        private readonly IConfiguration _config;
        public ItemsController(IItemsRepository repository, IMapper mapper, IMemoryCache memoryCache, IConfiguration config) //constructor Dependency Injection
        {
            this.repository = repository;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _config = config;
        }

        //Route to retrieve all the items
        //For the method to become a route and respond to some http requests, declare the right attribute.
        // GET /items
        [HttpGet]
        public IEnumerable<ItemDto> GetItems()
        {

            IEnumerable<ItemDto> items;
            bool AlreadyExist = _memoryCache.TryGetValue("CachedItems", out items);
            if (!AlreadyExist)
            {
                items = repository.GetItems().Select(item => _mapper.Map<ItemDto>(item));
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(_config.GetValue<int>("Cache:CacheTimeMinutes")));
                _memoryCache.Set("CachedItems", items, cacheEntryOptions);

            }
            return items;
        }
        // GET /items/{id}
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id) //also change to dto contract
        {
            ItemDto item;
            bool itemExist = _memoryCache.TryGetValue("CachedItems_" + id, out item);
            if (!itemExist)
            {
                item = _mapper.Map<ItemDto>(repository.GetItem(id));
                if (item is null)
                {
                    return NotFound();
                }
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(_config.GetValue<int>("Cache:CacheTimeMinutes")));
                _memoryCache.Set("CachedItems_" + id, item, cacheEntryOptions);
            }

            return Ok(item);
        }

        //POST /items
        [HttpPost]

        public ActionResult<ItemDto> CreateItem(CreatItemDto itemDto)
        {
            //Item item = new()
            //{
            //    Id = Guid.NewGuid(),
            //    Name = itemDto.Name,
            //    Price = itemDto.Price,
            //    CreatedDate = DateTimeOffset.UtcNow
            //};

            var item = _mapper.Map<CreatItemDto, Item>(itemDto);
            repository.CreatItem(item);

            //return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item.AsDto());
            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, _mapper.Map<ItemDto>(item));
            //return the item created and also the location of the created item.
        }


        //PUT /items/id
        [HttpPut("{id}")]

        public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = repository.GetItem(id);
            if (existingItem is null)
            {
                return NotFound();
            }
            Item updatedItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            repository.UpdateItem(updatedItem);

            return NoContent();
        }

        //DELETE /items/id
        [HttpDelete("{id}")]

        public ActionResult DeleteItem(Guid id)
        {
            var existingItem = repository.GetItem(id);
            if (existingItem is null)
            {
                return NotFound();
            }

            repository.DeleteItem(id);


            return NoContent();
        }

    }
}
